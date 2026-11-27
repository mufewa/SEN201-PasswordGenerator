using System.Drawing.Drawing2D;
using PasswordGenerator.Core;

namespace PasswordGenerator.UI;

internal sealed class GeneratePanel : UserControl, IThemeable
{
    private readonly IPasswordGenerator _generator = new PasswordGeneratorService();
    private readonly CardPanel _optionsCard = new() { HeaderIcon = IconKind.Sliders };
    private readonly CardPanel _outputCard = new() { HeaderIcon = IconKind.Document };
    private readonly CardPanel _strengthCard = new() { HeaderIcon = IconKind.Shield };
    private readonly Label _heading = LabelAt("Generate Password", 106, 34, 800, 50, 32, true);
    private readonly Label _subtitle = LabelAt("Create a random and strong password based on your preferred length and character types.", 106, 86, 1000, 32, 20);
    private readonly Label _optionsTitle = LabelAt("Password Options", 70, 10, 340, 36, 23, true);
    private readonly Label _lengthLabel = LabelAt("Password Length:", 20, 76, 160, 40, 18);
    private readonly NumericUpDown _length = new() { Minimum = 4, Maximum = 64, Value = 64, Bounds = new Rectangle(180, 76, 108, 40), Font = new Font("Segoe UI", 23, GraphicsUnit.Pixel), BorderStyle = BorderStyle.FixedSingle };
    private readonly SliderControl _slider = new() { Bounds = new Rectangle(20, 128, 386, 28), Value = 64 };
    private readonly Label _minLabel = LabelAt("4", 20, 160, 40, 30, 20);
    private readonly Label _maxLabel = LabelAt("64", 360, 160, 44, 30, 20);
    private readonly ModernCheckBox _uppercase = CheckAt("Include Uppercase Letters (A - Z)", 20, 204);
    private readonly ModernCheckBox _lowercase = CheckAt("Include Lowercase Letters (a - z)", 20, 247);
    private readonly ModernCheckBox _numbers = CheckAt("Include Numbers (0 - 9)", 20, 290);
    private readonly ModernCheckBox _special = CheckAt("Include Special Characters (!@#$%^&*)", 20, 333);
    private readonly Label _outputTitle = LabelAt("Generated Password", 70, 10, 500, 36, 23, true);
    private readonly Panel _outputBackground = new() { Bounds = new Rectangle(18, 69, 460, 74) };
    private readonly Label _output = new() { Bounds = new Rectangle(16, 14, 428, 52), TextAlign = ContentAlignment.TopLeft, Font = new Font("Consolas", 17, GraphicsUnit.Pixel) };
    private readonly ActionButton _reveal = new() { IconKind = IconKind.Eye, Bounds = new Rectangle(496, 79, 42, 46) };
    private readonly ActionButton _copy = new() { IconKind = IconKind.Copy, Bounds = new Rectangle(550, 79, 42, 46) };
    private readonly Label _strengthTitle = LabelAt("Password Strength", 70, 10, 500, 36, 23, true);
    private readonly StrengthBar _strengthBar = new() { Bounds = new Rectangle(18, 75, 466, 24) };
    private readonly Label _strengthName = LabelAt("Very Strong", 480, 70, 140, 33, 20, true);
    private readonly Panel _callout = new() { Bounds = new Rectangle(18, 118, 590, 75) };
    private readonly Label _calloutTitle = LabelAt("Very Strong Password", 70, 10, 490, 30, 21, true);
    private readonly Label _strengthMessage = LabelAt("This password is highly secure and resistant...", 70, 38, 490, 28, 19);
    private readonly ActionButton _generate = new() { Text = "Generate Password", IconKind = IconKind.Refresh, IsPrimary = true, Font = new Font("Segoe UI", 22, GraphicsUnit.Pixel) };
    private readonly ActionButton _clear = new() { Text = "Clear", Font = new Font("Segoe UI", 22, GraphicsUnit.Pixel) };
    private string _password = string.Empty;
    private bool _revealed = true;
    private ThemePalette _palette = ThemePalette.Get(AppTheme.Light);

    public GeneratePanel()
    {
        AutoScroll = true;
        AutoScrollMinSize = new Size(0, 690);
        AutoScaleMode = AutoScaleMode.None;
        DoubleBuffered = true;
        Font = new Font("Segoe UI", 14, GraphicsUnit.Pixel);
        _optionsCard.Controls.AddRange([_optionsTitle, _lengthLabel, _length, _slider, _minLabel, _maxLabel, _uppercase, _lowercase, _numbers, _special]);
        _outputBackground.Controls.Add(_output);
        _outputCard.Controls.AddRange([_outputTitle, _outputBackground, _reveal, _copy]);
        _callout.Controls.AddRange([_calloutTitle, _strengthMessage]);
        _strengthCard.Controls.AddRange([_strengthTitle, _strengthBar, _strengthName, _callout]);
        Controls.AddRange([_heading, _subtitle, _optionsCard, _outputCard, _strengthCard, _generate, _clear]);
        Paint += (_, e) => Icons.Draw(e.Graphics, IconKind.Layers, new Rectangle(35, 42, 46, 46), _palette.MutedText);
        _callout.Paint += DrawCallout;
        _outputBackground.Paint += (_, e) =>
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using var brush = new SolidBrush(_palette.PasswordBox);
            e.Graphics.FillRoundedRectangle(brush, new RectangleF(0, 0, _outputBackground.Width, _outputBackground.Height), 10);
        };
        Resize += (_, _) => LayoutCards();
        _length.ValueChanged += (_, _) => _slider.Value = (int)_length.Value;
        _slider.ValueChanged += (_, _) => _length.Value = _slider.Value;
        _generate.Click += (_, _) => Generate();
        _clear.Click += (_, _) => ClearPassword();
        _reveal.Click += (_, _) => { _revealed = !_revealed; UpdateOutput(); };
        _copy.Click += (_, _) => CopyPassword();
        _calloutTitle.BackColor = Color.Transparent;
        _strengthMessage.BackColor = Color.Transparent;
        LayoutCards();
        ApplyTheme(_palette);
        _length.Value = 64;
        _slider.Value = 64;
        Generate();
    }

    private void LayoutCards()
    {
        int width = ClientSize.Width;
        _heading.Width = width - 120;
        _subtitle.Width = width - 120;
        int rightWidth;
        int rightX;
        int outputY;
        int strengthY;
        int buttonsY;
        if (width >= 970)
        {
            _optionsCard.Bounds = new Rectangle(34, 139, 426, 396);
            rightX = 488;
            rightWidth = width - rightX - 17;
            outputY = 139;
            strengthY = 323;
            buttonsY = 590;
        }
        else
        {
            _optionsCard.Bounds = new Rectangle(34, 139, 426, 396);
            rightX = 34;
            rightWidth = Math.Max(426, width - 68);
            outputY = 559;
            strengthY = 743;
            buttonsY = 1010;
        }
        _outputCard.Bounds = new Rectangle(rightX, outputY, rightWidth, 159);
        _strengthCard.Bounds = new Rectangle(rightX, strengthY, rightWidth, 212);
        _outputBackground.Width = rightWidth - 165;
        _output.Width = _outputBackground.Width - 32;
        _outputTitle.Width = rightWidth - 80;
        _strengthTitle.Width = rightWidth - 80;
        _reveal.Left = rightWidth - 129;
        _copy.Left = rightWidth - 72;
        _strengthBar.Width = rightWidth - 175;
        _strengthName.Left = rightWidth - 153;
        _callout.Width = rightWidth - 36;
        _calloutTitle.Width = rightWidth - 100;
        _strengthMessage.Width = rightWidth - 100;
        _generate.Bounds = new Rectangle(34, buttonsY, 344, 59);
        _clear.Bounds = new Rectangle(406, buttonsY, 274, 59);
        AutoScrollMinSize = new Size(0, buttonsY + 100);
        UpdateOutput();
    }

    public void ApplyTheme(ThemePalette palette)
    {
        _palette = palette;
        BackColor = palette.Content;
        ForeColor = palette.Text;
        foreach (CardPanel card in new[] { _optionsCard, _outputCard, _strengthCard }) card.ApplyTheme(palette);
        foreach (Label label in new[] { _heading, _optionsTitle, _lengthLabel, _outputTitle, _strengthTitle }) label.ForeColor = palette.Text;
        foreach (Label label in new[] { _subtitle, _minLabel, _maxLabel, _strengthMessage }) label.ForeColor = palette.MutedText;
        _length.BackColor = palette.Input;
        _length.ForeColor = palette.Text;
        _output.BackColor = palette.PasswordBox;
        _output.ForeColor = palette.Text;
        _outputBackground.BackColor = palette.Card;
        _outputBackground.Invalidate();
        _slider.ApplyTheme(palette);
        foreach (ModernCheckBox box in new[] { _uppercase, _lowercase, _numbers, _special }) box.ApplyTheme(palette);
        foreach (ActionButton button in new[] { _reveal, _copy, _generate, _clear }) button.ApplyTheme(palette);
        _strengthBar.ApplyTheme(palette);
        _callout.Invalidate();
        UpdateStrength();
        Invalidate();
    }

    private void DrawCallout(object? sender, PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        Color fill = _palette.StrengthFill(PasswordStrength.VeryStrong);
        Color border = _palette.StrengthBorder(PasswordStrength.VeryStrong);
        using var brush = new SolidBrush(fill);
        using var pen = new Pen(border);
        e.Graphics.FillRoundedRectangle(brush, new RectangleF(0, 0, _callout.Width - 1, _callout.Height - 1), 11);
        e.Graphics.DrawRoundedRectangle(pen, new RectangleF(0.5f, 0.5f, _callout.Width - 2, _callout.Height - 2), 11);
        using var circle = new SolidBrush(_palette.StrengthColor(PasswordStrength.VeryStrong));
        e.Graphics.FillEllipse(circle, 18, 21, 34, 34);
        Icons.Draw(e.Graphics, IconKind.Check, new Rectangle(26, 29, 18, 18), Color.White);
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
        _revealed = true;
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
        string displayed = _revealed ? _password : new string('*', _password.Length);
        int firstLineLength = Math.Min(displayed.Length, Math.Max(1, _output.Width / 10));
        _output.Text = displayed.Length > firstLineLength
            ? displayed[..firstLineLength] + Environment.NewLine + displayed[firstLineLength..]
            : displayed;
        _reveal.IconKind = _revealed ? IconKind.Eye : IconKind.EyeOff;
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
        _callout.Visible = _password.Length > 0;
        _calloutTitle.Text = _strengthName.Text + " Password";
        _calloutTitle.ForeColor = _palette.Text;
        _strengthMessage.Text = _password.Length == 0 ? "" : strength == PasswordStrength.VeryStrong ? "This password is highly secure and resistant..." : PasswordStrengthEvaluator.Description(strength);
        _callout.Invalidate();
    }

    private void CopyPassword()
    {
        if (_password.Length == 0) return;
        try { Clipboard.SetText(_password); }
        catch (System.Runtime.InteropServices.ExternalException)
        {
            MessageBox.Show(this, "The clipboard is unavailable. Please try again.", "Copy password", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private static Label LabelAt(string text, int x, int y, int width, int height, int size, bool bold = false) => new()
    {
        Text = text,
        Bounds = new Rectangle(x, y, width, height),
        Font = new Font("Segoe UI", size, bold ? FontStyle.Bold : FontStyle.Regular, GraphicsUnit.Pixel),
        TextAlign = ContentAlignment.MiddleLeft
    };

    private static ModernCheckBox CheckAt(string text, int x, int y) => new()
    {
        Text = text,
        Checked = true,
        Font = new Font("Segoe UI", 20, GraphicsUnit.Pixel),
        Bounds = new Rectangle(x, y, 390, 38)
    };
}
