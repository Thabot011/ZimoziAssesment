using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Reposiroty_Interfaces
{
    public interface IUserRepository
    {
        Task<string> AddUser(User user);
        Task UpdateUser(User user);
        Task<User> GetUser(string userId);
        Task<User?> GetUserByUserIdentity(string userId);
    }
}
