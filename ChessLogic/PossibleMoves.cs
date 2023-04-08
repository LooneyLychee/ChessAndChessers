using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ChessLogic
{
    public class PossibleMoves
    {
        public static Position[] FindPossibleMoves(ChessBoard board, Position pos)
        {
            if(board.IsSquareEmpty(pos))
                return new Position[0];

            ChessPiece.Figure pieceType = board.GetPieceFromPosition(pos).figure;
            
            switch (pieceType)
            {
                case ChessPiece.Figure.Pawn:
                    return PossibleMovesPawn(board,pos);
                case ChessPiece.Figure.King:
                    return PossibleMovesKing(board, pos);
                case ChessPiece.Figure.Queen:
                    return PossibleMovesQueen(board, pos);
                case ChessPiece.Figure.Rook:
                    return PossibleMovesRook(board, pos);
                case ChessPiece.Figure.Bishop:
                    return PossibleMovesBishop(board, pos);
                case ChessPiece.Figure.Knight:
                    return PossibleMovesKnight(board, pos);
                default:
                    return new Position[0];
            }
        }

        private static Position[] PossibleMovesPawn(ChessBoard board, Position pos)
        {
            ChessPiece piece = board.GetPieceFromPosition(pos);
            List<Position> listPossibleMoves;

            if (piece.color == ChessPiece.Color.White)
                listPossibleMoves = ExploreWhitePawn(board, pos);
            else
                listPossibleMoves = ExploreBlackPawn(board, pos);

            listPossibleMoves = DeleteMoveWhenCheckExistAfterMove(board, pos, listPossibleMoves);

            // en passant
            Console.WriteLine(listPossibleMoves.Count);
            Console.WriteLine(EnPassantWhite(board, pos).Count);
           

            if (piece.color == ChessPiece.Color.White)
                return listPossibleMoves.Concat(EnPassantWhite(board,pos)).ToArray();
            else
                return listPossibleMoves.Concat(EnPassantBlack(board, pos)).ToArray();
            

        }
        private static Position[] PossibleMovesKing(ChessBoard board, Position pos)
        {
            List<Position> listPossiblePos;
            
            listPossiblePos = ExploreKing(board, pos);
            
            listPossiblePos = DeleteMoveWhenCheckExistAfterMove(board, pos, listPossiblePos);

            // castling
            return listPossiblePos.Concat(Castling(board, pos)).ToArray();

            
        }
        private static Position[] PossibleMovesQueen(ChessBoard board, Position pos)
        {
            List<Position> listPossiblePos;

            listPossiblePos = ExploreQueen(board, pos);

            listPossiblePos = DeleteMoveWhenCheckExistAfterMove(board, pos, listPossiblePos);

            return listPossiblePos.ToArray();
        }
        private static Position[] PossibleMovesRook(ChessBoard board, Position pos)
        {
            List<Position> listPossiblePos;

            listPossiblePos = ExploreRook(board, pos);

            listPossiblePos = DeleteMoveWhenCheckExistAfterMove(board, pos, listPossiblePos);

            return listPossiblePos.ToArray();
        }
        private static Position[] PossibleMovesBishop(ChessBoard board, Position pos)
        {
            List<Position> listPossiblePos;

            listPossiblePos = ExploreBishop(board, pos);
           
            listPossiblePos = DeleteMoveWhenCheckExistAfterMove(board, pos, listPossiblePos);


            return listPossiblePos.ToArray();
        }
        private static Position[] PossibleMovesKnight(ChessBoard board, Position pos)
        {
            List<Position> listPossiblePos;

            listPossiblePos = ExploreKnight(board, pos);

            listPossiblePos = DeleteMoveWhenCheckExistAfterMove(board, pos, listPossiblePos);

            return listPossiblePos.ToArray();
        }       
        private static List<Position> ExploreWhitePawn(ChessBoard board, Position pos)
        {
            List<Position> listpossibleMoves = new List<Position>();
            ChessPiece piece = board.GetPieceFromPosition(pos);
            

            var possiblePos = new Dictionary<string, Position>
            {
                ["normal"] = new Position(pos.row + 1, pos.column),
                ["start"] = new Position(pos.row + 2, pos.column),
                ["northWest"] = new Position(pos.row + 1, pos.column - 1),
                ["northEast"] = new Position(pos.row + 1, pos.column + 1),

            };

            //normal position
            if (possiblePos["normal"].IsPositionOnTheBoard() && board.IsSquareEmpty(possiblePos["normal"]))
                    listpossibleMoves.Add(possiblePos["normal"]);
            
            //starting position
            if (pos.row == 1 && board.IsSquareEmpty(possiblePos["normal"]) && board.IsSquareEmpty(possiblePos["start"]))
                    listpossibleMoves.Add(possiblePos["start"]);

            // captured 
            if (possiblePos["northWest"].IsPositionOnTheBoard() && board.IsBlackPieceOnSquare(possiblePos["northWest"]))
                    listpossibleMoves.Add(possiblePos["northWest"]);                       
            if (possiblePos["northEast"].IsPositionOnTheBoard() && board.IsBlackPieceOnSquare(possiblePos["northEast"]))
                    listpossibleMoves.Add(possiblePos["northEast"]);

            return listpossibleMoves;
        }
        private static List<Position> ExploreBlackPawn(ChessBoard board, Position pos)
        {
            List<Position> listpossibleMoves = new List<Position>();
            
            var possiblePos = new Dictionary<string, Position>
            {
                ["normal"] = new Position(pos.row - 1, pos.column),
                ["start"] = new Position(pos.row - 2, pos.column),
                ["northWest"] = new Position(pos.row - 1, pos.column - 1),
                ["northEast"] = new Position(pos.row - 1, pos.column + 1),

            };
            //normal position
            if (possiblePos["normal"].IsPositionOnTheBoard() && board.IsSquareEmpty(possiblePos["normal"]))
                listpossibleMoves.Add(possiblePos["normal"]);

            //starting position
            if (pos.row == 6 && board.IsSquareEmpty(possiblePos["normal"]) && board.IsSquareEmpty(possiblePos["start"]))
                listpossibleMoves.Add(possiblePos["start"]);

            // captured 
            if (possiblePos["northWest"].IsPositionOnTheBoard() && board.IsWhitePieceOnSquare(possiblePos["northWest"]))
                listpossibleMoves.Add(possiblePos["northWest"]);
            if (possiblePos["northEast"].IsPositionOnTheBoard() && board.IsWhitePieceOnSquare(possiblePos["northEast"]))
                listpossibleMoves.Add(possiblePos["northEast"]);

            return listpossibleMoves;
        }
        private static List<Position> ExploreKing(ChessBoard board, Position pos)
        {
            List<Position> listPossibleMoves = new List<Position>();
            ChessPiece piece = board.GetPieceFromPosition(pos);

            Position[] possiblePos =
            {
                new Position(pos.row + 1, pos.column),
                new Position(pos.row - 1, pos.column),
                new Position(pos.row, pos.column - 1),
                new Position(pos.row, pos.column + 1),
                new Position(pos.row + 1, pos.column-1),
                new Position(pos.row - 1, pos.column-1),
                new Position(pos.row+1, pos.column + 1),
                new Position(pos.row-1, pos.column + 1),

            };

            foreach(Position p in possiblePos)
            {
                if(p.IsPositionOnTheBoard() && (board.IsSquareEmpty(p) || board.IsBlackPieceOnSquare(p) != board.IsBlackPieceOnSquare(pos)))
                {
                    listPossibleMoves.Add(p);
                }
            }

            return listPossibleMoves;
        }
        private static List<Position> ExploreQueen(ChessBoard board, Position pos)
        {
            List<Position> listPossibleMoves = new List<Position>();

            listPossibleMoves = ExploreBishop(board, pos);
            return listPossibleMoves.Concat(ExploreRook(board, pos)).ToList();
            
        }
        private static List<Position> ExploreRook(ChessBoard board, Position pos)
        {
            List<Position> listPossibleMoves = new List<Position>();
            Position possiblePos;

            // North
            possiblePos = new Position(pos.row + 1, pos.column);
            while (possiblePos.IsPositionOnTheBoard() && board.IsSquareEmpty(possiblePos))
            {
                listPossibleMoves.Add(possiblePos);
                possiblePos = new Position(possiblePos.row + 1, possiblePos.column);
            }
            if (possiblePos.IsPositionOnTheBoard() && board.IsBlackPieceOnSquare(pos) != board.IsBlackPieceOnSquare(possiblePos))
                listPossibleMoves.Add(possiblePos);

            // South
            possiblePos = new Position(pos.row - 1, pos.column);
            while (possiblePos.IsPositionOnTheBoard() && board.IsSquareEmpty(possiblePos))
            {
                listPossibleMoves.Add(possiblePos);
                possiblePos = new Position(possiblePos.row - 1, possiblePos.column);
            }
            if (possiblePos.IsPositionOnTheBoard() && board.IsBlackPieceOnSquare(pos) != board.IsBlackPieceOnSquare(possiblePos))
                listPossibleMoves.Add(possiblePos);

            // West
            possiblePos = new Position(pos.row, pos.column - 1);
            while (possiblePos.IsPositionOnTheBoard() && board.IsSquareEmpty(possiblePos))
            {
                listPossibleMoves.Add(possiblePos);
                possiblePos = new Position(possiblePos.row, possiblePos.column - 1);
            }
            if (possiblePos.IsPositionOnTheBoard() && board.IsBlackPieceOnSquare(pos) != board.IsBlackPieceOnSquare(possiblePos))
                listPossibleMoves.Add(possiblePos);

            // South
            possiblePos = new Position(pos.row, pos.column + 1);
            while (possiblePos.IsPositionOnTheBoard() && board.IsSquareEmpty(possiblePos))
            {
                listPossibleMoves.Add(possiblePos);
                possiblePos = new Position(possiblePos.row, possiblePos.column + 1);
            }
            if (possiblePos.IsPositionOnTheBoard() && board.IsBlackPieceOnSquare(pos) != board.IsBlackPieceOnSquare(possiblePos))
                listPossibleMoves.Add(possiblePos);
            
            return listPossibleMoves;
        }
        private static List<Position> ExploreBishop(ChessBoard board, Position pos)
        {
            List<Position> listPossibleMoves = new List<Position>();
            Position possiblePos;

            // NorthWest
            possiblePos = new Position(pos.row + 1, pos.column - 1);
            while (possiblePos.IsPositionOnTheBoard() && board.IsSquareEmpty(possiblePos))
            {
                listPossibleMoves.Add(possiblePos);
                possiblePos = new Position(possiblePos.row + 1, possiblePos.column - 1);
            }
            if (possiblePos.IsPositionOnTheBoard() && board.IsBlackPieceOnSquare(pos) != board.IsBlackPieceOnSquare(possiblePos)) 
                listPossibleMoves.Add(possiblePos);
            
            // NorthEast
            possiblePos = new Position(pos.row + 1, pos.column + 1);
            while (possiblePos.IsPositionOnTheBoard() && board.IsSquareEmpty(possiblePos))
            {
                listPossibleMoves.Add(possiblePos);
                possiblePos = new Position(possiblePos.row + 1, possiblePos.column + 1);
            }
            if (possiblePos.IsPositionOnTheBoard() && board.IsBlackPieceOnSquare(pos) != board.IsBlackPieceOnSquare(possiblePos))
                listPossibleMoves.Add(possiblePos);

            // SouthWest
            possiblePos = new Position(pos.row - 1, pos.column - 1);
            while (possiblePos.IsPositionOnTheBoard() && board.IsSquareEmpty(possiblePos))
            {
                listPossibleMoves.Add(possiblePos);
                possiblePos = new Position(possiblePos.row - 1, possiblePos.column - 1);
            }
            if (possiblePos.IsPositionOnTheBoard() && board.IsBlackPieceOnSquare(pos) != board.IsBlackPieceOnSquare(possiblePos))
                listPossibleMoves.Add(possiblePos);

            // SouthEast
            possiblePos = new Position(pos.row - 1, pos.column + 1);
            while (possiblePos.IsPositionOnTheBoard() && board.IsSquareEmpty(possiblePos))
            {
                listPossibleMoves.Add(possiblePos);
                possiblePos = new Position(possiblePos.row - 1, possiblePos.column + 1);
            }
            if (possiblePos.IsPositionOnTheBoard() && board.IsBlackPieceOnSquare(pos) != board.IsBlackPieceOnSquare(possiblePos))
                listPossibleMoves.Add(possiblePos);

            return listPossibleMoves;
        }
        private static List<Position> ExploreKnight(ChessBoard board, Position pos)
        {
            List<Position> listPossibleMoves = new List<Position>();
        
            Position[] possiblePos =
            {
                new Position(pos.row + 2, pos.column+1),
                new Position(pos.row + 2 , pos.column-1),
                new Position(pos.row - 2, pos.column+1),
                new Position(pos.row - 2 , pos.column-1),
                new Position(pos.row + 1, pos.column+2),
                new Position(pos.row + 1, pos.column-2),
                new Position(pos.row - 1, pos.column+2),
                new Position(pos.row - 1, pos.column-2),

            };

            foreach (Position p in possiblePos)
            {
                if (p.IsPositionOnTheBoard() && (board.IsSquareEmpty(p) || board.IsBlackPieceOnSquare(p) != board.IsBlackPieceOnSquare(pos)))
                {
                    listPossibleMoves.Add(p);
                }
            }

            return listPossibleMoves;
        }
        private static List<Position> EnPassantWhite(ChessBoard board, Position capturingPiecePosition)
        {
            List<Position> listPossibleMoves = new List<Position>();
            ChessPiece piece = board.GetPieceFromPosition(capturingPiecePosition);

            var enPassantPossibleMoves = new Dictionary<string, Position>
            {
                ["northWest"] = new Position(capturingPiecePosition.row + 1, capturingPiecePosition.column - 1),
                ["northEast"] = new Position(capturingPiecePosition.row + 1, capturingPiecePosition.column + 1),
            };
            
            Position capturedPiecePosition = new Position(capturingPiecePosition.row, capturingPiecePosition.column - 1);
            
            if (piece.isEnPassantPossibleWest && enPassantPossibleMoves["northWest"].IsPositionOnTheBoard() && board.IsBlackPieceOnSquare(capturedPiecePosition))
            {
                
                board[capturingPiecePosition] = null;
                board.SetPieceOnPosition(enPassantPossibleMoves["northWest"], piece);
                ChessPiece capturedPiece = board.GetPieceFromPosition(capturedPiecePosition);
                board[capturedPiecePosition] = null;

                if (!DoesCheckExist(board, FindKing(board, ChessPiece.Color.White)))
                {
                    enPassantPossibleMoves["northWest"].SetMoveOnThisPositionEnPassant(true);
                    listPossibleMoves.Add(enPassantPossibleMoves["northWest"]);
                }

                board.SetPieceOnPosition(capturingPiecePosition, piece);
                board[enPassantPossibleMoves["northWest"]] = null;
                board.SetPieceOnPosition(capturedPiecePosition, capturedPiece);
            }    
            
            capturedPiecePosition = new Position(capturingPiecePosition.row, capturingPiecePosition.column + 1);
            if (piece.isEnPassantPossibleEast && enPassantPossibleMoves["northEast"].IsPositionOnTheBoard() && board.IsBlackPieceOnSquare(capturedPiecePosition))
            {
                board[capturingPiecePosition] = null;
                board.SetPieceOnPosition(enPassantPossibleMoves["northEast"], piece);
                ChessPiece capturedPiece = board.GetPieceFromPosition(capturedPiecePosition);
                board[capturedPiecePosition] = null;

                if (!DoesCheckExist(board, FindKing(board, ChessPiece.Color.White)))
                {
                    enPassantPossibleMoves["northEast"].SetMoveOnThisPositionEnPassant(true);
                    listPossibleMoves.Add(enPassantPossibleMoves["northEast"]);
                }

                board.SetPieceOnPosition(capturingPiecePosition, piece);
                board[enPassantPossibleMoves["northEast"]] = null;
                board.SetPieceOnPosition(capturedPiecePosition, capturedPiece);
            }

            
            return listPossibleMoves;

        }
        private static List<Position> EnPassantBlack(ChessBoard board, Position capturingPiecePosition)
        {
            List<Position> listPossibleMoves = new List<Position>();
            ChessPiece piece = board.GetPieceFromPosition(capturingPiecePosition);

            var enPassantPossibleMoves = new Dictionary<string, Position>
            {
                ["southWest"] = new Position(capturingPiecePosition.row - 1, capturingPiecePosition.column - 1),
                ["southEast"] = new Position(capturingPiecePosition.row - 1, capturingPiecePosition.column + 1),
            };

            Position capturedPiecePosition = new Position(capturingPiecePosition.row, capturingPiecePosition.column - 1);
            if (piece.isEnPassantPossibleWest && enPassantPossibleMoves["southWest"].IsPositionOnTheBoard() && board.IsWhitePieceOnSquare(capturedPiecePosition))
            {
                board[capturingPiecePosition] = null;
                board.SetPieceOnPosition(enPassantPossibleMoves["southWest"], piece);
                ChessPiece capturedPiece = board.GetPieceFromPosition(capturedPiecePosition);
                board[capturedPiecePosition] = null;

                if (!DoesCheckExist(board, FindKing(board, ChessPiece.Color.Black)))
                {
                    enPassantPossibleMoves["southWest"].SetMoveOnThisPositionEnPassant(true);
                    listPossibleMoves.Add(enPassantPossibleMoves["southWest"]);
                }

                board.SetPieceOnPosition(capturingPiecePosition, piece);
                board[enPassantPossibleMoves["southWest"]] = null;
                board.SetPieceOnPosition(capturedPiecePosition, capturedPiece);
            }

            capturedPiecePosition = new Position(capturingPiecePosition.row, capturingPiecePosition.column + 1);
            if (piece.isEnPassantPossibleEast && enPassantPossibleMoves["southEast"].IsPositionOnTheBoard() && board.IsWhitePieceOnSquare(capturedPiecePosition))
            {
                board[capturingPiecePosition] = null;
                board.SetPieceOnPosition(enPassantPossibleMoves["southEast"], piece);
                ChessPiece capturedPiece = board.GetPieceFromPosition(capturedPiecePosition);
                board[capturedPiecePosition] = null;

                if (!DoesCheckExist(board, FindKing(board, ChessPiece.Color.Black)))
                {
                    enPassantPossibleMoves["southEast"].SetMoveOnThisPositionEnPassant(true);
                    listPossibleMoves.Add(enPassantPossibleMoves["southEast"]);
                }

                board.SetPieceOnPosition(capturingPiecePosition, piece);
                board[enPassantPossibleMoves["southEast"]] = null;
                board.SetPieceOnPosition(capturedPiecePosition, capturedPiece);
            }

            return listPossibleMoves;

        }
        private static List<Position> Castling(ChessBoard board, Position kingPosition)
        {
            

            List<Position> listPossibleMoves = new List<Position>();
            ChessPiece king = board.GetPieceFromPosition(kingPosition);
            if (king.color == ChessPiece.Color.Black)
            {

                ChessPiece rook = new ChessPiece(ChessPiece.Figure.Rook, ChessPiece.Color.Black);

                if (board.IsSquareEmpty(new Position("e8")) || !(board.GetPieceFromPosition(new Position("e8")).Equals(king)))
                {
                    king.isCastlingPossibleEast = false;
                    king.isCastlingPossibleWest = false;
                }
                else if (board.IsSquareEmpty(new Position("h8")) || !(board.GetPieceFromPosition(new Position("h8")).Equals(rook)))
                    king.isCastlingPossibleEast = false;
                else if (board.IsSquareEmpty(new Position("a8")) || !(board.GetPieceFromPosition(new Position("a8")).Equals(rook)))
                    king.isCastlingPossibleWest = false;
            }
            else
            {
                ChessPiece rook = new ChessPiece(ChessPiece.Figure.Rook, ChessPiece.Color.White);

                if (board.IsSquareEmpty(new Position("e1")) || !(board.GetPieceFromPosition(new Position("e1")).Equals(king)))
                {
                    king.isCastlingPossibleEast = false;
                    king.isCastlingPossibleWest = false;
                }
                else if (board.IsSquareEmpty(new Position("h1")) || !(board.GetPieceFromPosition(new Position("h1")).Equals(rook)))
                    king.isCastlingPossibleEast = false;
                else if (board.IsSquareEmpty(new Position("a1")) || !(board.GetPieceFromPosition(new Position("a1")).Equals(rook)))
                    king.isCastlingPossibleWest = false;
            }

            board[kingPosition] = null;

            if (king.isCastlingPossibleWest)
            {
                
                Position[] kingPath =
                {
                    new Position(kingPosition.row, 2),
                    new Position(kingPosition.row, 3)
                };
                
                bool castlingPossible = true;


                foreach (Position pos in kingPath)
                {
                    ChessPiece pieceOnPos = board.GetPieceFromPosition(pos);
                    board.SetPieceOnPosition(pos, king);
                    if (pieceOnPos != null || DoesCheckExist(board, pos))
                    {
                        castlingPossible = false;
                        board.SetPieceOnPosition(pos, pieceOnPos);
                        break;
                    }

                    board.SetPieceOnPosition(pos, pieceOnPos);
                }

                if (castlingPossible && board.IsSquareEmpty(new Position(kingPosition.row,1)))
                {
                    Position possibleMove = new Position(kingPosition.row, 2);
                    possibleMove.SetMoveOnThisPositionCastling(true);
                    listPossibleMoves.Add(possibleMove);
                }
            }
            
            if (king.isCastlingPossibleEast)
            {
                
                Position[] kingPath =
                {
                    new Position(kingPosition.row, 5),
                    new Position(kingPosition.row, 6)
                };

                bool castlingPossible = true;


                foreach (Position pos in kingPath)
                {
                    
                    ChessPiece pieceOnPos = board.GetPieceFromPosition(pos);
                    board.SetPieceOnPosition(pos, king);
                    if (pieceOnPos !=null || DoesCheckExist(board, pos))
                    {
                        castlingPossible = false;
                        board.SetPieceOnPosition(pos, pieceOnPos);
                        break;
                    }
                    board.SetPieceOnPosition(pos, pieceOnPos);
                }

                if (castlingPossible)
                {

                    Position possibleMove = new Position(kingPosition.row, 6);
                    possibleMove.SetMoveOnThisPositionCastling(true);
                    listPossibleMoves.Add(possibleMove);
                }
            }

            board.SetPieceOnPosition(kingPosition, king);
            return listPossibleMoves;
        }
        private static List<Position> DeleteMoveWhenCheckExistAfterMove(ChessBoard board, Position piecePosition, List<Position> possibleMoves)
        {
            ChessPiece piece = board.GetPieceFromPosition(piecePosition);

            List<Position> listpossibleMoves = new List<Position>();

            Position kingPosition = FindKing(board,piece.color);
           
            board[piecePosition] = null;

            foreach (Position pieceNewPosition in possibleMoves)
            {
                ChessPiece pieceOnChangedPositionBeforeChange;

                if (board.IsSquareEmpty(pieceNewPosition))
                    pieceOnChangedPositionBeforeChange = null;
                else
                    pieceOnChangedPositionBeforeChange = board.GetPieceFromPosition(pieceNewPosition);

                board.SetPieceOnPosition(pieceNewPosition, piece);

                if (piece.figure == ChessPiece.Figure.King)
                    kingPosition = FindKing(board, piece.color); // if the piece is king, king's possition is changed
                
                if (!DoesCheckExist(board, kingPosition))
                    listpossibleMoves.Add(pieceNewPosition);

                board.SetPieceOnPosition(pieceNewPosition, pieceOnChangedPositionBeforeChange);
            }
            board.SetPieceOnPosition(piecePosition,piece);
            return listpossibleMoves;
        }
        private static bool DoesCheckExist(ChessBoard board, Position kingPosition)
        {
            Position pos;
            List<Position> possibleMoves;
            
            for (int i = 0; i <= 7; i++)
            {
                for (int j = 0; j <= 7; j++)
                {
                    pos = new Position(i, j);
                    
                        if (!board.IsSquareEmpty(pos) && board.GetPieceFromPosition(pos).color != board.GetPieceFromPosition(kingPosition).color)
                        {
                            if (board.GetPieceFromPosition(pos).figure == ChessPiece.Figure.Pawn && board.GetPieceFromPosition(pos).color == ChessPiece.Color.White)
                                possibleMoves = ExploreWhitePawn(board, pos);
                            else if (board.GetPieceFromPosition(pos).figure == ChessPiece.Figure.Pawn)
                                possibleMoves = ExploreBlackPawn(board, pos);
                            else if (board.GetPieceFromPosition(pos).figure == ChessPiece.Figure.Bishop)
                                possibleMoves = ExploreBishop(board, pos);
                            else if (board.GetPieceFromPosition(pos).figure == ChessPiece.Figure.King)
                                possibleMoves = ExploreKing(board, pos);
                            else if (board.GetPieceFromPosition(pos).figure == ChessPiece.Figure.Knight)
                                possibleMoves = ExploreKnight(board, pos);
                            else if (board.GetPieceFromPosition(pos).figure == ChessPiece.Figure.Queen)
                                possibleMoves = ExploreQueen(board, pos);
                            else
                                possibleMoves = ExploreRook(board, pos);
                
                            foreach(Position p in possibleMoves)
                                if(p.Equals(kingPosition))
                                    return true;
                        }     
                }
            }
            
            return false;
        }
        private static Position FindKing(ChessBoard board, ChessPiece.Color kingColor)
        {
            Position pos;
            for (int i = 0; i <= 7; i++)
            {
                for (int j = 0; j <= 7; j++)
                {
                    pos = new Position(i, j);
                    
                    if (!board.IsSquareEmpty(pos) && board.GetPieceFromPosition(pos).figure == ChessPiece.Figure.King && board.GetPieceFromPosition(pos).color == kingColor)
                        return pos;
                }
            }

            throw new Exception("something went wrong and King don't exsist");
        }
        

    }
}
