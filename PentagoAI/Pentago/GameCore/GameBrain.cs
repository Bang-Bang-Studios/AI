using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows;

namespace Pentago.GameCore
{
    class GameBrain
    {
        private Board board = null;
        private Player player1 = null;
        private Player player2 = null;
        private const int MAXMOVES = 36;

        public GameBrain(Player player1, Player player2)
        {
            this.player1 = player1;
            this.player2 = player2;
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            board = new Board();
        }

        public bool PlacePiece(short row, short col)
        {
            int player;
            if (player1.ActivePlayer)
                player = 1;
            else
                player = 2;

            if (ValidateMove(row, col))
            {
                board.UpdateBoard(row, col, player);
                return true;
            }
            else
            {
                //Show the user thats a bad move
                return false;
            }
        }

        public void RotateBoard(bool rotateClockWise, short quad)
        {
            switch (quad)
            {
                case 1:
                    if (rotateClockWise)
                        board.RotateQuad1ClockWise();
                    else
                        board.RotateQuad1CounterClockWise();
                    break;
                case 2:
                    if (rotateClockWise)
                        board.RotateQuad2ClockWise();
                    else
                        board.RotateQuad2CounterClockWise();
                    break;
                case 3:
                    if (rotateClockWise)
                        board.RotateQuad3ClockWise();
                    else
                        board.RotateQuad3CounterClockWise();
                    break;
                case 4:
                    if (rotateClockWise)
                        board.RotateQuad4ClockWise();
                    else
                        board.RotateQuad4CounterClockWise();
                    break;
                default:
                    break;
            }
        }

        private bool ValidateMove(short row, short col)
        {
            if (board.GetPlayer(row, col) == 0)
                return true;
            return false;
        }

        public bool isPlayer1Turn()
        {
            return player1.ActivePlayer;
        }

        public void ChangeTurn()
        {
            if (player1.ActivePlayer)
            {
                player1.ActivePlayer = false;
                player2.ActivePlayer = true;
            }
            else
            {
                player1.ActivePlayer = true;
                player2.ActivePlayer = false;
            }
        }

        public int[] GetBoard
        {
            get { return board.GetBoard; }
        }

        public void ResetGame()
        {
            board.ClearBoard();
        }

        public int CheckForWin()
        {
            bool res = true;
            bool p1w = false;
            bool p2w = false;
            bool tie = false;
            int numMoves = board.PiecesOnBoard();

            if (numMoves >= 9) // First check to see if it's even possible to win (Fifth move for player 1)
            {
                // Check for horizontal win. If no win, continue to checking vert and diag.
                int horiz = checkHorizontals();
                if (horiz == 0) // No one won on a horizontal. Check for verticals.
                {

                }
                else if (horiz == 1) // Player 1 won on a horizontal
                {
                    p1w = true;
                    res = false;
                }
                else if (horiz == 2) // Player 2 wins on a horizontal
                {
                    p2w = true;
                    res = false;
                }
                else
                {
                    tie = true;
                    res = false;
                }

                int vert = checkVerticals();

                if (vert == 0) // No one won on a vertical. Check for diagonals.
                {

                }
                else if (vert == 1) // Player 1 won on a vertical
                {
                    p1w = true;
                    res = false;
                }
                else if (vert == 2) // Player 2 won on a vertical
                {
                    p2w = true;
                    res = false;
                }
                else // vert is 3 (A tie was caused by the move)
                {
                    tie = true;
                    res = false;
                }

                int diag = checkDiags();
                if (diag == 0) // No one won on a diagonal. Check to see if it's possible to make more moves.
                {
                }
                else if (diag == 1) // Player 1 won on a diagonal
                {
                    p1w = true;
                    res = false;
                }
                else if (diag == 2) // Player 2 won on a diagonal
                {
                    p2w = true;
                    res = false;
                }
                else // diag is 3 (A tie was caused by the move)
                {
                    tie = true;
                    res = false;
                }


                if (res && numMoves < MAXMOVES)
                {
                    return 0; // The game continues
                }
                if (tie || (p1w && p2w))
                {
                    return 3;
                }
                if (p1w)
                {
                    return 1;
                }
                if (p2w)
                {
                    return 2;
                }
                if (numMoves == MAXMOVES)
                {
                    return 3;
                }
            }
            return 0;
        }

        int checkHorizontals()
        {
            bool res = true;
            bool p1w = false;
            bool p2w = false;

            int returnValue = 0;
            short[] possibilities = new short[12];
            possibilities[0] = (short)checkPiecesOnBoard(new Point(0, 0), new Point(0, 1), new Point(0, 2), new Point(0, 3), new Point(0, 4));
            possibilities[1] = (short)checkPiecesOnBoard(new Point(0, 1), new Point(0, 2), new Point(0, 3), new Point(0, 4), new Point(0, 5));
            possibilities[2] = (short)checkPiecesOnBoard(new Point(1, 0), new Point(1, 1), new Point(1, 2), new Point(1, 3), new Point(1, 4));
            possibilities[3] = (short)checkPiecesOnBoard(new Point(1, 1), new Point(1, 2), new Point(1, 3), new Point(1, 4), new Point(1, 5));
            possibilities[4] = (short)checkPiecesOnBoard(new Point(2, 0), new Point(2, 1), new Point(2, 2), new Point(2, 3), new Point(2, 4));
            possibilities[5] = (short)checkPiecesOnBoard(new Point(2, 1), new Point(2, 2), new Point(2, 3), new Point(2, 4), new Point(2, 5));
            possibilities[6] = (short)checkPiecesOnBoard(new Point(3, 0), new Point(3, 1), new Point(3, 2), new Point(3, 3), new Point(3, 4));
            possibilities[7] = (short)checkPiecesOnBoard(new Point(3, 1), new Point(3, 2), new Point(3, 3), new Point(3, 4), new Point(3, 5));
            possibilities[8] = (short)checkPiecesOnBoard(new Point(4, 0), new Point(4, 1), new Point(4, 2), new Point(4, 3), new Point(4, 4));
            possibilities[9] = (short)checkPiecesOnBoard(new Point(4, 1), new Point(4, 2), new Point(4, 3), new Point(4, 4), new Point(4, 5));
            possibilities[10] = (short)checkPiecesOnBoard(new Point(5, 0), new Point(5, 1), new Point(5, 2), new Point(5, 3), new Point(5, 4));
            possibilities[11] = (short)checkPiecesOnBoard(new Point(5, 1), new Point(5, 2), new Point(5, 3), new Point(5, 4), new Point(5, 5));

            foreach (short s in possibilities)
            {
                if (s == 1)
                {
                    p1w = true;
                    res = false;
                }
                if (s == 2)
                {
                    p2w = true;
                    res = false;
                }
            }

            if (res)
            {
                return 0;
            }
            if (p1w && p2w)
            {
                return 3;
            }
            if (p1w)
            {
                return 1;
            }
            if (p2w)
            {
                return 2;
            }
            return returnValue;
        }

        int checkVerticals()
        {
            bool res = true;
            bool p1w = false;
            bool p2w = false;

            int returnValue = 0;
            short[] possibilities = new short[12];

            possibilities[0] = (short)checkPiecesOnBoard(new Point(0, 0), new Point(1, 0), new Point(2, 0), new Point(3, 0), new Point(4, 0));
            possibilities[1] = (short)checkPiecesOnBoard(new Point(1, 0), new Point(2, 0), new Point(3, 0), new Point(4, 0), new Point(5, 0));
            possibilities[2] = (short)checkPiecesOnBoard(new Point(0, 1), new Point(1, 1), new Point(2, 1), new Point(3, 1), new Point(4, 1));
            possibilities[3] = (short)checkPiecesOnBoard(new Point(1, 1), new Point(2, 1), new Point(3, 1), new Point(4, 1), new Point(5, 1));
            possibilities[4] = (short)checkPiecesOnBoard(new Point(0, 2), new Point(1, 2), new Point(2, 2), new Point(3, 2), new Point(4, 2));
            possibilities[5] = (short)checkPiecesOnBoard(new Point(1, 2), new Point(2, 2), new Point(3, 2), new Point(4, 2), new Point(5, 2));
            possibilities[6] = (short)checkPiecesOnBoard(new Point(0, 3), new Point(1, 3), new Point(2, 3), new Point(3, 3), new Point(4, 3));
            possibilities[7] = (short)checkPiecesOnBoard(new Point(1, 3), new Point(2, 3), new Point(3, 3), new Point(4, 3), new Point(5, 3));
            possibilities[8] = (short)checkPiecesOnBoard(new Point(0, 4), new Point(1, 4), new Point(2, 4), new Point(3, 4), new Point(4, 4));
            possibilities[9] = (short)checkPiecesOnBoard(new Point(1, 4), new Point(2, 4), new Point(3, 4), new Point(4, 4), new Point(5, 4));
            possibilities[10] = (short)checkPiecesOnBoard(new Point(0, 5), new Point(1, 5), new Point(2, 5), new Point(3, 5), new Point(4, 5));
            possibilities[11] = (short)checkPiecesOnBoard(new Point(1, 5), new Point(2, 5), new Point(3, 5), new Point(4, 5), new Point(5, 5));

            foreach (short s in possibilities)
            {
                if (s == 1)
                {
                    p1w = true;
                    res = false;
                }
                if (s == 2)
                {
                    p2w = true;
                    res = false;
                }
            }

            if (res)
            {
                return 0;
            }
            if (p1w && p2w)
            {
                return 3;
            }
            if (p1w)
            {
                return 1;
            }
            if (p2w)
            {
                return 2;
            }
            return returnValue;
        }

        int checkDiags()
        {
            bool res = true;
            bool p1w = false;
            bool p2w = false;

            int returnValue = 0;
            short[] possibilities = new short[8];

            // Top Left to Bottom Rights
            possibilities[0] = (short)checkPiecesOnBoard(new Point(0, 1), new Point(1, 2), new Point(2, 3), new Point(3, 4), new Point(4, 5));
            possibilities[1] = (short)checkPiecesOnBoard(new Point(0, 0), new Point(1, 1), new Point(2, 2), new Point(3, 3), new Point(4, 4));
            possibilities[2] = (short)checkPiecesOnBoard(new Point(1, 1), new Point(2, 2), new Point(3, 3), new Point(4, 4), new Point(5, 5));
            possibilities[3] = (short)checkPiecesOnBoard(new Point(1, 0), new Point(2, 1), new Point(3, 2), new Point(4, 3), new Point(5, 4));

            // Bottom Left to Top Rights
            possibilities[4] = (short)checkPiecesOnBoard(new Point(0, 4), new Point(1, 3), new Point(2, 2), new Point(3, 1), new Point(4, 0));
            possibilities[5] = (short)checkPiecesOnBoard(new Point(0, 5), new Point(1, 4), new Point(2, 3), new Point(3, 2), new Point(4, 1));
            possibilities[6] = (short)checkPiecesOnBoard(new Point(1, 4), new Point(2, 3), new Point(3, 2), new Point(4, 1), new Point(5, 0));
            possibilities[7] = (short)checkPiecesOnBoard(new Point(1, 5), new Point(2, 4), new Point(3, 3), new Point(4, 2), new Point(5, 1));

            foreach (short s in possibilities)
            {
                if (s == 1)
                {
                    p1w = true;
                    res = false;
                }
                if (s == 2)
                {
                    p2w = true;
                    res = false;
                }
            }

            if (res)
            {
                return 0;
            }
            if (p1w && p2w)
            {
                return 3;
            }
            if (p1w)
            {
                return 1;
            }
            if (p2w)
            {
                return 2;
            }
            return returnValue;

        }

        int checkPiecesOnBoard(Point piece1, Point piece2, Point piece3, Point piece4, Point piece5)
        {
            int playerAtPiece1 = board.GetPlayer((short)piece1.X, (short)piece1.Y);
            int playerAtPiece2 = board.GetPlayer((short)piece2.X, (short)piece2.Y);
            int playerAtPiece3 = board.GetPlayer((short)piece3.X, (short)piece3.Y);
            int playerAtPiece4 = board.GetPlayer((short)piece4.X, (short)piece4.Y);
            int playerAtPiece5 = board.GetPlayer((short)piece5.X, (short)piece5.Y);

            if (playerAtPiece1 == playerAtPiece2 && playerAtPiece2 == playerAtPiece3 &&
                playerAtPiece3 == playerAtPiece4 && playerAtPiece4 == playerAtPiece5)
            {
                return playerAtPiece1;
            }
            return 0;
        }
    }
}
