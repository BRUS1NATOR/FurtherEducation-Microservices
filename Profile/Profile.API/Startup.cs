using AutoMapper;
using FurtherEducation.Common.Mediator;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Profile.Application.Profile.Commands;
using Profile.Domain.Diary.Repository;
using Profile.Infrastracture;
using Profile.Infrastracture.Extensions;
using Profile.Infrastracture.Diary;
using Profile.Infrastracture.Profile;
using System.Reflection;
using User.Domain.Profile.Repository;
using Profile.Application.Profile;

namespace Profile.API
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
            Assembly applicationAssembly = Assembly.GetAssembly(typeof(StudentSignedToCourseHandler));

            var profiles = applicationAssembly.GetTypes().
                Where(type => typeof(AutoMapper.Profile).IsAssignableFrom(type));

            //add profiles to config
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                // Magic should happen here
                foreach (var profile in profiles)
                {
                    var resolvedProfile = System.Activator.CreateInstance(profile) as AutoMapper.Profile;
                    cfg.AddProfile(profile);
                }
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddTransient<IEduMediator, EduMediator>();

            services.AddProfileContext(Configuration);

            services.AddTransient<IProfileRepository, ProfileRepository>();
            services.AddTransient<ITeacherDiaryRepository, TeacherDiaryRepository>();
            services.AddTransient<IStudentDiaryRepository, StudentDiaryRepository>();
            services.AddTransient<IDiaryRecordRepository, StudentDiaryRecordRepository>();

            services.AddTransient<IProfileService, ProfileService>();

            services.AddMediator(configurator =>
            {
                configurator.AddConsumers(applicationAssembly);
            });

            services.AddMassTransit(x =>
            {
                x.AddConsumer<StudentSignedToCourseHandler>();
                x.AddConsumer<StudentGaveAnswerHandler>();
                x.AddConsumer<TeacherCreatedCourseHandler>();

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
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Profile.API", Version = "v1" });
            });

            services.AddHostedService<ProfileMigrationService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Profile.API v1"));

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(x => x.MapControllers());
        }
    }
}
