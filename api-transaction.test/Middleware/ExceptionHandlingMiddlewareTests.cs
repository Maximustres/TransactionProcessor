using api_transaction.Middleware;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text.Json;

namespace api_transaction.test.Middleware
{
    public class ExceptionHandlingMiddlewareTests
    {
        [Fact]
        public async Task InvokeAsync_ShouldHandleException()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            var loggerMock = new Mock<ILogger<ExceptionHandlingMiddleware>>();
            var middleware = new ExceptionHandlingMiddleware(
                async (innerHttpContext) => throw new Exception("Test exception"),
                loggerMock.Object
            );

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            context.Response.StatusCode.Should().Be(500);
            context.Response.ContentType.Should().StartWith("application/json");

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();

            var json = JsonDocument.Parse(responseBody);
            json.RootElement.GetProperty("message").GetString().Should().Be("An internal server error has occurred.");
        }
    }
}
