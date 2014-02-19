using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Pentago.GUI;
using Pentago.AI;

namespace Pentago
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnQuickMatch_Click(object sender, RoutedEventArgs e)
        {
            //Human vs Human
            //Idieally all this options will be set from GUI and then extracted
            //and passed to the gameOptions constructor
            string player1Name = "Diego Castillo";
            Brush player1Color = Brushes.Red;
            string player2Name = "Antonio Banderas";
            Brush player2Color = Brushes.Blue;

            GameOptions gameOptions = new GameOptions(GameOptions.TypeOfGame.QuickMatch, player1Name, player1Color, player2Name, player2Color);
            Window gameWindow = new Game(gameOptions);
            App.Current.MainWindow = gameWindow;
            gameWindow.Show();
        }

        private void btnQuickMatchAI_Click(object sender, RoutedEventArgs e)
        {
            //Human vs AI
            //Idieally all this options will be set from GUI and then extracted
            //and passed to the gameOptions constructor
            string player1Name = "Diego Castillo";
            Brush player1Color = Brushes.Red;
            computerAI.Difficulty  difficulty = computerAI.Difficulty.Hard;

            GameOptions gameOptions = new GameOptions(GameOptions.TypeOfGame.AI, player1Name, player1Color, difficulty);
            Window gameWindow = new Game(gameOptions);
            App.Current.MainWindow = gameWindow;
            gameWindow.Show();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
