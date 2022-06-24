using AutoMapper;
using Education.Application.EduQuizes.Dto;
using Education.Application.EduTests.Commands;
using Education.Application.EduTests.Dto;
using Education.Domain.EduTests;

namespace Education.Application.Mappers
{
    public class EduTestMapper : Profile
    {
        public EduTestMapper()
        {
            //Tests
            CreateMap<EduTest, CreateEduTestCommand>().ReverseMap().IncludeAllDerived();
            CreateMap<EduTest, EduTestPreviewDto>().ReverseMap();
            CreateMap<EduTest, EduTestDto>().ReverseMap();
            CreateMap<EduTest, UpdateEduTestCommand>().ReverseMap().IncludeAllDerived();

            CreateMap<EduTestQuestion, EduTestQuestionDto>().ReverseMap().IncludeAllDerived();
            CreateMap<EduTestAnswerVariant, EduTestAnswerVariantDto>().ReverseMap().IncludeAllDerived();

            CreateMap<EduTestQuestionListDto, List<EduTestQuestion>>().ConvertUsing(typeof(QuestionListConverter<,>));
            CreateMap<List<EduTestQuestion>, EduTestQuestionListDto>().ForMember(x => x.Questions, opts => opts.MapFrom(s => s));
        }

        public class QuestionListConverter<TSource, TDestination> : ITypeConverter<TSource, List<TDestination>> where TSource : EduTestQuestionListDto where TDestination : EduTestQuestion
        {
            public List<TDestination> Convert(TSource source, List<TDestination> destination, ResolutionContext context)
            {
                return context.Mapper.Map<List<EduTestQuestionDto>, List<TDestination>>(source.Questions);
            }
        }
    }
}
