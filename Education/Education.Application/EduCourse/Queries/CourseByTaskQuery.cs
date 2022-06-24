using FurtherEducation.Common.CQRS.Queries;

namespace Education.Application.EduCourses.Queries
{
    public class CourseByTaskQuery : IQuery
    {
        public string TaskId { get; set; }
        public CourseByTaskQuery()
        {

        }
        public CourseByTaskQuery(string TaskId)
        {
            this.TaskId = TaskId;
        }
    }
}