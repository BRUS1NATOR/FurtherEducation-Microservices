using AutoMapper;
using Education.Application.Data.Repositories;
using FurtherEducation.Common.Commands;
using FurtherEducation.Common.Exceptions;
using FurtherEducation.Common.Helpers;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Education.Application.EduTaskAnswers.Commands
{
    public class RateEduTaskAnswerHandler : IConsumer<RateEduTaskAnswerCommand>
    {
        private readonly IEduTaskAnswerRepository _answerRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateEduTaskAnswerHandler> _logger;

        public RateEduTaskAnswerHandler(IEduTaskAnswerRepository answerRepository, IMapper mapper,
            ILogger<CreateEduTaskAnswerHandler> logger)
        {
            _answerRepository = answerRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<RateEduTaskAnswerCommand> context)
        {
            if (context.Message.Score < 0)
            {
                await context.RespondAsync(new CommandResponse(new EduExceptionMessage("Score must be larger than 0", StatusCodes.Status400BadRequest)));
                return;
            }

            var answer = await _answerRepository.FindAsync(MongoHelper.Parse(context.Message.Id));

            if (answer is null)
            {
                await context.RespondAsync(new CommandResponse(new EduExceptionMessage("Answer not found", StatusCodes.Status404NotFound)));
                return;
            }

            answer.Score = context.Message.Score;
            var result = await _answerRepository.UpdateAsync(answer);

            await context.RespondAsync(new CommandResponse(context.Message.Id));
        }
    }
}
