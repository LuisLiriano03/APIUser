using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using User.API.Utility;
using User.BLL.Services.Contracts;
using User.DTOs;

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
