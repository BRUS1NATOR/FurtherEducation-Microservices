using AutoMapper;
using FurtherEducation.Common.Commands;
using FurtherEducation.Common.Exceptions;
using MassTransit;
using User.Domain.Data.Interfaces;
using User.Domain.Dto;
using User.Application.User.Queries;
using FurtherEducation.SharedModels.Commands;

namespace User.Domain.Handlers.Queries
{
    public class GetUserFullNameHandler : IConsumer<GetUserFullNameRequest>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUserFullNameHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<GetUserFullNameRequest> context)
        {
            var user = await _userRepository.FindByIdAsync(context.Message.UserId);

            if (user is null)
            {
                await context.RespondAsync(new QueryResponse<UserFullNameDto>(new EduExceptionMessage($"UserEntity '{context.Message.UserId}' not found", 404)));
            }

            await context.RespondAsync(new QueryResponse<UserFullNameDto>(new UserFullNameDto()
            {
                FirstName = user.FirstName,
                LastName = user.LastName
            }));
        }
    }
}