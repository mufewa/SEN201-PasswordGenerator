namespace PasswordGenerator.Core;

/// <summary>Scores a password by length and character diversity.</summary>
public static class PasswordStrengthEvaluator
{
    public static int GetScore(string? password)
    {
        if (string.IsNullOrEmpty(password)) return 0;
        var score = 0;
        if (password.Length >= 8) score += 20;
        if (password.Length >= 12) score += 20;
        if (password.Length >= 16) score += 20;
        if (password.Any(CharacterSets.Uppercase.Contains)) score += 10;
        if (password.Any(CharacterSets.Lowercase.Contains)) score += 10;
        if (password.Any(CharacterSets.Digits.Contains)) score += 10;
        if (password.Any(CharacterSets.Special.Contains)) score += 10;
        return score;
    }

    public static int Score(string? password) => GetScore(password);

    public static PasswordStrength Evaluate(string? password) => GetScore(password) switch
    {
        < 25 => PasswordStrength.Weak,
        < 50 => PasswordStrength.Fair,
        < 75 => PasswordStrength.Strong,
        _ => PasswordStrength.VeryStrong
    };

    public static string GetDescription(PasswordStrength strength) => strength switch
    {
        PasswordStrength.Weak => "Easy to guess. Use more characters and more character types.",
        PasswordStrength.Fair => "Acceptable, but a longer password is more secure.",
        PasswordStrength.Strong => "Strong and resistant to guessing.",
        _ => "Highly secure and resistant to guessing."
    };

    public static string Description(PasswordStrength strength) => GetDescription(strength);
}
