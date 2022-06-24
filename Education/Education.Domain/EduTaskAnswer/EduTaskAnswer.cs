using FurtherEducation.Common.Data.Models;
using MongoDB.Bson;

namespace Education.Domain.EduTaskAnswers
{
    public class EduTaskAnswer : Document
    {
        public ObjectId TaskId { get; set; }
        public Guid UserId { get; set; }
        public string TextAnswer { get; set; }
        public List<FileAnswer> Files { get; set; }
        public int Score { get; set; }
    }

    public class FileAnswer
    {
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string FileUrl { get; set; }
    }
}
