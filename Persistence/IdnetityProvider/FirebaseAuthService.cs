using Contracts.User;
using Domain.Entities;
using Domain.Reposiroty_Interfaces;
using Firebase.Auth;
using FirebaseAdmin.Auth;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;

namespace Persistence.IdnetityProvider
{
    public class FirebaseAuthService : IFirebaseAuthService
    {
        private readonly FirebaseAuthClient _firebaseAuth;
        private readonly IServiceProvider _serviceProvider;

        public FirebaseAuthService(FirebaseAuthClient firebaseAuth, IServiceProvider serviceProvider)
        {
            _firebaseAuth = firebaseAuth;
            _serviceProvider = serviceProvider;
        }
        public async Task<UserDto?> GoogleSignIn(string idToken)
        {
            var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idToken);
            var uid = decodedToken.Uid;
            var email = decodedToken.Claims.FirstOrDefault(x => x.Key == "email").Value.ToString();

            using (var scope = _serviceProvider.CreateScope())
            {
                var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
                var user = await userRepository.GetUserByUserIdentity(uid);
                if (user == null)
                {
                    var userId = await userRepository.AddUser(new Domain.Entities.User
                    {
                        FullName = "",
                        Email = email,
                        UserId = uid,
                    });
                }

                return new UserDto { Email = user.Email, FullName = user.FullName, Id = user.Id, Role = (Contracts.User.UserRole)user.Role, UserId = user.UserId, Token = idToken };
            }
        }

        public async Task<UserDto?> Login(string email, string password)
        {
            var userCredentials = await _firebaseAuth.SignInWithEmailAndPasswordAsync(email, password);
            if (userCredentials is null)
            {
                return null;
            }
            var token = await userCredentials.User.GetIdTokenAsync();
            var userId = userCredentials.User.Uid;


            using (var scope = _serviceProvider.CreateScope())
            {
                var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
                var user = await userRepository.GetUserByUserIdentity(userId);
                if (user is null)
                {
                    return null;
                }
                return new UserDto
                {
                    Email = user.Email,
                    FullName = user.FullName,
                    Id = user.Id,
                    Role = (Contracts.User.UserRole)user.Role,
                    UserId = user.UserId,
                    Token = token,
                    ShoppingCart = new Contracts.ShoppingCart.ShoppingCartDto
                    {
                        Id = user.ShoppingCart?.Id,
                        TotalPrice = user.ShoppingCart?.TotalPrice,
                        Products = user.ShoppingCart?.Products?.Select(x => new Contracts.Product.ProductDto
                        {
                            Id = x.Id,
                            Category = (Contracts.Product.Category)x.Category,
                            ImagePath = x.ImagePath,
                            Name = x.Name,
                            Price = x.Price,
                            StockAvailability = x.StockAvailability
                        }).ToList(),
                        ProductIds = user.ShoppingCart?.Products?.Select(x => x.Id).ToList()
                    }
                };
            }
        }

        public void SignOut()
        {
            _firebaseAuth.SignOut();
        }

        public async Task<string?> SignUp(AddUserDto user)
        {
            var userCredentials = await _firebaseAuth.CreateUserWithEmailAndPasswordAsync(user.Email, user.Password);
            if (userCredentials is null)
            {
                return null;
            }

            var userId = userCredentials.User.Uid;

            using (var scope = _serviceProvider.CreateScope())
            {
                var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

                await userRepository.AddUser(new Domain.Entities.User { Email = user.Email, FullName = user.FullName, UserId = userId, Role = (Domain.Entities.UserRole)user.UserRole });

                var claims = new Dictionary<string, object>
            {
            { "role", user.UserRole }
        };
                await FirebaseAuth.DefaultInstance.SetCustomUserClaimsAsync(userId, claims);

                return userId;
            }
        }
    }
}
