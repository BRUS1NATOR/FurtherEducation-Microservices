using System.Collections.Generic;

namespace User.Domain.Dto
{
    public class UserListDto
    {
        public IEnumerable<UserDto> Users { get; set; }
    }
}
