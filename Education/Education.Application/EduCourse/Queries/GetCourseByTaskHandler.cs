using AutoMapper;
using Education.Application.Data.Repositories;
using Education.Application.EduCourses.Dto;
using FurtherEducation.Common.Commands;
using FurtherEducation.Common.Exceptions;
using FurtherEducation.Common.Helpers;
using MassTransit;
using Microsoft.AspNetCore.Http;

namespace Education.Application.EduCourses.Queries
{
    public class GetCourseByTaskHandler : IConsumer<CourseByTaskQuery>
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IMapper _mapper;

        public GetCourseByTaskHandler(ICourseRepository courseRepository, IMapper mapper)
        {
            _courseRepository = courseRepository;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<CourseByTaskQuery> context)
        {
            var course = await _courseRepository.FindByTaskAsync(MongoHelper.Parse(context.Message.TaskId));

            if (course is null)
            {
                await context.RespondAsync(new QueryResponse<EduCourseDetailedDto>(new EduExceptionMessage("Course not found", StatusCodes.Status404NotFound)));
                return;
            }

            await context.RespondAsync(new QueryResponse<EduCourseDetailedDto>(_mapper.Map<EduCourseDetailedDto>(course)));
        }
    }
}
