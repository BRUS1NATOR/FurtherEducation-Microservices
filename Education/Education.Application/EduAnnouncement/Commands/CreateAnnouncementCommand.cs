﻿using FurtherEducation.Common;
using FurtherEducation.Common.CQRS.Commands;
using System.ComponentModel.DataAnnotations;

namespace Education.Application.Announcements.Commands
{
    public class CreateAnnouncementCommand : ICommand
    {
        [MongoObjectId]
        [Required]
        public string? CourseId { get; set; }
        [MongoObjectId]
        public string? ModuleId { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public string? Content { get; set; }
        [Required]
        public int Order { get; set; }
    }
}