﻿using ChessGame;

namespace DemoChess;

internal class Program
{
    static void Main(string[] args)
    {
        Chess chess = new Chess();
        while (true)
        {
            Console.WriteLine(chess.fen);
            string move = Console.ReadLine();
            if (move == "") break;
            chess = chess.Move(move);
        }
    }
}
