namespace Profile.Application.Diary.Dto
{
    public class StudentDiaryRecordDto
    {
        public Guid UserId { get; set; }
        public string CourseId { get; set; }
        public string ModuleId { get; set; }
        public string TaskId { get; set; }
    
        public DateTime AnswerGivenAt { get; set; }
        public float? Score { get; set; }
    }
}