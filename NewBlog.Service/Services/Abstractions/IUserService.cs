using Microsoft.AspNetCore.Identity;
using NewBlog.Entity.DTOs.Users;
using NewBlog.Entity.Entities;

namespace NewBlog.Service.Services.Abstractions
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllUsersWithRoleAsync();
        Task<List<AppRole>> GetAllRolesAsync();
        Task<IdentityResult> CreateUserAsync(UserAddDto model);
        Task<IdentityResult> UpdateUserAsync(UserUpdateDto model);
        Task<(IdentityResult identityResult, string? userEmail)> DeleteUserAsync(Guid userId);
        Task<AppUser> GetAppUserByIdAsync(Guid id);
        Task<string> GetUserRoleAsync(AppUser user);
        Task<UserProfileDto> GetUserProfileAsync();
        Task<bool> UserProfileUpdate(UserProfileDto userProfileDto);
    }
}
