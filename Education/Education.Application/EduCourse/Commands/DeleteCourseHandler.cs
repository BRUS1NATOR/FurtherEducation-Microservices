using AutoMapper;
using Education.Application.Data.Repositories;
using Education.Domain.EduCourses;
using FurtherEducation.Common.Commands;
using FurtherEducation.Common.Exceptions;
using MassTransit;
using Microsoft.AspNetCore.Http;

namespace Education.Application.EduCourses.Commands
{
    public class DeleteCourseHandler : IConsumer<MongoDeleteCommand<EduCourse>>
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IMapper _mapper;

        public DeleteCourseHandler(ICourseRepository courseRepository, IMapper mapper)
        {
            _courseRepository = courseRepository;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<MongoDeleteCommand<EduCourse>> context)
        {
            var success = await _courseRepository.DeleteAsync(context.Message.Id);

            if (!success)
            {
                await context.RespondAsync(new CommandResponse(new EduExceptionMessage("Course not found", StatusCodes.Status404NotFound)));
                return;
            }

            await context.RespondAsync(new CommandResponse());
        }
    }
}