using AutoMapper;
using Education.Application.Data.Repositories;
using Education.Domain.EduTaskAnswers;
using FurtherEducation.Common.Commands;
using FurtherEducation.Common.Exceptions;
using FurtherEducation.SharedModels.Commands;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace Education.Application.EduTaskAnswers.Commands
{
    public class CreateEduTaskAnswerHandler : IConsumer<CreateEduTaskAnswerCommand>
    {
        private readonly IEduTaskAnswerRepository _answerRepository;
        private readonly IEduTaskRepository _taskRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateEduTaskAnswerHandler> _logger;
        private readonly IBus _bus;

        public CreateEduTaskAnswerHandler(IEduTaskAnswerRepository answerRepository, IEduTaskRepository taskRepository,
            IBus bus, IMapper mapper, ILogger<CreateEduTaskAnswerHandler> logger)
        {
            _answerRepository = answerRepository;
            _taskRepository = taskRepository;
            _bus = bus;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<CreateEduTaskAnswerCommand> context)
        {
            var task = await _taskRepository.FindAsync(new ObjectId(context.Message.TaskId));

            if (task is null)
            {
                await context.RespondAsync(new CommandResponse(new EduExceptionMessage("Task or test not found", StatusCodes.Status404NotFound)));
                return;
            }

            var entity = _mapper.Map<EduTaskAnswer>(context.Message);
            entity.Id = ObjectId.GenerateNewId(DateTime.UtcNow);

            var answer = await _answerRepository.Create(_mapper.Map<EduTaskAnswer>(context.Message));
            await context.RespondAsync(new CommandResponse(entity.Id.ToString()));

            await _bus.Publish(new StudentGaveAnswer
            {
                UserId = context.Message.UserId,
                CourseId = task.CourseId.ToString(),
                ModuleId = task.ModuleId.ToString(),
                TaskId = context.Message.TaskId,
                AnswerGivenAt = DateTime.UtcNow
            });
        }
    }
}
