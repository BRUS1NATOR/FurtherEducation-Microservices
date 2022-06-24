using FurtherEducation.Common.CQRS.Queries;

namespace Education.Application.EduCourses.Queries
{
    public class CourseByAnnouncementQuery : IQuery
    {
        public string AnnouncementId { get; set; }
        public CourseByAnnouncementQuery()
        {

        }
        public CourseByAnnouncementQuery(string ModuleId)
        {
            this.AnnouncementId = ModuleId;
        }
    }
}