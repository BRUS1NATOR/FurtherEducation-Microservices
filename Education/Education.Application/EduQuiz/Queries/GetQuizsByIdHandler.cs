using AutoMapper;
using Education.Application.Data.Repositories;
using Education.Application.EduQuizes.Dto;
using Education.Domain.EduQuizes;
using Education.Domain.EduTests;
using FurtherEducation.Common.Commands;
using FurtherEducation.Common.Exceptions;
using MassTransit;
using Microsoft.AspNetCore.Http;

namespace Education.Application.EduQuizes.Queries
{
    public class GetQuizQueryHandler : IConsumer<GetQuizQuery>
    {
        private readonly IEduTestQuestionRepository _questionRepository;
        private readonly IEduQuizRepository _quizRepository;
        private readonly IEduTestRepository _testRepository;
        private readonly IMapper _mapper;

        public GetQuizQueryHandler(IEduTestQuestionRepository questionRepository, IEduQuizRepository answerRepository, IEduTestRepository testRepository, IMapper mapper)
        {
            _questionRepository = questionRepository;
            _quizRepository = answerRepository;
            _testRepository = testRepository;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<GetQuizQuery> context)
        {
            EduQuiz result = null;
            if (context.Message.Id is not null)
            {
                result = await _quizRepository.FindAsync(context.Message.Id.Value);
            }
            else if (context.Message.TestId is not null && context.Message.UserId is not null)
            {
                result = await _quizRepository.FindByParentAndUserAsync(context.Message.TestId.Value, context.Message.UserId.Value);
            }

            if (result is null)
            {
                await context.RespondAsync(new QueryResponse<EduQuizInProgressDto>(new EduExceptionMessage("Test answer not found", StatusCodes.Status404NotFound)));
                return;
            }

            var test = await _testRepository.FindAsync(result.TestId);
            //Получить список вопросов
            var questions = await _questionRepository.GetQuestionsAsync(result.TestId, result.Questions.Select(x => x.Id));
            var timeLeft = result.TimeLeft(test.TestSettings.TimeToSolve);

            await context.RespondAsync(new QueryResponse<EduQuizInProgressDto>(new EduQuizInProgressDto()
            {
                Id = result.Id.ToString(),
                UserId = result.UserId,
                TestId = test.Id.ToString(),
                TimeLeft = Convert.ToInt32(timeLeft),
                Questions = new EduTestQuestionListDto()
                {
                    Questions = _mapper.Map<List<EduTestQuestionDto>>(questions)
                }
            }));

            await context.RespondAsync(new QueryResponse<EduQuizInProgressDto>(_mapper.Map<EduQuizInProgressDto>(result)));
        }
    }
}
