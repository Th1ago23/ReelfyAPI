using Moq;
using Application.Interface.UserInterface;
using Domain.Interface.Repository;
using Application.DTO.Users;
using ReelfyAPI.Services;
using Application.Interface.UtilsInterface;

public class AuthServiceTests
{
    private readonly Mock<IUnitOfWork> _uowMock = new();
    private readonly Mock<IUserRepository> _userRepoMock = new();
    private readonly Mock<IUserMapper> _mapperMock = new();
    private readonly Mock<IJwtService> _jwtMock = new();

    [Fact]
    public async Task Register_ShouldReturnSuccess_WhenUserIsValid()
    {
        var dto = new UserRegisterDTO("thiago@test.com", "Thiago", "123456", DateOnly.FromDateTime(DateTime.Today.AddYears(-20)), "string");

        var userEntity = new Domain.Models.Users.User { Email = dto.Email, Birthday = dto.Birthday };

        _mapperMock.Setup(m => m.ToUser(dto)).Returns(userEntity);
        _userRepoMock.Setup(r => r.UserExists(dto.Email)).ReturnsAsync(false);
        _jwtMock.Setup(j => j.CreatePasswordHash(It.IsAny<string>(), out It.Ref<byte[]>.IsAny, out It.Ref<byte[]>.IsAny))
                .Callback((string p, out byte[] h, out byte[] s) =>
                {
                    h = new byte[] { 1 }; s = new byte[] { 2 };
                });
        _jwtMock.Setup(j => j.CreateToken(It.IsAny<int>(), It.IsAny<string>())).Returns("fake-token");

        var service = new AuthService(_uowMock.Object, _userRepoMock.Object, _mapperMock.Object, _jwtMock.Object);

        var result = await service.Register(dto);

        Assert.Equal(201, result.StatusCode);
        Assert.Equal("Cadastro realizado com sucesso!", result.Message);
        Assert.NotNull(result.Data.token);
        _userRepoMock.Verify(r => r.Add(It.IsAny<Domain.Models.Users.User>()), Times.Once);
        _uowMock.Verify(u => u.CommitAsync(), Times.Once);
    }
}
