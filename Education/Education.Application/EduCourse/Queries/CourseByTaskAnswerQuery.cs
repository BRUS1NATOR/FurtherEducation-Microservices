using FurtherEducation.Common.CQRS.Queries;

namespace Education.Application.EduCourses.Queries
{
    public class CourseByTaskAnswerQuery : IQuery
    {
        public string AnswerId { get; set; }
        public CourseByTaskAnswerQuery()
        {

        }
        public CourseByTaskAnswerQuery(string AnswerId)
        {
            this.AnswerId = AnswerId;
        }
    }
}
