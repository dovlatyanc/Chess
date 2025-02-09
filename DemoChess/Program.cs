using ChessGame;

namespace DemoChess;

internal class Program
{
    static void Main(string[] args)
    {
        ChessGame.Chess chess = new("rnbqkbnr/pppp1ppp/8/2P5/4p3/8/PP1PPPPP/R111K11R w KQkq - 0 1");
        List<string> list;
        while (true)
        {
            Random random = new Random();
            list = chess.GetAllMoves();
            Console.Clear();
            Console.WriteLine(chess.fen);
            Print(ChessToAscii(chess));

            Console.WriteLine(chess.IsCheck()?"\nШАХ!!!":"-");
            Console.WriteLine();
            Console.WriteLine("Варианты ходов:");

            foreach(string  moves in chess.GetAllMoves())// выводим в консоль список всех возожных ходов
                Console.Write(moves+"\n");
            Console.WriteLine();
            Console.WriteLine("> ");
            string move = Console.ReadLine();
            if (move == "q") break;//выход
            if( move=="")move = list[random.Next(list.Count)];//случвйный ход на ентер
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