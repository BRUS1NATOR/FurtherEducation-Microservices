using AutoMapper;
using Education.Application.Data.Repositories;
using Education.Application.EduTasks.Dto;
using FurtherEducation.Common.Commands;
using FurtherEducation.Common.CQRS.Queries;
using FurtherEducation.Common.Exceptions;
using MassTransit;
using Microsoft.AspNetCore.Http;

namespace Education.Application.EduTaskAsnwers.Queries
{
    public class GetUserTaskAnswerByParentAndUserHandler : IConsumer<MongoGetQueryWithUserId<EduTaskAnswerDto>>
    {
        private readonly IEduTaskAnswerRepository _answerRepository;
        private readonly IMapper _mapper;

        public GetUserTaskAnswerByParentAndUserHandler(IEduTaskAnswerRepository answerRepository, IMapper mapper)
        {
            _answerRepository = answerRepository;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<MongoGetQueryWithUserId<EduTaskAnswerDto>> context)
        {
            var result = await _answerRepository.FindByParentAndUserAsync(context.Message.Id, context.Message.UserId);

            if (result is null)
            {
                await context.RespondAsync(new QueryResponse<EduTaskAnswerDto>(new EduExceptionMessage("Task answer not found", StatusCodes.Status404NotFound)));
                return;
            }

            await context.RespondAsync(new QueryResponse<EduTaskAnswerDto>(_mapper.Map<EduTaskAnswerDto>(result)));
        }
    }
}
