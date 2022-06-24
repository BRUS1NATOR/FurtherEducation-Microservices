using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace FurtherEducation.Common.Data.Models
{
    public interface IDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        public DateTime CreationTime { get; }
    }

    public abstract class Document : IDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public DateTime CreationTime => Id.CreationTime;
    }
}