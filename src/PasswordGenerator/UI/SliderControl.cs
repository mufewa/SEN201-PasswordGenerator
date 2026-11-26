using System.Drawing.Drawing2D;

namespace PasswordGenerator.UI;

public sealed class SliderControl : Control, IThemeable
{
    private ThemePalette _palette = ThemePalette.Get(AppTheme.Light);
    private int _value = 16;

    public SliderControl()
    {
        DoubleBuffered = true;
        Height = 25;
        Cursor = Cursors.Hand;
    }

    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public int Minimum { get; set; } = 4;
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public int Maximum { get; set; } = 64;
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public int Value
    {
        get => _value;
        set
        {
            var next = Math.Clamp(value, Minimum, Maximum);
            if (_value == next) return;
            _value = next;
            Invalidate();
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public event EventHandler? ValueChanged;

    public void ApplyTheme(ThemePalette palette) { _palette = palette; Invalidate(); }

    protected override void OnMouseDown(MouseEventArgs e) { base.OnMouseDown(e); SetFromX(e.X); }
    protected override void OnMouseMove(MouseEventArgs e) { base.OnMouseMove(e); if (e.Button == MouseButtons.Left) SetFromX(e.X); }

    private void SetFromX(int x)
    {
        var fraction = Math.Clamp((x - 9f) / Math.Max(1, Width - 18), 0, 1);
        Value = Minimum + (int)Math.Round(fraction * (Maximum - Minimum));
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        var x = 9 + (Width - 18) * (_value - Minimum) / (float)(Maximum - Minimum);
        using var track = new Pen(_palette.PasswordBox, 5) { StartCap = LineCap.Round, EndCap = LineCap.Round };
        using var fill = new Pen(_palette.PrimaryButton, 5) { StartCap = LineCap.Round, EndCap = LineCap.Round };
        using var thumb = new SolidBrush(_palette.PrimaryButton);
        e.Graphics.DrawLine(track, 9, Height / 2, Width - 9, Height / 2);
        e.Graphics.DrawLine(fill, 9, Height / 2, x, Height / 2);
        e.Graphics.FillEllipse(thumb, x - 9, Height / 2f - 9, 18, 18);
    }
}
