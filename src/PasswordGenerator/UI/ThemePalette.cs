using PasswordGenerator.Core;

namespace PasswordGenerator.UI;

public sealed class ThemePalette
{
    private ThemePalette(AppTheme theme)
    {
        Theme = theme;
        var dark = theme == AppTheme.Dark;
        Sidebar = dark ? C(13, 15, 19) : C(233, 238, 246);
        SidebarText = dark ? C(229, 231, 235) : C(15, 23, 42);
        SidebarMuted = dark ? C(124, 132, 147) : C(100, 116, 139);
        NavHover = dark ? C(26, 30, 38) : C(220, 227, 238);
        NavActive = dark ? C(30, 58, 102) : C(30, 45, 77);
        NavActiveText = Color.White;
        Content = dark ? C(20, 22, 27) : C(244, 246, 250);
        Card = dark ? C(27, 30, 37) : Color.White;
        CardBorder = dark ? C(42, 47, 58) : C(226, 232, 240);
        Separator = CardBorder;
        Text = dark ? C(229, 231, 235) : C(15, 23, 42);
        MutedText = dark ? C(156, 163, 175) : C(100, 116, 139);
        DisabledText = dark ? C(85, 91, 104) : C(160, 170, 185);
        Input = dark ? C(16, 17, 21) : Color.White;
        InputBorder = dark ? C(58, 64, 76) : C(203, 213, 225);
        PasswordBox = dark ? C(15, 17, 21) : C(238, 242, 247);
        PrimaryButton = dark ? C(30, 58, 102) : C(30, 45, 77);
        PrimaryButtonHover = dark ? C(42, 74, 128) : C(42, 61, 99);
        PrimaryButtonText = Color.White;
        SecondaryButton = dark ? C(27, 30, 37) : Color.White;
        SecondaryButtonHover = dark ? C(36, 40, 50) : C(241, 245, 249);
        SecondaryButtonText = Text;
        Accent = dark ? C(59, 130, 246) : C(30, 45, 77);
        Badge = dark ? C(30, 58, 102) : C(219, 234, 254);
        BadgeText = dark ? C(147, 197, 253) : C(29, 78, 216);
        Footer = dark ? C(30, 58, 102) : C(230, 235, 242);
        FooterText = dark ? Color.White : C(30, 45, 77);
        Success = C(22, 163, 74);
    }

    public AppTheme Theme { get; }
    public Color Sidebar { get; }
    public Color SidebarText { get; }
    public Color SidebarMuted { get; }
    public Color NavHover { get; }
    public Color NavActive { get; }
    public Color NavActiveText { get; }
    public Color Content { get; }
    public Color Card { get; }
    public Color CardBorder { get; }
    public Color Separator { get; }
    public Color Text { get; }
    public Color MutedText { get; }
    public Color DisabledText { get; }
    public Color Input { get; }
    public Color InputBorder { get; }
    public Color PasswordBox { get; }
    public Color PrimaryButton { get; }
    public Color PrimaryButtonHover { get; }
    public Color PrimaryButtonText { get; }
    public Color SecondaryButton { get; }
    public Color SecondaryButtonHover { get; }
    public Color SecondaryButtonText { get; }
    public Color Accent { get; }
    public Color Badge { get; }
    public Color BadgeText { get; }
    public Color Footer { get; }
    public Color FooterText { get; }
    public Color Success { get; }

    public static ThemePalette Get(AppTheme theme) => new(theme);

    public Color StrengthColor(PasswordStrength strength) => strength switch
    {
        PasswordStrength.Weak => C(220, 38, 38),
        PasswordStrength.Fair => C(217, 119, 6),
        PasswordStrength.Strong => C(22, 163, 74),
        _ => C(21, 128, 61)
    };

    public Color StrengthFill(PasswordStrength strength) => Blend(StrengthColor(strength), Card, 0.10);
    public Color StrengthBorder(PasswordStrength strength) => Blend(StrengthColor(strength), Card, 0.35);

    public static Color Blend(Color foreground, Color background, double foregroundWeight)
    {
        var weight = Math.Clamp(foregroundWeight, 0, 1);
        return C(
            (int)Math.Round(foreground.R * weight + background.R * (1 - weight)),
            (int)Math.Round(foreground.G * weight + background.G * (1 - weight)),
            (int)Math.Round(foreground.B * weight + background.B * (1 - weight)));
    }

    private static Color C(int red, int green, int blue) => Color.FromArgb(red, green, blue);
}
