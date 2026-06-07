using Application.Common.Interfaces;
using Application.Common.Models;
using Azure.Storage.Blobs;
using Infrastructure.Authentication;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("Connection")));

            services.AddScoped<ITicketRepository, TicketRepository>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.AddTransient<IEmailService, EmailService>();

            var blobConnectionString = configuration.GetConnectionString("AzureBlobStorage");
            services.AddSingleton(x => new BlobServiceClient(blobConnectionString));

            services.AddScoped<IFileStorageService, AzureBlobStorageService>();

            services.AddScoped<IApplicationDbContext>(provider =>
                    (IApplicationDbContext)provider.GetRequiredService<ApplicationDbContext>());

            services.AddAuthentication(defaultScheme: "Bearer")
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["JwtSettings:Issuer"],
                        ValidAudience = configuration["JwtSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Secret"]!))
                    };
                });

            services.AddAuthorization();

            return services;
        }
    }
}