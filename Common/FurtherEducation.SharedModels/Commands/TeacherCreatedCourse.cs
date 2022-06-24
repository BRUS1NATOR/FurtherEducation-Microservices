namespace FurtherEducation.SharedModels.Commands
{
    public class TeacherCreatedCourse
    {
        public Guid UserId { get; set; }
        public string CourseId { get; set; }
        public string CourseName { get; set; }
    }
}