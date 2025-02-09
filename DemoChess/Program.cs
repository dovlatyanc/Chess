using ChessGame;

namespace DemoChess;

internal class Program
{
    static void Main(string[] args)
    {
        ChessGame.Chess chess = new("rnbqkbnr/p1111111/8/8/8/8/1P1111P1/RNBQK11R w KQkq - 0 1");
        while (true)
        {
            Console.Clear();
            Console.WriteLine(chess.fen);
            Print(ChessToAscii(chess));
            Console.WriteLine();
            Console.WriteLine("Варианты ходов:");

            foreach(string  moves in chess.GetAllMoves())// выводим в консоль список всех возожных ходов
                Console.Write(moves+"\n");
            Console.WriteLine();
            Console.WriteLine("> ");
            string move = Console.ReadLine();
            if (move == "") break;
            chess = chess.Move(move);
        }

        static string ChessToAscii(ChessGame.Chess chess)
        {
            string text = "  +-----------------+\n";

            for (int y = 7; y >= 0; y--)
            {
                text += y + 1;
                text += " | ";
                for (int x = 0; x < 8; x++)
                    text += chess.GetFigureAt(x, y) + " ";
                text += "|\n";
            }
            text += "  +-----------------+\n";
            text += "    a b c d e f g h";
            return text;
        }

        static void Print(string text)
        {
            ConsoleColor oldForeColor = Console.ForegroundColor;
            foreach (char x in text)
            {
                if (x >= 'a' && x <= 'z')
                    Console.ForegroundColor = ConsoleColor.Red;
                else if (x >= 'A' && x <= 'Z')
                    Console.ForegroundColor = ConsoleColor.White;
                else
                    Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(x);
            }
            Console.ForegroundColor = oldForeColor;
        }
    }
}