using AutoMapper;
using Education.Application.Data.Repositories;
using Education.Domain.EduTasks;
using FurtherEducation.Common.Commands;
using FurtherEducation.Common.Exceptions;
using MassTransit;
using Microsoft.AspNetCore.Http;

namespace Education.Application.EduTasks.Commands
{
    public class DeleteEduTaskHandler : IConsumer<MongoDeleteCommand<EduTask>>
    {
        private readonly IEduTaskRepository _taskRepository;
        private readonly IMapper _mapper;

        public DeleteEduTaskHandler(IEduTaskRepository taskRepository, IMapper mapper)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<MongoDeleteCommand<EduTask>> context)
        {
            var success = await _taskRepository.DeleteAsync(context.Message.Id);

            if (!success)
            {
                await context.RespondAsync(new CommandResponse(new EduExceptionMessage("Task not found", StatusCodes.Status404NotFound)));
                return;
            }

            await context.RespondAsync(new CommandResponse());
        }
    }
}
