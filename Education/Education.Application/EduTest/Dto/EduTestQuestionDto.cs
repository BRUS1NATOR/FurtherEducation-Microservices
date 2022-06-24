using Education.Application.EduQuizes.Commands;
using Education.Application.EduQuizes.Dto;

namespace Education.Application.EduTests.Dto
{
    public class EduQuizFinishDto
    {
        public int TimeLeft { get; set; }
        public bool IsFinished { get; set; }
        public float Score => UserAnswers.Sum(x => x.Score) / UserAnswers.Count;
        public List<EduQuizAnswerDto> UserAnswers { get; set; }
        public EduTestQuestionListDto Questions { get; set; }
    }
}