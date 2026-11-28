using System.Drawing.Drawing2D;

namespace PasswordGenerator.UI;

public sealed class CardPanel : Panel, IThemeable
{
    private ThemePalette _palette = ThemePalette.Get(AppTheme.Light);

    public CardPanel()
    {
        DoubleBuffered = true;
        Padding = new Padding(1);
    }

    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public int CornerRadius { get; set; } = 10;
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public IconKind HeaderIcon { get; set; } = IconKind.Lock;

    public void ApplyTheme(ThemePalette palette)
    {
        _palette = palette;
        BackColor = palette.Card;
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        using var pen = new Pen(_palette.CardBorder);
        e.Graphics.DrawRoundedRectangle(pen, new RectangleF(0.5f, 0.5f, Width - 1.5f, Height - 1.5f), CornerRadius);
        e.Graphics.DrawLine(pen, 1, 53, Width - 2, 53);
        Icons.Draw(e.Graphics, HeaderIcon, new Rectangle(22, 14, 27, 27), _palette.MutedText);
    }
}
