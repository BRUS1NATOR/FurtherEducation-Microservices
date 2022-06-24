using AutoMapper;
using Education.Application.Data.Repositories;
using Education.Application.EduTasks.Dto;
using FurtherEducation.Common.Commands;
using FurtherEducation.Common.Exceptions;
using FurtherEducation.Common.Queries;
using MassTransit;
using Microsoft.AspNetCore.Http;

namespace Education.Application.EduTaskAsnwers.Queries
{
    public class GetUserTaskAnswerHandler : IConsumer<MongoGetQuery<EduTaskAnswerDto>>
    {
        private readonly IEduTaskAnswerRepository _answerRepository;
        private readonly IMapper _mapper;

        public GetUserTaskAnswerHandler(IEduTaskAnswerRepository answerRepository, IMapper mapper)
        {
            _answerRepository = answerRepository;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<MongoGetQuery<EduTaskAnswerDto>> context)
        {
            var result = await _answerRepository.FindAsync(context.Message.Id);

            if (result is null)
            {
                await context.RespondAsync(new QueryResponse<EduTaskAnswerDto>(new EduExceptionMessage("Task answer not found", StatusCodes.Status404NotFound)));
                return;
            }

            await context.RespondAsync(new QueryResponse<EduTaskAnswerDto>(_mapper.Map<EduTaskAnswerDto>(result)));
        }
    }
}
