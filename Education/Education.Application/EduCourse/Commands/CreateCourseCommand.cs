using FurtherEducation.Common.CQRS.Commands;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Education.Application.EduCourses.Commands
{
    public class CreateCourseCommand : ICommand
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public string? Speciality { get; set; }
        [JsonIgnore]
        public Guid Teacher { get; set; }
        [Required]
        public int Hours { get; set; }
        [Required]
        public string Image { get; set; }
    }
}
