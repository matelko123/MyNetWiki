using ResultPattern.Api.Entities;
using Shouldly;

namespace ResultPattern.Tests;

public class UserServiceTests
{
    [Fact]
    public void Should_Return_UserAlreadyExist_Error()
    {
        Result<User> result = UserService.CreateInvalidUser();
        result.Error.ShouldBe(User.Errors.UserAlreadyExist);
    }
}