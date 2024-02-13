using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.DTOs
{
    public class RefreshTokenRequestDTO
    {
        public string ExpirationToken { get; set; }
        public string RefreshToken { get; set; }

    }

}
