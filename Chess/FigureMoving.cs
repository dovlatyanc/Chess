using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    internal class FigureMoving
    {
        public Figure Figure { get; set; }
        public Square from { get; set; }
        public Square to { get; set; }
        public Figure promotion { get; set; }//если пешка превращается в джругую фигуру

        public FigureMoving(FigureOnSquare fs, Square to, Figure promotion = Figure.None)
        {
            this.Figure = fs.Figure;
            this.from = fs.Square;
            this.to = to;
            this.promotion = promotion;
        }

        public FigureMoving(string move)//Pe2e4  Pe7e8Q
        {                               //01234  012345

            this.Figure = (Figure)move[0];
            this.from = new Square(move.Substring(1, 2));
            this.to = new Square(move.Substring(3, 2));
            this.promotion = (move.Length == 6) ? (Figure)move[5] : Figure.None;
        }

        public int DeltaX { get { return to.x - from.x; } } 
        public int DeltaY { get { return to.y - from.y; } } 
        public int AbsDeltaX { get { return Math.Abs(DeltaX); } }
        public int AbsDeltaY { get { return Math.Abs(DeltaY); } }
        public int SignX { get { return Math.Sign(DeltaX); } }
        public int SignY { get { return Math.Sign(DeltaY); } }

        public override string ToString()
        {
            string str = (char)Figure + from.Name + to.Name;
            if (promotion != Figure.None)
                str += (char)promotion;
            return str;
        }

    }

}
