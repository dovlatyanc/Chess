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
        private bool isCastling;

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
        bool CanMoveFrom()
        {
            return fm.from.OnBoard() &&
                    fm.Figure.GetColor() == board.moveColor;
        }
        bool CanMoveTo()
        {
            return fm.from.OnBoard() &&
                fm.from != fm.to &&
                board.GetFigure(fm.to).GetColor() != board.moveColor;
        }
        bool CanFigureMove()
        {
            switch (fm.Figure)
            {

                case Figure.whiteKing:
                case Figure.blackKing:
                    return CanKingMove();
                case Figure.whiteQueen:
                case Figure.blackQueen:
                    return CanStraightMove();
                case Figure.whiteRook:
                case Figure.blackRook:
                    return (fm.SignX==0||fm.SignY==0) &&
                        CanStraightMove();
                case Figure.whiteBishop:
                case Figure.blackBishop:
                    return (fm.SignX != 0 && fm.SignY != 0) &&
                        CanStraightMove();
                case Figure.whiteKnight:
                case Figure.blackKnight:
                    return СanKnightMove();
                case Figure.whitePawn:
                case Figure.blackPawn:
                    return CanPawnMove();





                default: return false;
            }
        }

        private bool CanPawnMove()
        {
            if(fm.from.y<1||fm.from.y>6)
                return false;
            int stepY = fm.Figure.GetColor() == Color.white ? 1 : -1;//если белая - идет вверх, если черная идет вниз
            return CanPawnGo(stepY) ||
                   CanPawnJump(stepY) ||
                   CanPawnEat(stepY);

        }

        private bool CanPawnEat(int stepY)
        {
            if (board.GetFigure(fm.to) != Figure.None)//если клетка не пуста и там есть фигура противника
                if (fm.AbsDeltaX == 1)
                    if (fm.DeltaY == stepY)
                        return true;
            return false;

        }

        private bool CanPawnJump(int stepY)
        {
            if (board.GetFigure(fm.to) == Figure.None)
                if (fm.DeltaX == 0)//если пешка идеит прямо
                    if (fm.DeltaY == 2 * stepY)//может ли пойти на две клетки
                        if (fm.from.y == 1 || fm.from.y == 6)//если стоит на изначальной половсе движения
                            if (board.GetFigure(new Square(fm.from.x, fm.from.y + stepY)) == Figure.None)//не перепрыгивает кли кого то пешка
                                return true;
            return false;
        }

        private bool CanPawnGo(int stepY)
        {
            if (board.GetFigure(fm.to) == Figure.None)
                if (fm.DeltaX == 0)//если пешка идеит прямо
                    if (fm.DeltaY == stepY)
                        return true;
            return false;
        }

        private bool CanStraightMove()
        {
            Square at = fm.from;
            do
            {
                at = new Square(at.x + fm.SignX, at.y + fm.SignY);
                if (at == fm.to)
                    return true;
            } while (at.OnBoard()&&board.GetFigure(at)==Figure.None);
            return false;
        }



        private bool CanKingMove()//добавить рокировку
        {
            if (fm.AbsDeltaY <= 1 && fm.AbsDeltaY <= 1)
                return true;
            if (isCastling)
            {
                // Для примера, предположим, что 'hasMovedKing' и 'hasMovedRook' - это свойства, 
                // которые отслеживают движение короля и ладьи соответственно.
                //if (!hasMovedKing && !hasMovedRook && !IsUnderAttack()
                //    && IsPathClearForCastling())
                //{
                //    return true; // Рокировка разрешена
                //}
            }

            return false;
        }




        private bool СanKnightMove()
        {
            if (fm.AbsDeltaX == 1 && fm.AbsDeltaY == 2) return true;
            if (fm.AbsDeltaX == 2 && fm.AbsDeltaY == 1) return true;
            return false;

        }
    }
}
