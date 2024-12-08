using GoWheels_WebAPI.Models.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace GoWheels_WebAPI.Service.Interface
{
    public interface IAuthenticationService
    {
        Task<IdentityResult> SignUpAsync(SignUpVM signUpViewModel);
        void RemoveUserFromSession();
        Task<string> LoginAsync(LoginVM loginViewModel);
        Task<bool> CheckLockoutStatus();
        Task<UserVM?> GetUserFromToken(string token);
    }
}
