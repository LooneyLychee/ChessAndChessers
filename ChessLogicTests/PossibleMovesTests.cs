using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;


using ChessLogic;

namespace ChessLogicTests
{
    [TestClass]
    public class PossibleMovesTests
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


        [TestMethod]
        public void TestPawn_WhiteStartingPosition()
        {
            ChessBoard board = new ChessBoard();

            Position[] moves = PossibleMoves.FindPossibleMoves(board, Pos("a2"));
            Position[] m =
            {
                new Position(2,0),
                new Position(3,0)

            };

            CollectionAssert.AreEquivalent(m, moves);
        }

        [TestMethod]
        public void TestPawn_BlackStartingPosition()
        {
            ChessBoard board = new ChessBoard();

            Position[] moves = PossibleMoves.FindPossibleMoves(board, Pos("d7"));

            CollectionAssert.AreEquivalent(ManyPos("d5", "d6"), moves);
        }

        [TestMethod]
        public void TestPawn_WhiteNormalPosition()
        {
            ChessBoard board = Board(
                "........",
                "..K.....",
                "........",
                "........",
                "...p....",
                "........",
                "R..p..k.",
                "........");

            Position[] moves = PossibleMoves.FindPossibleMoves(board, Pos("d4"));

            CollectionAssert.AreEquivalent(ManyPos("d5"), moves);
        }

        [TestMethod]
        public void TestPawn_BlackNormalPosition()
        {
            ChessBoard board = Board(
                "........",
                "..K.....",
                ".P......",
                "........",
                "...p....",
                "........",
                "R..p..k.",
                "........");

            Position[] moves = PossibleMoves.FindPossibleMoves(board, Pos("b6"));

            CollectionAssert.AreEquivalent(ManyPos("b5"), moves);
        }

        [TestMethod]
        public void TestPawn_WhiteTakes()
        {
            ChessBoard board = Board(
                "........",
                "..K.....",
                "........",
                "....P...",
                "...p....",
                "........",
                "R..p..k.",
                "........");

            Position[] moves = PossibleMoves.FindPossibleMoves(board, Pos("d4"));

            CollectionAssert.AreEquivalent(ManyPos("d5", "e5"), moves);
        }

        [TestMethod]
        public void TestPawn_BlackTakes()
        {
            ChessBoard board = Board(
                "........",
                "..K.....",
                "........",
                "....P...",
                "...p....",
                "........",
                "R..p..k.",
                "........");

            Position[] moves = PossibleMoves.FindPossibleMoves(board, Pos("e5"));

            CollectionAssert.AreEquivalent(ManyPos("d4", "e4"), moves);
        }

        [TestMethod]
        public void TestPawn_DiscoveredCheck()
        {
            ChessBoard board = Board(
                "........",
                "..K.....",
                "........",
                "........",
                "........",
                "........",
                "R..p..k.",
                "........");

            Position[] moves = PossibleMoves.FindPossibleMoves(board, Pos("d2"));

            CollectionAssert.AreEquivalent(ManyPos(), moves);
        }

        [TestMethod]
        public void TestPawn_BlockedCheck()
        {
            ChessBoard board = Board(
                "........",
                "..K.....",
                "........",
                "........",
                "........",
                "R.....k.",
                "...p....",
                "........");

            Position[] moves = PossibleMoves.FindPossibleMoves(board, Pos("d2"));

            CollectionAssert.AreEquivalent(ManyPos("d3"), moves);
        }

        [TestMethod]
        public void TestPawn_BlockedCheckByOtherPiece()
        {
            ChessBoard board = Board(
                "....R...",
                "..K...r.",
                "........",
                "..P.....",
                "........",
                "......k.",
                "........",
                "........");

            Position[] moves = PossibleMoves.FindPossibleMoves(board, Pos("c5"));

            CollectionAssert.AreEquivalent(ManyPos(), moves);
        }

        [TestMethod]
        public void TestPawn_EnPassant_WhiteTakes()
        {
            ChessBoard board = Board(
                "K......k",
                "...P....",
                "........",
                "....p...",
                "........",
                "........",
                "........",
                "........");

            Moves.Move(board, Pos("d7"), Pos("d5"));

            Position[] moves = PossibleMoves.FindPossibleMoves(board, Pos("e5"));

            CollectionAssert.AreEquivalent(ManyPos("d6", "e6"), moves);
        }

        [TestMethod]
        public void TestPawn_EnPassant_BlackTakes()
        {
            ChessBoard board = Board(
                "K......k",
                "........",
                "........",
                "........",
                "....P...",
                "........",
                "...p....",
                "........");

            Moves.Move(board, Pos("d2"), Pos("d4"));

            Position[] moves = PossibleMoves.FindPossibleMoves(board, Pos("e4"));

            CollectionAssert.AreEquivalent(ManyPos("d3", "e3"), moves);
        }

        [TestMethod]
        public void TestPawn_EnPassantImpossible_White()
        {
            ChessBoard board = Board(
                "K......k",
                ".P.P....",
                "........",
                "....p...",
                "........",
                "........",
                ".p......",
                "........");

            Moves.Move(board, Pos("d7"), Pos("d5"));
            Moves.Move(board, Pos("b2"), Pos("b3"));
            Moves.Move(board, Pos("b7"), Pos("b6"));

            Position[] moves = PossibleMoves.FindPossibleMoves(board, Pos("e5"));

            CollectionAssert.AreEquivalent(ManyPos("e6"), moves);
        }

        [TestMethod]
        public void TestPawn_EnPassantImpossible_Black()
        {
            ChessBoard board = Board(
                "K......k",
                ".P......",
                "........",
                "........",
                "....P...",
                "........",
                ".p.p....",
                "........");

            Moves.Move(board, Pos("d2"), Pos("d4"));
            Moves.Move(board, Pos("b7"), Pos("b6"));
            Moves.Move(board, Pos("b2"), Pos("b3"));

            Position[] moves = PossibleMoves.FindPossibleMoves(board, Pos("e4"));


            CollectionAssert.AreEquivalent(ManyPos("e3"), moves);
        }

        [TestMethod]
        public void TestKnight_WhiteStartingPosition()
        {
            ChessBoard board = new ChessBoard();

            Position[] moves = PossibleMoves.FindPossibleMoves(board, Pos("g1"));

            CollectionAssert.AreEquivalent(ManyPos("f3", "h3"), moves);
        }

        [TestMethod]
        public void TestKnight_BlackStartingPosition()
        {
            ChessBoard board = new ChessBoard();

            Position[] moves = PossibleMoves.FindPossibleMoves(board, Pos("b8"));

            CollectionAssert.AreEquivalent(ManyPos("a6", "c6"), moves);
        }

        [TestMethod]
        public void TestKnight_WhiteNormalPosition()
        {
            ChessBoard board = new ChessBoard();
            board.board[3, 4] = new ChessPiece(ChessPiece.Figure.Knight, ChessPiece.Color.White);

            Position[] moves = PossibleMoves.FindPossibleMoves(board, Pos("d5"));

            CollectionAssert.AreEquivalent(ManyPos("c7", "e7", "b6", "f6", "b4", "f4", "c3", "e3"), moves);
        }

        [TestMethod]
        public void TestKnight_BlackNormalPosition()
        {
            ChessBoard board = new ChessBoard();
            board.board[4, 3] = new ChessPiece(ChessPiece.Figure.Knight, ChessPiece.Color.Black);

            Position[] moves = PossibleMoves.FindPossibleMoves(board, Pos("e4"));

            CollectionAssert.AreEquivalent(ManyPos("d2", "f2", "c3", "g3", "c5", "g5", "d6", "f6"), moves);
        }

        [TestMethod]
        public void TestKnight_MoveFromKing()
        {
            ChessBoard board = new ChessBoard();
            board.board[4, 5] = new ChessPiece(ChessPiece.Figure.King, ChessPiece.Color.Black);
            board.board[3, 4] = new ChessPiece(ChessPiece.Figure.Knight, ChessPiece.Color.Black);
            board.board[2, 3] = new ChessPiece(ChessPiece.Figure.Bishop, ChessPiece.Color.White);
            Position[] moves = PossibleMoves.FindPossibleMoves(board, Pos("d5"));

            CollectionAssert.AreEquivalent(ManyPos(), moves);
        }
        [TestMethod]
        public void TestRook_WhiteStartingPosition()
        {
            ChessBoard board = new ChessBoard();

            Position[] moves = PossibleMoves.FindPossibleMoves(board, Pos("a1"));

            CollectionAssert.AreEquivalent(ManyPos(), moves);
        }
        [TestMethod]
        public void TestRook_BlackStartingPosition()
        {
            ChessBoard board = new ChessBoard();

            Position[] moves = PossibleMoves.FindPossibleMoves(board, Pos("a8"));

            CollectionAssert.AreEquivalent(ManyPos(), moves);
        }

        [TestMethod]
        public void TestRook_WhiteMovingAheadAfterPawnMove()
        {
            ChessBoard board = new ChessBoard();
            Moves.Move(board, Pos("a2"), Pos("a4"));

            Position[] moves = PossibleMoves.FindPossibleMoves(board, Pos("a1"));

            CollectionAssert.AreEquivalent(ManyPos("a2", "a3"), moves);
        }
        [TestMethod]
        public void TestRook_BlackMovingAheadAfterPawnMove()
        {
            ChessBoard board = new ChessBoard();
            Moves.Move(board, Pos("a7"), Pos("a5"));

            Position[] moves = PossibleMoves.FindPossibleMoves(board, Pos("a8"));

            CollectionAssert.AreEquivalent(ManyPos("a7", "a6"), moves);
        }
        [TestMethod]
        public void TestRook_WhiteTakes()
        {
            ChessBoard board = new ChessBoard();
            Moves.Move(board, Pos("a2"), Pos("a4"));
            Moves.Move(board, Pos("a1"), Pos("a3"));
            Moves.Move(board, Pos("a3"), Pos("b3"));

            Position[] moves = PossibleMoves.FindPossibleMoves(board, Pos("b3"));

            CollectionAssert.AreEquivalent(ManyPos("h3", "b7", "b6", "b5", "b4", "a3", "c3", "d3", "e3", "f3", "g3"), moves);
        }

        [TestMethod]
        public void TestRook_BlackTakes()
        {
            ChessBoard board = new ChessBoard();
            Moves.Move(board, Pos("a7"), Pos("a5"));
            Moves.Move(board, Pos("a8"), Pos("a6"));
            Moves.Move(board, Pos("a6"), Pos("b6"));

            Position[] moves = PossibleMoves.FindPossibleMoves(board, Pos("b6"));

            CollectionAssert.AreEquivalent(ManyPos("h6", "b2", "a6", "c6", "d6", "e6", "f6", "g6", "b3", "b4", "b5"), moves);
        }
        [TestMethod]
        public void TestRook_MoveFromKing()
        {
            ChessBoard board = new ChessBoard();
            board.board[4, 5] = new ChessPiece(ChessPiece.Figure.King, ChessPiece.Color.Black);
            board.board[3, 4] = new ChessPiece(ChessPiece.Figure.Rook, ChessPiece.Color.Black);
            board.board[2, 3] = new ChessPiece(ChessPiece.Figure.Bishop, ChessPiece.Color.White);
            Position[] moves = PossibleMoves.FindPossibleMoves(board, Pos("d5"));

            CollectionAssert.AreEquivalent(ManyPos(), moves);
        }

        [TestMethod]
        public void TestBishop_WhiteStartingPosition()
        {
            ChessBoard board = new ChessBoard();

            Position[] moves = PossibleMoves.FindPossibleMoves(board, Pos("f1"));

            CollectionAssert.AreEquivalent(ManyPos(), moves);
        }

        [TestMethod]
        public void TestBishop_BlackStartingPosition()
        {
            ChessBoard board = new ChessBoard();

            Position[] moves = PossibleMoves.FindPossibleMoves(board, Pos("f8"));

            CollectionAssert.AreEquivalent(ManyPos(), moves);
        }

        [TestMethod]
        public void TestBishop_ExploreBishopMoves()
        {
            ChessBoard board = Board(
                "........",
                "........",
                "........",
                "........",
                "........",
                "k....b.K",
                "........",
                "........");
            Position[] moves = PossibleMoves.FindPossibleMoves(board, Pos("f3"));

            CollectionAssert.AreEquivalent(ManyPos("g4", "h5", "g2", "h1", "e2", "d1", "e4", "d5", "c6", "b7", "a8"), moves);

        }
        [TestMethod]
        public void TestBishop_WhiteBlockedByOwnColor()
        {
            ChessBoard board = Board(
                "....K...",
                "........",
                "........",
                "........",
                "....p.p.",
                ".....b..",
                "........",
                "k.......");
            Position[] moves = PossibleMoves.FindPossibleMoves(board, Pos("f3"));

            CollectionAssert.AreEquivalent(ManyPos("g2", "h1", "e2", "d1"), moves);

        }
        [TestMethod]
        public void TestBishop_WhiteTakes()
        {
            ChessBoard board = Board(
                "........",
                "........",
                "K.......",
                "........",
                "....p.P.",
                ".....b..",
                ".....k..",
                "........");
            Position[] moves = PossibleMoves.FindPossibleMoves(board, Pos("f3"));

            CollectionAssert.AreEquivalent(ManyPos("g2", "h1", "e2", "d1", "g4"), moves);

        }
        [TestMethod]
        public void TestBishop_BlackBlockedByOwnColor()
        {
            ChessBoard board = Board(
                "k......K",
                "........",
                "........",
                "........",
                "....P.P.",
                ".....B..",
                "........",
                "........");
            Position[] moves = PossibleMoves.FindPossibleMoves(board, Pos("f3"));

            CollectionAssert.AreEquivalent(ManyPos("g2", "h1", "e2", "d1"), moves);

        }
        [TestMethod]
        public void TestBishop_BlackTakes()
        {
            ChessBoard board = Board(
                "........",
                "........",
                "..K..k..",
                "........",
                "....P.p.",
                ".....B..",
                "........",
                "........");
            Position[] moves = PossibleMoves.FindPossibleMoves(board, Pos("f3"));

            CollectionAssert.AreEquivalent(ManyPos("g2", "h1", "e2", "d1", "g4"), moves);

        }
        [TestMethod]
        public void TestBishop_DiscoveredCheck()
        {
            ChessBoard board = Board(
                "........",
                "..K.....",
                "........",
                "........",
                "........",
                "........",
                "R..b..k.",
                "........");

            Position[] moves = PossibleMoves.FindPossibleMoves(board, Pos("d2"));

            CollectionAssert.AreEquivalent(ManyPos(), moves);
        }
        [TestMethod]
        public void TestQueen_WhiteStartingPosition()
        {
            ChessBoard board = new ChessBoard();

            Position[] moves = PossibleMoves.FindPossibleMoves(board, Pos("d1"));

            CollectionAssert.AreEquivalent(ManyPos(), moves);
        }
        [TestMethod]
        public void TestQueen_BlackStartingPosition()
        {
            ChessBoard board = new ChessBoard();

            Position[] moves = PossibleMoves.FindPossibleMoves(board, Pos("d8"));

            CollectionAssert.AreEquivalent(ManyPos(), moves);
        }
        [TestMethod]
        public void TestQueen_ExploreWhiteQueenMovesFromStartingPosition()
        {
            ChessBoard board = Board(
                "........",
                "........",
                "........",
                "........",
                "........",
                "........",
                "K.....k.",
                "...q....");

            Position[] moves = PossibleMoves.FindPossibleMoves(board, Pos("d1"));

            CollectionAssert.AreEquivalent(ManyPos("a1", "b1", "c1", "e1", "f1", "g1", "h1", "d2", "d3", "d4", "d5", "d6", "d7", "d8", "a4", "b3", "c2", "e2", "f3", "g4", "h5"), moves);
        }
        [TestMethod]
        public void TestQueen_ExploreBlackQueenMovesFromNormalPosition()
        {
            ChessBoard board = Board(
                "........",
                "........",
                "........",
                "........",
                "...Q....",
                "........",
                "........",
                "..K..k..");

            Position[] moves = PossibleMoves.FindPossibleMoves(board, Pos("d4"));

            CollectionAssert.AreEquivalent(ManyPos("d1", "d2", "d3", "d5", "d6", "d7", "d8", "a1", "b2", "c3", "e5", "f6", "g7", "h8", "a7", "b6", "c5", "e3", "f2", "g1", "a4", "b4", "c4", "e4", "f4", "g4", "h4"), moves);
        }
        [TestMethod]
        public void TestQueen_WhiteQueenTakesBlack()
        {
            ChessBoard board = Board(
                "k......K",
                "........",
                "........",
                "........",
                "........",
                "........",
                "..pPp...",
                "..pqp...");

            Position[] moves = PossibleMoves.FindPossibleMoves(board, Pos("d1"));

            CollectionAssert.AreEquivalent(ManyPos("d2"), moves);
        }
        [TestMethod]
        public void TestQueen_BlackQueenBlockedByOwnColor()
        {
            ChessBoard board = Board(
                ".......K",
                ".k......",
                "........",
                "........",
                "........",
                "........",
                "..ppp...",
                "..pqp...");

            Position[] moves = PossibleMoves.FindPossibleMoves(board, Pos("d1"));

            CollectionAssert.AreEquivalent(ManyPos(), moves);
        }
        [TestMethod]
        public void TestKing_WhiteStartingPosition()
        {
            ChessBoard board = new ChessBoard();

            Position[] moves = PossibleMoves.FindPossibleMoves(board, Pos("e1"));

            CollectionAssert.AreEquivalent(ManyPos(), moves);
        }
        [TestMethod]
        public void TestKing_BlackStartingPosition()
        {
            ChessBoard board = new ChessBoard();

            Position[] moves = PossibleMoves.FindPossibleMoves(board, Pos("e8"));

            CollectionAssert.AreEquivalent( ManyPos(), moves);
        }
        [TestMethod]
        public void TestKing_ExploreWhiteKingMoves()
        {
            ChessBoard board = Board(
                ".K......",
                "........",
                "........",
                "........",
                "........",
                "....k...",
                "........",
                "........");
            Position[] moves = PossibleMoves.FindPossibleMoves(board, Pos("e3"));

            CollectionAssert.AreEquivalent(ManyPos("e4", "f4", "f3", "f2", "e2", "d2", "d3", "d4"), moves);

        }
        [TestMethod]
        public void TestKing_ExploreBlackKingMoves()
        {
            ChessBoard board = Board(
                "......k.",
                "........",
                "........",
                "........",
                "........",
                "........",
                "........",
                "K.......");
            Position[] moves = PossibleMoves.FindPossibleMoves(board, Pos("a1"));

            CollectionAssert.AreEquivalent( ManyPos("a2", "b2", "b1"), moves);

        }

        [TestMethod]
        public void TestKing_CastlingWest()
        {
            ChessBoard board = Board(
                "....K..R",
                "........",
                "........",
                "........",
                "........",
                "........",
                "........",
                "r...k..r");
           
            Moves.Move(board, new Position("h8"), new Position("h3"));
            Moves.Move(board, new Position("h1"), new Position("h3"));
            Position[] moves = PossibleMoves.FindPossibleMoves(board, Pos("e1"));
            CollectionAssert.AreEquivalent( ManyPos("d1", "d2", "e2", "f2", "f1", "c1"),moves);

        }

        [TestMethod]
        public void TestKing_CastlingWestNotPossible()
        {
            ChessBoard board = Board(
                "....KR..",
                "........",
                "........",
                "........",
                "........",
                "........",
                "........",
                "rn..k..r");

            Position[] moves = PossibleMoves.FindPossibleMoves(board, Pos("e1"));
            CollectionAssert.AreEquivalent(ManyPos("d1", "d2", "e2"), moves);

        }

        [TestMethod]
        public void TestKing_CastlingEast()
        {
            ChessBoard board = Board(
                "R...K..R",
                "........",
                "........",
                "........",
                "........",
                "........",
                "........",
                "r...k...r");

            Moves.Move(board, new Position("a1"), new Position("a3"));
            Moves.Move(board, new Position("a8"), new Position("a3"));
            Position[] moves = PossibleMoves.FindPossibleMoves(board, Pos("e8"));

            CollectionAssert.AreEquivalent(ManyPos("d8", "d7", "e7", "f7", "f8", "g8"),moves);

        }

    
    
    }


}
