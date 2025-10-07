using Domain.Models.Users;

namespace Tests.UnitUserTests.Users;

public class GetAgeUserTest
{
    [Fact]
    public void GetAge_ShouldReturnCorrectAge()
    {
        var birthDate = new DateOnly(2000, 10, 7);
        var user = new User { Birthday = birthDate };

        var age = user.GetAge();

        var expected = DateTime.Today.Year - 2000;
        if (birthDate > DateOnly.FromDateTime(DateTime.Today.AddYears(-expected)))
            expected--;

        Assert.Equal(expected, age);
    }
}
