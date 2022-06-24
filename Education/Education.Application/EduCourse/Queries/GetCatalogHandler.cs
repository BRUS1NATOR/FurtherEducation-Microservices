using AutoMapper;
using Education.Application.Data.Repositories;
using Education.Application.EduCourses.Dto;
using FurtherEducation.Common.Commands;
using FurtherEducation.Common.Models;
using FurtherEducation.Common.Queries;
using Education.Domain.EduCourses;
using MassTransit;

namespace Education.Application.EduCourses.Queries
{
    public class GetCatalogHandler : IConsumer<MongoGetPagedQuery<CourseCatalogDto>>
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IMapper _mapper;

        public GetCatalogHandler(ICourseRepository courseRepository, IMapper mapper)
        {
            _courseRepository = courseRepository;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<MongoGetPagedQuery<CourseCatalogDto>> context)
        {
            PagedList<EduCourse> result = await _courseRepository.FindAsync(context.Message.Search,
                    context.Message.Page, context.Message.PageSize);

            await context.RespondAsync(new QueryResponse<CourseCatalogDto>(new CourseCatalogDto
            {
                Courses = _mapper.Map<PagedList<EduCoursePreviewDto>>(result)
            }));
        }
    }
}
