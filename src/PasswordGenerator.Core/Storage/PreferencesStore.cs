using System.Text.Json;

namespace PasswordGenerator.Core.Storage;

public sealed class PreferencesStore
{
    private readonly string _filePath;

    public PreferencesStore(string filePath) => _filePath = filePath;

    public static PreferencesStore CreateDefault() => new(Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "SEN201PasswordGenerator", "preferences.json"));

    public UserPreferences Load()
    {
        try
        {
            if (!File.Exists(_filePath)) return new UserPreferences();
            var preferences = JsonSerializer.Deserialize<UserPreferences>(File.ReadAllText(_filePath))
                ?? new UserPreferences();
            preferences.Length = Math.Clamp(preferences.Length, 4, 64);
            if (preferences.Theme is not ("Light" or "Dark")) preferences.Theme = "Light";
            return preferences;
        }
        catch (JsonException) { return new UserPreferences(); }
        catch (IOException) { return new UserPreferences(); }
        catch (UnauthorizedAccessException) { return new UserPreferences(); }
    }

    public bool Save(UserPreferences preferences)
    {
        ArgumentNullException.ThrowIfNull(preferences);
        try
        {
            var directory = Path.GetDirectoryName(Path.GetFullPath(_filePath))!;
            Directory.CreateDirectory(directory);
            var stored = preferences.RememberPreferences ? preferences : new UserPreferences
            {
                Theme = preferences.Theme,
                HideByDefault = preferences.HideByDefault,
                RememberPreferences = false
            };
            var temporaryPath = Path.Combine(directory, Path.GetRandomFileName());
            try
            {
                File.WriteAllText(temporaryPath, JsonSerializer.Serialize(stored,
                    new JsonSerializerOptions { WriteIndented = true }));
                File.Move(temporaryPath, _filePath, overwrite: true);
                return true;
            }
            finally
            {
                if (File.Exists(temporaryPath)) File.Delete(temporaryPath);
            }
        }
        catch (IOException) { return false; }
        catch (UnauthorizedAccessException) { return false; }
    }
}
