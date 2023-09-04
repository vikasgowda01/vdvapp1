using API.data;
using API.Interfaces;
using API.Services;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IO;
using API.Middleware;
using API.Extensions;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using API.entites;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
/*{
    app.UseSwagger();
    app.UseSwaggerUI();
}*/

//app.UseHttpsRedirection();

//app.UseAuthorization();
var app = builder.Build();
// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();

app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();
var services =scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<DataContext>();
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
    await context.Database.MigrateAsync();
    await Seed.SeedUsers(userManager, roleManager);
}
catch (Exception ex)
{
    var logger =services.GetService<ILogger<Program>>();
    logger.LogError(ex, "An error occured during migration");
    
}

app.Run();
