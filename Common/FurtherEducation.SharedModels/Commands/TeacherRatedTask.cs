namespace FurtherEducation.SharedModels.Commands
{
    public class TeacherRatedTask
    {
        public Guid UserId { get; set; }
        public string TaskId { get; set; }    
        public DateTime DateTime { get; set; }
        public float Score { get; set; }
    }
}