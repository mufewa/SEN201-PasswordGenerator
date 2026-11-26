using PasswordGenerator.UI;

namespace PasswordGenerator;

internal sealed class MainForm : Form
{
    private readonly Panel _sidebar = new() { Dock = DockStyle.Left, Width = 160 };
    private readonly Panel _content = new() { Dock = DockStyle.Fill };
    private readonly Label _brand = new() { Text = "Password\nGenerator", Font = new Font("Segoe UI", 15, FontStyle.Bold), TextAlign = ContentAlignment.MiddleCenter, Bounds = new Rectangle(8, 100, 144, 70) };
    private readonly Label _course = new() { Text = "SEN201", Font = new Font("Segoe UI", 10), TextAlign = ContentAlignment.MiddleCenter, Bounds = new Rectangle(8, 173, 144, 26) };
    private readonly NavButton _generateNav = new() { Text = "Generate", IconKind = IconKind.Home, IsActive = true, Bounds = new Rectangle(10, 235, 140, 46) };
    private readonly NavButton _settingsNav = new() { Text = "Settings", IconKind = IconKind.Gear, Bounds = new Rectangle(10, 288, 140, 46), Enabled = false };
    private readonly NavButton _aboutNav = new() { Text = "About", IconKind = IconKind.Info, Bounds = new Rectangle(10, 341, 140, 46), Enabled = false };
    private readonly Button _themeButton = new() { FlatStyle = FlatStyle.Flat, Bounds = new Rectangle(23, 460, 114, 34), Cursor = Cursors.Hand };
    private readonly GeneratePanel _generatePanel = new() { Dock = DockStyle.Fill };
    private AppTheme _theme = AppTheme.Light;

    public MainForm()
    {
        Text = "SEN201 - Password Generator";
        ClientSize = new Size(820, 540);
        MinimumSize = new Size(820, 540);
        StartPosition = FormStartPosition.CenterScreen;
        Font = new Font("Segoe UI", 10);
        DoubleBuffered = true;
        _sidebar.Controls.AddRange([_brand, _course, _generateNav, _settingsNav, _aboutNav, _themeButton]);
        _content.Controls.Add(_generatePanel);
        Controls.Add(_content);
        Controls.Add(_sidebar);
        _sidebar.Paint += (_, e) => Icons.Draw(e.Graphics, IconKind.Lock, new Rectangle(62, 30, 36, 44), _brand.ForeColor);
        _themeButton.Click += (_, _) =>
        {
            _theme = _theme == AppTheme.Light ? AppTheme.Dark : AppTheme.Light;
            ApplyTheme();
        };
        ApplyTheme();
    }

    private void ApplyTheme()
    {
        ThemePalette palette = ThemePalette.Get(_theme);
        BackColor = palette.Content;
        ForeColor = palette.Text;
        _sidebar.BackColor = palette.Sidebar;
        _sidebar.ForeColor = palette.SidebarText;
        _brand.ForeColor = palette.SidebarText;
        _course.ForeColor = palette.SidebarMuted;
        _content.BackColor = palette.Content;
        _generateNav.ApplyTheme(palette);
        _settingsNav.ApplyTheme(palette);
        _aboutNav.ApplyTheme(palette);
        _themeButton.Text = _theme == AppTheme.Light ? "Dark theme" : "Light theme";
        _themeButton.BackColor = palette.SecondaryButton;
        _themeButton.ForeColor = palette.SecondaryButtonText;
        _themeButton.FlatAppearance.BorderColor = palette.CardBorder;
        _generatePanel.ApplyTheme(palette);
        _sidebar.Invalidate();
    }
}
