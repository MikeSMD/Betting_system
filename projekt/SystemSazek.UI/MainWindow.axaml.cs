using Avalonia.Controls;

namespace SystemSazek.UI;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        this.Title = "Grand Theft Auto VI"; 
    }

 private void OnPrihlaseniClicked(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var newWindow = new Prihlaseni();
        newWindow.Show();
        this.Close();
    }
  private void OnRegistraceClicked(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var newWindow = new Registrace();
        newWindow.Show();
        this.Close();
    }
    
}
