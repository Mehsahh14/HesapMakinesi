using System.Globalization;

namespace HesapMakinesi;

public partial class MainPage : ContentPage
{
    private double ilkSayi = 0;
    private string secilenIslem = "";
    private bool yeniGiris = true;

    public MainPage()
    {
        InitializeComponent();
    }

   
    private double EkrandakiSayiyiOku()
    {
        string metin = DisplayLabel.Text ?? "0";

        metin = metin.Replace(',', '.');

        return double.Parse(
            metin,
            CultureInfo.InvariantCulture);
    }

   
    private void EkranaYaz(double sayi)
    {
        if (double.IsNaN(sayi) || double.IsInfinity(sayi))
        {
            DisplayLabel.Text = "Hata";
            yeniGiris = true;
            return;
        }

        DisplayLabel.Text = sayi.ToString(
            "G",
            CultureInfo.InvariantCulture).Replace('.', ',');

        yeniGiris = true;
    }

    
    private void OnNumberClicked(object sender, EventArgs e)
    {
        if (sender is not Button buton)
            return;

        string rakam = buton.Text;

        if (yeniGiris || DisplayLabel.Text == "Hata")
        {
            DisplayLabel.Text = rakam == "0" ? "0" : rakam;
            yeniGiris = false;
        }
        else if (DisplayLabel.Text == "0")
        {
            DisplayLabel.Text = rakam;
        }
        else
        {
            DisplayLabel.Text += rakam;
        }
    }

   
    private void OnDecimalClicked(object sender, EventArgs e)
    {
        if (yeniGiris || DisplayLabel.Text == "Hata")
        {
            DisplayLabel.Text = "0,";
            yeniGiris = false;
            return;
        }

        if (!DisplayLabel.Text!.Contains(','))
        {
            DisplayLabel.Text += ",";
        }
    }

    
    private void OnOperatorClicked(object sender, EventArgs e)
    {
        if (sender is not Button buton)
            return;

        try
        {
            ilkSayi = EkrandakiSayiyiOku();
            secilenIslem = buton.Text;
            yeniGiris = true;
        }
        catch
        {
            HataGoster();
        }
    }

    
    private void OnEqualClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(secilenIslem))
            return;

        try
        {
            double ikinciSayi = EkrandakiSayiyiOku();
            double sonuc;

            switch (secilenIslem)
            {
                case "+":
                    sonuc = ilkSayi + ikinciSayi;
                    break;

                case "−":
                    sonuc = ilkSayi - ikinciSayi;
                    break;

                case "×":
                    sonuc = ilkSayi * ikinciSayi;
                    break;

                case "÷":
                    if (ikinciSayi == 0)
                    {
                        DisplayLabel.Text = "Sıfıra bölünemez";
                        secilenIslem = "";
                        yeniGiris = true;
                        return;
                    }

                    sonuc = ilkSayi / ikinciSayi;
                    break;

                default:
                    return;
            }

            EkranaYaz(sonuc);
            secilenIslem = "";
        }
        catch
        {
            HataGoster();
        }
    }

    
    private void OnSignClicked(object sender, EventArgs e)
    {
        try
        {
            double sayi = EkrandakiSayiyiOku();
            EkranaYaz(-sayi);
        }
        catch
        {
            HataGoster();
        }
    }

   
    private void OnClearClicked(object sender, EventArgs e)
    {
        DisplayLabel.Text = "0";
        ilkSayi = 0;
        secilenIslem = "";
        yeniGiris = true;
    }

   
    private void HataGoster()
    {
        DisplayLabel.Text = "Hata";
        ilkSayi = 0;
        secilenIslem = "";
        yeniGiris = true;
    }
}