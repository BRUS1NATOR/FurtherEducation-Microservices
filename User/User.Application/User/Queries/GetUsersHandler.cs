using AutoMapper;
using FurtherEducation.Common.Commands;
using MassTransit;
using User.Application.User.Queries;
using User.Domain.Data.Interfaces;
using User.Domain.Dto;
using User.Domain.User;

namespace User.Domain.Handlers.Queries
{
    public class GetUsersHandler : IConsumer<GetUsersQuery>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUsersHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<GetUsersQuery> context)
        {
            IEnumerable<UserEntity> result;
            if (context.Message.PageSize != 0)
            {
                result = await _userRepository.FindAsync(
                    context.Message.Page, context.Message.PageSize);
            }
            else
            {
                result = await _userRepository.FindAsync();
            }

            await context.RespondAsync(new QueryResponse<UserListDto>(new UserListDto
            {
                Users = _mapper.Map<IEnumerable<UserDto>>(result)
            }));
        }
    }
}
