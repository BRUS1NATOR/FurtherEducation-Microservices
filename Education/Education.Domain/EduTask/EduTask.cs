using Education.Domain.Announcement;
using Education.Domain.EduCourses;
using Education.Domain.Enum;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace Education.Domain.EduTasks
{
    public class EduTask : EduAnnouncement, ICourseData
    {
        public List<ObjectId> StudentAnswers { get; set; }
        public DateTime ExpirationDate { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]  // System.Text.Json.Serialization
        [BsonRepresentation(BsonType.String)]
        public AnswerType AnswerType { get; set; }

        public EduCourse[] courses { get; set; }
    }

    public class EduTaskCourseAggregation : EduTask
    {
        public EduCourse[] Courses { get; set; }
    }
}