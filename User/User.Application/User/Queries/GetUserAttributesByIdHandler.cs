using AutoMapper;
using FurtherEducation.Common.Commands;
using FurtherEducation.Common.Exceptions;
using MassTransit;
using User.Domain.Data.Interfaces;
using User.Domain.Dto;
using User.Application.User.Queries;

namespace User.Domain.Handlers.Queries
{
    public class GetUserAttributesByIdHandler : IConsumer<GetUserAttributesByIdQuery>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUserAttributesByIdHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<GetUserAttributesByIdQuery> context)
        {
            var attributes = await _userRepository.GetAttributesAsync(context.Message.UserId);

            if (attributes is null)
            {
                await context.RespondAsync(new QueryResponse<AttributeDto>(new EduExceptionMessage($"UserEntity '{context.Message.UserId}' not found", 404)));
            }

            await context.RespondAsync(new QueryResponse<AttributeListDto>(new AttributeListDto(_mapper.Map<List<AttributeDto>>(attributes))));
        }
    }
}