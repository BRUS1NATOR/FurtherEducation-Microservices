using AutoMapper;
using Education.Application.Data.Repositories;
using Education.Domain.EduCourses;
using FluentValidation;
using FurtherEducation.Common.Commands;
using FurtherEducation.Common.Exceptions;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Education.Application.EduCourses.Commands
{
    public class UpdateCourseHandler : IConsumer<UpdateCourseCommand>
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IMapper _mapper;
        private readonly AbstractValidator<EduCourse> _validator;
        private readonly ILogger<UpdateCourseHandler> _logger;

        public UpdateCourseHandler(ICourseRepository courseRepository, IMapper mapper, AbstractValidator<EduCourse> validator,
            ILogger<UpdateCourseHandler> logger)
        {
            _courseRepository = courseRepository;
            _mapper = mapper;
            _logger = logger;
            _validator = validator;
        }

        public async Task Consume(ConsumeContext<UpdateCourseCommand> context)
        {
            var course = _mapper.Map<EduCourse>(context.Message);

            var validation = await _validator.ValidateAsync(course);

            if (!validation.IsValid)
            {
                await context.RespondAsync(new CommandResponse(ValidationExceptionMessage.CreateExceptionMessage(validation)));
                return;
            }

            var result = await _courseRepository.UpdateAsync(course);
            if (result is false)
            {
                await context.RespondAsync(new CommandResponse(new EduExceptionMessage("Course not found", StatusCodes.Status404NotFound)));
                return;
            }
            await context.RespondAsync(new CommandResponse(context.Message.Id));
        }
    }
}
