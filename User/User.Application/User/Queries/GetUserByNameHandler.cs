using AutoMapper;
using FurtherEducation.Common.Commands;
using FurtherEducation.Common.Exceptions;
using MassTransit;
using User.Domain.Data.Interfaces;
using User.Domain.Dto;
using User.Application.User.Queries;

namespace User.Domain.Handlers.Queries
{
    public class GetUserByNameHandler : IConsumer<GetUserByNameQuery>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUserByNameHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<GetUserByNameQuery> context)
        {
            var user = await _userRepository.FindByUsernameAsync(context.Message.Username);

            if (user is null)
            {
                await context.RespondAsync(new QueryResponse<UserDto>(new EduExceptionMessage($"UserEntity '{context.Message.Username}' not found", 404)));
            }

            await context.RespondAsync(new QueryResponse<UserDto>(_mapper.Map<UserDto>(user)));
        }
    }
}