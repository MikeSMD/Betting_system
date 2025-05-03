using Avalonia.Controls;
using SystemSazek.Core.Sazky;
using System;

namespace SystemSazek.UI
{
    public partial class Registrace : Window
    {
        public Registrace()
        {
            InitializeComponent();
        }

        
        private void OnRegisterClicked(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var jmeno = FirstNameInput.Text;
            var prostredniJmeno = MiddleNameInput.Text;
            var prijmeni = LastNameInput.Text;
            var email = EmailInput.Text;
            var telefon = PhoneInput.Text;
            var heslo = PasswordInput.Text;
            var stat = CountryInput.Text;
            var mesto = CityInput.Text;
            var ulice = StreetInput.Text;
            string psc = PostalCodeInput.Text;
            DateTime? datum_narozeni = datePicker.SelectedDate?.Date; 

            if ( datum_narozeni.HasValue )
            {
                ServiceReturn sr = UzivatelService.RegistraceUzivatele( jmeno, prostredniJmeno, prijmeni, telefon, email, heslo, datum_narozeni.Value, stat, mesto, ulice, psc);
                if ( sr.Uspech == true )
                {
                    var zpet = new MainWindow();
                    zpet.Show();
                    this.Close();
                }
            
                ChybovaHlaska.ShowError( this, sr.ChybovaHlaska );
                
            }

            // Zde můžete přidat logiku pro zpracování registrace, např. validace, uložení do databáze atd.
         //   MessageBox.Show("Registrace úspěšná");
        }
    }
}

