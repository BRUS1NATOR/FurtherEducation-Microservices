using AutoMapper;
using Education.Application.Data.Repositories;
using Education.Domain.EduQuizes;
using Education.Domain.EduTests;
using FurtherEducation.Common.Commands;
using FurtherEducation.Common.Exceptions;
using FurtherEducation.Common.Helpers;
using FurtherEducation.SharedModels.Commands;
using MassTransit;
using Microsoft.AspNetCore.Http;

namespace Education.Application.EduQuizes.Commands
{
    public class FinishEduQuizCommandHandler : IConsumer<FinishEduQuizCommand>
    {
        private readonly IEduTestRepository _testRepository;
        private readonly IEduQuizRepository _quizRepository;
        private readonly IEduTestQuestionRepository _eduTestQuestionRepository;
        private readonly IMapper _mapper;
        private readonly IBus _bus;

        public FinishEduQuizCommandHandler(IEduTestRepository testRepository, IEduQuizRepository quizRepository, IEduTestQuestionRepository eduTestQuestionRepository, IBus bus, IMapper mapper)
        {
            _testRepository = testRepository;
            _quizRepository = quizRepository;
            _eduTestQuestionRepository = eduTestQuestionRepository;
            _bus = bus;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<FinishEduQuizCommand> context)
        {
            var testId = MongoHelper.Parse(context.Message.TestId);
            var userId = context.Message.UserId;

            var test = await _testRepository.FindAsync(testId);
            if (test is null)
            {
                await context.RespondAsync(new CommandResponse(new EduExceptionMessage("Test not found", StatusCodes.Status404NotFound)));
                return;
            }

            var quizResult = await _quizRepository.FindByParentAndUserAsync(testId, userId);
            if (quizResult is null)
            {
                await context.RespondAsync(new CommandResponse(new EduExceptionMessage("Quiz not found", StatusCodes.Status404NotFound)));
                return;
            }

            if (quizResult.IsFinished)
            {
                await context.RespondAsync(new CommandResponse(new EduExceptionMessage("Quiz has been finished", StatusCodes.Status409Conflict)));
                return;
            }

            quizResult.IsFinished = true;

            var timeLeft = quizResult.TimeLeft(test.TestSettings.TimeToSolve) + EduTestSettings.AdditionalTime;
            //Время вышло
            if (timeLeft <= 0)
            {
                await _quizRepository.UpdateAsync(quizResult);

                await context.RespondAsync(new CommandResponse(new EduExceptionMessage("Out of time", StatusCodes.Status409Conflict)));

                await _bus.Publish(new StudentGaveAnswer
                {
                    UserId = context.Message.UserId,
                    CourseId = test.CourseId.ToString(),
                    ModuleId = test.ModuleId.ToString(),
                    TaskId = context.Message.TestId,
                    AnswerGivenAt = DateTime.UtcNow,
                    Score = quizResult.Score
                });
                return;
            }

            //Получить список вопросов
            var questionIds = quizResult.Answers.Select(x => x.QuestionId);
            var targetQuestions = await _eduTestQuestionRepository.GetQuestionsAsync(testId, questionIds);

            //ПРОВЕРКА ВОПРОСОВ

            var quizAnswers = CalculateScore(targetQuestions, context.Message.Answers);

            quizResult.IsFinished = true;
            quizResult.Answers = quizAnswers;

            var result = await _quizRepository.UpdateAsync(quizResult);
            await context.RespondAsync(new CommandResponse(quizResult.Id.ToString()));

            await _bus.Publish(new StudentGaveAnswer
            {
                UserId = context.Message.UserId,
                CourseId = test.CourseId.ToString(),
                ModuleId = test.ModuleId.ToString(),
                TaskId = context.Message.TestId,
                AnswerGivenAt = DateTime.UtcNow,
                Score = quizResult.Score
            });
        }

        public List<QuizAnswer> CalculateScore(IEnumerable<EduTestQuestion> targetQuestions, IEnumerable<EduQuizAnswerDto> asnwers)
        {
            List<QuizAnswer> Answers = new List<QuizAnswer>();

            foreach (var targetQuestion in targetQuestions)
            {
                var match = asnwers.FirstOrDefault(x => x.QuestionId == targetQuestion.Id);
                if (match is not null)
                {
                    float answerScore = 0;
                    if (match.Answer is not null)
                    {
                        answerScore = targetQuestion.IsCorrect(match.Answer);
                    }

                    Answers.Add(new QuizAnswer()
                    {
                        Answer = match.Answer,
                        QuestionId = targetQuestion.Id,
                        Score = answerScore
                    });
                }
            }

            return Answers;
        }
    }
}