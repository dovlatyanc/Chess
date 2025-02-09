﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Chess
{
    internal class Board
    {
        public string fen { get; set; }
        private Figure[,] figures;
        public Color moveColor { get; set; }
        public int moveNumber { get; set; }

        // Состояние для рокировки и взятия на проходе
        public bool hasMovedWhiteKing { get; set; }
        public bool hasMovedBlackKing { get; set; }
        public bool hasMovedWhiteRookKingSide { get; set; }
        public bool hasMovedWhiteRookQueenSide { get; set; }
        public bool hasMovedBlackRookKingSide { get; set; }
        public bool hasMovedBlackRookQueenSide { get; set; }
        public Square lastPawnMove { get; set; }

        public Board(string fen)
        {
            this.fen = fen;
            figures = new Figure[8, 8];
            Init();
        }

        private void Init()
        {
            string[] parts = fen.Split();
            if (parts.Length != 6) return;
            InitFigures(parts[0]);
            moveColor = (parts[1] == "b") ? Color.black : Color.white;
            moveNumber = int.Parse(parts[5]);
        }

        private void InitFigures(string data)
        {
            for (int j = 8; j >= 2; j--)
                data = data.Replace(j.ToString(), (j - 1).ToString() + "1");
            data = data.Replace("1", ".");
            string[] lines = data.Split('/');

            for (int y = 7; y >= 0; y--)
                for (int x = 0; x < 8; x++)
                    figures[x, y] = lines[7 - y][x] == '.' ? Figure.None : (Figure)lines[7 - y][x];
        }

        private void GenerateFen()
        {
            fen = FenFigures() + " " +
                  (moveColor == Color.white ? "w" : "b") + " " +
                  GetCastleFlags() + " " +
                  (lastPawnMove.OnBoard() ? lastPawnMove.Name : "-") + " " +
                  "0 " + moveNumber.ToString();
        }

        private string GetCastleFlags()
        {
            StringBuilder flags = new StringBuilder();
            if (!hasMovedWhiteKing)
            {
                if (!hasMovedWhiteRookKingSide) flags.Append('K');
                if (!hasMovedWhiteRookQueenSide) flags.Append('Q');
            }
            if (!hasMovedBlackKing)
            {
                if (!hasMovedBlackRookKingSide) flags.Append('k');
                if (!hasMovedBlackRookQueenSide) flags.Append('q');
            }
            return flags.Length == 0 ? "-" : flags.ToString();
        }

        private string FenFigures()
        {
            StringBuilder sb = new StringBuilder();
            for (int y = 7; y >= 0; y--)
            {
                for (int x = 0; x < 8; x++)
                    sb.Append(figures[x, y] == Figure.None ? '1' : (char)figures[x, y]);
                if (y > 0)
                    sb.Append('/');
            }
            string eight = "11111111";

            for (int j = 8; j >= 2; j--)
                sb.Replace(eight.Substring(0, j).ToString(), j.ToString());

            return sb.ToString();
        }

        public Figure GetFigureAt(Square square)
        {
            if (square.OnBoard())
                return figures[square.x, square.y];
            return Figure.None;
        }

        public void SetFigureAt(Square square, Figure figure)
        {
            if (square.OnBoard())
                figures[square.x, square.y] = figure;
        }

        //public Board Move(FigureMoving fm)
        //{
        //    Board next = new Board(fen);
        //    next.SetFigureAt(fm.from, Figure.None);
        //    next.SetFigureAt(fm.to, fm.promotion == Figure.None ? fm.Figure : fm.promotion);

        //    // Рокировка
        //    if (fm.Figure == Figure.whiteKing || fm.Figure == Figure.blackKing)
        //    {
        //        if (fm.AbsDeltaX == 2) // Это рокировка
        //        {
        //            int rookFromX, rookToX;

        //            if (fm.to.x == 6) // Короткая рокировка
        //            {
        //                rookFromX = 7;
        //                rookToX = 5;
        //            }
        //            else // Длинная рокировка
        //            {
        //                rookFromX = 0;
        //                rookToX = 3;
        //            }

        //            // Перемещаем ладью
        //            Figure rook = GetFigureAt(new Square(rookFromX, fm.from.y));
        //            next.SetFigureAt(new Square(rookToX, fm.from.y), rook);
        //            next.SetFigureAt(new Square(rookFromX, fm.from.y), Figure.None);
        //        }

        //        // Отмечаем, что король двигался
        //        if (fm.Figure == Figure.whiteKing)
        //            next.hasMovedWhiteKing = true;
        //        else
        //            next.hasMovedBlackKing = true;
        //    }

        //    // Отмечаем, что ладьи двигались
        //    if (fm.Figure == Figure.whiteRook)
        //    {
        //        if (fm.from.x == 7)
        //            next.hasMovedWhiteRookKingSide = true;
        //        else if (fm.from.x == 0)
        //            next.hasMovedWhiteRookQueenSide = true;
        //    }
        //    else if (fm.Figure == Figure.blackRook)
        //    {
        //        if (fm.from.x == 7)
        //            next.hasMovedBlackRookKingSide = true;
        //        else if (fm.from.x == 0)
        //            next.hasMovedBlackRookQueenSide = true;
        //    }

        //    // Взятие на проходе
        //    if (fm.Figure == Figure.whitePawn || fm.Figure == Figure.blackPawn)
        //    {
        //        if (Math.Abs(fm.DeltaY) == 2)
        //        {
        //            next.lastPawnMove = fm.to;
        //            next.SetFigureAt(new Square(fm.to.x, fm.from.y), Figure.None);
        //        }
        //        else
        //            next.lastPawnMove = Square.None;
        //    }

        //    if (moveColor == Color.black)
        //        next.moveNumber++;
        //    next.moveColor = moveColor.FlipColor();
        //    next.GenerateFen();
        //    return next;
        //}
        public Board Move(FigureMoving fm)
        {
            Board next = new Board(fen);
            next.SetFigureAt(fm.from, Figure.None);
            next.SetFigureAt(fm.to, fm.promotion == Figure.None ? fm.Figure : fm.promotion);

            // Рокировка
            if (fm.Figure == Figure.whiteKing || fm.Figure == Figure.blackKing)
            {
                if (fm.AbsDeltaX == 2) // Это рокировка
                {
                    int rookFromX, rookToX;

                    if (fm.to.x == 6) // Короткая рокировка
                    {
                        rookFromX = 7;
                        rookToX = 5;
                    }
                    else // Длинная рокировка
                    {
                        rookFromX = 0;
                        rookToX = 3;
                    }

                    // Перемещаем ладью
                    Figure rook = GetFigureAt(new Square(rookFromX, fm.from.y));
                    next.SetFigureAt(new Square(rookToX, fm.from.y), rook);
                    next.SetFigureAt(new Square(rookFromX, fm.from.y), Figure.None);
                }

                // Отмечаем, что король двигался
                if (fm.Figure == Figure.whiteKing)
                    next.hasMovedWhiteKing = true;
                else
                    next.hasMovedBlackKing = true;
            }

            // Отмечаем, что ладьи двигались
            if (fm.Figure == Figure.whiteRook)
            {
                if (fm.from.x == 7)
                    next.hasMovedWhiteRookKingSide = true;
                else if (fm.from.x == 0)
                    next.hasMovedWhiteRookQueenSide = true;
            }
            else if (fm.Figure == Figure.blackRook)
            {
                if (fm.from.x == 7)
                    next.hasMovedBlackRookKingSide = true;
                else if (fm.from.x == 0)
                    next.hasMovedBlackRookQueenSide = true;
            }

            // Взятие на проходе
            if (fm.Figure == Figure.whitePawn || fm.Figure == Figure.blackPawn)
            {
                // Проверка на взятие на проходе
                if (fm.AbsDeltaX == 1 && fm.DeltaY != 0 && GetFigureAt(fm.to) == Figure.None)
                {
                    // Пешка противника только что сделала ход на две клетки
                    Square enemyPawnSquare = new Square(fm.to.x, fm.from.y);
                    Figure enemyPawn = GetFigureAt(enemyPawnSquare);

                    if (enemyPawn == (fm.Figure == Figure.whitePawn ? Figure.blackPawn : Figure.whitePawn))
                    {
                        // Удаляем пешку противника
                        next.SetFigureAt(enemyPawnSquare, Figure.None);
                    }
                }

                // Обновляем последний ход пешки
                if (Math.Abs(fm.DeltaY) == 2)
                {
                    next.lastPawnMove = fm.to;
                }
                else
                {
                    next.lastPawnMove = Square.None;
                }
            }

            if (moveColor == Color.black)
                next.moveNumber++;
            next.moveColor = moveColor.FlipColor();
            next.GenerateFen();
            return next;
        }

        public IEnumerable<FigureOnSquare> YieldFigures()
        {
            foreach (Square square in Square.YieldSquares())
                if (GetFigureAt(square).GetColor() == moveColor)
                    yield return new FigureOnSquare(GetFigureAt(square), square);
        }

        public bool IsCheck()
        {
            Board after = new Board(fen);
            after.moveColor = moveColor.FlipColor();
            return after.CanEatKing();
        }

        private bool CanEatKing()
        {
            Square badKing = FindBadKing();
            Moves moves = new Moves(this);
            foreach (FigureOnSquare fs in YieldFigures())
            {
                FigureMoving figmoving = new FigureMoving(fs, badKing);
                if (moves.CanMove(figmoving))
                    return true;
            }
            return false;
        }

        private Square FindBadKing()
        {
            Figure badKing = moveColor == Color.black ? Figure.whiteKing : Figure.blackKing;
            foreach (Square square in Square.YieldSquares())
                if (GetFigureAt(square) == badKing)
                    return square;

            return Square.None;
        }

        public bool IsCheckAfterMove(FigureMoving fm)
        {
            Board after = Move(fm);
            return after.CanEatKing();
        }
    }
}