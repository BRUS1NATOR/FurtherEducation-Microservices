using System;
using System.Collections.Generic;
using User.Domain.User;

#nullable disable

namespace User.Domain.User
{
    public partial class UserAttribute
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string UserId { get; set; }
        public string Id { get; set; }

        public virtual UserEntity User { get; set; }
    }
}
