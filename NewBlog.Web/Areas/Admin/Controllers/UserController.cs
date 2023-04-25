using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewBlog.Entity.DTOs.Users;
using NewBlog.Entity.Entities;
using NewBlog.Service.Extensions;
using NewBlog.Service.Services.Abstractions;
using NewBlog.Web.Consts;
using NewBlog.Web.ResultMessages;
using NToastNotify;

namespace NewBlog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IToastNotification _toast;
        private readonly IValidator<AppUser> _validator;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper, IToastNotification toast, IValidator<AppUser> validator)
        {
            _userService = userService;
            _mapper = mapper;
            _toast = toast;
            _validator = validator;
        }

        [HttpGet]
        [Authorize(Roles = $"{RoleConsts.SuperAdmin}, {RoleConsts.Admin}")]
        public async Task<IActionResult> Index()
        {
            var result = await _userService.GetAllUsersWithRoleAsync();
            return View(result);
        }

        [HttpGet]
        [Authorize(Roles = $"{RoleConsts.SuperAdmin}, {RoleConsts.Admin}")]
        public async Task<IActionResult> Add()
        {
            var roles = await _userService.GetAllRolesAsync();
            return View(new UserAddDto { Roles = roles });
        }

        [HttpPost]
        [Authorize(Roles = $"{RoleConsts.SuperAdmin}, {RoleConsts.Admin}")]
        public async Task<IActionResult> Add(UserAddDto model)
        {
            var map = _mapper.Map<AppUser>(model);
            var validation = await _validator.ValidateAsync(map);
            var roles = await _userService.GetAllRolesAsync();

            if (ModelState.IsValid)
            {
                var result = await _userService.CreateUserAsync(model);

                if (result.Succeeded)
                {
                    _toast.AddSuccessToastMessage(Messages.UserMessage.Add(model.Email), new ToastrOptions { Title = "Creating User" });
                    return RedirectToAction("Index", "User", new { Area = "Admin" });
                }

                else
                {
                    result.AddToIdentityModelState(ModelState);
                    validation.AddToModelState(ModelState);

                    return View(new UserAddDto { Roles = roles });
                }
            }

            return View(new UserAddDto { Roles = roles });
        }

        [HttpGet]
        [Authorize(Roles = $"{RoleConsts.SuperAdmin}, {RoleConsts.Admin}")]
        public async Task<IActionResult> Update(Guid userId)
        {
            var user = await _userService.GetAppUserByIdAsync(userId);
            var roles = await _userService.GetAllRolesAsync();

            var map = _mapper.Map<UserUpdateDto>(user);
            map.Roles = roles;

            return View(map);
        }

        [HttpPost]
        [Authorize(Roles = $"{RoleConsts.SuperAdmin}, {RoleConsts.Admin}")]
        public async Task<IActionResult> Update(UserUpdateDto user)
        {
            var updateUser = await _userService.GetAppUserByIdAsync(user.Id);

            if (updateUser != null)
            {
                var roles = await _userService.GetAllRolesAsync();

                if (ModelState.IsValid)
                {
                    var map = _mapper.Map(user, updateUser);
                    var validation = await _validator.ValidateAsync(map);

                    if (validation.IsValid)
                    {
                        updateUser.UserName = user.Email;
                        updateUser.SecurityStamp = Guid.NewGuid().ToString();

                        var result = await _userService.UpdateUserAsync(user);

                        if (result.Succeeded)
                        {
                            _toast.AddSuccessToastMessage(Messages.UserMessage.Update(user.Email), new ToastrOptions { Title = "Updating User" });
                            return RedirectToAction("Index", "User", new { Area = "Admin" });
                        }

                        else
                        {
                            result.AddToIdentityModelState(ModelState);
                            return View(new UserUpdateDto { Roles = roles });

                        }
                    }

                    else
                    {
                        validation.AddToModelState(ModelState);
                        return View(new UserUpdateDto { Roles = roles });
                    }
                }
            }
            return NotFound();
        }

        [Authorize(Roles = $"{RoleConsts.SuperAdmin}")]
        public async Task<IActionResult> Delete(Guid userId)
        {
            var result = await _userService.DeleteUserAsync(userId);

            if (result.identityResult.Succeeded)
            {
                _toast.AddSuccessToastMessage(Messages.UserMessage.Delete(result.userEmail), new ToastrOptions { Title = "Deleting User" });
                return RedirectToAction("Index", "User", new { Area = "Admin" });
            }

            else
                result.identityResult.AddToIdentityModelState(ModelState);

            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var profile = await _userService.GetUserProfileAsync();
            return View(profile);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(UserProfileDto user)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.UserProfileUpdate(user);

                if (result)
                {
                    _toast.AddSuccessToastMessage("Profile Updated Successfully!", new ToastrOptions { Title = "Updating User" });
                    return RedirectToAction("Index", "Home", new { Area = "Admin" });
                }
                else
                {
                    var profile = await _userService.GetUserProfileAsync();

                    _toast.AddErrorToastMessage("An Error Occured!", new ToastrOptions { Title = "Updating User" });
                    return View(profile);
                }
            }
            else
                return NotFound();
        }
    }
}
