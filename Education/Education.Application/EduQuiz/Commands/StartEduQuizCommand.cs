using FurtherEducation.Common;
using FurtherEducation.Common.CQRS.Commands;
using MongoDB.Bson;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Education.Application.EduQuizes.Commands
{
    public class StartEduQuizCommand : ICommand
    {
        [JsonIgnore]
        public ObjectId Id { get; set; }
        [MongoObjectId]
        [Required]
        public string TestId { get; set; }
        [JsonIgnore]
        public Guid UserId { get; set; }
    }
}
