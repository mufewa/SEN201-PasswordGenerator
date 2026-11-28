using PasswordGenerator.UI;

namespace PasswordGenerator;

internal sealed class MainForm : Form
{
    private readonly Panel _titleBar = new() { Dock = DockStyle.Top, Height = 40 };
    private readonly Panel _sidebar = new() { Dock = DockStyle.Left, Width = 245 };
    private readonly Panel _content = new() { Dock = DockStyle.Fill };
    private readonly Label _title = new() { Text = "SEN201 - Password Generator", Bounds = new Rectangle(52, 0, 620, 40), TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Segoe UI", 20, GraphicsUnit.Pixel) };
    private readonly Label _brand = new() { Text = "Password\nGenerator", Bounds = new Rectangle(12, 93, 221, 62), TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 25, FontStyle.Bold, GraphicsUnit.Pixel) };
    private readonly Label _course = new() { Text = "SEN201", Bounds = new Rectangle(12, 155, 221, 30), TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 20, GraphicsUnit.Pixel) };
    private readonly NavButton _generateNav = new() { Text = "Generate", IconKind = IconKind.Home, IsActive = true, Font = new Font("Segoe UI", 20, GraphicsUnit.Pixel), Bounds = new Rectangle(17, 204, 211, 51) };
    private readonly NavButton _settingsNav = new() { Text = "Settings", IconKind = IconKind.Gear, Font = new Font("Segoe UI", 20, GraphicsUnit.Pixel), Bounds = new Rectangle(17, 261, 211, 51) };
    private readonly NavButton _aboutNav = new() { Text = "About", IconKind = IconKind.Info, Font = new Font("Segoe UI", 20, GraphicsUnit.Pixel), Bounds = new Rectangle(17, 318, 211, 51) };
    private readonly Button _minimize = TitleButton("Minimize");
    private readonly Button _maximize = TitleButton("Maximize");
    private readonly Button _close = TitleButton("Close");
    private readonly GeneratePanel _generatePanel = new() { Dock = DockStyle.Fill };
    private AppTheme _theme = string.Equals(Environment.GetEnvironmentVariable("PASSWORD_GENERATOR_THEME"), "dark", StringComparison.OrdinalIgnoreCase) ? AppTheme.Dark : AppTheme.Light;

    public MainForm()
    {
        Text = "SEN201 - Password Generator";
        FormBorderStyle = FormBorderStyle.None;
        ClientSize = new Size(1375, 768);
        MinimumSize = new Size(1000, 650);
        StartPosition = FormStartPosition.CenterScreen;
        AutoScaleMode = AutoScaleMode.None;
        Font = new Font("Segoe UI", 14, GraphicsUnit.Pixel);
        DoubleBuffered = true;
        Icon = Icons.CreateAppIcon();
        _titleBar.Controls.AddRange([_title, _minimize, _maximize, _close]);
        _sidebar.Controls.AddRange([_brand, _course, _generateNav, _settingsNav, _aboutNav]);
        _content.Controls.Add(_generatePanel);
        Controls.AddRange([_content, _sidebar, _titleBar]);
        _titleBar.Paint += (_, e) => Icons.Draw(e.Graphics, IconKind.Lock, new Rectangle(22, 8, 25, 25), _title.ForeColor);
        _sidebar.Paint += (_, e) =>
        {
            Icons.Draw(e.Graphics, IconKind.Lock, new Rectangle(101, 38, 43, 47), _brand.ForeColor);
            using var pen = new Pen(ThemePalette.Get(_theme).CardBorder);
            e.Graphics.DrawLine(pen, _sidebar.Width - 1, 0, _sidebar.Width - 1, _sidebar.Height);
        };
        _titleBar.MouseDown += DragWindow;
        _title.MouseDown += DragWindow;
        _minimize.Click += (_, _) => WindowState = FormWindowState.Minimized;
        _maximize.Click += (_, _) => WindowState = WindowState == FormWindowState.Maximized ? FormWindowState.Normal : FormWindowState.Maximized;
        _close.Click += (_, _) => Close();
        _settingsNav.Click += (_, _) => MessageBox.Show(this, "Settings are coming in a later stage.", "Settings");
        _aboutNav.Click += (_, _) => MessageBox.Show(this, "SEN201 Password Generator", "About");
        var themeMenu = new ContextMenuStrip();
        themeMenu.Items.Add("Light theme", null, (_, _) => { _theme = AppTheme.Light; ApplyTheme(); });
        themeMenu.Items.Add("Dark theme", null, (_, _) => { _theme = AppTheme.Dark; ApplyTheme(); });
        _titleBar.ContextMenuStrip = themeMenu;
        _sidebar.ContextMenuStrip = themeMenu;
        KeyPreview = true;
        KeyDown += (_, e) =>
        {
            if (!e.Control || e.KeyCode != Keys.T) return;
            _theme = _theme == AppTheme.Light ? AppTheme.Dark : AppTheme.Light;
            ApplyTheme();
            e.SuppressKeyPress = true;
        };
        Resize += (_, _) => PositionTitleButtons();
        PositionTitleButtons();
        ApplyTheme();
    }

    private static Button TitleButton(string accessibleName) => new()
    {
        AccessibleName = accessibleName,
        FlatStyle = FlatStyle.Flat,
        TabStop = false,
        Font = new Font("Segoe UI Symbol", 19, GraphicsUnit.Pixel),
        Size = new Size(58, 40),
        Cursor = Cursors.Hand
    };

    private void PositionTitleButtons()
    {
        _close.Location = new Point(_titleBar.Width - 58, 0);
        _maximize.Location = new Point(_titleBar.Width - 116, 0);
        _minimize.Location = new Point(_titleBar.Width - 174, 0);
    }

    private void DragWindow(object? sender, MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Left) return;
        NativeMethods.ReleaseCapture();
        NativeMethods.SendMessage(Handle, 0xA1, 0x2, 0);
    }

    private void ApplyTheme()
    {
        ThemePalette palette = ThemePalette.Get(_theme);
        BackColor = palette.Content;
        _titleBar.BackColor = palette.Sidebar;
        _title.ForeColor = palette.SidebarText;
        _sidebar.BackColor = palette.Sidebar;
        _brand.ForeColor = palette.SidebarText;
        _course.ForeColor = palette.SidebarMuted;
        _content.BackColor = palette.Content;
        foreach (NavButton nav in new[] { _generateNav, _settingsNav, _aboutNav }) nav.ApplyTheme(palette);
        _minimize.Text = "_";
        _maximize.Text = "[]";
        _close.Text = "X";
        foreach (Button button in new[] { _minimize, _maximize, _close })
        {
            button.BackColor = palette.Sidebar;
            button.ForeColor = palette.SidebarText;
            button.FlatAppearance.BorderSize = 0;
        }
        _generatePanel.ApplyTheme(palette);
        _titleBar.Invalidate();
        _sidebar.Invalidate();
    }

    private static class NativeMethods
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        internal static extern bool ReleaseCapture();

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        internal static extern IntPtr SendMessage(IntPtr window, int message, int wParam, int lParam);
    }
}
