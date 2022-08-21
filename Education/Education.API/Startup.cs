using AutoMapper;
using Education.Application.Data.Context;
using Education.Application.Data.Repositories;
using Education.Application.EduCourses.Validators;
using Education.Domain.EduCourses;
using Education.Domain.EduTests;
using Education.SeedWork;
using FluentValidation;
using FurtherEducation.Common.Exceptions;
using FurtherEducation.Common.Mediator;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using static System.Net.Mime.MediaTypeNames;

namespace Education.API
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
            services.AddControllers().AddJsonOptions(options =>
            {
                // Use the default property (Pascal) casing.
                //                options.JsonSerializerOptions.PropertyNamingPolicy = null;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                // Configure a custom converter.
                options.JsonSerializerOptions.Converters.Add(new JsonObjectIdConverterBySystemTextJson());
            });

            // var ass = AppDomain.CurrentDomain.GetAssemblies().Select(x => x.GetName().Name);
            Assembly applicationAssembly = Assembly.GetAssembly(typeof(Education.Application.Mappers.EduProfile));
            var profiles = applicationAssembly.GetTypes().
                Where(type => typeof(Profile).IsAssignableFrom(type));

            //add profiles to config
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

            services.AddScoped<ICourseContext, CourseContext>();

            services.AddScoped<ICourseRepository, CourseRepository>();

            services.AddScoped<IAnnouncementRepository, AnnouncementRepository>();

            services.AddScoped<IModuleRepository, ModuleRepository>();

            services.AddScoped<IEduTaskRepository, EduTaskRepository>();
            services.AddScoped<IEduTaskAnswerRepository, EduTaskAnswerRepository>();

            services.AddScoped<IEduTestRepository, EduTestRepository>();
            services.AddScoped<IEduTestQuestionRepository, EduTestQuestionRepository>();
            services.AddScoped<IEduQuizRepository, EduQuizRepository>();

            services.AddTransient<AbstractValidator<EduCourse>, CourseValidator>();

            services.AddMediator(configurator =>
            {
                configurator.AddConsumers(applicationAssembly);
                //configurator.ConfigureMediator((context, mcfg) =>
                //{
                //    mcfg.UseConsumeFilter(typeof(CreateCourseValidationFilter<>), context);
                //});
            });

            services.AddMassTransit(x =>
            {
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
            // services.AddSingleton<IHostedService, BusBackgroundService>();
            services.AddScoped<IEduMediator, EduMediator>();

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
                    //т.к. сервер авторизации находится на другом домене
                    //отключаем проверку доменного имени
                    ValidateAudience = false
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("TeacherPolicy", policy =>
                    policy.Requirements.Add(new TeacherRequirement()));

                options.AddPolicy("StudentPolicy", policy =>
                    policy.Requirements.Add(new StudentRequirement()));

                options.AddPolicy("StudentOrTeacherPolicy", policy =>
                    policy.Requirements.Add(new StudentOrTeacherRequirement()));
            });

            services.AddSingleton<IAuthorizationHandler, CourseTeacherAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, CourseStudentAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, CourseStudentOrTeacherAuthorizationHandler>();

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
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Eduacation.API", Version = "v1" });
            });

            var sp = services.BuildServiceProvider();
            SeedData.EnsureDataCreated(sp);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Eduacation.API v1"));

            app.UseExceptionHandler(exceptionHandlerApp =>
            {
                exceptionHandlerApp.Run(async context =>
                {
                    // using static System.Net.Mime.MediaTypeNames;
                    context.Response.ContentType = Text.Plain;

                    //await context.Response.WriteAsync("An exception was thrown.");

                    var exceptionHandlerPathFeature =
                        context.Features.Get<IExceptionHandlerPathFeature>();

                    if (exceptionHandlerPathFeature?.Error is EduException eduException)
                    {
                        context.Response.StatusCode = eduException.EduExceptionMessage.Code;
                        await context.Response.WriteAsync(eduException.EduExceptionMessage.Message);
                    }
                    else if (exceptionHandlerPathFeature?.Error != null)
                    {
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        await context.Response.WriteAsync(exceptionHandlerPathFeature?.Error?.Message + "\n" + exceptionHandlerPathFeature?.Error.StackTrace);

                        var innerException = exceptionHandlerPathFeature?.Error?.InnerException;
                        while (innerException is not null)
                        {
                            await context.Response.WriteAsync("\n" + innerException.Message + "\n" + innerException.StackTrace);
                            innerException = innerException.InnerException;
                        }
                    }
                });
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(x => x.MapControllers());
        }
    }
}
