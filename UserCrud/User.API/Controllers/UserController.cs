﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using System.IdentityModel.Tokens.Jwt;
using User.API.Utility;
using User.BLL.Services.Contracts;
using User.DTOs;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace User.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAutorizacionService _autorizacionService;

        public UserController(IUserService userService, IAutorizacionService autorizacionService)
        {
            _userService = userService;
            _autorizacionService = autorizacionService;
        }

        [HttpPost]
        [Route("Authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] TokenAuthorizationRequestDTO requestDTO)
        {
            var AuthorizedResult = await _autorizacionService.ReturnToken(requestDTO);
            if(AuthorizedResult == null)
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

            if(TokenSupposedlyExpired.ValidTo > DateTime.UtcNow)
            {
                return BadRequest(new TokenAuthorizationResponseDTO
                {
                    Resul = false,
                    Message = "Refreshtoken does not expired"
                });

            }

            string userId = TokenSupposedlyExpired.Claims.First( x=>
                x.Type == JwtRegisteredClaimNames.NameId).Value.ToString();

            var AuthorizationResponse = await _autorizacionService.ReturnRefreshToken(requestDTO, int.Parse(userId));

            if (AuthorizationResponse.Resul)
                return Ok(AuthorizationResponse);
            else
                return BadRequest(AuthorizationResponse);

        }

        [Authorize]
        [HttpGet]
        [Route("List")]
        public async Task<IActionResult> List()
        {
            var response = new Response<List<UserInformationDTO>>();

            try
            {
                response.status = true;
                response.value = await _userService.List();
            }
            catch (Exception ex)
            {
                response.status = false;
                response.mensage = ex.Message;
            }

            return Ok(response);

        }

        [HttpPost]
        [Route("DataSave")]
        public async Task<IActionResult> DataSave([FromBody] UserInformationDTO user)
        {
            var response = new Response<UserInformationDTO>();

            try
            {
                response.status = true;
                response.value = await _userService.Create(user);
            }
            catch (Exception ex)
            {
                response.status = false;
                response.mensage = ex.Message;
            }

            return Ok(response);

        }

        [HttpPut]
        [Route("Edit")]
        public async Task<IActionResult> Edit([FromBody] UserInformationDTO user)
        {
            var response = new Response<bool>();

            try
            {
                response.status = true;
                response.value = await _userService.Edit(user);
            }
            catch (Exception ex)
            {
                response.status = false;
                response.mensage = ex.Message;
            }

            return Ok(response);

        }

        [HttpDelete]
        [Route("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = new Response<bool>();

            try
            {
                response.status = true;
                response.value = await _userService.Delete(id);
            }
            catch (Exception ex)
            {
                response.status = false;
                response.mensage = ex.Message;
            }

            return Ok(response);

        }
    }
}
