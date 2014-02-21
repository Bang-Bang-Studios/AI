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
using Pentago.AI;
using Pentago.GUI;

//just for visual help
using System.Threading;

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

        //These are the players for the GUI
        private Player player1 = null;
        private Player player2 = null;
        private computerAI computerPlayer = null;
        GameOptions gameOptions = null;

        public Game(GameOptions options)
        {
            InitializeComponent();
            gameOptions = options;
            PaintBoard();
            switch (gameOptions._TypeOfGame)
            {
                case GameOptions.TypeOfGame.QuickMatch:
                    player1 = new Player(gameOptions._Player1Name, true, gameOptions._Player1Color);
                    player2 = new Player(gameOptions._Player2Name, false, gameOptions._Player2Color);
                    gameBrain = new GameBrain(player1);
                    break;
                case GameOptions.TypeOfGame.AI:
                    player1 = new Player(gameOptions._Player1Name, true, gameOptions._Player1Color);
                    computerPlayer = new computerAI("I'm your boss", false, Brushes.Green, gameOptions._Difficulty);
                    gameBrain = new GameBrain(player1, computerPlayer);
                    if (!player1.ActivePlayer)
                        GetComputerMove();
                    break;
                default:
                    break;
            }
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
            if (gameOptions._TypeOfGame == GameOptions.TypeOfGame.QuickMatch || player1.ActivePlayer)
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
                    if (gameOptions._TypeOfGame == GameOptions.TypeOfGame.QuickMatch)
                        winnerAnnouncement = "Congratulations " + player2.Name + " you have won!";
                    else if (gameOptions._TypeOfGame == GameOptions.TypeOfGame.AI)
                        winnerAnnouncement = "Congratulations " + computerPlayer.Name + " you have won!";
                    break;
                case 3:
                    winnerAnnouncement = "It is a tie.";
                    break;
                default:
                    break;
            }

            MessageBox.Show(winnerAnnouncement, "Pentago", MessageBoxButton.OK);
        }

        private void ShowActivePlayer()
        {
            if (gameOptions._TypeOfGame == GameOptions.TypeOfGame.QuickMatch)
            {
                if (player1.ActivePlayer)
                    ActivePlayer.Fill = player1.Fill;
                else
                    ActivePlayer.Fill = player2.Fill;
            }
            else if (gameOptions._TypeOfGame == GameOptions.TypeOfGame.AI)
            {
                if (player1.ActivePlayer)
                    ActivePlayer.Fill = player1.Fill;
                else
                    ActivePlayer.Fill = computerPlayer.Fill;
            }
        }

        private void RePaintBoard()
        {
            int[] tempBoard = gameBrain.GetBoard;
            for (int i = 0; i < BOARDSIZE; i++)
            {
                Rectangle rec = (Rectangle)Board.Children[i];
                if (gameOptions._TypeOfGame == GameOptions.TypeOfGame.QuickMatch)
                {
                    if (tempBoard[i] == 1)
                        rec.Fill = player1.Fill;
                    else if (tempBoard[i] == 2)
                        rec.Fill = player2.Fill;
                    else
                        rec.Fill = Brushes.White;
                }
                else if (gameOptions._TypeOfGame == GameOptions.TypeOfGame.AI)
                {
                    if (tempBoard[i] == 1)
                        rec.Fill = player1.Fill;
                    else if (tempBoard[i] == 2)
                        rec.Fill = computerPlayer.Fill;
                    else
                        rec.Fill = Brushes.White;
                }
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
            userMadeRotation = true;
            btnClockWise1.Visibility = Visibility.Hidden;
            btnCounterClockWise1.Visibility = Visibility.Hidden;
            btnClockWise2.Visibility = Visibility.Hidden;
            btnCounterClockWise2.Visibility = Visibility.Hidden;
            btnClockWise3.Visibility = Visibility.Hidden;
            btnCounterClockWise3.Visibility = Visibility.Hidden;
            btnClockWise4.Visibility = Visibility.Hidden;
            btnCounterClockWise4.Visibility = Visibility.Hidden;

            //Changes turn of player in GUI
            player1.ActivePlayer = gameBrain.isPlayer1Turn();

            int winner = gameBrain.CheckForWin();
            if (winner != 0)
                ShowWinner(winner);
            else
                ShowActivePlayer();

            //if this is a human vs computer game
            if (gameOptions._TypeOfGame == GameOptions.TypeOfGame.AI)
            {
                if (!player1.ActivePlayer && winner == 0)
                    GetComputerMove();
            }
        }

        private void GetComputerMove()
        {
            int winner = gameBrain.CheckForWin();
            if (userMadeRotation && gameBrain.MakeComputerMove() && winner == 0)
            {
                userMadeRotation = false;
                //Update GUI player
                int computerMove = gameBrain.GetComputerMove();
                Rectangle rec = (Rectangle)Board.Children[computerMove];
                rec.Fill = computerPlayer.Fill;

                winner = gameBrain.CheckForWin();
                if (winner != 0)
                    ShowWinner(winner);
                else
                    GetComputerRotation();
            }
            else if (winner != 0)
                ShowWinner(winner);
        }

        private void GetComputerRotation()
        {
            gameBrain.MakeComputerRotation();
            MakeRotationsHidden();
            RePaintBoard();
        }

        private void btnCounterClockWise2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            gameBrain.RotateBoard(false, 2);
            RePaintBoard();
            MakeRotationsHidden();
        }

        private void btnClockWise1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            gameBrain.RotateBoard(true, 1);
            RePaintBoard();
            MakeRotationsHidden();
        }

        private void btnCounterClockWise1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            gameBrain.RotateBoard(false, 1);
            RePaintBoard();
            MakeRotationsHidden();
        }

        private void btnClockWise2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            gameBrain.RotateBoard(true, 2);
            RePaintBoard();
            MakeRotationsHidden();
        }

        private void btnClockWise3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            gameBrain.RotateBoard(true, 3);
            RePaintBoard();
            MakeRotationsHidden();
        }

        private void btnCounterClockWise3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            gameBrain.RotateBoard(false, 3);
            RePaintBoard();
            MakeRotationsHidden();
        }

        private void btnClockWise4_MouseDown(object sender, MouseButtonEventArgs e)
        {
            gameBrain.RotateBoard(true, 4);
            RePaintBoard();
            MakeRotationsHidden();
        }

        private void btnCounterClockWise4_MouseDown(object sender, MouseButtonEventArgs e)
        {
            gameBrain.RotateBoard(false, 4);
            RePaintBoard();
            MakeRotationsHidden();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Window main = new MainWindow();
            App.Current.MainWindow = main;
            this.Hide();
            main.Show();
            
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Window main = new MainWindow();
            App.Current.MainWindow = main;
            this.Hide();
            main.Show();
        }
    }
}
