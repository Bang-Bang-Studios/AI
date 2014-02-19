using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using Pentago.AI;

namespace Pentago.GUI
{
    public class GameOptions
    {
        public enum TypeOfGame { QuickMatch, Campaign, Network, AI };
        public TypeOfGame _TypeOfGame;

        //Human vs Human
        public string _Player1Name;
        public string _Player2Name;
        public Brush _Player1Color;
        public Brush _Player2Color;
        public GameOptions(TypeOfGame typeOfGame, string player1Name, Brush player1Color, string player2Name, Brush player2Color)
        {
            this._TypeOfGame = typeOfGame;
            this._Player1Name = player1Name;
            this._Player1Color = player1Color;
            this._Player2Name = player2Name;
            this._Player2Color = player2Color;
        }

        //Human vs AI
        public computerAI.Difficulty _Difficulty;
        public GameOptions(TypeOfGame typeOfGame, string player1Name, Brush player1Color, computerAI.Difficulty difficulty)
        {
            this._TypeOfGame = typeOfGame;
            this._Player1Name = player1Name;
            this._Player1Color = player1Color;
            this._Difficulty = difficulty;
        }
    }
}
