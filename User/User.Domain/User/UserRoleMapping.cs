using System;
using System.Collections.Generic;

#nullable disable

namespace User.Domain.User
{
    public partial class UserRoleMapping
    {
        public string RoleId { get; set; }
        public string UserId { get; set; }

        public virtual UserEntity User { get; set; }
    }
}
