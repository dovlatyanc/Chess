using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    internal class Board
    {
        public string fen { get; private set; }
        Figure[,] figures;
        public Color moveColor { get; private set; }//чей ход
        public int moveNumber { get; private set; }//номер хода
        public Board(string fen)
        {
            this.fen = fen;
            figures = new Figure[8, 8];
            Init();
        }
        //rnbqkbnr / pppppppp / 8 / 8 / 8 / 8 / PPPPPPPP / RNBQKBNR w KQkq - 0 1
        private void Init()
        {
            string[] parts = fen.Split();
            if (parts.Length != 6) return;
            InitFigures(parts[0]);
            // InitColor(parts[1]);
            moveColor = (parts[1] == "b") ? Color.black : Color.white;
            moveNumber = int.Parse(parts[5]);
        }

        void InitFigures(string data)
        {
            for (int j = 8; j >= 2; j--)
                data = data.Replace(j.ToString(), (j - 1).ToString() + "1");
            data = data.Replace("1", ".");
            string[] lines = data.Split('/');

            for (int y = 7; y >= 0; y--)
                for (int x = 0; x < 8; x++)
                    figures[x, y] = lines[7 - y][x] == '.' ? Figure.None : (Figure)lines[7 - y][x];

        }

        void GenerateFen()
        {
            fen = FenFigures() + " " + (moveColor == Color.white ? "w" : "b") + " - - 0 " + moveNumber.ToString();//добавить рокировку ничью и взятие на проходе
        }

        string FenFigures()
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
        public Figure GetFigure(Square square)
        {

            if (square.OnBoard())
                return figures[square.x, square.y];

            return Figure.None;
        }

        void SetFigureAt(Square square, Figure figure)
        {
            if (square.OnBoard())
                figures[square.x, square.y] = figure;
        }

        public Board Move(FigureMoving fm)
        {

            Board next = new Board(fen);
            next.SetFigureAt(fm.from, Figure.None);//клетка освобождается откуда сделан ход
            next.SetFigureAt(fm.to, fm.promotion == Figure.None ? fm.Figure : fm.promotion);//ход осуществляется, если есть превращение то фигура меняется 
            if (moveColor == Color.black)
                next.moveNumber++;
            next.moveColor = moveColor.FlipColor();
            next.GenerateFen();
            return next;
        }
    }
}
