using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using Pentago.GameCore;

namespace Pentago.AI
{
    class computerAI
    {
        private string _Name;
        private bool _ActivePlayer;
        private Brush _Fill;
        private enum Difficulty { Easy, Hard };
        private Difficulty _DifficultyLevel;
        private int _MaxTreeDepth;

        //board constants
        private const int BOARDSIZE = 36;

        //Related to the move selected by AI
        private short _MoveRow;
        private short _MoveCol;
        private bool _IsClockWise;
        private short _Quad;
        private int _Choice;
        private string _Active_Turn = "COMPUTER";

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
            int[] tempBoard = new int[BOARDSIZE];

            for (int i = 0; i < BOARDSIZE; i++)
                tempBoard[i] = board.GetPlayer(i);

            alphaBeta(tempBoard, 0, double.PositiveInfinity, double.NegativeInfinity);
        }

        private double alphaBeta(int[] board, int treeDepth, double alpha, double beta)
        {
            if (treeDepth == this._MaxTreeDepth)
                return GameScore(board, treeDepth);

            treeDepth++;
            List<int> availableMoves = GetAvailableMoves(board);
            int move;
            double result;
            int[] possible_game;
            //if it is computer
            if (_Active_Turn == "COMPUTER")
            {
                for (int i = 0; i < availableMoves.Count; i++)
                {
                    for (int c = 0; c < 4; c++)
                    {
                        for (int r = 0; r < 2; r++)
                        {
                            move = availableMoves.ElementAt(i);
                            possible_game = PlacePiece(move, board);
                            switch (c)
                            {
                                case 1:
                                    if (r == 1)
                                        possible_game = RotateQuad1ClockWise(board);
                                    else
                                        possible_game = RotateQuad1CounterClockWise(board);
                                    break;
                                case 2:
                                    if (r == 1)
                                        possible_game = RotateQuad2ClockWise(board);
                                    else
                                        possible_game = RotateQuad2CounterClockWise(board);
                                    break;
                                case 3:
                                    if (r == 1)
                                        possible_game = RotateQuad3ClockWise(board);
                                    else
                                        possible_game = RotateQuad3CounterClockWise(board);
                                    break;
                                case 4:
                                    if (r == 1)
                                        possible_game = RotateQuad4ClockWise(board);
                                    else
                                        possible_game = RotateQuad4CounterClockWise(board);
                                    break;
                                default:
                                        //Magic?
                                        possible_game = RotateQuad1ClockWise(board);
                                    break;
                            }
                            result = alphaBeta(possible_game, treeDepth, alpha, beta);
                            board = UndoMove(board, move, c, r);
                            if (result > alpha)
                            {
                                alpha = result;
                                if (treeDepth == 0)
                                    this._Choice = move;
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

            }



            /*
            Random rnd = new Random();
            //random row from 0 - 5
            _MoveRow = (short)rnd.Next(0, 6);
            //random col from 0 - 5
            _MoveCol = (short)rnd.Next(0, 6);

            if (rnd.Next(0, 2) == 0)
                _IsClockWise = false;
            else
                _IsClockWise = true;
            //rando quad from 1-4
            _Quad = (short)rnd.Next(1,5);
            */
            return 0;
        }

        private int[] UndoMove(int[] board, int move, int c, int r)
        {
            board[move] = 0;
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
                    //Magic?
                    board = RotateQuad1ClockWise(board);
                    break;
            }
            return board;
        }

        private int[] PlacePiece(int move, int[] board)
        {
            int piece = ChangeTurn();
            board[move] = piece;
            return board;
        }

        private int ChangeTurn()
        {
            int piece;
            if (_Active_Turn == "COMPUTER")
            {
                piece = 2;
                _Active_Turn = "HUMAN";
            }
            else
            {
                piece = 1;
                _Active_Turn = "COMPUTER";
            }

            return piece;
        }

        private List<int> GetAvailableMoves(int[] board)
        {
            List<int> possibleMoves = new List<int>();
            for (int i = 0; i < BOARDSIZE; i++)
            {
                if (board[i] == 0)
                    possibleMoves.Add(i);
            }
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

        private int GameScore(int[] board, int treeDepth)
        {
            return 1;
        }

        public string Name
        {
            get { return this._Name; }
        }

        public Brush Fill
        {
            get { return this._Fill; }
        }

        public short GetMoveRow()
        {
            return this._MoveRow;
        }

        public short GetMoveCol()
        {
            return this._MoveCol;
        }

        public bool GetRotationDirection()
        {
            return this._IsClockWise;
        }

        public short GetCuadrant()
        {
            return this._Quad;
        }
    }
}
