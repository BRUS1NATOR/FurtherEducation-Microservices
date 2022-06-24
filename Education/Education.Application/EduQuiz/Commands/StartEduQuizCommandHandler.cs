using AutoMapper;
using Education.Application.Data.Repositories;
using Education.Domain.EduQuizes;
using Education.Domain.EduTests;
using FurtherEducation.Common.Commands;
using FurtherEducation.Common.Exceptions;
using FurtherEducation.Common.Helpers;
using FurtherEducation.Common.Mediator;
using MassTransit;
using Microsoft.AspNetCore.Http;

namespace Education.Application.EduQuizes.Commands
{
    public class StartEduQuizCommandHandler : IConsumer<StartEduQuizCommand>
    {
        private readonly IEduTestRepository _testRepository;
        private readonly IEduQuizRepository _testResultRepository;
        private readonly IEduTestQuestionRepository _eduTestQuestionRepository;
        private readonly IMapper _mapper;
        private readonly IEduMediator _mediator;

        public StartEduQuizCommandHandler(IEduMediator mediator, IEduTestRepository testRepository,
            IEduQuizRepository testResultRepository, IEduTestQuestionRepository eduTestQuestionRepository, IMapper mapper)
        {
            _mediator = mediator;
            _testRepository = testRepository;
            _testResultRepository = testResultRepository;
            _eduTestQuestionRepository = eduTestQuestionRepository;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<StartEduQuizCommand> context)
        {
            var testId = MongoHelper.Parse(context.Message.TestId);
            var userId = context.Message.UserId;
            EduTest? test = await _testRepository.FindAsync(testId);
            if (test is null)
            {
                await context.RespondAsync(new CommandResponse(new EduExceptionMessage("Test not found", StatusCodes.Status404NotFound)));
                return;
            }

            var quiz = await _testResultRepository.FindByParentAndUserAsync(testId, userId);

            //Тест не завершен корректно
            if (quiz is not null && quiz.IsFinished == false)
            {
                var timeLeft = quiz.TimeLeft(test.TestSettings.TimeToSolve);

                //Время вышло
                if (timeLeft < 0)
                {
                    quiz.IsFinished = true;
                    await _testResultRepository.UpdateAsync(quiz);

                    await context.RespondAsync(new CommandResponse(new EduMessage("Out of time"), quiz.Id.ToString(), CommandStatus.Failed));
                    return;
                }

                //Вернуть список вопросов
                await context.RespondAsync(new CommandResponse(quiz.Id.ToString()));
                return;
            }
            if (quiz is not null && quiz.IsFinished)
            {
                if (test.TestSettings.OneTry)
                {
                    await context.RespondAsync(new CommandResponse(new EduMessage("Out of attempts"), quiz.Id.ToString()));
                    return;
                }
            }


            //Получить список вопросов
            var result = await _eduTestQuestionRepository.GetQuestionsAsync(testId);
            if (result is null || result.Count() == 0)
            {
                await context.RespondAsync(new CommandResponse(new EduMessage("No questions"), quiz.Id.ToString()));
                return;
            }
            //RND
            var questionsResponse = result.OrderBy(a => Guid.NewGuid()).Take(test.TestSettings.QuestionsAmount).ToList();

            EduQuiz eduTestResult = new EduQuiz()
            {
                Id = MongoDB.Bson.ObjectId.GenerateNewId(DateTime.UtcNow),
                TestId = testId,
                UserId = userId,
                Questions = questionsResponse,
                //Сразу создать пустой ответ пользователя с началом теста
                Answers = questionsResponse.Select(x => new QuizAnswer()
                {
                    QuestionId = x.Id,
                    Answer = new int[] { 0 },
                    Score = 0
                }).ToList()
            };

            await _testResultRepository.Create(eduTestResult);

            await context.RespondAsync(new CommandResponse(eduTestResult.Id.ToString()));
        }
    }
}
