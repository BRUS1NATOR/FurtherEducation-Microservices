using System.Text.Json.Serialization;

namespace Education.Application.EduQuizes.Dto
{
    public class EduQuizInProgressDto
    {
        public string? Id { get; set; }
        public string? TestId { get; set; }
        [JsonIgnore]
        public Guid UserId { get; set; }
        public int TimeLeft { get; set; }
        public EduTestQuestionListDto Questions { get; set; }
    }

    public class EduTestQuestionListDto
    {
        public List<EduTestQuestionDto> Questions { get; set; }
    }

    public class EduTestQuestionDto
    {
        public string? Id { get; set; }
        public string Question { get; set; }
        public List<EduTestAnswerVariantDto> Variants { get; set; }
        public bool MultipleAnswers { get; set; }
    }

    public class EduTestAnswerVariantDto
    {
        public int Id { get; set; }
        public string? Answer { get; set; }
    }
}
