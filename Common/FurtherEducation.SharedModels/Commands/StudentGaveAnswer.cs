namespace FurtherEducation.SharedModels.Commands
{
    public class StudentGaveAnswer
    {
        public Guid UserId { get; set; }
        public string CourseId { get; set; }
        public string ModuleId { get; set; }
        public string TaskId { get; set; }    
        public DateTime AnswerGivenAt { get; set; }
        //Может не быть Null если тест
        public float? Score { get; set; }
    }
}