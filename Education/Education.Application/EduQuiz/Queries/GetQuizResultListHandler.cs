using AutoMapper;
using Education.Application.Data.Repositories;
using Education.Application.EduQuizes.Dto;
using Education.Domain.EduQuizes;
using FurtherEducation.Common.Commands;
using FurtherEducation.Common.Models;
using FurtherEducation.Common.Queries;
using MassTransit;

namespace Education.Application.EduQuizes.Queries
{
    public class GetQuizResultListHandler : IConsumer<MongoGetPagedQuery<EduQuizResultListDto>>
    {
        private readonly IEduQuizRepository _answerRepository;
        private readonly IMapper _mapper;

        public GetQuizResultListHandler(IEduQuizRepository answerRepository, IMapper mapper)
        {
            _answerRepository = answerRepository;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<MongoGetPagedQuery<EduQuizResultListDto>> context)
        {
            PagedList<EduQuiz> result = await _answerRepository.FindByParentAsync(context.Message.ParentId, context.Message.Page, context.Message.PageSize);

            await context.RespondAsync(new QueryResponse<EduQuizResultListDto>(new EduQuizResultListDto()
            {
                Answers = _mapper.Map<PagedList<EduQuizResultDto>>(result)
            }));
        }
    }
}
