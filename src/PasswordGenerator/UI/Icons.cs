using System.Drawing.Drawing2D;

namespace PasswordGenerator.UI;

public enum IconKind
{
    Lock, Layers, Sliders, Document, Eye, EyeOff, Copy, Check, Shield,
    Info, Gear, Home, Monitor, Users, Person, Refresh
}

public static class Icons
{
    public static void Draw(Graphics graphics, IconKind kind, Rectangle bounds, Color color)
    {
        var state = graphics.Save();
        graphics.SmoothingMode = SmoothingMode.AntiAlias;
        graphics.TranslateTransform(bounds.X, bounds.Y);
        graphics.ScaleTransform(bounds.Width / 24f, bounds.Height / 24f);
        using var pen = new Pen(color, 1.8f) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round };
        using var brush = new SolidBrush(color);
        switch (kind)
        {
            case IconKind.Lock:
                graphics.DrawArc(pen, 6, 2, 12, 15, 180, 180);
                graphics.DrawRoundedRectangle(pen, new RectangleF(4, 10, 16, 12), 2);
                graphics.FillEllipse(brush, 11, 15, 2, 3);
                break;
            case IconKind.Layers:
                graphics.DrawPolygon(pen, [new(12, 2), new(22, 8), new(12, 14), new(2, 8)]);
                graphics.DrawLines(pen, [new(2, 12), new(12, 18), new(22, 12)]);
                graphics.DrawLines(pen, [new(2, 16), new(12, 22), new(22, 16)]);
                break;
            case IconKind.Sliders:
                graphics.DrawLine(pen, 3, 7, 21, 7);
                graphics.DrawLine(pen, 3, 17, 21, 17);
                graphics.FillEllipse(brush, 7, 4, 6, 6);
                graphics.FillEllipse(brush, 13, 14, 6, 6);
                break;
            case IconKind.Document:
                graphics.DrawLines(pen, [new(5, 2), new(15, 2), new(20, 7), new(20, 22), new(5, 22), new(5, 2)]);
                graphics.DrawLine(pen, 15, 2, 15, 7);
                graphics.DrawLine(pen, 15, 7, 20, 7);
                graphics.DrawLine(pen, 8, 13, 17, 13);
                graphics.DrawLine(pen, 8, 17, 17, 17);
                break;
            case IconKind.Eye:
            case IconKind.EyeOff:
                graphics.DrawBezier(pen, 2, 12, 7, 3, 17, 3, 22, 12);
                graphics.DrawBezier(pen, 2, 12, 7, 21, 17, 21, 22, 12);
                graphics.DrawEllipse(pen, 9, 9, 6, 6);
                if (kind == IconKind.EyeOff) graphics.DrawLine(pen, 3, 21, 21, 3);
                break;
            case IconKind.Copy:
                graphics.DrawRectangle(pen, 7, 6, 14, 16);
                graphics.DrawLines(pen, [new(3, 17), new(3, 2), new(17, 2)]);
                break;
            case IconKind.Check:
                graphics.DrawLines(pen, [new(3, 12), new(9, 18), new(21, 5)]);
                break;
            case IconKind.Shield:
                graphics.DrawLines(pen, [new(12, 2), new(21, 6), new(20, 15), new(12, 22), new(4, 15), new(3, 6), new(12, 2)]);
                graphics.DrawLines(pen, [new(7, 12), new(11, 16), new(17, 9)]);
                break;
            case IconKind.Info:
                graphics.DrawEllipse(pen, 2, 2, 20, 20);
                graphics.FillEllipse(brush, 11, 6, 2, 2);
                graphics.DrawLine(pen, 12, 11, 12, 17);
                break;
            case IconKind.Gear:
                graphics.DrawEllipse(pen, 5, 5, 14, 14);
                graphics.DrawEllipse(pen, 9, 9, 6, 6);
                for (var i = 0; i < 8; i++)
                {
                    var angle = i * Math.PI / 4;
                    graphics.DrawLine(pen, 12 + (int)(8 * Math.Cos(angle)), 12 + (int)(8 * Math.Sin(angle)),
                        12 + (int)(11 * Math.Cos(angle)), 12 + (int)(11 * Math.Sin(angle)));
                }
                break;
            case IconKind.Home:
                graphics.DrawLines(pen, [new(2, 11), new(12, 3), new(22, 11)]);
                graphics.DrawLines(pen, [new(5, 10), new(5, 21), new(19, 21), new(19, 10)]);
                graphics.DrawRectangle(pen, 10, 15, 4, 6);
                break;
            case IconKind.Monitor:
                graphics.DrawRectangle(pen, 2, 3, 20, 15);
                graphics.DrawLine(pen, 12, 18, 12, 22);
                graphics.DrawLine(pen, 8, 22, 16, 22);
                break;
            case IconKind.Users:
                graphics.DrawEllipse(pen, 9, 3, 6, 6);
                graphics.DrawArc(pen, 4, 11, 16, 13, 180, 180);
                graphics.DrawEllipse(pen, 2, 7, 4, 4);
                graphics.DrawEllipse(pen, 18, 7, 4, 4);
                break;
            case IconKind.Person:
                graphics.DrawEllipse(pen, 8, 3, 8, 8);
                graphics.DrawArc(pen, 3, 13, 18, 14, 180, 180);
                break;
            case IconKind.Refresh:
                graphics.DrawArc(pen, 3, 3, 18, 18, 40, 290);
                graphics.DrawLines(pen, [new(18, 4), new(21, 5), new(21, 10)]);
                graphics.DrawLines(pen, [new(6, 20), new(3, 19), new(3, 14)]);
                break;
        }
        graphics.Restore(state);
    }

    public static Icon CreateAppIcon()
    {
        using var bitmap = new Bitmap(32, 32);
        using (var graphics = Graphics.FromImage(bitmap))
        {
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using var brush = new SolidBrush(ThemePalette.Get(AppTheme.Light).PrimaryButton);
            graphics.FillRoundedRectangle(brush, new RectangleF(1, 1, 30, 30), 7);
            Draw(graphics, IconKind.Lock, new Rectangle(5, 4, 22, 24), Color.White);
        }
        using var temporary = Icon.FromHandle(bitmap.GetHicon());
        return (Icon)temporary.Clone();
    }
}
