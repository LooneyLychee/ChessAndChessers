using System;
using System.Collections.Generic;
using System.Text;
namespace ChessLogic
{
    class Program
    {
        public static ChessBoard Board(params String[] desc)
        {
            ChessBoard board = new ChessBoard();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Position pos = new Position(j, i);
                    switch (desc[7 - j][i])
                    {
                        case '.':
                            board.SetSquareEmpty(pos);
                            break;
                        case 'q':
                            board.SetPieceOnPosition(pos, new ChessPiece(ChessPiece.Figure.Queen, ChessPiece.Color.White));
                            break;
                        case 'k':
                            board.SetPieceOnPosition(pos, new ChessPiece(ChessPiece.Figure.King, ChessPiece.Color.White));
                            break;
                        case 'n':
                            board.SetPieceOnPosition(pos, new ChessPiece(ChessPiece.Figure.Knight, ChessPiece.Color.White));
                            break;
                        case 'b':
                            board.SetPieceOnPosition(pos, new ChessPiece(ChessPiece.Figure.Bishop, ChessPiece.Color.White));
                            break;
                        case 'r':
                            board.SetPieceOnPosition(pos, new ChessPiece(ChessPiece.Figure.Rook, ChessPiece.Color.White));
                            break;
                        case 'p':
                            board.SetPieceOnPosition(pos, new ChessPiece(ChessPiece.Figure.Pawn, ChessPiece.Color.White));
                            break;
                        case 'Q':
                            board.SetPieceOnPosition(pos, new ChessPiece(ChessPiece.Figure.Queen, ChessPiece.Color.Black));
                            break;
                        case 'K':
                            board.SetPieceOnPosition(pos, new ChessPiece(ChessPiece.Figure.King, ChessPiece.Color.Black));
                            break;
                        case 'N':
                            board.SetPieceOnPosition(pos, new ChessPiece(ChessPiece.Figure.Knight, ChessPiece.Color.Black));
                            break;
                        case 'B':
                            board.SetPieceOnPosition(pos, new ChessPiece(ChessPiece.Figure.Bishop, ChessPiece.Color.Black));
                            break;
                        case 'R':
                            board.SetPieceOnPosition(pos, new ChessPiece(ChessPiece.Figure.Rook, ChessPiece.Color.Black));
                            break;
                        case 'P':
                            board.SetPieceOnPosition(pos, new ChessPiece(ChessPiece.Figure.Pawn, ChessPiece.Color.Black));
                            break;
                    }
                }
            }
            return board;
        }
        static void Main(string[] args)
        {


            ChessBoard board = new ChessBoard();

            Moves.Move(board, new Position("g2"), new Position("g3"));
            Moves.Move(board, new Position("a7"), new Position("a6"));
            Console.WriteLine("a");
            Moves.Move(board, new Position("g1"), new Position("h3"));
            Moves.Move(board, new Position("b7"), new Position("b6"));
            Console.WriteLine("b");
            Moves.Move(board, new Position("f1"), new Position("g2"));
            Moves.Move(board, new Position("c7"), new Position("c6"));
            Console.WriteLine("c");
            Moves.Move(board, new Position("e1"), new Position("g1"));
            Console.WriteLine("d");

            Position[] moves = PossibleMoves.FindPossibleMoves(board, new Position("e1"));
            foreach (Position p in moves)
            {
                Console.WriteLine("c:{0} r:{1}", p.column, p.row);
                
            }
           

        }
    }
}
