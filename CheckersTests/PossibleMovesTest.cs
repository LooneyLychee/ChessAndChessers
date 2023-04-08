using Microsoft.VisualStudio.TestTools.UnitTesting;
using CheckersLogic;
using System.Collections.Generic;
using System.Linq;
using System;

namespace CheckersTests
{
    [TestClass]
    public class PossibleMovesTest
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
        private List<PossibleMoves> possibleMovesList(Position[] possiblePositions, PossibleMoves.Type typeOfMove)
        {
            List<PossibleMoves> possibleMoves = new List<PossibleMoves>();
            foreach(Position position in possiblePositions)
            {
                PossibleMoves possibleMove = new PossibleMoves(position, typeOfMove);
                possibleMoves.Add(possibleMove);
                
            }

            return possibleMoves;
        }
        [TestMethod]
        public void TestStone_WhiteStartingPosition()
        {
            CheckersBoard board = new CheckersBoard();

            PossibleMoves move = PossibleMoves.FindPossibleMove(board, Pos("a3"));

            PossibleMoves expectedMove = new PossibleMoves(Pos("a3"), PossibleMoves.Type.normalMove);
            Position[] positions =  ManyPos("b4");
            expectedMove.nextPosition = possibleMovesList(positions, PossibleMoves.Type.normalMove);
            Assert.AreEqual(expectedMove, move);
            /*Board(
            "  b8  d8  f8  h8",
            "a7  c7  e7  g7  ",
            "  b6  d6  f6  h6",
            "a5  c5  e5  g5  ",
            "  b4  d4  f4  h4",
            "a3  c3  e3  g3  "
            "  b2  d2  f2  h2",
            "a1  c1  e1  g1  ");*/
        }
        [TestMethod]
        public void TestStone_BlackStartingPosition()
        {
            CheckersBoard board = new CheckersBoard();

            PossibleMoves move = PossibleMoves.FindPossibleMove(board, Pos("h6"));

            PossibleMoves expectedMove = new PossibleMoves(Pos("h6"), PossibleMoves.Type.normalMove);
            Position[] positions = ManyPos("g5");
            expectedMove.nextPosition = possibleMovesList(positions, PossibleMoves.Type.normalMove);
            Assert.AreEqual(expectedMove, move);
        }
    }
}
