using AutoMapper;
using Education.Application.Data.Repositories;
using FurtherEducation.Common.Commands;
using FurtherEducation.Common.Exceptions;
using FurtherEducation.SharedModels.Commands;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace Education.Application.EduCourses.Commands
{
    public class SignUpUserToCourseHandler : IConsumer<SignUpUserToCourseCommand>
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<SignUpUserToCourseHandler> _logger;
        private readonly IBus _bus;

        public SignUpUserToCourseHandler(ICourseRepository courseRepository, IMapper mapper, IBus publishEndpoint,
            ILogger<SignUpUserToCourseHandler> logger)
        {
            _courseRepository = courseRepository;
            _bus = publishEndpoint;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<SignUpUserToCourseCommand> context)
        {
            var course = await _courseRepository.FindDetailedAsync(ObjectId.Parse(context.Message.CourseId));
            if (course is null)
            {
                await context.RespondAsync(new CommandResponse(new EduExceptionMessage("Course not found", StatusCodes.Status404NotFound)));
                return;
            }
            course.Students.Add(context.Message.UserId);
            var result = await _courseRepository.UpdateAsync(course);

            await context.RespondAsync(new CommandResponse());

            await _bus.Publish(new StudentSignedToCourse
            {
                UserId = context.Message.UserId,
                CourseId = context.Message.CourseId,
                CourseName = course.Name
            });
        }
    }
}
