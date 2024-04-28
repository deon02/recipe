using System.Threading.Tasks;
using Microsoft.Identity.Client;
using recipe.Models;

namespace recipe.Services
{
    public interface IAuthService
    {
        Task<AuthResult> LoginAsync(LoginRequestModel model);
        Task<AuthResult> SignUpAsync(SignUpRequestModel model);
        Task<AuthResult> ChangePasswordAsync(int userId, ChangePasswordRequestModel model);
        Task<AuthResult> ChangeUsernameAsync(int userId, ChangeUsernameRequestModel model);

    }
}
