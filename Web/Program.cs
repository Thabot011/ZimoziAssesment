
using Contracts.Order;
using Contracts.Product;
using Contracts.User;
using Domain.Reposiroty_Interfaces;
using Firebase.Auth;
using Firebase.Auth.Providers;
using FirebaseAdmin;
using Google.Api;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Persistence.IdnetityProvider;
using Persistence.Repositories;
using Presentation;
using Services;
using Services.Abstractions;
using System.Text.Json.Serialization;
using Web.MiddleWare;
namespace Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            string pathToServiceAccountKey = "zimozi-ac7c3-firebase-adminsdk-fbsvc-47a2ee972d.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", pathToServiceAccountKey);
            builder.Services.AddSingleton(FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(pathToServiceAccountKey)
            }));

            builder.Services.AddDirectoryBrowser();
            builder.Services.AddCors();

            // Add services to the container.

            builder.Services.AddControllers()
                .AddApplicationPart(typeof(IAssemblyReference).Assembly).AddJsonOptions(o =>
                {
                    o.JsonSerializerOptions.Converters.Add(new JsonNumberEnumConverter<Category>());
                    o.JsonSerializerOptions.Converters.Add(new JsonNumberEnumConverter<PyamentMethod>());
                    o.JsonSerializerOptions.Converters.Add(new JsonNumberEnumConverter<OrderStatus>());
                    o.JsonSerializerOptions.Converters.Add(new JsonNumberEnumConverter<Category>());
                    o.JsonSerializerOptions.Converters.Add(new JsonNumberEnumConverter<UserRole>());
                });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Zimozi", Version = "v1" });

                // Configure Swagger to accept JWT token for authorization
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid JWT token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
            });

            builder.Services.AddOpenApi();

            var firebaseProjectName = "zimozi-ac7c3";
            builder.Services.AddSingleton(new FirebaseAuthClient(new FirebaseAuthConfig
            {
                ApiKey = "AIzaSyAt_s6twBTnakR508WwXXmd67VqM6P0LkY",
                AuthDomain = $"{firebaseProjectName}.firebaseapp.com",
                Providers = new FirebaseAuthProvider[]
                {
        new EmailProvider(),
        new GoogleProvider()
                }
            }));
            builder.Services.AddSingleton<IFirebaseAuthService, FirebaseAuthService>();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = $"https://securetoken.google.com/{firebaseProjectName}";
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = $"https://securetoken.google.com/{firebaseProjectName}",
                        ValidateAudience = true,
                        ValidAudience = firebaseProjectName,
                        ValidateLifetime = false,
                        RequireExpirationTime = false
                    };
                });

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();


            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IProductService, ProductService>();

            builder.Services.AddTransient<ExceptionHandlingMiddleware>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            // Enable Swagger
            app.UseSwagger();

            // Enable Swagger UI
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Zimozi");
            });

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3000", "https://zimozi-ac7c3.web.app"));

            app.UseAuthentication();

            app.UseAuthorization();
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseStaticFiles();
            app.UseDirectoryBrowser();
            app.MapControllers();

            app.Run();
        }
    }
}
