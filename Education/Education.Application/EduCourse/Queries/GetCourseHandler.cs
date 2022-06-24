using AutoMapper;
using Education.Application.Data.Repositories;
using Education.Application.EduCourses.Dto;
using FurtherEducation.Common.Commands;
using FurtherEducation.Common.Exceptions;
using FurtherEducation.Common.Queries;
using MassTransit;
using Microsoft.AspNetCore.Http;

namespace Education.Application.EduCourses.Queries
{
    public class GetCourseHandler : IConsumer<MongoGetQuery<EduCourseDetailedDto>>
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IMapper _mapper;

        public GetCourseHandler(ICourseRepository courseRepository, IMapper mapper)
        {
            _courseRepository = courseRepository;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<MongoGetQuery<EduCourseDetailedDto>> context)
        {
            var course = await _courseRepository.FindDetailedAsync(context.Message.Id);

            if (course is null)
            {
                await context.RespondAsync(new QueryResponse<EduCourseDetailedDto>(new EduExceptionMessage("Course not found", StatusCodes.Status404NotFound)));
                return;
            }

            await context.RespondAsync(new QueryResponse<EduCourseDetailedDto>(_mapper.Map<EduCourseDetailedDto>(course)));
        }
    }
}
