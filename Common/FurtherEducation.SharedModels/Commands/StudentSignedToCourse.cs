namespace FurtherEducation.SharedModels.Commands
{
    public class StudentSignedToCourse
    {
        public Guid UserId { get; set; }
        public string CourseId { get; set; }
        public string CourseName { get; set; }
    }
}