using Avalonia.Controls;
using SystemSazek.Core.Sazky;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SystemSazek.UI
{
    public class SazkaViewModel
    {
        public int? id_sazka { get; set; }
        public DateTime DatumCasVytvoreni { get; set; }
        public DateTime? DatumCasUzavreni { get; set; }
        public double Castka { get; set; }
        public string Status { get; set; }
        public double CelkovyKurz { get; set; }
        public double MoznaVyhra { get; set; }
    }

    public partial class ZobrazitSazky : Window
    {
        private Uzivatel uzivatel;
        public ZobrazitSazky(Uzivatel uzivatel)
        {
            InitializeComponent();
            this.uzivatel = uzivatel;

            // Mapování dat na ViewModel
            var sazkyViewModel = uzivatel.sazky?.Select(s => MapToViewModel(s)).ToList() ?? new List<SazkaViewModel>();
            DataContext = sazkyViewModel;
        }

        public static SazkaViewModel MapToViewModel(Sazka sazka)
        {
            return new SazkaViewModel
            {
                id_sazka = sazka.id_sazka,
                DatumCasVytvoreni = sazka.datum_cas_vytvoreni,
                DatumCasUzavreni = sazka.datum_cas_uzavreni,
                Castka = sazka.castka,
                Status = sazka.status,
                CelkovyKurz = sazka.CelkovyKurz(),
                MoznaVyhra = sazka.MoznaVyhra()
                };
        }

        private void OnSazkaActionClick(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is SazkaViewModel sazka)
            {
                for ( int i = 0; i < this.uzivatel.sazky.Count(); ++i )
                {
                    if ( this.uzivatel.sazky[ i ].id_sazka.Value == sazka.id_sazka )
                    {
                        var pkt = new ZobrazitDetailSazky( this.uzivatel, this.uzivatel.sazky[ i ] );
                        pkt.Show();
                        this.Close();
        
                        break;
                    }
                }
            }
        }

        private void OnExportSazkyClick(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
           UzivatelService.SerializujSazky( this.uzivatel );
        }

        private void OnZpetClick(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
           var zpet = new MainMenu( this.uzivatel );
            zpet.Show();
            this.Close();
        }
    }
}
