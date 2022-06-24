using Education.Domain.Announcement;
using Education.Domain.EduModules;

namespace Education.Domain.EduCourses
{
    public class EduCourseDetailed : EduCourse
    {
        public IEnumerable<EduAnnouncement> Announcements { get; set; }
        public IEnumerable<EduModule> Modules { get; set; }
    }
}
