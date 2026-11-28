using System.Drawing.Drawing2D;

namespace PasswordGenerator.UI;

public sealed class NavButton : Button, IThemeable
{
    private ThemePalette _palette = ThemePalette.Get(AppTheme.Light);
    private bool _hover;

    public NavButton()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
        FlatStyle = FlatStyle.Flat;
        FlatAppearance.BorderSize = 0;
        DoubleBuffered = true;
        Cursor = Cursors.Hand;
    }

    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public IconKind IconKind { get; set; }
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public bool IsActive { get; set; }

    public void ApplyTheme(ThemePalette palette)
    {
        _palette = palette;
        BackColor = palette.Sidebar;
        Invalidate();
    }

    protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
    protected override void OnMouseLeave(EventArgs e) { _hover = false; Invalidate(); base.OnMouseLeave(e); }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.Clear(_palette.Sidebar);
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        var fill = IsActive ? _palette.NavActive : _hover && Enabled ? _palette.NavHover : _palette.Sidebar;
        using var brush = new SolidBrush(fill);
        e.Graphics.FillRoundedRectangle(brush, new RectangleF(0, 0, Width - 1, Height - 1), 9);
        var color = !Enabled ? _palette.DisabledText : IsActive ? _palette.NavActiveText : _palette.SidebarText;
        Icons.Draw(e.Graphics, IconKind, new Rectangle(22, (Height - 27) / 2, 27, 27), IsActive ? _palette.NavActiveText : _palette.MutedText);
        TextRenderer.DrawText(e.Graphics, Text, Font, new Rectangle(68, 0, Width - 74, Height), color, TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
    }
}
