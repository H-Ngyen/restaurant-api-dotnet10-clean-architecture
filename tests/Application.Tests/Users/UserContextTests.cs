using System.Security.Claims;
using Application.Users;
using Domain.Constraints;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Application.Tests.Users;

public class UserContextTests
{
    [Fact]
    public void GetCurrentUser_WithAuthenticateUser_ShouldReturnCurrentUser()
    {
        // Arrange
        var dateOfBirth = new DateOnly(1999, 1, 1);
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        var claims = new List<Claim>()
        {
            new(ClaimTypes.NameIdentifier, "1"),
            new(ClaimTypes.Email, "admin@test.com"),
            new(ClaimTypes.Role, UserRoles.Admin),
            new(ClaimTypes.Role, UserRoles.User),
            new(AppClaimTypes.Nationality, AppClaimTypes.VietNam),
            new(AppClaimTypes.DateOfBirth, dateOfBirth.ToString("yyyy-MM-dd"))
        };
        var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "Test"));

        httpContextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext()
        {
            User = user
        });
        var userContext = new UserContext(httpContextAccessorMock.Object);

        // Act
        var currentUser = userContext.GetCurrentUser();

        // Assert
        currentUser.Should().NotBeNull();
        currentUser.Id.Should().Be("1");
        currentUser.Email.Should().Be("admin@test.com");
        currentUser.Roles.Should().ContainInOrder(UserRoles.Admin, UserRoles.User);
        currentUser.Nationality.Should().Be(AppClaimTypes.VietNam);
        currentUser.DateOfBirth.Should().Be(dateOfBirth);
    }

    [Fact]
    public void GetCurrentUser_WithUserContextNoPresent_ThrowInvalidOperationException()
    {
        // arrange
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        httpContextAccessorMock.Setup(x => x.HttpContext).Returns((HttpContext)null);

        var userContext = new UserContext(httpContextAccessorMock.Object);
        
        // act
        Action action = () => userContext.GetCurrentUser();

        //assert
        action.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("User context is not present");
    
    }
}