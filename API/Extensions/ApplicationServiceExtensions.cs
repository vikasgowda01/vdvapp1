using API.data;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;
using API.Services;
using System;
using API.Helpers;
using API.SignalR;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<DataContext>(opt => 
        {
            opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
        });
            services.AddCors();
            services.AddScoped<ITokenServices, TokenService>();
            
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<LogUserActivity>();
           
            services.AddSignalR();
            services.AddSingleton<PresenceTracker>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}