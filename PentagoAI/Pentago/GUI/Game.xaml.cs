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
using Pentago.GameCore;

namespace Pentago
{
    /// <summary>
    /// Interaction logic for Game.xaml
    /// </summary>
    public partial class Game : Window
    {
        private GameBrain gameBrain = null;

        private const int BOARDSIZE = 36;
        private const int MAXCOLUMNS = 6;
        private const int MAXROWS = 6;

        private bool userMadeRotation = true;

        private Player player1 = null;
        private Player player2 = null;

        public Game()
        {
            InitializeComponent();
            PaintBoard();
            //to initialize player, a switch case could be used
            //according to the options the user selected on previous
            //page
            player1 = new Player("player1", true, Brushes.Green);
            player2 = new Player("player2", false, Brushes.Blue);
            gameBrain = new GameBrain(player1, player2);
            ShowActivePlayer();
        }

        public void PaintBoard()
        {
            Board.Rows = MAXROWS;
            Board.Columns = MAXCOLUMNS;
            for (int i = 0; i < BOARDSIZE; i++)
            {
                Rectangle rect = new Rectangle();
                rect.Fill = Brushes.White;
                rect.Stroke = Brushes.Black;
                Board.Children.Add(rect);
            }
        }

        private void Board_MouseDown(object sender, MouseButtonEventArgs e)
        {
            int rectSize = (int)Board.Width / MAXCOLUMNS;

            Point mousePosition = e.GetPosition(Board);
            short row = (short)(mousePosition.Y / rectSize);
            short col = (short)(mousePosition.X / rectSize);
            int winner = gameBrain.CheckForWin();
            if (userMadeRotation && gameBrain.PlacePiece(row, col) && winner == 0)
            {
                userMadeRotation = false;

                Rectangle rec = (Rectangle)Board.Children[MAXCOLUMNS * row + col];
                if (gameBrain.isPlayer1Turn())
                    rec.Fill = player1.Fill;
                else
                    rec.Fill = player2.Fill;

                winner = gameBrain.CheckForWin();
                if (winner != 0)
                    ShowWinner(winner);
                else
                    MakeRotationsVisible();
            }
            else if (winner != 0)
                ShowWinner(winner);
        }

        private void ShowWinner(int winner)
        {
            string winnerAnnouncement = "";
            switch (winner)
            {
                case 1:
                    winnerAnnouncement = "Congratulations " + player1.Name + " you have won!";
                    break;
                case 2:
                    winnerAnnouncement = "Congratulations " + player2.Name + " you have won!";
                    break;
                case 3:
                    winnerAnnouncement = "It is a tie.";
                    break;
                default:
                    break;
            }

            MessageBoxResult result = MessageBox.Show(winnerAnnouncement, "Pentago", MessageBoxButton.OK);
            if (result == MessageBoxResult.OK)
                StartNewGame();
        }

        private void StartNewGame() 
        {
            gameBrain.ResetGame();
            RePaintBoard();
            userMadeRotation = true;
            player1 = new Player("player1", true, Brushes.Green);
            player2 = new Player("player2", false, Brushes.Blue);
            ShowActivePlayer();
        }

        private void ShowActivePlayer()
        {
            if (player1.ActivePlayer)
                ActivePlayer.Fill = player1.Fill;
            else
                ActivePlayer.Fill = player2.Fill;
        }

        private void RePaintBoard()
        {
            int[] tempBoard = gameBrain.GetBoard;
            for (int i = 0; i < BOARDSIZE; i++)
            {
                Rectangle rec = (Rectangle)Board.Children[i];
                
                if (tempBoard[i] == 1)
                    rec.Fill = player1.Fill;
                else if (tempBoard[i] == 2)
                    rec.Fill = player2.Fill;
                else
                    rec.Fill = Brushes.White;
            }
        }

        private void MakeRotationsVisible() 
        {
            btnClockWise1.Visibility = Visibility.Visible;
            btnCounterClockWise1.Visibility = Visibility.Visible;
            btnClockWise2.Visibility = Visibility.Visible;
            btnCounterClockWise2.Visibility = Visibility.Visible;
            btnClockWise3.Visibility = Visibility.Visible;
            btnCounterClockWise3.Visibility = Visibility.Visible;
            btnClockWise4.Visibility = Visibility.Visible;
            btnCounterClockWise4.Visibility = Visibility.Visible;
        }

        //Hide all rotations and show which player turn is it
        private void MakeRotationsHidden()
        {
            btnClockWise1.Visibility = Visibility.Hidden;
            btnCounterClockWise1.Visibility = Visibility.Hidden;
            btnClockWise2.Visibility = Visibility.Hidden;
            btnCounterClockWise2.Visibility = Visibility.Hidden;
            btnClockWise3.Visibility = Visibility.Hidden;
            btnCounterClockWise3.Visibility = Visibility.Hidden;
            btnClockWise4.Visibility = Visibility.Hidden;
            btnCounterClockWise4.Visibility = Visibility.Hidden;

            gameBrain.ChangeTurn();
            int winner = gameBrain.CheckForWin();
            if (winner != 0)
                ShowWinner(winner);
            else
                ShowActivePlayer();
        }

        private void btnCounterClockWise2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            gameBrain.RotateBoard(false, 2);
            RePaintBoard();
            userMadeRotation = true;
            MakeRotationsHidden();
        }

        private void btnClockWise1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            gameBrain.RotateBoard(true, 1);
            RePaintBoard();
            userMadeRotation = true;
            MakeRotationsHidden();
        }

        private void btnCounterClockWise1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            gameBrain.RotateBoard(false, 1);
            RePaintBoard();
            userMadeRotation = true;
            MakeRotationsHidden();
        }

        private void btnClockWise2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            gameBrain.RotateBoard(true, 2);
            RePaintBoard();
            userMadeRotation = true;
            MakeRotationsHidden();
        }

        private void btnClockWise3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            gameBrain.RotateBoard(true, 3);
            RePaintBoard();
            userMadeRotation = true;
            MakeRotationsHidden();
        }

        private void btnCounterClockWise3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            gameBrain.RotateBoard(false, 3);
            RePaintBoard();
            userMadeRotation = true;
            MakeRotationsHidden();
        }

        private void btnClockWise4_MouseDown(object sender, MouseButtonEventArgs e)
        {
            gameBrain.RotateBoard(true, 4);
            RePaintBoard();
            userMadeRotation = true;
            MakeRotationsHidden();
        }

        private void btnCounterClockWise4_MouseDown(object sender, MouseButtonEventArgs e)
        {
            gameBrain.RotateBoard(false, 4);
            RePaintBoard();
            userMadeRotation = true;
            MakeRotationsHidden();
        }
    }
}
