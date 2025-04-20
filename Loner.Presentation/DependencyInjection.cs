using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Loner.Application.Features.Auth;
using Loner.Application.Features.Matches;
using Loner.Application.Features.Swipe;
using Loner.Application.Features.User;
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
        services.AddScoped<IMatchesRepository, MatchesRepository>();
        services.AddScoped<ISwipeRepository, SwipeRepository>();
        services.AddScoped<IPhotoRepository, PhotoRepository>();
        services.AddScoped<IInterestRepository, InterestRepository>();

        //Regis MediatR
        services.AddMediatR(cfg =>
        {
            _ = cfg.RegisterServicesFromAssemblies(
                Assembly.GetAssembly(typeof(SendOtpHandler)),
                Assembly.GetAssembly(typeof(VerifyOtpOrRegisterHandler)),
                Assembly.GetAssembly(typeof(SendFileResponseExtensions)),
                Assembly.GetAssembly(typeof(GetMatchesRequestHandler)),
                Assembly.GetAssembly(typeof(GetProfileDetailHandler)),
                Assembly.GetAssembly(typeof(GetProfileDetailHandler)),
                Assembly.GetAssembly(typeof(SwipeProfileHandler))
            );
        });

        return services;
    }
}