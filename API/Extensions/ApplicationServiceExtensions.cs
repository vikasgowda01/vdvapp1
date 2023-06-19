using API.data;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;
using API.Services;

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

            return services;
        }
    }
}