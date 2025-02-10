using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using VNTrackPal.Contract.Common.Enums;
using VNTrackPal.Contract.Settings;

namespace VNTrackPal.Api.DepedencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConfigurationAppSetting
     (this IServiceCollection services, IConfiguration configuration)
    {
        services
            .Configure<EmailSetting>(configuration.GetSection(EmailSetting.SectionName))
            .Configure<AuthSetting>(configuration.GetSection(AuthSetting.SectionName))
            .Configure<CloudinarySetting>(configuration.GetSection(CloudinarySetting.SectionName))
            .Configure<UserSetting>(configuration.GetSection(UserSetting.SectionName));

        return services;
    }

    public static IServiceCollection AddAuthenticationAndAuthorization(this IServiceCollection services, IConfiguration configuration)
    {
        var authSetting = new AuthSetting();
        configuration.GetSection(AuthSetting.SectionName).Bind(authSetting);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
       .AddJwtBearer(options =>
       {
           options.SaveToken = true;
           options.TokenValidationParameters = new TokenValidationParameters
           {
               ValidateIssuer = false,
               ValidateAudience = false,
               ValidateLifetime = true,
               ValidateIssuerSigningKey = true,
               ValidIssuer = authSetting.Issuer,
               ValidAudience = authSetting.Audience,
               IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSetting.AccessSecretToken)),
               ClockSkew = TimeSpan.Zero
           };

           options.Events = new JwtBearerEvents
           {
               OnAuthenticationFailed = context =>
               {
                   if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                   {
                       context.Response.Headers.Add("IS-TOKEN-EXPIRED", "true");
                   }
                   return Task.CompletedTask;
               },
           };
       });

        services.AddAuthorization(options =>
        {
            // Admin Policy
            options.AddPolicy("AdminPolicy", policy =>
                policy.RequireClaim(ClaimTypes.Role, RoleEnum.Admin.ToString()).ToString());

            // Member Policy
            options.AddPolicy("MemberPolicy", policy =>
                policy.RequireClaim(ClaimTypes.Role, RoleEnum.Member.ToString()).ToString());
        });

        return services;
    }
}