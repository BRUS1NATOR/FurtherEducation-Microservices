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
    public class GetCourseByModuleHandler : IConsumer<CourseByModuleQuery>
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IMapper _mapper;

        public GetCourseByModuleHandler(ICourseRepository courseRepository, IMapper mapper)
        {
            _courseRepository = courseRepository;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<CourseByModuleQuery> context)
        {
            var course = await _courseRepository.FindByModuleAsync(MongoHelper.Parse(context.Message.ModuleId));

            if (course is null)
            {
                await context.RespondAsync(new QueryResponse<EduCourseDetailedDto>(new EduExceptionMessage("Course not found", StatusCodes.Status404NotFound)));
                return;
            }

            await context.RespondAsync(new QueryResponse<EduCourseDetailedDto>(_mapper.Map<EduCourseDetailedDto>(course)));
        }
    }
}
