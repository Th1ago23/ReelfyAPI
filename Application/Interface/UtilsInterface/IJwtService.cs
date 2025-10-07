namespace Application.Interface.UtilsInterface;

public interface IJwtService
{
    void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
    string CreateToken(int userId, string email);
    bool VerifyPasswordHash(string password, byte[] hash, byte[] salt);
}
