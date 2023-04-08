using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using ChessLogic;

namespace ChessLogicTests
{
    [TestClass]
    public class MovesTest
    {
       private Position Pos(String desc)
        {
            return new Position(desc[1] - '1', desc[0] - 'a');
        }

        private Position[] ManyPos(params String[] desc)
        {
            return desc.Select(d => Pos(d)).ToArray();
        }

        private ChessBoard Board(params String[] desc)
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

        private ChessPiece[] MultidimensionalArrayToOneDimensionalArray(ChessBoard board)
        {
            List<ChessPiece> list = new List<ChessPiece>();
            foreach (ChessPiece a in board.board)
            {
                list.Add(a);
            }

            return list.ToArray();

        }

        [TestMethod]
        public void TestMethodToTest()
        {
            ChessBoard board = new ChessBoard();
            ChessBoard board2 = Board(
               "RNBQKBNE",
               "PPPPPPPP",
               "........",
               "........",
               "........",
               "........",
               "pppppppp",
               "rnbqkbnr");

            ChessPiece[] a = MultidimensionalArrayToOneDimensionalArray(board);
            ChessPiece[] b = MultidimensionalArrayToOneDimensionalArray(board2);

            CollectionAssert.AreEqual(a, b);
        }

        [TestMethod]
        public void WhitePawnStartMove()
        {
            ChessBoard board = new ChessBoard();

            Moves.Move(board, new Position("a2"), new Position("a4"));
            ChessBoard board2 = Board(
               "RNBQKBNE",
               "PPPPPPPP",
               "........",
               "........",
               "p.......",
               "........",
               ".ppppppp",
               "rnbqkbnr");

            ChessPiece[] a = MultidimensionalArrayToOneDimensionalArray(board);
            ChessPiece[] b = MultidimensionalArrayToOneDimensionalArray(board2);

            CollectionAssert.AreEqual(a, b);
        }
        [TestMethod]
        public void BlackPawnStartMove()
        {
            ChessBoard board = new ChessBoard();

            Moves.Move(board, new Position("f7"), new Position("f5"));
            ChessBoard board2 = Board(
               "RNBQKBNE",
               "PPPPP.PP",
               "........",
               ".....P..",
               "........",
               "........",
               "pppppppp",
               "rnbqkbnr");

            ChessPiece[] a = MultidimensionalArrayToOneDimensionalArray(board);
            ChessPiece[] b = MultidimensionalArrayToOneDimensionalArray(board2);

            CollectionAssert.AreEqual(a, b);
        }
       
        [TestMethod]
        public void MoveImpossible()
        {
            ChessBoard board = new ChessBoard();

            Moves.Move(board, new Position("a2"), new Position("b3"));
            ChessBoard board2 = Board(
               "RNBQKBNE",
               "PPPPPPPP",
               "........",
               "........",
               "........",
               "........",
               "pppppppp",
               "rnbqkbnr");

            ChessPiece[] a = MultidimensionalArrayToOneDimensionalArray(board);
            ChessPiece[] b = MultidimensionalArrayToOneDimensionalArray(board2);

            CollectionAssert.AreEqual(a, b);
        }
        [TestMethod]
        public void MoveImpossible2()
        {
            ChessBoard board = new ChessBoard();

            Moves.Move(board, new Position("h7"), new Position("f5"));
            ChessBoard board2 = Board(
               "RNBQKBNE",
               "PPPPPPPP",
               "........",
               "........",
               "........",
               "........",
               "pppppppp",
               "rnbqkbnr");

            ChessPiece[] a = MultidimensionalArrayToOneDimensionalArray(board);
            ChessPiece[] b = MultidimensionalArrayToOneDimensionalArray(board2);

            CollectionAssert.AreEqual(a, b);
        }

        [TestMethod]
        public void EnPassantWhiteTakes()
        {
            ChessBoard board = new ChessBoard();

            Moves.Move(board, new Position("h7"), new Position("h5"));
            Moves.Move(board, new Position("a2"), new Position("a3"));
            Moves.Move(board, new Position("h5"), new Position("h4"));
            Moves.Move(board, new Position("g2"), new Position("g4"));
            Moves.Move(board, new Position("h4"), new Position("g3"));

            ChessBoard board2 = Board(
               "RNBQKBNR",
               "PPPPPPP.",
               "........",
               "........",
               "........",
               "p.....P.",
               ".ppppp.p",
               "rnbqkbnr");

            ChessPiece[] a = MultidimensionalArrayToOneDimensionalArray(board);
            ChessPiece[] b = MultidimensionalArrayToOneDimensionalArray(board2);

            CollectionAssert.AreEqual(a, b);
        }

        [TestMethod]
        public void EnPassantBlackTakes()
        {
            ChessBoard board = new ChessBoard();

            Moves.Move(board, new Position("c2"), new Position("c4"));
            Moves.Move(board, new Position("a7"), new Position("a6"));
            Moves.Move(board, new Position("e2"), new Position("e4"));
            Moves.Move(board, new Position("a8"), new Position("a7"));

            Moves.Move(board, new Position("c4"), new Position("c5"));
            Moves.Move(board, new Position("a6"), new Position("a5"));
            Moves.Move(board, new Position("e4"), new Position("e5"));
            Moves.Move(board, new Position("d7"), new Position("d5"));
            Moves.Move(board, new Position("e5"), new Position("d6"));

            ChessBoard board2 = Board(
               ".NBQKBNR",
               "RPP.PPPP",
               "...p....",
               "P.p.....",
               "........",
               "........",
               "pp.p.ppp",
               "rnbqkbnr");

            ChessPiece[] a = MultidimensionalArrayToOneDimensionalArray(board);
            ChessPiece[] b = MultidimensionalArrayToOneDimensionalArray(board2);

            CollectionAssert.AreEqual(a, b);
        }

        [TestMethod]
        public void CastlingWest()
        {
            ChessBoard board = new ChessBoard();

            Moves.Move(board, new Position("g2"), new Position("g3"));
            Moves.Move(board, new Position("a7"), new Position("a6"));

            Moves.Move(board, new Position("g1"), new Position("h3"));
            Moves.Move(board, new Position("b7"), new Position("b6"));

            Moves.Move(board, new Position("f1"), new Position("g2"));
            Moves.Move(board, new Position("c7"), new Position("c6"));

            Moves.Move(board, new Position("e1"), new Position("g1"));
            

            ChessBoard board2 = Board(
               "RNBQKBNR",
               "...PPPPP",
               "PPP.....",
               "........",
               "........",
               "......pn",
               "ppppppbp",
               "rnbq.rk.");

            ChessPiece[] a = MultidimensionalArrayToOneDimensionalArray(board);
            ChessPiece[] b = MultidimensionalArrayToOneDimensionalArray(board2);

            CollectionAssert.AreEqual(a, b);
        }
    }
}
