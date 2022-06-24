using AutoMapper;
using FurtherEducation.Common.Commands;
using FurtherEducation.Common.Exceptions;
using MassTransit;
using User.Application.User.Queries;
using User.Domain.Data.Interfaces;
using User.Domain.Dto;

namespace User.Domain.Handlers.Queries
{
    public class GetUserByIdHandler : IConsumer<GetUserByIdQuery>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUserByIdHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<GetUserByIdQuery> context)
        {
            var user = await _userRepository.FindByIdAsync(context.Message.Id);

            if (user is null)
            {
                await context.RespondAsync(new QueryResponse<UserDto>(new EduExceptionMessage($"UserEntity '{context.Message.Id}' not found", 404)));
            }

            await context.RespondAsync(new QueryResponse<UserDto>(_mapper.Map<UserDto>(user)));
        }
    }
}