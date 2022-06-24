using FurtherEducation.Common.Data.Models;
using MongoDB.Bson;

namespace Education.Domain.EduModules
{
    public class EduModule : Document, ICourseData
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public int Order { get; set; }
        public ObjectId CourseId { get; set; }
    }
}
