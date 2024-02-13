using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.DTOs
{
    public class TokenAuthorizationResponseDTO
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public bool Resul { get; set; }
        public string Message { get; set; }

    }

}
