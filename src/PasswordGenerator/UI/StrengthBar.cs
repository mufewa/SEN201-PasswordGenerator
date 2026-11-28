using System.Drawing.Drawing2D;
using PasswordGenerator.Core;

namespace PasswordGenerator.UI;

public sealed class StrengthBar : Control, IThemeable
{
    private ThemePalette _palette = ThemePalette.Get(AppTheme.Light);
    private int _score;
    private PasswordStrength _strength;

    public StrengthBar()
    {
        DoubleBuffered = true;
        Height = 10;
    }

    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public int Score
    {
        get => _score;
        set { _score = Math.Clamp(value, 0, 100); Invalidate(); }
    }

    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public PasswordStrength Strength
    {
        get => _strength;
        set { _strength = value; Invalidate(); }
    }

    public void ApplyTheme(ThemePalette palette)
    {
        _palette = palette;
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        var bounds = new RectangleF(0, 0, Width, Height);
        using var track = new SolidBrush(_palette.PasswordBox);
        e.Graphics.FillRoundedRectangle(track, bounds, Height / 2f);
        if (_score == 0) return;
        var fill = new RectangleF(0, 0, Math.Max(Height, Width * _score / 100f), Height);
        using var brush = new SolidBrush(_palette.StrengthColor(_strength));
        e.Graphics.FillRoundedRectangle(brush, fill, Height / 2f);
    }
}
