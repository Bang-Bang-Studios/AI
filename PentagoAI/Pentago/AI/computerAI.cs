﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows;
using Pentago.GameCore;
using System.Diagnostics;

namespace Pentago.AI
{
    class computerAI
    {
        //AI player
        private string _Name;
        private bool _ActivePlayer;
        private Brush _Fill;
        private enum Difficulty { Easy, Hard };
        private Difficulty _DifficultyLevel;
        private int _MaxTreeDepth;

        //board constants
        private const int BOARDSIZE = 36;
        private const int MAXMOVES = 36;
        private const int BOARWIDTH = 6;

        //Related to the move selected by AI
        private int _Choice;
        private bool _IsClockWise;
        private short _Quad;
        private string _Active_Turn = "COMPUTER";
        private int[] _TempBoard = new int[BOARDSIZE];

        public computerAI (string name, bool isActive, Brush fill, int treeDepth)
        {
            this._Name = name;
            this._ActivePlayer = isActive;
            this._Fill = fill;
            this._MaxTreeDepth = treeDepth;
            if (treeDepth == 1)
                this._DifficultyLevel = Difficulty.Easy;
            else
                this._DifficultyLevel = Difficulty.Hard;
        }

        public bool ActivePlayer
        {
            set { this._ActivePlayer = value; }
            get { return this._ActivePlayer; }
        }

        public void MakeAIMove(Board board)
        {

            for (int i = 0; i < BOARDSIZE; i++)
                this._TempBoard[i] = board.GetPlayer(i);

            Stopwatch sw = Stopwatch.StartNew();
            alphaBeta(this._TempBoard, 0, double.NegativeInfinity, double.PositiveInfinity);
            sw.Stop();

            Console.WriteLine("Time taken: " + sw.Elapsed.TotalSeconds + " seconds.");
            //Console.WriteLine("_Choice: " + _Choice);
            //Console.WriteLine("_IsClockWise: " + _IsClockWise);
           // Console.WriteLine("_Quad: " + _Quad);
        }

        private double alphaBeta(int[] board, int treeDepth, double alpha, double beta)
        {
            //if statement should check if there is a winner also
            if (treeDepth == this._MaxTreeDepth || CheckForWin(board) != 0)
                return GameScore(board, treeDepth);

            treeDepth++;
            List<int> availableMoves = GetAvailableMoves(board);
            int move;
            double result;
            int[] possible_game;
            int maxNumerOfAvailableMoves = availableMoves.Count;
            //if it is computer
            if (_Active_Turn == "COMPUTER")
            {
                for (int i = 0; i < maxNumerOfAvailableMoves; i++)
                {
                    for (int quadrant = 1; quadrant < 5; quadrant++)
                    {
                        for (int rotationDirection = 0; rotationDirection < 2; rotationDirection++)
                        {
                            move = availableMoves.ElementAt(i);
                            possible_game = PlacePiece(move, board);
                            possible_game = MakeRotation(quadrant, rotationDirection, possible_game);
                            result = alphaBeta(possible_game, treeDepth, alpha, beta);
                            board = UndoMove(board, move, quadrant, rotationDirection);
                            if (result > alpha)
                            {
                                alpha = result;
                                if (treeDepth == 1)
                                {
                                    this._Choice = move;
                                    if (rotationDirection == 0)
                                        this._IsClockWise = false;
                                    else
                                        this._IsClockWise = true;
                                    this._Quad = (short)quadrant;
                                }
                            }
                            else if (alpha >= beta)
                            {
                                return alpha;
                            }
                        }
                    }
                }
                return alpha;
            }
            else
            {
                for (int i = 0; i < maxNumerOfAvailableMoves; i++)
                {
                    for (int quadrant = 1; quadrant < 5; quadrant++)
                    {
                        for (int rotationDirection = 0; rotationDirection < 2; rotationDirection++)
                        {
                            move = availableMoves.ElementAt(i);
                            possible_game = PlacePiece(move, board);
                            possible_game = MakeRotation(quadrant, rotationDirection, possible_game);
                            result = alphaBeta(possible_game, treeDepth, alpha, beta);
                            board = UndoMove(board, move, quadrant, rotationDirection);
                            if (result < beta)
                            {
                                beta = result;
                                if (treeDepth == 1) 
                                {
                                    this._Choice = move;
                                    if (rotationDirection == 0)
                                        this._IsClockWise = false;
                                    else
                                        this._IsClockWise = true;
                                    this._Quad = (short)quadrant;
                                }
                            }
                            else if (beta <= alpha)
                            {
                                return beta;
                            }
                        }
                    }
                }
                return beta;
            }
        }

        private int[] MakeRotation(int quadrant, int rotationDirection, int[] board)
        {
            int[] possible_game;
            switch (quadrant)
            {
                case 1:
                    if (rotationDirection == 1)
                        possible_game = RotateQuad1ClockWise(board);
                    else
                        possible_game = RotateQuad1CounterClockWise(board);
                    break;
                case 2:
                    if (rotationDirection == 1)
                        possible_game = RotateQuad2ClockWise(board);
                    else
                        possible_game = RotateQuad2CounterClockWise(board);
                    break;
                case 3:
                    if (rotationDirection == 1)
                        possible_game = RotateQuad3ClockWise(board);
                    else
                        possible_game = RotateQuad3CounterClockWise(board);
                    break;
                case 4:
                    if (rotationDirection == 1)
                        possible_game = RotateQuad4ClockWise(board);
                    else
                        possible_game = RotateQuad4CounterClockWise(board);
                    break;
                default:
                    //just to make compiler happy
                    possible_game = RotateQuad1ClockWise(board);
                    break;
            }
            ChangeTurn();
            return possible_game;
        }

        private int[] UndoMove(int[] board, int move, int c, int r)
        {
            switch (c)
            {
                case 1:
                    if (r != 1)
                        board = RotateQuad1ClockWise(board);
                    else
                        board = RotateQuad1CounterClockWise(board);
                    break;
                case 2:
                    if (r != 1)
                        board = RotateQuad2ClockWise(board);
                    else
                        board = RotateQuad2CounterClockWise(board);
                    break;
                case 3:
                    if (r != 1)
                        board = RotateQuad3ClockWise(board);
                    else
                        board = RotateQuad3CounterClockWise(board);
                    break;
                case 4:
                    if (r != 1)
                        board = RotateQuad4ClockWise(board);
                    else
                        board = RotateQuad4CounterClockWise(board);
                    break;
                default:
                    //just to make compiler happy
                    board = RotateQuad1ClockWise(board);
                    break;
            }
            board[move] = 0;
            ChangeTurn();
            return board;
        }

        private int[] PlacePiece(int move, int[] board)
        {
            int piece;
            if (_Active_Turn == "COMPUTER")
                piece = 2;
            else
                piece = 1;

            board[move] = piece;
            return board;
        }

        private void ChangeTurn()
        {
            if (_Active_Turn == "COMPUTER")
                _Active_Turn = "HUMAN";
            else
                _Active_Turn = "COMPUTER";
        }

        private List<int> GetAvailableMoves(int[] board)
        {
            List<int> possibleMoves = new List<int>();
            for (int i = 0; i < BOARDSIZE; i++)
                if (board[i] == 0)
                    possibleMoves.Add(i);
            return possibleMoves;
        }

        private int[] RotateQuad1ClockWise(int[] board)
        {
            int placeHolder = board[0];
            board[0] = board[12];
            board[12] = board[14];
            board[14] = board[2];
            board[2] = placeHolder;
            placeHolder = board[6];
            board[6] = board[13];
            board[13] = board[8];
            board[8] = board[1];
            board[1] = placeHolder;

            return board;
        }

        private int[] RotateQuad1CounterClockWise(int[] board)
        {
            int placeHolder = board[0];
            board[0] = board[2];
            board[2] = board[14];
            board[14] = board[12];
            board[12] = placeHolder;
            placeHolder = board[6];
            board[6] = board[1];
            board[1] = board[8];
            board[8] = board[13];
            board[13] = placeHolder;

            return board;
        }

        private int[] RotateQuad2ClockWise(int[] board)
        {

            int placeHolder = board[3];
            board[3] = board[15];
            board[15] = board[17];
            board[17] = board[5];
            board[5] = placeHolder;
            placeHolder = board[4];
            board[4] = board[9];
            board[9] = board[16];
            board[16] = board[11];
            board[11] = placeHolder;

            return board;
        }

        private int[] RotateQuad2CounterClockWise(int[] board)
        {
            int placeholder = board[3];
            board[3] = board[5];
            board[5] = board[17];
            board[17] = board[15];
            board[15] = placeholder;
            placeholder = board[4];
            board[4] = board[11];
            board[11] = board[16];
            board[16] = board[9];
            board[9] = placeholder;

            return board;
        }

        private int[] RotateQuad3ClockWise(int[] board)
        {
            int placeHolder = board[18];
            board[18] = board[30];
            board[30] = board[32];
            board[32] = board[20];
            board[20] = placeHolder;
            placeHolder = board[24];
            board[24] = board[31];
            board[31] = board[26];
            board[26] = board[19];
            board[19] = placeHolder;

            return board;
        }

        private int[] RotateQuad3CounterClockWise(int[] board)
        {
            int placeHolder = board[18];
            board[18] = board[20];
            board[20] = board[32];
            board[32] = board[30];
            board[30] = placeHolder;
            placeHolder = board[24];
            board[24] = board[19];
            board[19] = board[26];
            board[26] = board[31];
            board[31] = placeHolder;

            return board;
        }

        private int[] RotateQuad4ClockWise(int[] board)
        {
            int placeholder = board[21];
            board[21] = board[33];
            board[33] = board[35];
            board[35] = board[23];
            board[23] = placeholder;
            placeholder = board[22];
            board[22] = board[27];
            board[27] = board[34];
            board[34] = board[29];
            board[29] = placeholder;

            return board;
        }

        private int[] RotateQuad4CounterClockWise(int[] board)
        {
            int placeholder = board[21];
            board[21] = board[23];
            board[23] = board[35];
            board[35] = board[33];
            board[33] = placeholder;
            placeholder = board[22];
            board[22] = board[29];
            board[29] = board[34];
            board[34] = board[27];
            board[27] = placeholder;

            return board;
        }

        //A line is how many pieces a user has side by side
        //either horizontal, vertical or diagonal
        private int GameScore(int[] board, int treeDepth)
        {
            int score = 0;
            int checkWinner = CheckForWin(board);
            //tie score?
            if (checkWinner == 1)
                score += -100000;
            else if (checkWinner == 2)
                score += 100000;
            else
            {
                int piece;
                if (_Active_Turn == "COMPUTER")
                {
                    piece = 2;
                    score += CheckHorizontalOf2Lines(board, piece);
                    score += CheckHorizontalOf3Lines(board, piece);
                    score += CheckHorizontalOf4Lines(board, piece);
                    score += CheckVerticalOf2Lines(board, piece);
                    score += CheckVerticalOf3Lines(board, piece);
                    score += CheckVerticalOf4Lines(board, piece);
                }
                else
                {
                    piece = 1;
                    score -= CheckHorizontalOf2Lines(board, piece);
                    score -= CheckHorizontalOf3Lines(board, piece);
                    score -= CheckHorizontalOf4Lines(board, piece);
                    score -= CheckVerticalOf2Lines(board, piece);
                    score -= CheckVerticalOf3Lines(board, piece);
                    score -= CheckVerticalOf4Lines(board, piece);
                }
            }
            return score;
        }

        private int CheckHorizontalOf2Lines(int[] board, int piece)
        {
            int count = 0;
            for (int i = 0; i < BOARDSIZE - 2; i++)
                if (board[i] == piece && board[i + 1] == piece)
                    count += 4;
            return count;
        }

        private int CheckHorizontalOf3Lines(int[] board, int piece)
        {
            int count = 0;
            for (int i = 0; i < BOARDSIZE - 3; i++)
                if (board[i] == piece && board[i + 1] == piece && board[i + 2] == piece)
                    count += 6;
            return count;
        }

        private int CheckHorizontalOf4Lines(int[] board, int piece)
        {
            int count = 0;
            for (int i = 0; i < BOARDSIZE - 4; i++)
                if (board[i] == piece && board[i + 1] == piece && board[i + 2] == piece && board[i + 3] == piece)
                    count += 8;
            return count;
        }

        private int CheckVerticalOf2Lines(int[] board, int piece)
        {
            int count = 0;
            for (int i = 0; i < BOARDSIZE - 6; i++)
                if (board[i] == piece && board[i + 6] == piece)
                    count += 4;
            return count;
        }

        private int CheckVerticalOf3Lines(int[] board, int piece)
        {
            int count = 0;
            for (int i = 0; i < BOARDSIZE - 12; i++)
                if (board[i] == piece && board[i + 6] == piece && board[i + 12] == piece)
                    count += 6;
            return count;
        }

        private int CheckVerticalOf4Lines(int[] board, int piece)
        {
            int count = 0;
            for (int i = 0; i < BOARDSIZE - 18; i++)
                if (board[i] == piece && board[i + 6] == piece && board[i + 12] == piece && board[i + 18] == piece)
                    count += 8;
            return count;
        }

        public string Name
        {
            get { return this._Name; }
        }

        public Brush Fill
        {
            get { return this._Fill; }
        }

        public int GetMoveChoice()
        {
            return this._Choice;
        }

        public bool GetRotationDirection()
        {
            return this._IsClockWise;
        }

        public short GetCuadrant()
        {
            return this._Quad;
        }


        public int CheckForWin(int[] board)
        {
            bool res = true;
            bool p1w = false;
            bool p2w = false;
            bool tie = false;
            int numMoves = 0;
            for (int i = 0; i < BOARDSIZE; i++)
                if (board[i] != 0)
                    numMoves++;

            if (numMoves >= 9) // First check to see if it's even possible to win (Fifth move for player 1)
            {
                // Check for horizontal win. If no win, continue to checking vert and diag.
                int horiz = CheckHorizontals();
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

                int vert = CheckVerticals();
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

                int diag = CheckDiags();
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
                    return 0; // The game continues
                if (tie || (p1w && p2w))
                    return 3;
                if (p1w)
                    return 1;
                if (p2w)
                    return 2;
                if (numMoves == MAXMOVES)
                    return 3;
            }
            return 0;
        }

        private int CheckHorizontals()
        {
            bool res = true;
            bool p1w = false;
            bool p2w = false;

            int returnValue = 0;
            short[] possibilities = new short[12];
            possibilities[0] = (short)CheckPiecesOnBoard(new Point(0, 0), new Point(0, 1), new Point(0, 2), new Point(0, 3), new Point(0, 4));
            possibilities[1] = (short)CheckPiecesOnBoard(new Point(0, 1), new Point(0, 2), new Point(0, 3), new Point(0, 4), new Point(0, 5));
            possibilities[2] = (short)CheckPiecesOnBoard(new Point(1, 0), new Point(1, 1), new Point(1, 2), new Point(1, 3), new Point(1, 4));
            possibilities[3] = (short)CheckPiecesOnBoard(new Point(1, 1), new Point(1, 2), new Point(1, 3), new Point(1, 4), new Point(1, 5));
            possibilities[4] = (short)CheckPiecesOnBoard(new Point(2, 0), new Point(2, 1), new Point(2, 2), new Point(2, 3), new Point(2, 4));
            possibilities[5] = (short)CheckPiecesOnBoard(new Point(2, 1), new Point(2, 2), new Point(2, 3), new Point(2, 4), new Point(2, 5));
            possibilities[6] = (short)CheckPiecesOnBoard(new Point(3, 0), new Point(3, 1), new Point(3, 2), new Point(3, 3), new Point(3, 4));
            possibilities[7] = (short)CheckPiecesOnBoard(new Point(3, 1), new Point(3, 2), new Point(3, 3), new Point(3, 4), new Point(3, 5));
            possibilities[8] = (short)CheckPiecesOnBoard(new Point(4, 0), new Point(4, 1), new Point(4, 2), new Point(4, 3), new Point(4, 4));
            possibilities[9] = (short)CheckPiecesOnBoard(new Point(4, 1), new Point(4, 2), new Point(4, 3), new Point(4, 4), new Point(4, 5));
            possibilities[10] = (short)CheckPiecesOnBoard(new Point(5, 0), new Point(5, 1), new Point(5, 2), new Point(5, 3), new Point(5, 4));
            possibilities[11] = (short)CheckPiecesOnBoard(new Point(5, 1), new Point(5, 2), new Point(5, 3), new Point(5, 4), new Point(5, 5));

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
                return 0;
            if (p1w && p2w)
                return 3;
            if (p1w)
                return 1;
            if (p2w)
                return 2;
            return returnValue;
        }

        private int CheckVerticals()
        {
            bool res = true;
            bool p1w = false;
            bool p2w = false;

            int returnValue = 0;
            short[] possibilities = new short[12];
            possibilities[0] = (short)CheckPiecesOnBoard(new Point(0, 0), new Point(1, 0), new Point(2, 0), new Point(3, 0), new Point(4, 0));
            possibilities[1] = (short)CheckPiecesOnBoard(new Point(1, 0), new Point(2, 0), new Point(3, 0), new Point(4, 0), new Point(5, 0));
            possibilities[2] = (short)CheckPiecesOnBoard(new Point(0, 1), new Point(1, 1), new Point(2, 1), new Point(3, 1), new Point(4, 1));
            possibilities[3] = (short)CheckPiecesOnBoard(new Point(1, 1), new Point(2, 1), new Point(3, 1), new Point(4, 1), new Point(5, 1));
            possibilities[4] = (short)CheckPiecesOnBoard(new Point(0, 2), new Point(1, 2), new Point(2, 2), new Point(3, 2), new Point(4, 2));
            possibilities[5] = (short)CheckPiecesOnBoard(new Point(1, 2), new Point(2, 2), new Point(3, 2), new Point(4, 2), new Point(5, 2));
            possibilities[6] = (short)CheckPiecesOnBoard(new Point(0, 3), new Point(1, 3), new Point(2, 3), new Point(3, 3), new Point(4, 3));
            possibilities[7] = (short)CheckPiecesOnBoard(new Point(1, 3), new Point(2, 3), new Point(3, 3), new Point(4, 3), new Point(5, 3));
            possibilities[8] = (short)CheckPiecesOnBoard(new Point(0, 4), new Point(1, 4), new Point(2, 4), new Point(3, 4), new Point(4, 4));
            possibilities[9] = (short)CheckPiecesOnBoard(new Point(1, 4), new Point(2, 4), new Point(3, 4), new Point(4, 4), new Point(5, 4));
            possibilities[10] = (short)CheckPiecesOnBoard(new Point(0, 5), new Point(1, 5), new Point(2, 5), new Point(3, 5), new Point(4, 5));
            possibilities[11] = (short)CheckPiecesOnBoard(new Point(1, 5), new Point(2, 5), new Point(3, 5), new Point(4, 5), new Point(5, 5));

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
                return 0;
            if (p1w && p2w)
                return 3;
            if (p1w)
                return 1;
            if (p2w)
                return 2;
            return returnValue;
        }

        private int CheckDiags()
        {
            bool res = true;
            bool p1w = false;
            bool p2w = false;

            int returnValue = 0;
            short[] possibilities = new short[8];

            // Top Left to Bottom Rights
            possibilities[0] = (short)CheckPiecesOnBoard(new Point(0, 1), new Point(1, 2), new Point(2, 3), new Point(3, 4), new Point(4, 5));
            possibilities[1] = (short)CheckPiecesOnBoard(new Point(0, 0), new Point(1, 1), new Point(2, 2), new Point(3, 3), new Point(4, 4));
            possibilities[2] = (short)CheckPiecesOnBoard(new Point(1, 1), new Point(2, 2), new Point(3, 3), new Point(4, 4), new Point(5, 5));
            possibilities[3] = (short)CheckPiecesOnBoard(new Point(1, 0), new Point(2, 1), new Point(3, 2), new Point(4, 3), new Point(5, 4));
            // Bottom Left to Top Rights
            possibilities[4] = (short)CheckPiecesOnBoard(new Point(0, 4), new Point(1, 3), new Point(2, 2), new Point(3, 1), new Point(4, 0));
            possibilities[5] = (short)CheckPiecesOnBoard(new Point(0, 5), new Point(1, 4), new Point(2, 3), new Point(3, 2), new Point(4, 1));
            possibilities[6] = (short)CheckPiecesOnBoard(new Point(1, 4), new Point(2, 3), new Point(3, 2), new Point(4, 1), new Point(5, 0));
            possibilities[7] = (short)CheckPiecesOnBoard(new Point(1, 5), new Point(2, 4), new Point(3, 3), new Point(4, 2), new Point(5, 1));

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
                return 0;
            if (p1w && p2w)
                return 3;
            if (p1w)
                return 1;
            if (p2w)
                return 2;
            return returnValue;
        }

        private int CheckPiecesOnBoard(Point piece1, Point piece2, Point piece3, Point piece4, Point piece5)
        {
            int playerAtPiece1 = _TempBoard[BOARWIDTH * (int)piece1.X + (int)piece1.Y];
            int playerAtPiece2 = _TempBoard[BOARWIDTH * (int)piece2.X + (int)piece2.Y];
            int playerAtPiece3 = _TempBoard[BOARWIDTH * (int)piece3.X + (int)piece3.Y];
            int playerAtPiece4 = _TempBoard[BOARWIDTH * (int)piece4.X + (int)piece4.Y];
            int playerAtPiece5 = _TempBoard[BOARWIDTH * (int)piece5.X + (int)piece5.Y];

            if (playerAtPiece1 == playerAtPiece2 && playerAtPiece2 == playerAtPiece3 &&
                playerAtPiece3 == playerAtPiece4 && playerAtPiece4 == playerAtPiece5)
                return playerAtPiece1;
            return 0;
        }
    }
}