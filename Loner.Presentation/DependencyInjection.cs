using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Loner.Application.Features.Auth;
using Loner.Domain.Interfaces;
using Loner.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Loner.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {
        //Regis DbContext
        services.AddDbContext<LonerDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.Configure<JwtConfig>(configuration.GetSection("JwtConfig"));
        services.AddAutoMapper(typeof(Program));

        //Regis Service
        services.AddScoped<ISendOTPtoPhoneNumberService, SendOTPtoPhoneNumberService>();
        services.AddScoped<ISendOTPToMailService, SendOTPToMailService>();
        services.AddScoped<IJWTTokenService, JWTTokenService>();
        //Regis Repositories
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IOtpRepository, OtpRepository>();

        //Regis MediatR
        services.AddMediatR(cfg =>
        {
            _ = cfg.RegisterServicesFromAssemblies(
                Assembly.GetAssembly(typeof(SendOtpHandler)),
                Assembly.GetAssembly(typeof(VerifyOtpOrRegisterHandler))
            );
        });

        return services;
    }
}