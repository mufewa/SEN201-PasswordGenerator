namespace PasswordGenerator.Core;

/// <summary>Generates passwords from user-selected options.</summary>
public interface IPasswordGenerator
{
    string GeneratePassword(PasswordOptions options);
}
