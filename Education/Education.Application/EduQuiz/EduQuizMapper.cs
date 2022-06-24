using AutoMapper;
using Education.Application.EduQuizes.Commands;
using Education.Application.EduQuizes.Dto;
using Education.Domain.EduQuizes;

namespace Education.Application.EduQuizes
{
    public class EduQuizMapper : Profile
    {
        public EduQuizMapper()
        {
            CreateMap<EduQuiz, EduQuizInProgressDto>().ReverseMap().IncludeAllDerived();
            CreateMap<EduQuiz, UpdateEduQuizResultCommand>().ReverseMap().IncludeAllDerived();

            CreateMap<EduQuiz, EduQuizResultDto>().ForMember(x => x.AnswerGivenAt, (IMemberConfigurationExpression<EduQuiz, EduQuizResultDto, object> c) => c.MapFrom(a => a.CreationTime)).IncludeAllDerived();
            CreateMap<EduQuizResultDto, EduQuizResultDto>().IncludeAllDerived();

            CreateMap<QuizAnswer, EduQuizAnswerDto>().ReverseMap().IncludeAllDerived();
        }
    }
}
