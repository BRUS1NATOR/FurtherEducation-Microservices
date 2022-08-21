using AutoMapper;
using FurtherEducation.Common.Mediator;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Linq;
using System.Reflection;
using User.Domain.Data.Interfaces;
using User.Domain.Data.Repositories;
using User.Domain.Handlers.Queries;
using User.Infrastracture.Extensions;

namespace User.Domain
{
    public class Startup
    {
        private readonly IConfiguration Configuration;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // add services to the DI container
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            Assembly applicationAssembly = Assembly.GetAssembly(typeof(global::User.Application.User.UserMapper));
            var profiles = applicationAssembly.GetTypes().
                Where(type => typeof(Profile).IsAssignableFrom(type));
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                // Magic should happen here
                foreach (var profile in profiles)
                {
                    var resolvedProfile = System.Activator.CreateInstance(profile) as Profile;
                    cfg.AddProfile(profile);
                }
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddUserContext(Configuration);

            services.AddTransient<IEduMediator, EduMediator>();

            services.AddTransient<IUserRepository, UserRepository>();

            services.AddMediator(configurator =>
            {
                configurator.AddConsumers(applicationAssembly);
            });

            services.AddMassTransit(x =>
            {
                x.AddConsumer<GetUserFullNameHandler>();

                x.SetKebabCaseEndpointNameFormatter();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("rabbit", 5672, "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o =>
            {
                o.Authority = "https://keycloak.local.dev/auth/realms/EDU";

                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false
                };
            });

            //  UserSeedData.EnsureSeedData(services);
            services.AddSwaggerGen(options =>
            {

                OpenApiSecurityScheme securityDefinition = new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    BearerFormat = "JWT",
                    Scheme = "bearer",
                    Description =
                        "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                };

                OpenApiSecurityRequirement securityRequirements = new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                };

                options.AddSecurityDefinition("Bearer", securityDefinition);
                options.AddSecurityRequirement(securityRequirements);

                options.EnableAnnotations();
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Users.API", Version = "v1" });
            });
        }

        // configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Users.API v1"));

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(x => x.MapControllers());
        }
    }
}
