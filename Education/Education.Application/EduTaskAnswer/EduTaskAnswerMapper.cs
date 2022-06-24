using AutoMapper;
using Education.Application.EduTaskAnswers.Commands;
using Education.Application.EduTasks.Dto;
using Education.Domain.EduTaskAnswers;

namespace Education.Application.Mappers
{
    public class EduTaskAnswerMapper : Profile
    {
        public EduTaskAnswerMapper()
        {
            CreateMap<EduTaskAnswer, CreateEduTaskAnswerCommand>().ReverseMap().IncludeAllDerived();
            CreateMap<EduTaskAnswer, UpdateEduTaskAnswerCommand>().ReverseMap().IncludeAllDerived();
            CreateMap<EduTaskAnswer, EduTaskAnswerDto>().ReverseMap().IncludeAllDerived();

            CreateMap<FileAnswer, AttachedFile>().ReverseMap().IncludeAllDerived();
        }
    }
}
