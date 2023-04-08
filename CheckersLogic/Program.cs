using System;
using System.Linq;

namespace CheckersLogic
{
    class Program
    {
        private Position Pos(String desc)
        {
            return new Position(desc[1] - '1', desc[0] - 'a');
        }
        private Position[] ManyPos(params String[] desc)
        {
            return desc.Select(d => Pos(d)).ToArray();
        }
        public static CheckersBoard Board(params String[] desc)
        {
            CheckersBoard board = new CheckersBoard();
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
                        case 'd':
                            board.SetPieceOnPosition(pos, new CheckersPiece(CheckersPiece.Type.Dame, CheckersPiece.Color.White));
                            break;
                        case 's':
                            board.SetPieceOnPosition(pos, new CheckersPiece(CheckersPiece.Type.Stone, CheckersPiece.Color.White));
                            break;
                        case 'D':
                            board.SetPieceOnPosition(pos, new CheckersPiece(CheckersPiece.Type.Dame, CheckersPiece.Color.Black));
                            break;
                        case 'S':
                            board.SetPieceOnPosition(pos, new CheckersPiece(CheckersPiece.Type.Stone, CheckersPiece.Color.Black));
                            break;

                    }
                }
            }
            return board;
        }
        static void Wypisz(PossibleMoves p)
        {
           
            Console.Write("{0} -> ",p.position.ToString());
            foreach(PossibleMoves m in p.nextPosition)
            {
                //if(m.howManyPieceWillBeCaptured == p.howManyPieceWillBeCaptured-1 || p.typeOfMove == PossibleMove.Type.normalMove)
                    Console.Write("{0}, ", m.position.ToString());
            }
            Console.WriteLine();
            foreach (PossibleMoves m in p.nextPosition)
            {
                //if (m.howManyPieceWillBeCaptured == p.howManyPieceWillBeCaptured - 1)
                    Wypisz(m);
            }
            
        }
        static void Main(string[] args)
        {

            CheckersBoard b = new CheckersBoard();
            CheckersBoard a = b.GetCheckersBoard();

            CheckersPiece piece = b.GetPieceFromPosition(new Position("a3"));
            piece.ChangeColor();
            Console.WriteLine(b.GetPieceFromPosition(new Position("a3")).color);

            CheckersBoard board =  Board(
               /*8*/ "........",
               /*7*/ "........",
               /*6*/ ".s.s...S",
               /*5*/ "........",
               /*4*/ ".s......",
               /*3*/ "....s...",
               /*2*/ "...s....",
               /*1*/ "s...D...");
                 /*   abcdefgh*/
            PossibleMoves possibleMoves = PossibleMoves.FindPossibleMove(board, new Position("h6"));
            if(possibleMoves != null)
                Wypisz(possibleMoves);

            PossibleMoves p1 = new PossibleMoves(new Position("e1"), PossibleMoves.Type.capturing);
            PossibleMoves p2 = new PossibleMoves(new Position("c3"), PossibleMoves.Type.capturing);
            PossibleMoves p3 = new PossibleMoves(new Position("a5"), PossibleMoves.Type.capturing);
            PossibleMoves p4 = new PossibleMoves(new Position("c7"), PossibleMoves.Type.capturing);
            PossibleMoves p5 = new PossibleMoves(new Position("e5"), PossibleMoves.Type.capturing);
            PossibleMoves p6 = new PossibleMoves(new Position("f4"), PossibleMoves.Type.capturing);
            PossibleMoves p7 = new PossibleMoves(new Position("g3"), PossibleMoves.Type.capturing);
            PossibleMoves p8 = new PossibleMoves(new Position("h2"), PossibleMoves.Type.capturing);

            p1.nextPosition.Add(p2);
            p2.nextPosition.Add(p3);
            p3.nextPosition.Add(p4);
            p4.nextPosition.Add(p5);
            p4.nextPosition.Add(p6);
            p4.nextPosition.Add(p7);
            p4.nextPosition.Add(p8);

            Console.WriteLine(p1.Equals(possibleMoves));
        }
    }
}
