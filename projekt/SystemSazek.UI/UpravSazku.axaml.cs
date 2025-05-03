using Avalonia.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using SystemSazek.Core.Sazky;
using System;
using System.Collections.Generic;

namespace SystemSazek.UI
{
    public partial class UpravitSazku : Window
    {
        public ObservableCollection<PolozkaSazky> PolozkaList { get; } = new ObservableCollection<PolozkaSazky>();
        public double Castka { get; set; }
        
        private Uzivatel uzivatel;
        private Sazka existujiciSazka;

        public UpravitSazku(Sazka sazka, Uzivatel uzivatel)
        {
            InitializeComponent();
            this.uzivatel = uzivatel;
            this.existujiciSazka = sazka; 
            Castka = this.existujiciSazka.castka;
            this.DataContext = this;
            
            foreach (var polozka in sazka.polozky)
            {
                Zapas q = null;
                new PolozkaSazky();
                foreach (Zapas zapas in PolozkaSazky.Zapasy)
                {
                    if ( polozka.zapas.id_zapas == zapas.id_zapas ) q = zapas;
                }
               
                PolozkaList.Add(new PolozkaSazky
                {
                    id_sazka_zapas = (polozka.id_sazka_zapas ?? -1),
                    IdZapasu = q,
                    Vysledek = polozka.vsazeno_na.ToString()
                });
            }
        }

        private void OnAddZapasClick(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            PolozkaList.Add(new PolozkaSazky());
        }

        private void OnUpravitSazkuClick(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (double.TryParse(CastkaTextBox.Text, out double castka))
            {
                List<Zadana_polozka> polozky = new List<Zadana_polozka>();
                for (int i = 0; i < PolozkaList.Count; ++i)
                {     
                    Zadana_polozka polozka = new Zadana_polozka
                    {
                        id_sazka_zapas = PolozkaList[i].id_sazka_zapas,                         
                        idZapasu = PolozkaList[i].IdZapasu.id_zapas.Value,
                        vysledek = int.Parse(PolozkaList[i].Vysledek)
                    };
                    polozky.Add(polozka);
                }
                
                ServiceReturn sr = SazkaService.upravSazku(this.existujiciSazka, castka, polozky);
                if ( sr.Uspech )
                {
                    var zpet = new MainMenu( this.uzivatel );
                    zpet.Show();
                    this.Close();
                }
                else
                {
                    ChybovaHlaska.ShowError( this, sr.ChybovaHlaska );
                }
            }
            else ChybovaHlaska.ShowError( this, "zadejte castku");
        }
    }
}

