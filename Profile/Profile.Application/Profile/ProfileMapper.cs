using Education.Domain.Announcement;
using Profile.Application.Profile.Dto;
using Profile.Domain.Profile;

namespace Profile.Application.Mappers
{
    public class ProfileMapper : AutoMapper.Profile
    {
        public ProfileMapper()
        {
            CreateMap<ProfileEntity, ProfileDto>().ReverseMap().IncludeAllDerived();
    
            //         .ForMember(dest => dest.Announcements, opt => opt.MapFrom(src => src.Announcements));
        }
    }
}