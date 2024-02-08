using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.DTOs
{
    public class UserInformationDTO
    {
        public int UserId { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public int? Age { get; set; }

        public string? Email { get; set; }

        public string? UserPassword { get; set; }

        public int? IsActive { get; set; }

    }
}
