using AutoMapper;
using Education.Application.Data.Repositories;
using Education.Domain.EduTasks;
using FurtherEducation.Common.Commands;
using FurtherEducation.Common.Exceptions;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace Education.Application.EduTasks.Commands
{
    public class CreateEduTaskHandler : IConsumer<CreateEduTaskCommand>
    {
        private readonly IEduTaskRepository _taskRepository;
        private readonly IModuleRepository _moduleRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateEduTaskHandler> _logger;

        public CreateEduTaskHandler(IModuleRepository moduleRepository, IEduTaskRepository taskRepository, IMapper mapper,
            ILogger<CreateEduTaskHandler> logger)
        {
            _taskRepository = taskRepository;
            _moduleRepository = moduleRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<CreateEduTaskCommand> context)
        {
            var module = await _moduleRepository.FindAsync(new ObjectId(context.Message.ModuleId));
            if (module == null)
            {
                await context.RespondAsync(new CommandResponse(new EduExceptionMessage("Parent module not found", StatusCodes.Status404NotFound)));
                return;
            }

            var entity = _mapper.Map<EduTask>(context.Message);
            entity.CourseId = module.CourseId;
            entity.Id = ObjectId.GenerateNewId(DateTime.UtcNow);

            var task = await _taskRepository.Create(entity);

            await context.RespondAsync(new CommandResponse(entity.Id.ToString()));
        }
    }
}
