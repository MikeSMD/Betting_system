using System;
using Avalonia.Controls;
using System.Threading.Tasks;
using Avalonia.Layout;
using Avalonia.Media;

namespace SystemSazek.UI
{
    public static class ChybovaHlaska
    {
        public static async Task ShowError(Window owner, string errorMessage)
        {
            var dialog = new Window
            {
                Title = "Chyba",
                Width = 300,
                Height = 150,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Content = new StackPanel
                {
                    Spacing = 10,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Children =
                    {
                        new TextBlock
                        {
                            Text = errorMessage,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            TextWrapping = TextWrapping.Wrap,
                            FontSize = 14,
                            Margin = new Avalonia.Thickness(10)
                        },
                        new Button
                        {
                            Content = "Zavřít",
                            HorizontalAlignment = HorizontalAlignment.Center,
                            Padding = new Avalonia.Thickness(10),
                            Background = Brushes.LightGray
                        }
                    }
                }
            };

            // Přiřadíme funkci zavření k tlačítku
            var closeButton = (dialog.Content as StackPanel)?.Children[1] as Button;
            if (closeButton != null)
            {
                closeButton.Click += (_, __) => dialog.Close();
            }

            // Zobrazíme dialog
            await dialog.ShowDialog(owner);
        }
    }
}

