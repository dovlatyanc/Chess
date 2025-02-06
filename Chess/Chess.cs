using Chess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame
{
    public class Chess
    {
        public string fen { get; set; }
        Board board;
        Moves moves;
        public Chess(string fen = @"rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1")
        {

            this.fen = fen;
           board = new Board(fen);
            moves = new Moves(board);
        }

        Chess(Board board)
        {
            this.board = board;
            this.fen = board.fen;
            moves = new Moves(board);
        }

        public Chess Move(string move) //Pe2e4 Pe7e8 пешка
        {
            FigureMoving fm = new FigureMoving(move);
            if (!moves.CanMove(fm))//можно ли сделать ход
                return this;

            Board nextBoard = board.Move(fm);

            Chess nextChess = new Chess(nextBoard);
         
          
            return nextChess;
        }

        public char GetFigureAt(int x, int y)
        {
            Square square = new Square(x, y);
            Figure f = board.GetFigure(square);
            return f == Figure.None ? '.' : (char)f;
        }
    }
}
