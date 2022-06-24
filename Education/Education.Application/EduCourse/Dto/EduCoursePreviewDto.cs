using FurtherEducation.Common.Models;

namespace Education.Application.EduCourses.Dto
{
    public class EduCoursePreviewDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Speciality { get; set; }
        public Guid Teacher { get; set; }
        public int Hours { get; set; }
        public int TotalMembers { get; set; }
        public string Image { get; set; }
    }

    public class CourseCatalogDto
    {
        public PagedList<EduCoursePreviewDto> Courses { get; set; }
    }
}
