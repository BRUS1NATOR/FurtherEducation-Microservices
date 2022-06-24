using Education.Domain.EduCourses;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Education.Domain.Aggregation
{

    public class EduChildOfCourseProjection
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public ObjectId CourseId { get; set; }
    }

    public class EduChildOfCourseAggregation : EduChildOfCourseProjection
    {
        public EduCourse[] Courses { get; set; }
    }
}
