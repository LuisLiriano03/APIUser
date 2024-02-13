using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using User.BLL.Services.Contracts;
using User.DTOs;
using User.Model;

namespace User.BLL.Services
{
    public class AuthenticationService : IAuthenticationService
    {

        private readonly UserDbContext _dbcontext;
        private readonly IConfiguration _configuration;

        public AuthenticationService(UserDbContext dbcontext, IConfiguration configuration)
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

        private string GenerateRefreshToken()
        {
            var byteArray = new byte[64];
            var refreshToken = "";

            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                randomNumberGenerator.GetBytes(byteArray);
                refreshToken = Convert.ToBase64String(byteArray);
            }
            
            return refreshToken;

        }

        private async Task<TokenAuthorizationResponseDTO> SaveRefreshTokenHistory(int UserId, string Token, string RefreshToken)
        {
            var RefreshTokenActivity = new RefreshTokenHistory
            {
                UserId = UserId,
                Token = Token,
                RefreshToken = RefreshToken,
                CreationDate = DateTime.UtcNow,
                ExpirationDate = DateTime.UtcNow.AddMinutes(2)
            };

            await _dbcontext.RefreshTokenHistories.AddAsync(RefreshTokenActivity);
            await _dbcontext.SaveChangesAsync();

            return new TokenAuthorizationResponseDTO { 
                Token = Token, 
                RefreshToken = RefreshToken, 
                Resul = true, 
                Message = "OK" 
            };


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

            string RefreshTokenCreated = GenerateRefreshToken();

            return await SaveRefreshTokenHistory(UserFound.UserId, TokenCreated, RefreshTokenCreated);
        }

        public async Task<TokenAuthorizationResponseDTO> ReturnRefreshToken(RefreshTokenRequestDTO refreshToken, int UserId)
        {
            var RefreshTokenFound = _dbcontext.RefreshTokenHistories.FirstOrDefault(x =>
                x.Token == refreshToken.ExpirationToken &&
                x.RefreshToken == refreshToken.RefreshToken &&
                x.UserId == UserId);

            if(RefreshTokenFound == null)
            {
                return new TokenAuthorizationResponseDTO { Resul = false, Message = "Refreshtoken does not exist" };
            }

            var RefreshTokenCreated = GenerateRefreshToken();
            var TokenCreated = GenerateToken(UserId.ToString());

            return await SaveRefreshTokenHistory(UserId, TokenCreated, RefreshTokenCreated);




        }
    }
}
