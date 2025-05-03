using Avalonia.Controls;
using SystemSazek.Core.Sazky;
using System;
using System.Collections.Generic;
using System.Linq;
namespace SystemSazek.UI
{
    public class DetailSazkaViewModel
    {
        public string DatumCasVytvoreni { get; set; }
        public double Castka { get; set; }
        public string Status { get; set; }

        public string id_zapas { get; set; }
        public string id_tym_domaci { get; set; }
        public string id_tym_hoste   { get; set; }
        public double kurz{ get; set; } 
        public string vsazeno_na{ get; set; } 
        public string datum_cas_zacatku { get; set; }
    }

    public partial class ZobrazitDetailSazky : Window
    {
        private Uzivatel uzivatel;
        private Sazka sazka;
         public DetailSazkaViewModel HlavniDetail { get; set; }
         public List<DetailSazkaViewModel> Polozky { get; set; }

    public ZobrazitDetailSazky(Uzivatel uzivatel, Sazka sazka)
    {
        InitializeComponent();

        this.HlavniDetail = new DetailSazkaViewModel
        {
            DatumCasVytvoreni = sazka.datum_cas_vytvoreni.ToString("dd.MM.yyyy HH:mm"),
            Castka = sazka.castka,
            Status = sazka.status
        };
        this.sazka = sazka;
        this.Polozky = sazka.polozky?.Select(s => MapToViewModel(s)).ToList() ?? new List<DetailSazkaViewModel>();
        this.uzivatel = uzivatel;
        DataContext = this;
    }


         /*
        public ZobrazitDetailSazky(Uzivatel uzivatel, Sazka sazka)
        {
            InitializeComponent();

            // Mapování dat na ViewModel
            this.uzivatel = uzivatel;
            this.sazka = sazka;
            var detailSazkaViewModel = sazka.polozky?.Select(s => MapToViewModel(s)).ToList() ?? new List<DetailSazkaViewModel>();
            DataContext = detailSazkaViewModel;
        }
*/
        public DetailSazkaViewModel MapToViewModel(SystemSazek.Core.Sazky.Polozka polozka)
        {
            string parser = "";
            double kurz = -1.0;
            if ( polozka.vsazeno_na == 1 )
            {
                 parser = "domaci tym = " + polozka.zapas.tym_domaci.nazev;
                 kurz = polozka.zapas.kurz_domaci;
            }
            else if ( polozka.vsazeno_na == -1 )
            {
                 parser = "hosty = " + polozka.zapas.tym_hoste.nazev;
                 kurz = polozka.zapas.kurz_hoste;
            }
            else 
            {
                parser = "remizu";
                 kurz = polozka.zapas.kurz_shoda;
            }
            return new DetailSazkaViewModel
            {
                DatumCasVytvoreni = this.sazka.datum_cas_vytvoreni.ToString("dd.MM.yyyy HH:mm"),
                Castka = this.sazka.castka,
                Status = this.sazka.status,
                id_zapas = polozka.zapas.tym_domaci.nazev + " vs " + polozka.zapas.tym_hoste.nazev,
                id_tym_domaci = polozka.zapas.tym_domaci.nazev,
                id_tym_hoste = polozka.zapas.tym_hoste.nazev,
                vsazeno_na = parser,
                kurz = kurz,
                datum_cas_zacatku = polozka.zapas.datum_cas_zacatku.ToString("dd.MM.yyyy HH:mm")
            };
        }
        public async void OnAkceClicked(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var potvrzeni = new Potvrzeni();
            bool potvrzeno = await potvrzeni.ShowDialogAsync(this);
            if ( ! potvrzeno ) return;
            ServiceReturn sr = SazkaService.ZrusSazku(this.uzivatel, this.sazka.id_sazka ?? -1);
          if (sr.Uspech)
          {
            ChybovaHlaska.ShowError( this, sr.ChybovaHlaska );
              var zpet = new ZobrazitSazky( this.uzivatel );
              zpet.Show();
              this.Close();
          }
          else
          {
            ChybovaHlaska.ShowError( this, sr.ChybovaHlaska );
          }
        }
         public void OnUpravitClicked(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
          var uprava = new UpravitSazku( this.sazka, this.uzivatel );
          uprava.Show();
          this.Close();
        }
        
         private void OnZpetClick(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
           var zpet = new ZobrazitSazky( this.uzivatel );
            zpet.Show();
            this.Close();
        }
        
    }
}
