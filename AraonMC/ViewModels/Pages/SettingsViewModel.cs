using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AraonMC.ViewModels.Pages;

public partial class SettingsViewModel : PageViewModelBase
{
    public SettingsViewModel()
    {
        Title = "Settings";
    }

    public string AppVersion => "AraonMC 0.1.0 (dev)";

    // General
    [ObservableProperty] private string _language = "English (US)";
    [ObservableProperty] private bool _keepLauncherOpen = true;
    [ObservableProperty] private bool _discordRpc = false;
    [ObservableProperty] private bool _checkUpdatesOnStart = true;

    // Java
    [ObservableProperty] private string _javaPath = @"C:\Program Files\Java\jdk-21\bin\javaw.exe";
    [ObservableProperty] private string _javaArguments = "-Xmx4G -XX:+UseG1GC";
    [ObservableProperty] private double _maxMemoryMb = 4096;
    [ObservableProperty] private double _minMemoryMb = 512;

    // Game
    [ObservableProperty] private string _gameDirectory = @"%APPDATA%\.araonmc";
    [ObservableProperty] private double _windowWidth = 1280;
    [ObservableProperty] private double _windowHeight = 720;
    [ObservableProperty] private bool _fullscreen = false;

    [RelayCommand]
    private void Save()
    {
        // Persistence backend not implemented — values are UI-only.
    }
}
