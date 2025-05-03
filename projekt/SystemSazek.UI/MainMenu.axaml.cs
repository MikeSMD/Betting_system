using Avalonia.Controls;
using SystemSazek.Core.Sazky;

namespace SystemSazek.UI;

public partial class MainMenu : Window
{
    private Uzivatel uzivatel;
    public string UspesnostText { get; set; }
    public string VsazenaText { get; set; }
    public string VyhranaText { get; set; }
    public MainMenu(Uzivatel uzivatel)
    {
        this.uzivatel = uzivatel;
        UspesnostText = "Úspěšnost sázek = " + this.uzivatel.ProcentualniUspesnost() + " %";
        VsazenaText =  "Celková vsazená částka = " + this.uzivatel.CelkovaVsazenaCastkaUzivatele()+ " Kc";
        VyhranaText = "Vyhraná částka = " + this.uzivatel.CelkovaVyhranaCastkaUzivatele()+" Kc";
        InitializeComponent();
        DataContext = this;
  
    }

    private void OnVytovritSazkuClicked(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var vytvorSazku = new VytvorSazku( this.uzivatel );
        vytvorSazku.Show();
        this.Close();
    }
    private void OnZobrazitSazkyClicked(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var zobrazitSazky = new ZobrazitSazky( this.uzivatel );
        zobrazitSazky.Show();
        this.Close();
    }

    private void OnDeaktivaceClicked(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ServiceReturn sr = UzivatelService.DeaktivaceUzivatele( this.uzivatel );
        if ( sr.Uspech )
        {
            this.Close();
        }
        ChybovaHlaska.ShowError( this, sr.ChybovaHlaska );
    }

    private void OnOdejitClicked(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        this.Close();
    }



}
