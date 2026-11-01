using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace HesapMakinesi;

public partial class SettingsPage : ContentPage
{
    private bool sayfaHazir = false;

    public SettingsPage()
    {
        InitializeComponent();

        string kaydedilenTema = Preferences.Get("SecilenTema", "System");

        ThemePicker.SelectedIndex = kaydedilenTema switch
        {
            "Light" => 0,
            "Dark" => 1,
            _ => 2
        };

        sayfaHazir = true;
    }

    private void OnThemeChanged(object sender, EventArgs e)
    {
        if (!sayfaHazir || ThemePicker.SelectedIndex < 0)
            return;

        string secilenTema = ThemePicker.SelectedIndex switch
        {
            0 => "Light",
            1 => "Dark",
            _ => "System"
        };

        Preferences.Set("SecilenTema", secilenTema);

        Application.Current!.UserAppTheme = secilenTema switch
        {
            "Light" => AppTheme.Light,
            "Dark" => AppTheme.Dark,
            _ => AppTheme.Unspecified
        };
    }
}