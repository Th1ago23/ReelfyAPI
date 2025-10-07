using Domain.Models.Users;

public class UserTests
{
    [Fact]
    public void ValidateAge_ShouldReturnTrue_WhenUserIs16OrOlder()
    {
        var user = new User
        {
            Name = "Thiago",
            Birthday = DateOnly.FromDateTime(DateTime.Today.AddYears(-18))
        };

        var result = user.ValidateAge();

        Assert.True(result);
    }

    [Fact]
    public void ValidateAge_ShouldReturnFalse_WhenUserIsYoungerThan16()
    {
        var user = new User
        {
            Name = "Thiago",
            Birthday = DateOnly.FromDateTime(DateTime.Today.AddYears(-15))
        };

        var result = user.ValidateAge();

        Assert.False(result);
    }
}
