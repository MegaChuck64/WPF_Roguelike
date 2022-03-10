using System.Windows;

namespace WPF_Roguelike;
public partial class MainWindow : Window
{
    TestGame game;
    public MainWindow()
    {
        InitializeComponent();
    }


    private void PlayButton_Click(object sender, RoutedEventArgs e)
    {
        GameOverLabel.Content = string.Empty;

        game?.Dispose();
        game = new TestGame(this);

        GameGrid.Children.Clear();
        GameGrid.Children.Add(game);
    }

    private void EnterButton_Click(object sender, RoutedEventArgs e)
    {
        game.ResetMap();
    }
}

