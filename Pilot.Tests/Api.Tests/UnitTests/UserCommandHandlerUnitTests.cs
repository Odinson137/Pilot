// using Microsoft.Extensions.Logging;
// using Moq;
// using Moq.Protected;
// using Newtonsoft.Json;
// using Pilot.Api.Commands;
// using Pilot.Api.Handlers;
// using Pilot.Contracts.DTO;
// using Pilot.Contracts.Exception.ProjectExceptions;
// using Xunit;
//
// namespace Pilot.Tests.Api.Tests.UnitTests;
//
//
// public class UserCommandHandlerUnitTests
// {
//     private readonly Mock<HttpMessageHandler> _mockHandler;
//     private readonly UserCommandHandler _sut;
//
//     public UserCommandHandlerUnitTests()
//     {
//         var mockFactory = new Mock<IHttpClientFactory>();
//         _mockHandler = new Mock<HttpMessageHandler>();
//         var httpClient = new HttpClient(_mockHandler.Object);
//         httpClient.BaseAddress = new Uri("http://example.com/api/");
//         mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);
//
//         var loggerMock = new Mock<ILogger<UserCommandHandler>>();
//         _sut = new UserCommandHandler(loggerMock.Object, mockFactory.Object);
//     }
//     
//     [Fact]
//     public async Task AuthorizationHandle_SuccessfulAuthorization_ReturnsAuthUserDto()
//     {
//         // Arrange
//         var request = new UserAuthorizationCommand("testuser", "testpassword");
//
//         var expectedResponse = new AuthUserDto(123, "testtoken");
//         var httpResponse = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
//         {
//             Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(expectedResponse))
//         };
//         
//         _mockHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
//                 ItExpr.IsAny<CancellationToken>())
//             .ReturnsAsync(httpResponse);
//
//         // Act
//         var result = await _sut.Handle(request, CancellationToken.None);
//
//         // Assert
//         Assert.NotNull(result);
//         Assert.Equal(expectedResponse.UserId, result.UserId);
//         Assert.Equal(expectedResponse.Token, result.Token);
//     }
//
//     [Fact]
//     public async Task AuthorizationHandle_UserAlreadyExistAuthorization_ReturnsBadRequestException()
//     {
//         // Arrange
//         var request = new UserAuthorizationCommand("testuser", "testpassword");
//
//         var httpResponse = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest)
//         {
//             Content = new StringContent("User not found")
//         };
//         
//         _mockHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
//                 ItExpr.IsAny<CancellationToken>())
//             .ReturnsAsync(httpResponse);
//
//         // Act
//         try
//         {
//             var result = await _sut.Handle(request, CancellationToken.None);
//         }
//         catch (Exception e)
//         {
//             // Assert
//             Assert.IsType<BadRequestException>(e);
//             return;
//         }
//
//         // Assert
//         Assert.True(false);
//     }
//     
//     [Fact]
//     public async Task RegistrationHandle_SuccessfulRegistration_ReturnsOkResult()
//     {
//         // Arrange
//         var request = new UserRegistrationCommand("testuser", "testname", "testlastname", "testpassword");
//
//         var httpResponse = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
//         {
//             Content = new StringContent(JsonConvert.SerializeObject(request)),
//         };
//         
//         _mockHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
//                 ItExpr.IsAny<CancellationToken>())
//             .ReturnsAsync(httpResponse);
//
//         // Act
//         try
//         {
//             await _sut.Handle(request, CancellationToken.None);
//         }
//         catch (Exception e)
//         {
//             // Assert
//             Assert.True(false);
//         }
//     }
//     
//     [Fact]
//     public async Task RegistrationHandle_UserNameAlreadyExist_ReturnsBadRequestResult()
//     {
//         // Arrange
//         var request = new UserRegistrationCommand("testuser", "testname", "testlastname", "testpassword");
//
//         var httpResponse = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest)
//         {
//             Content = new StringContent(JsonConvert.SerializeObject(request)),
//         };
//         
//         _mockHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
//                 ItExpr.IsAny<CancellationToken>())
//             .ReturnsAsync(httpResponse);
//
//         // Act
//         try
//         {
//             await _sut.Handle(request, CancellationToken.None);
//         }
//         catch (Exception e)
//         {
//             // Assert
//             Assert.True(true);
//             return;
//         }
//         
//         Assert.True(false);
//     }
//
// }