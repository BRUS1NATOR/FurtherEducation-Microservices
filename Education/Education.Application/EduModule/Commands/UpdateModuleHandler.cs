using AutoMapper;
using Education.Application.Data.Repositories;
using Education.Domain.EduCourses;
using Education.Domain.EduModules;
using FluentValidation;
using FurtherEducation.Common.Commands;
using FurtherEducation.Common.Exceptions;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Education.Application.EduModules.Commands
{
    public class UpdateModuleHandler : IConsumer<UpdateModuleCommand>
    {
        private readonly IModuleRepository _moduleRepository;
        private readonly IMapper _mapper;
        private readonly AbstractValidator<EduCourse> _validator;
        private readonly ILogger<UpdateModuleHandler> _logger;

        public UpdateModuleHandler(IModuleRepository moduleRepository, IMapper mapper, AbstractValidator<EduCourse> validator,
            ILogger<UpdateModuleHandler> logger)
        {
            _moduleRepository = moduleRepository;
            _mapper = mapper;
            _logger = logger;
            _validator = validator;
        }

        public async Task Consume(ConsumeContext<UpdateModuleCommand> context)
        {
            var result = await _moduleRepository.UpdateAsync(_mapper.Map<EduModule>(context.Message));

            if (result is false)
            {
                await context.RespondAsync(new CommandResponse(new EduExceptionMessage("Module not found", StatusCodes.Status404NotFound)));
                return;
            }

            await context.RespondAsync(new CommandResponse(context.Message.Id));
        }
    }
}
