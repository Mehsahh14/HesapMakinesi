using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace HesapMakinesi;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        string kaydedilenTema = Preferences.Get("SecilenTema", "System");

        UserAppTheme = kaydedilenTema switch
        {
            "Light" => AppTheme.Light,
            "Dark" => AppTheme.Dark,
            _ => AppTheme.Unspecified
        };

        MainPage = new AppShell();
    }
}