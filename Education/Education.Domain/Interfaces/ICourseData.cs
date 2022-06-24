using FurtherEducation.Common.Data.Models;
using MongoDB.Bson;

namespace Education.Domain
{
    public interface ICourseData : IDocument
    {
        public ObjectId CourseId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public int Order { get; set; }
    }
}