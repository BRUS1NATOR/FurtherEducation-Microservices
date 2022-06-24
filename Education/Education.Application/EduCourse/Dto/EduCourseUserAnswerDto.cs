namespace Education.Application.EduCourses.Dto
{
    public class EduCourseUserAnswerDto
    {
        public string? AnswerId { get; set; }
        public string? TaskId { get; set; }
        public Guid UserId { get; set; }
        public List<EduCourseAnswerDto> Answers { get; set; }
        public int FinalScore { get; set; }
    }

    public class EduCourseAnswerDto
    {
        public string? StudentAnswer { get; set; }
        public int Score { get; set; }
    }
}
