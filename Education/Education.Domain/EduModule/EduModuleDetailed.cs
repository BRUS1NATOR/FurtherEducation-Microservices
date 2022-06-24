using Education.Domain.EduTasks;
using Education.Domain.EduTests;

namespace Education.Domain.EduModules
{
    public class EduModuleDetailed : EduModule
    {
        public List<Announcement.EduAnnouncement> Announcements { get; set; }
        public List<EduTask> Tasks { get; set; }
        public List<EduTest> Tests { get; set; }
    }
}
