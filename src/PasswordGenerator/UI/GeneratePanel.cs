using PasswordGenerator.Core;

namespace PasswordGenerator.UI;

internal sealed class GeneratePanel : UserControl, IThemeable
{
    private readonly IPasswordGenerator _generator = new PasswordGeneratorService();
    private readonly CardPanel _optionsCard = new() { Bounds = new Rectangle(22, 94, 292, 346) };
    private readonly CardPanel _outputCard = new() { Bounds = new Rectangle(330, 94, 306, 122) };
    private readonly CardPanel _strengthCard = new() { Bounds = new Rectangle(330, 232, 306, 208) };
    private readonly Label _heading = LabelAt("Generate Password", 22, 15, 620, 34, 19, true);
    private readonly Label _subtitle = LabelAt("Create a strong password with your chosen length and characters.", 22, 50, 620, 25, 9);
    private readonly Label _optionsTitle = LabelAt("Password Options", 16, 12, 260, 32, 13, true);
    private readonly Label _lengthLabel = LabelAt("Password Length:", 16, 62, 160, 30, 10);
    private readonly NumericUpDown _length = new() { Minimum = 4, Maximum = 64, Value = 16, Bounds = new Rectangle(184, 63, 78, 30) };
    private readonly SliderControl _slider = new() { Bounds = new Rectangle(16, 107, 260, 30), Value = 16 };
    private readonly Label _minLabel = LabelAt("4", 16, 139, 40, 23, 9);
    private readonly Label _maxLabel = LabelAt("64", 239, 139, 38, 23, 9);
    private readonly ModernCheckBox _uppercase = CheckAt("Uppercase letters (A-Z)", 16, 176);
    private readonly ModernCheckBox _lowercase = CheckAt("Lowercase letters (a-z)", 16, 215);
    private readonly ModernCheckBox _numbers = CheckAt("Numbers (0-9)", 16, 254);
    private readonly ModernCheckBox _special = CheckAt("Special characters (!@#$%...)", 16, 293);
    private readonly Label _outputTitle = LabelAt("Generated Password", 16, 12, 275, 32, 13, true);
    private readonly TextBox _output = new() { Bounds = new Rectangle(16, 57, 200, 46), Multiline = true, ReadOnly = true, TabStop = false, BorderStyle = BorderStyle.FixedSingle, Font = new Font("Consolas", 9) };
    private readonly ActionButton _reveal = new() { IconKind = IconKind.Eye, Bounds = new Rectangle(220, 62, 33, 36), Enabled = false };
    private readonly ActionButton _copy = new() { IconKind = IconKind.Copy, Bounds = new Rectangle(257, 62, 33, 36), Enabled = false };
    private readonly Label _strengthTitle = LabelAt("Password Strength", 16, 12, 275, 32, 13, true);
    private readonly StrengthBar _strengthBar = new() { Bounds = new Rectangle(16, 66, 190, 20) };
    private readonly Label _strengthName = LabelAt("No password", 210, 60, 82, 30, 9, true);
    private readonly Label _strengthMessage = LabelAt("Generate a password to see its strength.", 16, 111, 270, 64, 10);
    private readonly ActionButton _generate = new() { Text = "Generate Password", IconKind = IconKind.Refresh, IsPrimary = true, Bounds = new Rectangle(22, 458, 292, 49) };
    private readonly ActionButton _clear = new() { Text = "Clear", Bounds = new Rectangle(330, 458, 180, 49), Enabled = false };
    private string _password = string.Empty;
    private bool _revealed;
    private ThemePalette _palette = ThemePalette.Get(AppTheme.Light);

    public GeneratePanel()
    {
        AutoScroll = true;
        AutoScrollMinSize = new Size(655, 524);
        Font = new Font("Segoe UI", 10);
        _optionsCard.Controls.AddRange([_optionsTitle, _lengthLabel, _length, _slider, _minLabel, _maxLabel, _uppercase, _lowercase, _numbers, _special]);
        _outputCard.Controls.AddRange([_outputTitle, _output, _reveal, _copy]);
        _strengthCard.Controls.AddRange([_strengthTitle, _strengthBar, _strengthName, _strengthMessage]);
        Controls.AddRange([_heading, _subtitle, _optionsCard, _outputCard, _strengthCard, _generate, _clear]);
        _length.ValueChanged += (_, _) => _slider.Value = (int)_length.Value;
        _slider.ValueChanged += (_, _) => _length.Value = _slider.Value;
        _generate.Click += (_, _) => Generate();
        _clear.Click += (_, _) => ClearPassword();
        _reveal.Click += (_, _) => { _revealed = !_revealed; UpdateOutput(); };
        _copy.Click += (_, _) => CopyPassword();
        ApplyTheme(_palette);
    }

    public void ApplyTheme(ThemePalette palette)
    {
        _palette = palette;
        BackColor = palette.Content;
        ForeColor = palette.Text;
        foreach (CardPanel card in new[] { _optionsCard, _outputCard, _strengthCard }) card.ApplyTheme(palette);
        foreach (Label label in new[] { _heading, _optionsTitle, _lengthLabel, _minLabel, _maxLabel, _outputTitle, _strengthTitle }) label.ForeColor = palette.Text;
        _subtitle.ForeColor = palette.MutedText;
        _strengthMessage.ForeColor = palette.MutedText;
        _length.BackColor = palette.Input;
        _length.ForeColor = palette.Text;
        _output.BackColor = palette.PasswordBox;
        _output.ForeColor = palette.Text;
        _slider.ApplyTheme(palette);
        foreach (ModernCheckBox box in new[] { _uppercase, _lowercase, _numbers, _special }) box.ApplyTheme(palette);
        foreach (ActionButton button in new[] { _reveal, _copy, _generate, _clear }) button.ApplyTheme(palette);
        _strengthBar.ApplyTheme(palette);
        UpdateStrength();
    }

    private void Generate()
    {
        PasswordOptions options = new()
        {
            Length = (int)_length.Value,
            IncludeUppercase = _uppercase.Checked,
            IncludeLowercase = _lowercase.Checked,
            IncludeNumbers = _numbers.Checked,
            IncludeSpecial = _special.Checked
        };
        if (!options.HasAnyCharacterSet)
        {
            MessageBox.Show(this, "Select at least one character type.", "Password options", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        _password = _generator.GeneratePassword(options);
        _revealed = false;
        _reveal.Enabled = true;
        _copy.Enabled = true;
        _clear.Enabled = true;
        UpdateOutput();
        UpdateStrength();
    }

    private void ClearPassword()
    {
        _password = string.Empty;
        _revealed = false;
        _reveal.Enabled = false;
        _copy.Enabled = false;
        _clear.Enabled = false;
        UpdateOutput();
        UpdateStrength();
    }

    private void UpdateOutput()
    {
        _output.Text = _revealed ? _password : new string('*', _password.Length);
        _reveal.IconKind = _revealed ? IconKind.EyeOff : IconKind.Eye;
        _reveal.Invalidate();
    }

    private void UpdateStrength()
    {
        int score = PasswordStrengthEvaluator.Score(_password);
        PasswordStrength strength = PasswordStrengthEvaluator.Evaluate(_password);
        _strengthBar.Score = score;
        _strengthBar.Strength = strength;
        _strengthName.Text = _password.Length == 0 ? "No password" : strength.ToString() switch { "VeryStrong" => "Very Strong", string name => name };
        _strengthName.ForeColor = _password.Length == 0 ? _palette.MutedText : _palette.StrengthColor(strength);
        _strengthMessage.Text = _password.Length == 0 ? "Generate a password to see its strength." : PasswordStrengthEvaluator.Description(strength);
    }

    private void CopyPassword()
    {
        if (_password.Length == 0) return;
        try
        {
            Clipboard.SetText(_password);
        }
        catch (System.Runtime.InteropServices.ExternalException)
        {
            MessageBox.Show(this, "The clipboard is unavailable. Please try again.", "Copy password", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private static Label LabelAt(string text, int x, int y, int width, int height, int size, bool bold = false) => new()
    {
        Text = text,
        Bounds = new Rectangle(x, y, width, height),
        Font = new Font("Segoe UI", size, bold ? FontStyle.Bold : FontStyle.Regular),
        TextAlign = ContentAlignment.MiddleLeft
    };

    private static ModernCheckBox CheckAt(string text, int x, int y) => new()
    {
        Text = text,
        Checked = true,
        Bounds = new Rectangle(x, y, 264, 33)
    };
}
