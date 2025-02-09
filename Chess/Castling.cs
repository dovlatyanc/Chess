using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    internal static class Castling
    {
        public static bool hasMovedWhiteKing { get; set; }

        public static bool hasMovedBlackKing { get; set; }
        public static bool hasMovedWhiteRookKingSide { get; set; }
        public static bool hasMovedWhiteRookQueenSide { get; set; }
        public static bool hasMovedBlackRookKingSide { get; set; }
        public static bool hasMovedBlackRookQueenSide { get; set; }
    }
}
