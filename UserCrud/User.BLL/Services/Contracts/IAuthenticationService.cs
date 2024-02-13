using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.DTOs;

namespace User.BLL.Services.Contracts
{
    public interface IAuthenticationService
    {
        Task<TokenAuthorizationResponseDTO> ReturnToken(TokenAuthorizationRequestDTO tokenAuthorization);
        Task<TokenAuthorizationResponseDTO> ReturnRefreshToken(RefreshTokenRequestDTO refreshToken, int UserId);

    }

}
