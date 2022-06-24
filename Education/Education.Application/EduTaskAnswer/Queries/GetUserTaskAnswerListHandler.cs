using AutoMapper;
using Education.Application.Data.Repositories;
using Education.Application.EduTasks.Dto;
using FurtherEducation.Common.Commands;
using FurtherEducation.Common.Models;
using FurtherEducation.Common.Queries;
using Education.Domain.EduTaskAnswers;
using MassTransit;

namespace Education.Application.EduTaskAsnwers.Queries
{
    public class GetUserTaskAnswerListHandler : IConsumer<MongoGetPagedQuery<EduTaskAnswerListDto>>
    {
        private readonly IEduTaskAnswerRepository _answerRepository;
        private readonly IMapper _mapper;

        public GetUserTaskAnswerListHandler(IEduTaskAnswerRepository answerRepository, IMapper mapper)
        {
            _answerRepository = answerRepository;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<MongoGetPagedQuery<EduTaskAnswerListDto>> context)
        {
            PagedList<EduTaskAnswer> result = await _answerRepository.FindByParentAsync(context.Message.ParentId, context.Message.Page, context.Message.PageSize);

            await context.RespondAsync(new QueryResponse<EduTaskAnswerListDto>(new EduTaskAnswerListDto()
            {
                Answers = _mapper.Map<PagedList<EduTaskAnswerDto>>(result)
            }));
        }
    }
}
