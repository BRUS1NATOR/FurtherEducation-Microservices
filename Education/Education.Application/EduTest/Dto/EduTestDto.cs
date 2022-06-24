using Education.Application.EduTasks.Dto;
using Education.Domain.EduTests;

namespace Education.Application.EduTests.Dto
{
    public class EduTestDto : EduTaskDto
    {
        public EduTestSettings TestSettings { get; set; }
    }
}
