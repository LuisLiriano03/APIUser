using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using User.BLL.Services.Contracts;
using User.DTOs;

namespace User.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        [Route("Authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] TokenAuthorizationRequestDTO requestDTO)
        {
            var AuthorizedResult = await _authenticationService.ReturnToken(requestDTO);
            if (AuthorizedResult == null)
            {
                return Unauthorized();
            }

            return Ok(AuthorizedResult);
        }

        [HttpPost]
        [Route("GetRefreshToken")]
        public async Task<IActionResult> GetRefreshToken([FromBody] RefreshTokenRequestDTO requestDTO)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var TokenSupposedlyExpired = tokenHandler.ReadJwtToken(requestDTO.ExpirationToken);

            if (TokenSupposedlyExpired.ValidTo > DateTime.UtcNow)
            {
                return BadRequest(new TokenAuthorizationResponseDTO
                {
                    Resul = false,
                    Message = "Refreshtoken does not expired"
                });

            }

            string userId = TokenSupposedlyExpired.Claims.First(x =>
                x.Type == JwtRegisteredClaimNames.NameId).Value.ToString();

            var AuthorizationResponse = await _authenticationService.ReturnRefreshToken(requestDTO, int.Parse(userId));

            if (AuthorizationResponse.Resul)
                return Ok(AuthorizationResponse);
            else
                return BadRequest(AuthorizationResponse);

        }

    }

}
