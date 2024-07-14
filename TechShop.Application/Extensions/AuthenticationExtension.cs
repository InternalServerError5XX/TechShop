using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace TechShop.Application.Extensions
{
    public static class AuthenticationExtension
    {
        public static void AddJwtAuthentication(this IServiceCollection services,
        string? issuer, string? audience, string? jwtKey, int? expires)
        {
            if (jwtKey == null) 
                throw new NullReferenceException("Jwt key is null");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddCookie("Cookies", options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromDays(expires!.Value);
                })
                .AddJwtBearer("Bearer", options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = audience,
                        ValidIssuer = issuer,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                        ValidateIssuerSigningKey = true
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            if (context.Request.Cookies.TryGetValue("token", out var token))
                            {
                                context.Token = token;
                            }
                            else if (context.HttpContext.Response.Headers.TryGetValue("Set-Cookie", out var setCookieHeaders))
                            {
                                var cookieHeader = setCookieHeaders.FirstOrDefault(h => h.StartsWith("token="));
                                if (cookieHeader != null)
                                {
                                    token = cookieHeader.Split('=')[1].Split(';')[0];
                                    context.Token = token;
                                }
                            }

                            return Task.CompletedTask;
                        },

                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.Response.Redirect("/Identity/Login");
                            return Task.CompletedTask;
                        },

                        OnForbidden = context =>
                        {
                            context.Response.StatusCode = StatusCodes.Status403Forbidden;
                            context.Response.Redirect("/Error/Error403");
                            return Task.CompletedTask;
                        }
                    };
                });
        }
    }
}
