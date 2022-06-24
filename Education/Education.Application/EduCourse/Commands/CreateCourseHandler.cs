using AutoMapper;
using Education.Application.Data.Repositories;
using Education.Domain.EduCourses;
using FluentValidation;
using FurtherEducation.Common.Commands;
using FurtherEducation.Common.Exceptions;
using FurtherEducation.SharedModels.Commands;
using MassTransit;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace Education.Application.EduCourses.Commands
{
    public class CreateCourseHandler : IConsumer<CreateCourseCommand>
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IMapper _mapper;
        private readonly IBus _bus;
        private readonly AbstractValidator<EduCourse> _validator;
        private readonly ILogger<CreateCourseHandler> _logger;

        public CreateCourseHandler(ICourseRepository courseRepository, IMapper mapper,
            IBus bus,
            AbstractValidator<EduCourse> validator,
            ILogger<CreateCourseHandler> logger)
        {
            _courseRepository = courseRepository;
            _mapper = mapper;
            _bus = bus;
            _logger = logger;
            _validator = validator;
        }

        public async Task Consume(ConsumeContext<CreateCourseCommand> context)
        {
            var course = _mapper.Map<EduCourse>(context.Message);
            course.Id = ObjectId.GenerateNewId(DateTime.UtcNow);
            var validation = await _validator.ValidateAsync(course);

            if (!validation.IsValid)
            {
                await context.RespondAsync(new CommandResponse(ValidationExceptionMessage.CreateExceptionMessage(validation)));
                return;
            }

            await _courseRepository.Create(course);

            await context.RespondAsync(new CommandResponse(course.Id.ToString()));

            await _bus.Publish(new TeacherCreatedCourse
            {
                UserId = context.Message.Teacher,
                CourseId = course.Id.ToString(),
                CourseName = course.Name
            });
        }
    }
}
