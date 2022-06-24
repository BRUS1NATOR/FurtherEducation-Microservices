using System;

namespace User.Domain.Dto
{
    public class UserDeactivateDto
    {
        public Guid SubjectId { get; set; }
        public string Username { get; set; }
        public string IsActive { get; set; }
    }
}
