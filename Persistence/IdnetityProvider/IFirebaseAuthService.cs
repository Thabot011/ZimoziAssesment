using Contracts.User;

namespace Persistence.IdnetityProvider

{
    public interface IFirebaseAuthService
    {
        Task<string?> SignUp(AddUserDto user);
        Task<UserDto?> Login(string email, string password);
        Task<UserDto?> GoogleSignIn(string idToken);
        void SignOut();

    }
}
