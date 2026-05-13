# Tray Icon Implementation Guide

## Overview

The Fast project now uses **Avalonia's TrayIcon** for cross-platform system tray support.

## Implementation

### Files Modified

1. **App.cs** - Added `InitializeTray()` method to create and configure the tray icon
2. **Program.cs** - Calls `App.InitializeTray()` after creating `TrayStatusWindow`
3. **SiliconLife.Fast.csproj** - Ensures `slc.ico` is copied to output directory

### Key Features

- ✅ Cross-platform (Windows, Linux, macOS)
- ✅ Right-click context menu (Open Web Interface, Exit)
- ✅ Left-click to show/hide status window
- ✅ Automatic icon path detection
- ✅ Dynamic web port detection (no hardcoding)
- ✅ Tooltip shows actual port number

---

## Platform Requirements

### Windows

✅ **No additional dependencies required**

Windows has native tray support built into the OS.

---

### Linux

⚠️ **Requires libappindicator support**

#### Ubuntu/Debian

```bash
sudo apt-get update
sudo apt-get install -y libappindicator3-1 libindicator3-7
```

#### Fedora/RHEL

```bash
sudo dnf install -y libappindicator-gtk3 libappindicator-sharp
```

#### Arch Linux

```bash
sudo pacman -S libappindicator-gtk3
```

#### Verify Installation

```bash
# Check if libappindicator is available
ldconfig -p | grep appindicator
```

---

### macOS

✅ **No additional dependencies required**

macOS has native status bar support.

---

## Troubleshooting

### Tray Icon Not Showing on Linux

**Symptom**: Application runs but no tray icon appears

**Solutions**:

1. **Check libappindicator installation**:
   ```bash
   dpkg -l | grep appindicator  # Debian/Ubuntu
   rpm -qa | grep appindicator  # Fedora/RHEL
   ```

2. **Check desktop environment**:
   - GNOME: Install `gnome-shell-extension-appindicator`
   - KDE: System tray should work by default
   - XFCE: May need to add "Status Tray Plugin" to panel

3. **Check application logs**:
   ```
   Look for: "TrayIcon initialized: /path/to/slc.ico"
   Or: "Tray icon not found at /path/to/slc.ico"
   ```

4. **Verify icon file exists**:
   ```bash
   ls -la /path/to/Fast/bin/Debug/net9.0/slc.ico
   ```

### Icon File Not Found

**Error**: `Tray icon not found at /path/to/slc.ico`

**Solution**:
```bash
# Ensure slc.ico exists in project root
ls -la src/SiliconLife.Fast/slc.ico

# Rebuild project
dotnet clean
dotnet build

# Check output directory
ls -la src/SiliconLife.Fast/bin/Debug/net9.0/slc.ico
```

### Context Menu Not Working

**Symptom**: Tray icon shows but right-click does nothing

**Possible causes**:
- Linux desktop environment doesn't support context menus in tray
- Missing GTK dependencies

**Solution**:
```bash
# Install GTK dependencies
sudo apt-get install -y libgtk-3-0
```

---

## Code Example

### Initialize Tray (Automatic)

```csharp
// In Program.cs - called automatically during startup
string iconPath = Path.Combine(AppContext.BaseDirectory, "slc.ico");
if (File.Exists(iconPath))
{
    App.InitializeTray(_trayWindow, iconPath, configData.WebPort);
}
```

### Custom Context Menu

```csharp
// In App.cs - modify InitializeTray method
var menu = new ContextMenu();

menu.Items.Add(new MenuItem { Header = "Open Web Interface" });
menu.Items.Add(new MenuItem { Header = "Dashboard" });
menu.Items.Add(new Separator());
menu.Items.Add(new MenuItem { Header = "Exit" });

_trayIcon.Menu = menu;
```

---

## Testing

### Build and Run

```bash
cd src/SiliconLife.Fast
dotnet build
dotnet run
```

### Expected Behavior

1. **On Startup**:
   - Tray icon appears in system tray
   - Status window may show briefly (configurable)
   - Application logs: "TrayIcon initialized: /path/to/slc.ico"

2. **On Left-Click**:
   - Status window toggles show/hide

3. **On Right-Click**:
   - Context menu appears
   - "Open Web Interface" opens browser
   - "Exit" closes application

---

## Migration from Windows Forms

### Old Code (Windows Forms)

```csharp
// OLD: Windows Forms ApplicationContext
var trayContext = new TrayApplicationContext(_trayWindow);
Application.Run(trayContext);
```

### New Code (Avalonia)

```csharp
// NEW: Avalonia TrayIcon (with dynamic port)
App.InitializeTray(_trayWindow, iconPath, configData.WebPort);
builder.StartWithClassicDesktopLifetime(args, shutdownMode: ShutdownMode.OnExplicitShutdown);
```

### Benefits

- ✅ Cross-platform support
- ✅ No Windows Forms dependency
- ✅ Better integration with Avalonia UI
- ✅ Consistent behavior across platforms

---

## Known Limitations

| Platform | Limitation |
|----------|------------|
| Linux (GNOME) | May require extension for tray support |
| Linux (Wayland) | Some tray features may not work |
| Linux (Headless) | Tray icon won't display (expected) |
| All | Icon size may vary by platform |

---

## References

- [Avalonia TrayIcon Documentation](https://docs.avaloniaui.net/docs/next/reference/controls/trayicon)
- [libappindicator](https://launchpad.net/libappindicator)
- [GNOME Shell Extension](https://extensions.gnome.org/extension/615-appindicator-support/)

---

## Version History

| Version | Date | Changes |
|---------|------|---------|
| 1.0 | 2026-05-13 | Initial Avalonia TrayIcon implementation |
