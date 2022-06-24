using AutoMapper;
using Education.Application.Data.Repositories;
using Education.Domain.EduTasks;
using FurtherEducation.Common.Commands;
using FurtherEducation.Common.Exceptions;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Education.Application.EduTasks.Commands
{
    public class UpdateEduTaskHandler : IConsumer<UpdateEduTaskCommand>
    {
        private readonly IEduTaskRepository _taskRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateEduTaskHandler> _logger;

        public UpdateEduTaskHandler(IEduTaskRepository taskRepository, IMapper mapper,
            ILogger<UpdateEduTaskHandler> logger)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<UpdateEduTaskCommand> context)
        {
            var result = await _taskRepository.UpdateAsync(_mapper.Map<EduTask>(context.Message));

            if (result is false)
            {
                await context.RespondAsync(new CommandResponse(new EduExceptionMessage("Task not found", StatusCodes.Status404NotFound)));
                return;
            }

            await context.RespondAsync(new CommandResponse(context.Message.Id));
        }
    }
}
