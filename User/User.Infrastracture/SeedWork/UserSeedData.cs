using MassTransit.Mediator;
using Microsoft.Extensions.DependencyInjection;
using User.Domain.Data.Interfaces;
namespace User.Domain.Data
{
    public class UserSeedData
    {
        public static async void EnsureSeedData(IServiceCollection services)
        {
            await using (var serviceProvider = services.BuildServiceProvider())
            {
                using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var mediator = scope.ServiceProvider.GetService<IMediator>();

                    var userMgr = scope.ServiceProvider.GetRequiredService<IUserRepository>();

                    //if (await userMgr.FindByUsernameAsync("Test") == null)
                    //{
                    //    var user = new CreateUserDto()
                    //    {
                    //        Username = "Test",
                    //        Password = "Pass",
                    //        Firstname = "Name",
                    //        Lastname = "Lastname",
                    //        Email = "Test@email.com",
                    //        EmailConfirmed = true
                    //    };

                    //    var response = await mediator.CreateRequestClient<CreateUserCommand>()
                    //        .GetResponse<ValidationExceptionMessage, UserResponse>(message: new() { User = user });
                    //}
                }
            }
        }
    }
}