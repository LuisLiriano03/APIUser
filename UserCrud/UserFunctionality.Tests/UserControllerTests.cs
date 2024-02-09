using Microsoft.AspNetCore.Mvc;
using User.API.Controllers;
using User.BLL.Services.Contracts;
using User.DTOs;
using Moq;
using User.API.Utility;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Authorization;

namespace UserFunctionality.Tests
{
    public class UserControllerTests
    {
        [Fact]
        public async Task Authenticate_ReturnsUnauthorized_WhenUserNotFound()
        {
       
            var userServiceMock = new Mock<IUserService>();
            var autorizacionServiceMock = new Mock<IAutorizacionService>();

            autorizacionServiceMock.Setup(x => x.ReturnToken(It.IsAny<TokenAuthorizationRequestDTO>()))
                .ReturnsAsync((TokenAuthorizationResponseDTO)null); 

            var controller = new UserController(userServiceMock.Object, autorizacionServiceMock.Object);
            
            var result = await controller.Authenticate(new TokenAuthorizationRequestDTO());

            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task Authenticate_ReturnsOkResult_WhenUserFound()
        {

            var userServiceMock = new Mock<IUserService>();
            var autorizacionServiceMock = new Mock<IAutorizacionService>();

            autorizacionServiceMock.Setup(x => x.ReturnToken(It.IsAny<TokenAuthorizationRequestDTO>()))
                .ReturnsAsync(new TokenAuthorizationResponseDTO
                {
                    Token = "fakeToken",
                    Resul = true,
                    Message = "OK"
                });

            var controller = new UserController(userServiceMock.Object, autorizacionServiceMock.Object);

            var result = await controller.Authenticate(new TokenAuthorizationRequestDTO());

            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseDTO = Assert.IsType<TokenAuthorizationResponseDTO>(okResult.Value);

            Assert.Equal("fakeToken", responseDTO.Token);
            Assert.True(responseDTO.Resul);
            Assert.Equal("OK", responseDTO.Message);
        }


        [Fact]
        public async Task List_ReturnsOkResult_WhenAuthorized()
        {
            var userServiceMock = new Mock<IUserService>();
            var autorizacionServiceMock = new Mock<IAutorizacionService>();

            autorizacionServiceMock.Setup(x => x.ReturnToken(It.IsAny<TokenAuthorizationRequestDTO>()))
                .ReturnsAsync(new TokenAuthorizationResponseDTO
                {
                    Token = "fakeToken",
                    Resul = true,
                    Message = "OK"
                });

            var controller = new UserController(userServiceMock.Object, autorizacionServiceMock.Object);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "testuser")
            };

            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var principal = new ClaimsPrincipal(identity);

            userServiceMock.Setup(x => x.List())
                           .ReturnsAsync(new List<UserInformationDTO>{});

            var result = await controller.List();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseDTO = Assert.IsType<Response<List<UserInformationDTO>>>(okResult.Value);

            Assert.NotNull(responseDTO.value);
        }


        [Fact]
        public async Task DataSave_ReturnsOkResult_WhenAuthorized()
        {
            var userServiceMock = new Mock<IUserService>();
            var autorizacionServiceMock = new Mock<IAutorizacionService>();

            autorizacionServiceMock.Setup(x => x.ReturnToken(It.IsAny<TokenAuthorizationRequestDTO>()))
                .ReturnsAsync(new TokenAuthorizationResponseDTO
                {
                    Token = "fakeToken",
                    Resul = true,
                    Message = "OK"
                });

            var controller = new UserController(userServiceMock.Object, autorizacionServiceMock.Object);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    Request = { Scheme = "https", Host = new HostString("localhost"), Path = "/DataSave" }
                }
            };

            controller.Request.Headers["Authorization"] = "Bearer fakeToken";

            var userToSave = new UserInformationDTO{};

            userServiceMock.Setup(x => x.Create(It.IsAny<UserInformationDTO>()))
                           .ReturnsAsync(userToSave);

            var result = await controller.DataSave(userToSave);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseDTO = Assert.IsType<Response<UserInformationDTO>>(okResult.Value);

            Assert.NotNull(responseDTO.value);
        }






    }
}