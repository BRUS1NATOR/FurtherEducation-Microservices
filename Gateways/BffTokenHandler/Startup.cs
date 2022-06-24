using BffTokenHandler.Extensions;
using BffTokenHandler.Services;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System;
using System.Net.Http;

namespace BffTokenHandler
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton<ITokenService, TokenService>();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
            //https://stackoverflow.com/questions/60858985/addopenidconnect-and-refresh-tokens-in-asp-net-core
            .AddCookie("Cookies", options =>
            {
                options.Events = new CookieAuthenticationEvents
                {
                    OnValidatePrincipal = async cookieCtx =>
                    {
                        var now = DateTimeOffset.UtcNow;
                        var expiresAt = cookieCtx.Properties.GetTokenValue("expires_at");
                        var accessTokenExpiration = DateTimeOffset.Parse(expiresAt);
                        var timeRemaining = accessTokenExpiration.Subtract(now);

                        var refreshThresholdMinutes = Configuration.GetValue<int>("OpenID:RefreshThresholdMinutes");
                        var refreshThreshold = TimeSpan.FromMinutes(refreshThresholdMinutes);

                        if (timeRemaining < refreshThreshold)
                        {
                            var refreshToken = cookieCtx.Properties.GetTokenValue("refresh_token");
                            // TODO: Get this HttpClient from a factory
                            var response = await new HttpClient().RequestRefreshTokenAsync(new RefreshTokenRequest
                            {
                                Address = Configuration["OpenID:TokenEndpoint"],
                                ClientId = Configuration["OpenID:ClientID"],
                                ClientSecret = Configuration["OpenID:Secret"],
                                RefreshToken = refreshToken
                            });

                            if (!response.IsError)
                            {
                                var expiresInSeconds = response.ExpiresIn;
                                var updatedExpiresAt = DateTimeOffset.UtcNow.AddSeconds(expiresInSeconds);
                                cookieCtx.Properties.UpdateTokenValue("expires_at", updatedExpiresAt.ToString());
                                cookieCtx.Properties.UpdateTokenValue("access_token", response.AccessToken);
                                cookieCtx.Properties.UpdateTokenValue("refresh_token", response.RefreshToken);

                                // Indicate to the cookie middleware that the cookie should be remade (since we have updated it)
                                cookieCtx.ShouldRenew = true;
                            }
                            else
                            {
                                cookieCtx.RejectPrincipal();
                                await cookieCtx.HttpContext.SignOutAsync();
                            }
                        }
                    }
                };
            })
            .AddOpenIdConnect("oidc", options =>
            {
                options.SignInScheme = "Cookies";
                options.Authority = Configuration["OpenID:Authority"];
                options.RequireHttpsMetadata = true;

                options.ClientId = Configuration["OpenID:ClientID"];
                options.ClientSecret = Configuration["OpenID:Secret"];
                options.GetClaimsFromUserInfoEndpoint = true;

                // OpenID flow to use
                options.ResponseType = OpenIdConnectResponseType.Code;
                // This aligns the life of the cookie with the life of the token.
                options.UseTokenLifetime = false;
                options.SaveTokens = true;
            });

            services.AddOcelot();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseRouting();

            app.UseCookiePolicy(new CookiePolicyOptions()
            {
                Secure = CookieSecurePolicy.Always,
                HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always,
                MinimumSameSitePolicy = SameSiteMode.None
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.GetTokenFromCookie();

            app.UseEndpoints(x => x.MapControllers());

            await app.UseOcelot();
        }
    }
}
