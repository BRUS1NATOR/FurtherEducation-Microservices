using FurtherEducation.Common.CQRS.Queries;

namespace Education.Application.EduCourses.Queries
{
    public class CourseByTestQuery : IQuery
    {
        public string TestId { get; set; }
        public CourseByTestQuery()
        {

        }
        public CourseByTestQuery(string TestId)
        {
            this.TestId = TestId;
        }
    }
}