using Avalonia.Controls;
using SystemSazek.Core.Sazky;
using System.Threading.Tasks;

namespace SystemSazek.UI
{

    public partial class Prihlaseni : Window
    {
        public Prihlaseni()
        {
            InitializeComponent();
        }

         private void OnLoginClicked(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var username = UsernameInput.Text;
            var password = PasswordInput.Text;

            Uzivatel uzivatel = UzivatelService.PrihlaseniUzivatele( username, password );
            if ( uzivatel != null )
            {
              var newWindow = new MainMenu( uzivatel );
              newWindow.Show();
              this.Close(); 
            }
            else
            {
                ChybovaHlaska.ShowError( this, "Zadane neplatne udaje" );
            }
            
        //    var newWindow = new MainMenu();
        //    newWindow.Show();
        //    this.Close();

           
           
        } 
    }
}

