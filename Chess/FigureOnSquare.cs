using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    internal class FigureOnSquare
    {
        public Figure Figure { get; set; }
        public Square Square { get; set; }

        public FigureOnSquare(Figure figure, Square square)
        {
            this.Square = square;
            this.Figure = figure;

        }
    }
}
