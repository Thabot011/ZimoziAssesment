using Contracts.User;

namespace Services.Abstractions
{
    public interface IUserService
    {
        Task UpdateUser(UpdateUserDto user);
        Task<UserDto?> GetUserById(string userId);
    }
}
