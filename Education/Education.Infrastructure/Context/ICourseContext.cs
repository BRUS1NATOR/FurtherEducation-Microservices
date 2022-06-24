using Education.Domain.Announcement;
using Education.Domain.EduCourses;
using Education.Domain.EduModules;
using Education.Domain.EduQuizes;
using Education.Domain.EduTaskAnswers;
using Education.Domain.EduTasks;
using Education.Domain.EduTests;
using MongoDB.Driver;

namespace Education.Application.Data.Context
{
    public interface ICourseContext
    {
        public IMongoCollection<EduCourse> Courses { get; set; }
        public IMongoCollection<EduAnnouncement> Announcements { get; set; }
        public IMongoCollection<EduModule> Modules { get; set; }
        public IMongoCollection<EduTask> EduTasks { get; set; }
        public IMongoCollection<EduTaskAnswer> EduTaskAnswers { get; set; }
        public IMongoCollection<EduTest> EduTests { get; set; }
        public IMongoCollection<EduQuiz> EduTestResult { get; set; }
    }
}
