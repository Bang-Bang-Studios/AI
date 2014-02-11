using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Pentago.GameCore
{
    class Player
    {
        private string _Name;
        private bool _ActivePlayer;
        private Brush _Fill;

        public Player(string name, bool activeTurn, Brush color)
        {
            this._Name = name;
            this._ActivePlayer = activeTurn;
            this._Fill = color;
        }

        public string Name
        {
            get { return this._Name; } 
        }

        public bool ActivePlayer
        {
            set { this._ActivePlayer = value; }
            get { return this._ActivePlayer; }
        }

        public Brush Fill
        {
            get { return this._Fill; }
        }

    }
}