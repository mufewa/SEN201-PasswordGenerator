namespace PasswordGenerator.Core.Storage;

public sealed class UserPreferences
{
    public int Length { get; set; } = 16;
    public bool IncludeUppercase { get; set; } = true;
    public bool IncludeLowercase { get; set; } = true;
    public bool IncludeNumbers { get; set; } = true;
    public bool IncludeSpecialChars { get; set; } = true;
    public string Theme { get; set; } = "Light";
    public bool HideByDefault { get; set; } = true;
    public bool RememberPreferences { get; set; } = true;
}
