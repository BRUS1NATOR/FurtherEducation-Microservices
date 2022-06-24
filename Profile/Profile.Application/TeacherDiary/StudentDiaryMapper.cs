using Profile.Application.Diary.Dto;
using Profile.Domain.Diary;

namespace Profile.Application.Mappers
{
    public class TeacherDiaryMapper : AutoMapper.Profile
    {
        public TeacherDiaryMapper()
        {
            CreateMap<TeacherDiaryEntity, TeacherDiaryDto>().ReverseMap().IncludeAllDerived();
        }
    }
}