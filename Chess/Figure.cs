using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    enum Figure
    {
        None,
        whiteKing = 'K',
        whiteQueen = 'Q',
        whiteRook = 'R',
        whiteBishop = 'B',
        whiteKnight = 'N',
        whitePawn = 'P',

        blackKing = 'k',
        blackQueen = 'q',
        blackRook = 'r',
        blackBishop = 'b',
        blackKnight = 'n',
        blackPawn = 'P'
    }
    static class FigureMethods
    {
        public static Color GetColor(this Figure figure)
        {
            if (figure == Figure.None)
                return Color.none;
            return (figure == Figure.whiteKing ||
                figure == Figure.whiteQueen ||
                figure == Figure.whiteRook ||
                figure == Figure.whiteKnight ||
                figure == Figure.whitePawn ||
                figure == Figure.whiteBishop)
                ?
                Color.white : Color.black;
        }
    }
}
