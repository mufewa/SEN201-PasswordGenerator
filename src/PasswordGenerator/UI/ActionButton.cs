using System.Drawing.Drawing2D;

namespace PasswordGenerator.UI;

public sealed class ActionButton : Button, IThemeable
{
    private ThemePalette _palette = ThemePalette.Get(AppTheme.Light);
    private bool _hover;

    public ActionButton()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
        FlatStyle = FlatStyle.Flat;
        FlatAppearance.BorderSize = 0;
        DoubleBuffered = true;
        Cursor = Cursors.Hand;
    }

    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public bool IsPrimary { get; set; }
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public IconKind? IconKind { get; set; }

    public void ApplyTheme(ThemePalette palette)
    {
        _palette = palette;
        BackColor = palette.Content;
        Invalidate();
    }

    protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
    protected override void OnMouseLeave(EventArgs e) { _hover = false; Invalidate(); base.OnMouseLeave(e); }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.Clear(_palette.Content);
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        var fill = IsPrimary ? (_hover ? _palette.PrimaryButtonHover : _palette.PrimaryButton) : (_hover ? _palette.SecondaryButtonHover : _palette.SecondaryButton);
        var color = IsPrimary ? _palette.PrimaryButtonText : _palette.SecondaryButtonText;
        if (!Enabled) color = _palette.DisabledText;
        var bounds = new RectangleF(0.5f, 0.5f, Width - 1.5f, Height - 1.5f);
        using var brush = new SolidBrush(fill);
        using var border = new Pen(IsPrimary ? fill : _palette.InputBorder);
        e.Graphics.FillRoundedRectangle(brush, bounds, 12);
        e.Graphics.DrawRoundedRectangle(border, bounds, 12);
        var textBounds = new Rectangle(0, 0, Width, Height);
        if (IconKind is { } icon)
        {
            var textWidth = TextRenderer.MeasureText(Text, Font).Width;
            var iconSize = Text.Length == 0 ? 24 : 27;
            var start = Text.Length == 0 ? (Width - iconSize) / 2 : Math.Max(10, (Width - textWidth - iconSize - 12) / 2);
            Icons.Draw(e.Graphics, icon, new Rectangle(start, (Height - iconSize) / 2, iconSize, iconSize), color);
            textBounds = new Rectangle(start + iconSize + 12, 0, Width - start - iconSize - 12, Height);
        }
        if (Text.Length > 0) TextRenderer.DrawText(e.Graphics, Text, Font, textBounds, color, TextFormatFlags.VerticalCenter | (IconKind is null ? TextFormatFlags.HorizontalCenter : TextFormatFlags.Left) | TextFormatFlags.EndEllipsis);
    }
}
