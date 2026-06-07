using Application.Common.Interfaces;
using Azure.Storage.Blobs;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("Connection")));

            services.AddScoped<ITicketRepository, TicketRepository>();

            var blobConnectionString = configuration.GetConnectionString("AzureBlobStorage");
            services.AddSingleton(x => new BlobServiceClient(blobConnectionString));

            services.AddScoped<IFileStorageService, AzureBlobStorageService>();

            return services;
        }
    }
}