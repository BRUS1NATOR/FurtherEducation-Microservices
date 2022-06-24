using AutoMapper;
using Education.Application.EduCourses.Commands;
using Education.Application.EduCourses.Dto;
using Education.Domain.EduCourses;

namespace Education.Application.Mappers
{
    public class CourseMapper : Profile
    {
        public CourseMapper()
        {
            //COURSE PREVIEWS
            CreateMap<EduCourse, EduCoursePreviewDto>().ReverseMap();
            CreateMap<EduCourse, CreateCourseCommand>().ReverseMap();
            CreateMap<EduCourse, UpdateCourseCommand>().ReverseMap();
            CreateMap<EduCourse, EduCourseDetailedDto>().ReverseMap().IncludeAllDerived();
            CreateMap<EduCourseDetailed, EduCourseDetailedDto>().ReverseMap().IncludeAllDerived();
        }
    }
}
