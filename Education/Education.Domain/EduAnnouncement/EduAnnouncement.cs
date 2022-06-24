using Education.Domain.EduCourses;
using FurtherEducation.Common.Data.Models;
using MongoDB.Bson;

namespace Education.Domain.Announcement
{
    public class EduAnnouncement : Document, ICourseData
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public int Order { get; set; }
        public ObjectId CourseId { get; set; }
        public ObjectId ModuleId { get; set; }
    }

    public class EduAnnouncementCourseAggregation : EduAnnouncement
    {
        public EduCourse[] Courses { get; set; }
    }
}