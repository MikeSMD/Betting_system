using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Threading.Tasks;

namespace SystemSazek.UI
{
    public partial class Potvrzeni : Window
    {
        public bool Confirmed { get; private set; }

        public Potvrzeni()
        {
            BuildUI();
        }

        private void BuildUI()
        {
            // Nastavení základního layoutu okna
            var stackPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(20)
            };

            // Přidání textu potvrzení
            var textBlock = new TextBlock
            {
                Text = "Chcete potvrdit akci?",
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 10)
            };
            stackPanel.Children.Add(textBlock);

            // Přidání tlačítek Ano a Ne
            var buttonPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center,
                Spacing = 10
            };

            var anoButton = new Button
            {
                Content = "Ano",
                Width = 75
            };
            anoButton.Click += Ok;
            buttonPanel.Children.Add(anoButton);

            var neButton = new Button
            {
                Content = "Ne",
                Width = 75
            };
            neButton.Click += Zrusit;
            buttonPanel.Children.Add(neButton);

            stackPanel.Children.Add(buttonPanel);

            // Nastavení obsahu okna
            this.Content = stackPanel;
            this.Width = 300;
            this.Height = 150;
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }

        public async Task<bool> ShowDialogAsync(Window owner)
        {
            Owner = owner;
            await ShowDialog(owner);
            return Confirmed;
        }

        private void Ok(object sender, RoutedEventArgs e)
        {
            Confirmed = true;
            Close();
        }

        private void Zrusit(object sender, RoutedEventArgs e)
        {
            Confirmed = false;
            Close();
        }
    }
}

