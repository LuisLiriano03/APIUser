using Microsoft.AspNetCore.Mvc;
using User.API.Controllers;
using User.BLL.Services.Contracts;
using User.DTOs;
using Moq;
using User.API.Utility;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace UserFunctionality.Tests
{
    public class UserControllerTests
    {
        [Fact]
        public async Task Authenticate_ReturnsUnauthorized_WhenUserNotFound()
        {
            var userServiceMock = new Mock<IUserService>();
            var autorizacionServiceMock = new Mock<IAuthenticationService>();

            autorizacionServiceMock.Setup(x => x.ReturnToken(It.IsAny<TokenAuthorizationRequestDTO>()))
                .ReturnsAsync((TokenAuthorizationResponseDTO)null);

            var controller = new AuthenticationController(autorizacionServiceMock.Object);

            var result = await controller.Authenticate(new TokenAuthorizationRequestDTO());

            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task Authenticate_ReturnsOkResult_WhenUserFound()
        {
            var userServiceMock = new Mock<IUserService>();
            var autorizacionServiceMock = new Mock<IAuthenticationService>();

            autorizacionServiceMock.Setup(x => x.ReturnToken(It.IsAny<TokenAuthorizationRequestDTO>()))
                .ReturnsAsync(new TokenAuthorizationResponseDTO
                {
                    Token = "fakeToken",
                    Resul = true,
                    Message = "OK"
                });

            var controller = new AuthenticationController(autorizacionServiceMock.Object);

            var result = await controller.Authenticate(new TokenAuthorizationRequestDTO());

            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseDTO = Assert.IsType<TokenAuthorizationResponseDTO>(okResult.Value);

            Assert.Equal("fakeToken", responseDTO.Token);
            Assert.True(responseDTO.Resul);
            Assert.Equal("OK", responseDTO.Message);
        }

        [Fact]
        public async Task AllUsers_ReturnsOkResult_WhenAuthorized()
        {
            var userServiceMock = new Mock<IUserService>();
            var autorizacionServiceMock = new Mock<IAuthenticationService>();

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

            userServiceMock.Setup(x => x.AllUsersList())
                           .ReturnsAsync(new List<UserInformationDTO> { });

            var result = await controller.GetAllUsers();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseDTO = Assert.IsType<Response<List<UserInformationDTO>>>(okResult.Value);

            Assert.NotNull(responseDTO.value);
        }

        [Fact]
        public async Task SaveUser_ReturnsOkResult_WhenAuthorized()
        {
            var userServiceMock = new Mock<IUserService>();
            var autorizacionServiceMock = new Mock<IAuthenticationService>();

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
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                new Claim(ClaimTypes.Name, "testuser")
                    }))
                }
            };

            controller.Request.Headers["Authorization"] = "Bearer fakeToken";

            var userToSave = new UserInformationDTO { };

            userServiceMock.Setup(x => x.CreateUser(It.IsAny<UserInformationDTO>()))
                           .ReturnsAsync(userToSave);

            var result = await controller.SaveUser(userToSave);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseDTO = Assert.IsType<Response<UserInformationDTO>>(okResult.Value);

            Assert.NotNull(responseDTO.value);
        }

    }

}