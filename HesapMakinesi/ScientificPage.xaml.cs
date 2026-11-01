
using System.Globalization;

namespace HesapMakinesi;

public partial class ScientificPage : ContentPage
{
    private double ilkSayi = 0;
    private string secilenIslem = "";
    private bool yeniGiris = true;

    public ScientificPage()
    {
        InitializeComponent();
    }

    
    private double SayiyiOku()
    {
        string metin = (DisplayLabel.Text ?? "0").Replace(',', '.');

        return double.Parse(metin, CultureInfo.InvariantCulture);
    }

    
    private void SonucuGoster(double sonuc)
    {
        if (double.IsNaN(sonuc) || double.IsInfinity(sonuc))
        {
            HataGoster("Geçersiz işlem");
            return;
        }

       
        if (Math.Abs(sonuc) < 0.0000000001)
        {
            sonuc = 0;
        }

        DisplayLabel.Text = sonuc.ToString(
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
            DisplayLabel.Text = rakam;
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
        }
        else if (!DisplayLabel.Text!.Contains(','))
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
            ilkSayi = SayiyiOku();
            secilenIslem = buton.Text;
            yeniGiris = true;
        }
        catch
        {
            HataGoster("Geçersiz sayı");
        }
    }

   
    private void OnEqualClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(secilenIslem))
            return;

        try
        {
            double ikinciSayi = SayiyiOku();
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
                        HataGoster("Sıfıra bölünemez");
                        return;
                    }

                    sonuc = ilkSayi / ikinciSayi;
                    break;

                case "xʸ":
                    sonuc = Math.Pow(ilkSayi, ikinciSayi);
                    break;

                default:
                    return;
            }

            SonucuGoster(sonuc);
            secilenIslem = "";
        }
        catch
        {
            HataGoster("Hesaplama hatası");
        }
    }

    
    private void OnScientificFunctionClicked(object sender, EventArgs e)
    {
        if (sender is not Button buton)
            return;

        try
        {
            double sayi = SayiyiOku();
            double sonuc;

           
            double radyan = sayi * Math.PI / 180;

            switch (buton.Text)
            {
                case "√":
                    if (sayi < 0)
                    {
                        HataGoster("Negatif sayının karekökü alınamaz");
                        return;
                    }

                    sonuc = Math.Sqrt(sayi);
                    break;

                case "x²":
                    sonuc = Math.Pow(sayi, 2);
                    break;

                case "sin":
                    sonuc = Math.Sin(radyan);
                    break;

                case "cos":
                    sonuc = Math.Cos(radyan);
                    break;

                case "tan":
                    if (Math.Abs(Math.Cos(radyan)) < 1e-10)
                    {
                        HataGoster("Tanımsız işlem");
                        return;
                    }

                    sonuc = Math.Tan(radyan);
                    break;

                case "log":
                    if (sayi <= 0)
                    {
                        HataGoster("Logaritma için sayı pozitif olmalı");
                        return;
                    }

                    sonuc = Math.Log10(sayi);
                    break;

                case "ln":
                    if (sayi <= 0)
                    {
                        HataGoster("Logaritma için sayı pozitif olmalı");
                        return;
                    }

                    sonuc = Math.Log(sayi);
                    break;

                default:
                    return;
            }

            SonucuGoster(sonuc);
        }
        catch
        {
            HataGoster("Hesaplama hatası");
        }
    }

    
    private void OnSignClicked(object sender, EventArgs e)
    {
        try
        {
            SonucuGoster(-SayiyiOku());
        }
        catch
        {
            HataGoster("Geçersiz sayı");
        }
    }

    
    private void OnClearClicked(object sender, EventArgs e)
    {
        DisplayLabel.Text = "0";
        ilkSayi = 0;
        secilenIslem = "";
        yeniGiris = true;
    }

   
    private void HataGoster(string mesaj)
    {
        DisplayLabel.Text = "Hata";
        ilkSayi = 0;
        secilenIslem = "";
        yeniGiris = true;

        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await DisplayAlert("Hesaplama Hatası", mesaj, "Tamam");
        });
    }
}
