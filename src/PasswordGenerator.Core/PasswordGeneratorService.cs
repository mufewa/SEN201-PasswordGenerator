using System.Security.Cryptography;

namespace PasswordGenerator.Core;

/// <summary>Generates passwords with cryptographically secure randomness.</summary>
public sealed class PasswordGeneratorService : IPasswordGenerator
{
    public string GeneratePassword(PasswordOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        var sets = options.GetSelectedSets();
        if (sets.Count == 0)
            throw new ArgumentException("At least one character set must be selected.", nameof(options));

        if (options.Length is < PasswordOptions.MinLength or > PasswordOptions.MaxLength)
            throw new ArgumentOutOfRangeException(nameof(options), "Password length must be between 4 and 64.");

        var pool = string.Concat(sets);
        var characters = new char[options.Length];
        var count = 0;
        foreach (var set in sets)
            characters[count++] = set[RandomNumberGenerator.GetInt32(set.Length)];

        while (count < characters.Length)
            characters[count++] = pool[RandomNumberGenerator.GetInt32(pool.Length)];

        for (var i = characters.Length - 1; i > 0; i--)
        {
            var j = RandomNumberGenerator.GetInt32(i + 1);
            (characters[i], characters[j]) = (characters[j], characters[i]);
        }

        return new string(characters);
    }
}
