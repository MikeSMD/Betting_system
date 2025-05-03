using Avalonia.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using SystemSazek.Core.Sazky;
using System;
using System.Collections.Generic;

namespace SystemSazek.UI
{
    public partial class VytvorSazku : Window
    {
        public ObservableCollection<PolozkaSazky> PolozkaList { get; } = new ObservableCollection<PolozkaSazky>();
        private Uzivatel uzivatel;

        // Bezparametrický konstruktor pro XAML
        public VytvorSazku()
        {
            InitializeComponent();
            this.DataContext = this;
            PolozkaList.Add(new PolozkaSazky());
        }

        // Konstruktor s parametrem
        public VytvorSazku(Uzivatel uzivatel)
        {
              InitializeComponent();
            this.DataContext = this;
            this.uzivatel = uzivatel;
            PolozkaList.Add(new PolozkaSazky());
        }

        private void OnAddZapasClick(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            PolozkaList.Add(new PolozkaSazky());
        }
        private void OnZpetClick(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
           
                    var zpet = new MainMenu( this.uzivatel );
                    zpet.Show();
                    this.Close();

        }
        private void OnPodatSazkuClick(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (double.TryParse(CastkaTextBox.Text, out double castka))
            {
                List<Zadana_polozka> polozky = new List<Zadana_polozka>();
                for ( int i = 0; i < PolozkaList.Count; ++i )
                {     
                    Zadana_polozka polozka = new Zadana_polozka { idZapasu = PolozkaList[ i ].IdZapasu.id_zapas.Value, vysledek = int.Parse( PolozkaList[ i ].Vysledek ) };
                    polozky.Add(polozka);
                }
                if ( this.uzivatel == null) Console.WriteLine("JE TO NULL");
                ServiceReturn sr = SazkaService.podejSazku ( this.uzivatel, castka, polozky );
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
            else
            {
                   ChybovaHlaska.ShowError( this, "Musite zadat castku" );                
            }
            
        }
    }

    public class PolozkaSazky : INotifyPropertyChanged
    {
        public static ObservableCollection<Zapas> Zapasy{ get; set; } = new ObservableCollection<Zapas>();
        
        private int? _id_sazka_zapas;
        public int id_sazka_zapas { get => _id_sazka_zapas ?? -1;set { if ( _id_sazka_zapas != value ) _id_sazka_zapas = value; } }
        private Zapas _idZapasu;
        private string _vysledek;
        private static bool loaded = false;
            public PolozkaSazky()
        {
            if ( !loaded ) {
            ZapasDataMapper zdm = new ZapasDataMapper("Data source=soubor.db");
            var zapasy = zdm.GetAllZapasy();
            Zapasy = new ObservableCollection<Zapas>();
            foreach (Zapas i in zapasy)
            {
                Zapasy.Add(i);
            }
            loaded = true;
            }
        }

        public Zapas IdZapasu
        {
            get => _idZapasu;
            set
            {
                if (_idZapasu != value)
                {
                    _idZapasu = value;
                    OnPropertyChanged(nameof(IdZapasu));
                }
            }
        }

        public string Vysledek
        {
            get => _vysledek;
            set
            {
                if (_vysledek != value)
                {
                    _vysledek = value;
                    OnPropertyChanged(nameof(Vysledek));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    
}
