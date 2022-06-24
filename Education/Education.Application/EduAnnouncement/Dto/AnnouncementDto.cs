using Education.Domain;
using FurtherEducation.Common.Data.Models;
using MongoDB.Bson;

namespace Education.Application.Announcements.Dto
{
    public class AnnouncementDto : IEntity, ICourseData
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Content { get; set; }
        public int Order { get; set; }
        public ObjectId CourseId { get; set; }
        public ObjectId ModuleId { get; set; }
        public DateTime CreationTime { get; set; }

        ObjectId IDocument.Id { get; set; }
    }
}
