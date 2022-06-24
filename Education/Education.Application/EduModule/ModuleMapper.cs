using AutoMapper;
using Education.Application.EduModules.Commands;
using Education.Application.EduModules.Dto;
using Education.Domain.EduModules;

namespace Education.Application.Mappers
{
    public class ModuleMapper : Profile
    {
        public ModuleMapper()
        {
            //Modules
            CreateMap<EduModule, CreateModuleCommand>().ReverseMap();
            CreateMap<EduModule, ModulePreviewDto>().ReverseMap();
            CreateMap<EduModule, ModuleDto>();
            CreateMap<EduModuleDetailed, ModuleDto>();
            CreateMap<EduModule, UpdateModuleCommand>().ReverseMap();
        }
    }
}
