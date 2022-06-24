namespace Profile.Domain.Diary
{
    public class DiaryRecordEntity
    {
        //UNIQUE - CourseID from Education + TaskId from Education + USER ID
        public Guid UserId { get; set; }
        public string CourseId { get; set; }
        public StudentDiaryEntity Diary { get; set; }
        public string TaskId { get; set; }

        public string ModuleId { get; set; }
        public DateTime AnswerGivenAt { get; set; }
        public float? Score { get; set; }
    }
}
