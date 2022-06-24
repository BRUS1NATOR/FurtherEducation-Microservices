using FurtherEducation.Common.CQRS.Queries;

namespace Education.Application.EduCourses.Queries
{
    public class CourseByModuleQuery : IQuery
    {
        public string ModuleId { get; set; }
        public CourseByModuleQuery()
        {

        }
        public CourseByModuleQuery(string ModuleId)
        {
            this.ModuleId = ModuleId;
        }
    }
}