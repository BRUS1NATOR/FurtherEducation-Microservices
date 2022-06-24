using AutoMapper;
using Education.Application.Announcements.Commands;
using Education.Application.Announcements.Dto;
using Education.Domain.Announcement;

namespace Education.Application.Mappers
{
    public class AnnouncementMapper : Profile
    {
        public AnnouncementMapper()
        {
            CreateMap<EduAnnouncement, CreateAnnouncementCommand>().ReverseMap().IncludeAllDerived();
            CreateMap<EduAnnouncement, AnnouncementDto>().ReverseMap().IncludeAllDerived();
            CreateMap<EduAnnouncement, AnnouncementPreviewDto>().ReverseMap().IncludeAllDerived();
            CreateMap<EduAnnouncement, UpdateAnnouncementCommand>().ReverseMap().IncludeAllDerived();
            //         .ForMember(dest => dest.Announcements, opt => opt.MapFrom(src => src.Announcements));
        }
    }
}