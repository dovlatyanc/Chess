using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    internal class Moves
    {
        FigureMoving fm;
        Board board;

        public Moves(Board board)
        {
            this.board = board;
        }

        public bool CanMove(FigureMoving fm)
        {
            this.fm = fm;
            return
                CanMoveFrom() &&
                CanMoveTo() &&
                CanFigureMove();
        }
        bool CanMoveFrom() {
            return fm.from.OnBoard() &&
                    fm.Figure.GetColor() == board.moveColor;
                }
        bool CanMoveTo()
        {
            return fm.from.OnBoard() &&
                board.GetFigure(fm.to).GetColor()!=board.moveColor;
        }
        bool CanFigureMove()
        {
            return true;
        }
    }
}
