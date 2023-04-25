using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NewBlog.Data.UnitOfWorks;
using NewBlog.Entity.DTOs.Users;
using NewBlog.Entity.Entities;
using NewBlog.Entity.Enums;
using NewBlog.Service.Extensions;
using NewBlog.Service.Helpers.Images;
using NewBlog.Service.Services.Abstractions;
using System.Security.Claims;

namespace NewBlog.Service.Services.Concrete
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IHttpContextAccessor _accessor;
        private readonly ClaimsPrincipal _user;
        private readonly IImageHelper _imageHelper;
        private readonly SignInManager<AppUser> _signInManager;

        public UserService(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor accessor, SignInManager<AppUser> signInManager, IImageHelper imageHelper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _accessor = accessor;
            _user = _accessor.HttpContext.User;
            _signInManager = signInManager;
            _imageHelper = imageHelper;
        }

        public async Task<List<UserDto>> GetAllUsersWithRoleAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var map = _mapper.Map<List<UserDto>>(users);

            foreach (var user in map)
            {
                var findUser = await _userManager.FindByIdAsync(user.Id.ToString());
                var role = string.Join("", await _userManager.GetRolesAsync(findUser));

                user.Role = role;
            }

            return map;
        }

        public async Task<List<AppRole>> GetAllRolesAsync()
        {
            return await _roleManager.Roles.ToListAsync();
        }

        public async Task<IdentityResult> CreateUserAsync(UserAddDto model)
        {
            var map = _mapper.Map<AppUser>(model);
            map.UserName = model.Email;

            var result = await _userManager.CreateAsync(map, string.IsNullOrEmpty(model.Password) ? "" : model.Password);

            if (result.Succeeded)
            {
                var findRole = await _roleManager.FindByIdAsync(model.RoleId.ToString());
                await _userManager.AddToRoleAsync(map, findRole.ToString());
                return result;
            }
            else
                return result;

        }

        public async Task<AppUser> GetAppUserByIdAsync(Guid id)
        {
            return await _userManager.FindByIdAsync(id.ToString());
        }

        public async Task<IdentityResult> UpdateUserAsync(UserUpdateDto model)
        {
            var user = await GetAppUserByIdAsync(model.Id);
            var userRole = GetUserRoleAsync(user);
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                await _userManager.RemoveFromRoleAsync(user, userRole.ToString());
                var findRole = await _roleManager.FindByIdAsync(model.RoleId.ToString());
                await _userManager.AddToRoleAsync(user, findRole.Name);

                return result;
            }
            else
                return result;
        }

        public async Task<string> GetUserRoleAsync(AppUser user)
        {
            return string.Join("", await _userManager.GetRolesAsync(user));
        }

        public async Task<(IdentityResult identityResult, string? userEmail)> DeleteUserAsync(Guid userId)
        {
            var user = await GetAppUserByIdAsync(userId);
            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
                return (result, user.Email);
            else
                return (result, null);
        }

        public async Task<UserProfileDto> GetUserProfileAsync()
        {
            var userId = _user.GetLoggedInUserId();

            var getUserWithImage = await _unitOfWork.GetRepository<AppUser>().GetAsync(x => x.Id == userId, x => x.Image);

            var map = _mapper.Map<UserProfileDto>(getUserWithImage);
            map.Image.FileName = getUserWithImage.Image.FileName;

            return map;
        }

        private async Task<Guid> UploadImageForUser(UserProfileDto userProfileDto)
        {
            var userEmail = _user.GetLoggedInEmail();

            var imageUpload = await _imageHelper.Upload($"{userProfileDto.FirstName}{userProfileDto.LastName}", userProfileDto.Photo, ImageType.User);
            Image image = new(imageUpload.FullName, userProfileDto.Photo.ContentType, userEmail);
            await _unitOfWork.GetRepository<Image>().AddAsync(image);

            return image.Id;
        }

        public async Task<bool> UserProfileUpdate(UserProfileDto userProfileDto)
        {
            var userId = _user.GetLoggedInUserId();
            var user = await GetAppUserByIdAsync(userId);

            var isVerified = await _userManager.CheckPasswordAsync(user, userProfileDto.CurrentPassword);

            if (isVerified && userProfileDto.NewPassword != null)
            {
                var result = await _userManager.ChangePasswordAsync(user, userProfileDto.CurrentPassword, userProfileDto.NewPassword);

                if (result.Succeeded)
                {
                    await _userManager.UpdateSecurityStampAsync(user);
                    await _signInManager.SignOutAsync();
                    await _signInManager.PasswordSignInAsync(user, userProfileDto.NewPassword, true, false);

                    _mapper.Map(userProfileDto, user);

                    if (userProfileDto.Photo != null)
                        user.ImageId = await UploadImageForUser(userProfileDto);
                    
                    await _userManager.UpdateAsync(user);
                    await _unitOfWork.SaveAsync();

                    return true;
                }

                else
                    return false;
            }

            else if (isVerified)
            {
                await _userManager.UpdateSecurityStampAsync(user);

                _mapper.Map(userProfileDto, user);

                if (userProfileDto.Photo != null)
                    user.ImageId = await UploadImageForUser(userProfileDto);

                await _userManager.UpdateAsync(user);
                await _unitOfWork.SaveAsync();

                return true;
            }

            else
                return false;
        }
    }
}
