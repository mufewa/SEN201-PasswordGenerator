namespace PasswordGenerator.Core;

/// <summary>User-selected password generation options.</summary>
public sealed class PasswordOptions
{
    public const int MinLength = 4;
    public const int MaxLength = 64;
    public const int DefaultLength = 16;

    public int Length { get; set; } = DefaultLength;
    public bool IncludeUppercase { get; set; } = true;
    public bool IncludeLowercase { get; set; } = true;
    public bool IncludeNumbers { get; set; } = true;
    public bool IncludeSpecial { get; set; } = true;

    public bool HasAnyCharacterSet => IncludeUppercase || IncludeLowercase || IncludeNumbers || IncludeSpecial;

    /// <summary>Returns enabled character sets in display order.</summary>
    public IReadOnlyList<string> GetSelectedSets()
    {
        var sets = new List<string>(4);
        if (IncludeUppercase) sets.Add(CharacterSets.Uppercase);
        if (IncludeLowercase) sets.Add(CharacterSets.Lowercase);
        if (IncludeNumbers) sets.Add(CharacterSets.Digits);
        if (IncludeSpecial) sets.Add(CharacterSets.Special);
        return sets;
    }
}
