using Profile.Application.Diary.Dto;
using Profile.Domain.Diary;

namespace Profile.Application.Mappers
{
    public class StudentDiaryMapper : AutoMapper.Profile
    {
        public StudentDiaryMapper()
        {
            CreateMap<StudentDiaryEntity, StudentDiaryDtoPreview>().ReverseMap().IncludeAllDerived();
            CreateMap<StudentDiaryEntity, StudentDiaryDto>().ReverseMap().IncludeAllDerived();

            CreateMap<DiaryRecordEntity, StudentDiaryRecordDto>().ReverseMap().IncludeAllDerived();
        }
    }
}