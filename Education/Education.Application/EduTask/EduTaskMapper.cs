using AutoMapper;
using Education.Application.EduTasks.Commands;
using Education.Application.EduTasks.Dto;
using Education.Domain.EduTasks;

namespace Education.Application.Mappers
{
    public class EduTaskMapper : Profile
    {
        public EduTaskMapper()
        {
            //Tasks
            CreateMap<EduTask, CreateEduTaskCommand>().ReverseMap().IncludeAllDerived();
            CreateMap<EduTask, EduTaskPreviewDto>().ReverseMap().IncludeAllDerived();
            CreateMap<EduTask, EduTaskDto>().ReverseMap().IncludeAllDerived();
            CreateMap<EduTask, UpdateEduTaskCommand>().ReverseMap().IncludeAllDerived();
        }
    }
}
