using System.Drawing.Drawing2D;

namespace PasswordGenerator.UI;

public sealed class ModernCheckBox : CheckBox, IThemeable
{
    private ThemePalette _palette = ThemePalette.Get(AppTheme.Light);

    public ModernCheckBox()
    {
        DoubleBuffered = true;
        AutoSize = false;
        Height = 30;
        Cursor = Cursors.Hand;
    }

    public void ApplyTheme(ThemePalette palette)
    {
        _palette = palette;
        ForeColor = palette.Text;
        BackColor = palette.Card;
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.Clear(BackColor);
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        var box = new RectangleF(2, (Height - 18) / 2f, 18, 18);
        using var fill = new SolidBrush(Checked ? _palette.PrimaryButton : _palette.Input);
        using var border = new Pen(Checked ? _palette.PrimaryButton : _palette.InputBorder);
        e.Graphics.FillRoundedRectangle(fill, box, 4);
        e.Graphics.DrawRoundedRectangle(border, box, 4);
        if (Checked) Icons.Draw(e.Graphics, IconKind.Check, new Rectangle(4, (Height - 16) / 2, 14, 14), Color.White);
        TextRenderer.DrawText(e.Graphics, Text, Font, new Rectangle(29, 0, Width - 29, Height), Enabled ? _palette.Text : _palette.DisabledText, TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
    }
}
