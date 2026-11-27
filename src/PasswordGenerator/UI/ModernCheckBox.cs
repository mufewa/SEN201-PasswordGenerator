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
        var box = new RectangleF(2, (Height - 26) / 2f, 26, 26);
        using var fill = new SolidBrush(Checked ? _palette.PrimaryButton : _palette.Input);
        using var border = new Pen(Checked ? _palette.PrimaryButton : _palette.InputBorder);
        e.Graphics.FillRoundedRectangle(fill, box, 5);
        e.Graphics.DrawRoundedRectangle(border, box, 5);
        if (Checked) Icons.Draw(e.Graphics, IconKind.Check, new Rectangle(6, (Height - 20) / 2, 19, 19), Color.White);
        TextRenderer.DrawText(e.Graphics, Text, Font, new Rectangle(39, 0, Width - 39, Height), Enabled ? _palette.Text : _palette.DisabledText, TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
    }
}
