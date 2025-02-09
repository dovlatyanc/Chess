using Chess;
using System;

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

    bool CanMoveFrom()
    {
        return fm.from.OnBoard() &&
                fm.Figure.GetColor() == board.moveColor;
    }

    bool CanMoveTo()
    {
        return fm.from.OnBoard() &&
            fm.from != fm.to &&
            board.GetFigureAt(fm.to).GetColor() != board.moveColor;
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
                return (fm.SignX == 0 || fm.SignY == 0) &&
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
            default:
                return false;
        }
    }

    private bool CanPawnMove()
    {
        if (fm.from.y < 1 || fm.from.y > 6)
            return false;
        int stepY = fm.Figure.GetColor() == Color.white ? 1 : -1;
        return CanPawnGo(stepY) ||
               CanPawnJump(stepY) ||
               CanPawnEat(stepY);
    }

    //private bool CanPawnEat(int stepY)
    //{
    //    if (board.GetFigure(fm.to) != Figure.None)
    //        if (fm.AbsDeltaX == 1)
    //            if (fm.DeltaY == stepY)
    //                return true;
    //    return false;
    //}

    private bool CanPawnEat(int stepY)
    {
        if (board.GetFigureAt(fm.to) != Figure.None)
        {
            if (fm.AbsDeltaX == 1 && fm.DeltaY == stepY)
                return true;
        }
        else if (board.lastPawnMove != null && fm.to.x == board.lastPawnMove.x && fm.from.y == board.lastPawnMove.y)
        {
            if (fm.AbsDeltaX == 1 && fm.DeltaY == stepY)
                return true;
        }

        return false;
    }

    private bool CanPawnJump(int stepY)
    {
        if (board.GetFigureAt(fm.to) == Figure.None)
            if (fm.DeltaX == 0)
                if (fm.DeltaY == 2 * stepY)
                    if (fm.from.y == 1 || fm.from.y == 6)
                        if (board.GetFigureAt(new Square(fm.from.x, fm.from.y + stepY)) == Figure.None)
                            return true;
        return false;
    }

    private bool CanPawnGo(int stepY)
    {
        if (board.GetFigureAt(fm.to) == Figure.None)
            if (fm.DeltaX == 0)
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
        } while (at.OnBoard() && board.GetFigureAt(at) == Figure.None);
        return false;
    }

    private bool CanKingMove()
    {
        // Обычное движение короля
        if (fm.AbsDeltaX <= 1 && fm.AbsDeltaY <= 1)
            return true;

        // Проверка на рокировку
        if (fm.AbsDeltaX == 2 && fm.AbsDeltaY == 0)
        {
            if (fm.Figure == Figure.whiteKing && !board.hasMovedWhiteKing)
            {
                if (fm.to.x == 6 && !board.hasMovedWhiteRookKingSide && IsPathClearForCastling(7, 5))
                    return true; // Короткая рокировка
                if (fm.to.x == 2 && !board.hasMovedWhiteRookQueenSide && IsPathClearForCastling(0, 3))
                    return true; // Длинная рокировка
            }
            else if (fm.Figure == Figure.blackKing && !board.hasMovedBlackKing)
            {
                if (fm.to.x == 6 && !board.hasMovedBlackRookKingSide && IsPathClearForCastling(7, 5))
                    return true; // Короткая рокировка
                if (fm.to.x == 2 && !board.hasMovedBlackRookQueenSide && IsPathClearForCastling(0, 3))
                    return true; // Длинная рокировка
            }
        }

        return false;
    }

    private bool IsPathClearForCastling(int rookX, int kingX)
    {
        int start = Math.Min(rookX, kingX);
        int end = Math.Max(rookX, kingX);

        for (int x = start + 1; x < end; x++)
        {
            if (board.GetFigureAt(new Square(x, fm.from.y)) != Figure.None)
                return false;
        }

        return true;
    }

    private bool СanKnightMove()
    {
        if (fm.AbsDeltaX == 1 && fm.AbsDeltaY == 2) return true;
        if (fm.AbsDeltaX == 2 && fm.AbsDeltaY == 1) return true;
        return false;
    }


}