using API.Middlewares;
using Domain.Entities;
using Domain.Exceptions;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace API.Tests.Middlewares;

public class ErrorHandlingMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_WhenNoExceptionThrow_ShouldCallNextDelegate()
    {
        // arrange

        var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
        var middleware = new ErrorHandlingMiddleware(loggerMock.Object);
        var context = new DefaultHttpContext();
        var nextDelegateMock = new Mock<RequestDelegate>();

        // act

        await middleware.InvokeAsync(context, nextDelegateMock.Object);

        // assert

        nextDelegateMock.Verify(next => next.Invoke(context), Times.Once);
    }    

    [Fact]
    public async Task InvokeAsync_WhenNotFoundExceptionThrow_ShouldSetStatus404()
    {
        // arrange

        var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
        var middleware = new ErrorHandlingMiddleware(loggerMock.Object);
        var context = new DefaultHttpContext();
        var notFoundException = new NotFoundException(nameof(Restaurant), "1");
        // act

        await middleware.InvokeAsync(context, _ => throw notFoundException);

        // assert

        context.Response.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task InvokeAsync_WhenForbidExceptionThrow_ShouldSetStatus403()
    {
        // arrange

        var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
        var middleware = new ErrorHandlingMiddleware(loggerMock.Object);
        var context = new DefaultHttpContext();
        var forbidException = new ForbidException();
        // act

        await middleware.InvokeAsync(context, _ => throw forbidException);

        // assert

        context.Response.StatusCode.Should().Be(403);
    }

    [Fact]
    public async Task InvokeAsync_WhenGenericExceptionThrow_ShouldSetStatus500()
    {
        // arrange

        var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
        var middleware = new ErrorHandlingMiddleware(loggerMock.Object);
        var context = new DefaultHttpContext();
        var exception = new Exception();
        // act

        await middleware.InvokeAsync(context, _ => throw exception);

        // assert

        context.Response.StatusCode.Should().Be(500);
    }
}