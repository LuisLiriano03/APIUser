using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using User.BLL.Services.Contracts;
using User.DTOs;
using User.Model;

namespace User.BLL.Services
{
    public class AutorizacionService : IAutorizacionService
    {

        private readonly UserDbContext _dbcontext;
        private readonly IConfiguration _configuration;

        public AutorizacionService(UserDbContext dbcontext, IConfiguration configuration)
        {
            _dbcontext = dbcontext;
            _configuration = configuration;
        }

        private string GenerateToken(string UserId)
        {

            var key = _configuration.GetValue<string>("JwtSettings:key");
            var keyBytes = Encoding.ASCII.GetBytes(key);

            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, UserId));

            var TokenCredentials = new SigningCredentials(
                new SymmetricSecurityKey(keyBytes),
                SecurityAlgorithms.HmacSha256Signature
                );

            var DecryptionToken = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = TokenCredentials
            };

            var TokenHandler = new JwtSecurityTokenHandler();
            var TokenConfig = TokenHandler.CreateToken(DecryptionToken);

            string TokenCreated = TokenHandler.WriteToken(TokenConfig);

            return TokenCreated;

        }

        public async Task<TokenAuthorizationResponseDTO> ReturnToken(TokenAuthorizationRequestDTO tokenAuthorization)
        {
            var UserFound = _dbcontext.UserInformations.FirstOrDefault(x =>
                x.Email == tokenAuthorization.Email &&
                x.UserPassword == tokenAuthorization.UserPassword
            );

            if(UserFound == null )
            {
                return await Task.FromResult<TokenAuthorizationResponseDTO>(null);
            }

            string TokenCreated = GenerateToken(UserFound.UserId.ToString());

            return new TokenAuthorizationResponseDTO()
            {
                Token = TokenCreated,
                Resul = true,
                Message = "OK"
            };
        }
    }
}
