using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.DTOs
{
    public class TokenAuthorizationRequestDTO
    {
        public string? Email { get; set; }
        public string? UserPassword { get; set; }
    }
}
