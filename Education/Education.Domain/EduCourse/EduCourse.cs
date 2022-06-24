using FurtherEducation.Common.Data.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Education.Domain.EduCourses
{
    public class EduCourse : Document
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Speciality { get; set; }
        public int Hours { get; set; }
        public int TotalMembers { get; set; }
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid Teacher { get; set; }
        public string Image { get; set; }

        //Only proffesor and admins can see
        public List<Guid> Students { get; set; }

        //public List<string> Users { get; set; }
    }

    //public class StudentGuid
    //{
    //    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    //    public Guid Id { get; set; }
    //}
}
