using AutoMapper;
using Education.Application.EduQuizes.Dto;
using Education.Domain.EduTests;
using FurtherEducation.Common.Commands;
using FurtherEducation.Common.Exceptions;
using FurtherEducation.Common.Queries;
using MassTransit;
using Microsoft.AspNetCore.Http;

namespace Education.Application.EduTests.Queries
{
    public class GetQuestionHandler : IConsumer<MongoGetPagedQuery<EduTestQuestionListDto>>
    {
        private readonly IEduTestQuestionRepository _testQuestionRepository;
        private readonly IMapper _mapper;

        public GetQuestionHandler(IEduTestQuestionRepository testQuestionRepository, IMapper mapper)
        {
            _testQuestionRepository = testQuestionRepository;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<MongoGetPagedQuery<EduTestQuestionListDto>> context)
        {
            var result = await _testQuestionRepository.GetQuestionsAsync(context.Message.ParentId);

            if (result is null)
            {
                await context.RespondAsync(new QueryResponse<EduTestQuestionListDto>(new EduExceptionMessage("Test not found", StatusCodes.Status404NotFound)));
                return;
            }

            await context.RespondAsync(new QueryResponse<EduTestQuestionListDto>(new EduTestQuestionListDto
            {
                Questions = _mapper.Map<List<EduTestQuestionDto>>(result.Take(context.Message.PageSize))
            }));
        }
    }
}
