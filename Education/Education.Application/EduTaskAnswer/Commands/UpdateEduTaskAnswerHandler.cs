using AutoMapper;
using Education.Application.Data.Repositories;
using Education.Domain.EduTaskAnswers;
using FurtherEducation.Common.Commands;
using FurtherEducation.Common.Exceptions;
using FurtherEducation.Common.Helpers;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Education.Application.EduTaskAnswers.Commands
{
    public class UpdateEduTaskAnswerHandler : IConsumer<UpdateEduTaskAnswerCommand>
    {
        private readonly IEduTaskAnswerRepository _answerRepository;
        private readonly IEduTaskAnswerRepository _taskAnswerRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateEduTaskAnswerHandler> _logger;

        public UpdateEduTaskAnswerHandler(IEduTaskAnswerRepository answerRepository, IEduTaskAnswerRepository taskAnswerRepository, IEduTestRepository testRepository, IMapper mapper,
            ILogger<CreateEduTaskAnswerHandler> logger)
        {
            _answerRepository = answerRepository;
            _taskAnswerRepository = taskAnswerRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<UpdateEduTaskAnswerCommand> context)
        {
            var old_entity = await _taskAnswerRepository.FindByParentAndUserAsync(MongoHelper.Parse(context.Message.TaskId), context.Message.UserId);

            if (old_entity is null)
            {
                await context.RespondAsync(new CommandResponse(new EduExceptionMessage("Answer not found", StatusCodes.Status404NotFound)));
                return;
            }

            var new_entity = _mapper.Map<EduTaskAnswer>(context.Message);

            new_entity.Id = old_entity.Id;
            new_entity.UserId = old_entity.UserId;

            var result = await _taskAnswerRepository.UpdateAsync(new_entity);

            await context.RespondAsync(new CommandResponse(new_entity.Id.ToString()));
        }
    }
}
