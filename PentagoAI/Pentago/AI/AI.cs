using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Pentago.AI
{
    class AI
    {
        private string _Name;
        private bool _ActivePlayer;
        private Brush _Fill;
        private enum Difficulty { Easy, Hard };
        private Difficulty _DifficultyLevel;
        private int _MaxTreeDepth;

        public AI (string name, bool isActive, Brush fill, Difficulty level, int treeDepth)
        {
            this._Name = name;
            this._ActivePlayer = isActive;
            this._Fill = fill;
            this._MaxTreeDepth = treeDepth;
            this._DifficultyLevel = level;
        }
        
    }
}
