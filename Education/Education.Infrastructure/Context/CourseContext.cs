using Education.Domain.Announcement;
using Education.Domain.EduCourses;
using Education.Domain.EduModules;
using Education.Domain.EduQuizes;
using Education.Domain.EduTaskAnswers;
using Education.Domain.EduTasks;
using Education.Domain.EduTests;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Education.Application.Data.Context
{
    public class CourseContext : ICourseContext
    {
        public IMongoCollection<EduCourse> Courses { get; set; }
        public IMongoCollection<EduAnnouncement> Announcements { get; set; }
        public IMongoCollection<EduModule> Modules { get; set; }
        public IMongoCollection<EduTask> EduTasks { get; set; }
        public IMongoCollection<EduTaskAnswer> EduTaskAnswers { get; set; }
        public IMongoCollection<EduTest> EduTests { get; set; }
        public IMongoCollection<EduQuiz> EduTestResult { get; set; }

        public CourseContext(IConfiguration settings)
        {
            //https://mongodb.github.io/mongo-csharp-driver/2.11/reference/bson/guidserialization/serializerchanges/guidserializerchanges/
            //https://stackoverflow.com/questions/63443445/trouble-with-mongodb-c-sharp-driver-when-performing-queries-using-guidrepresenta
#pragma warning disable 618
            BsonDefaults.GuidRepresentationMode = GuidRepresentationMode.V2;
            BsonDefaults.GuidRepresentation = GuidRepresentation.Standard;
#pragma warning restore

            var client = new MongoClient(settings.GetValue<string>("MongoDatabaseSettings:ConnectionString"));
            var database = client.GetDatabase(settings.GetValue<string>("MongoDatabaseSettings:DatabaseName"));

            Courses = database.GetCollection<EduCourse>(settings.GetValue<string>("MongoDatabaseSettings:CourseCollection"));
            Modules = database.GetCollection<EduModule>(settings.GetValue<string>("MongoDatabaseSettings:ModuleCollection"));

            Announcements = database.GetCollection<EduAnnouncement>(settings.GetValue<string>("MongoDatabaseSettings:AnnouncementCollection"));

            EduTasks = database.GetCollection<EduTask>(settings.GetValue<string>("MongoDatabaseSettings:EduTaskCollection"));
            EduTaskAnswers = database.GetCollection<EduTaskAnswer>(settings.GetValue<string>("MongoDatabaseSettings:EduTaskAnswerCollection"));

            EduTests = database.GetCollection<EduTest>(settings.GetValue<string>("MongoDatabaseSettings:EduTestCollection"));
            EduTestResult = database.GetCollection<EduQuiz>(settings.GetValue<string>("MongoDatabaseSettings:EduQuizCollection"));
        }
    }

    //public class GuidSerializationProvider : IBsonSerializationProvider
    //{
    //    public IBsonSerializer GetSerializer(Type type)
    //    {
    //        return type == typeof(Guid) ? new GuidSerializer(BsonType.String) : null;
    //    }
    //}
}