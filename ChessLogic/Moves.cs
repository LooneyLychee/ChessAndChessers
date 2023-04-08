using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace ChessLogic
{
    public class Moves
    {
        public static bool Move(ChessBoard board, Position startPosition, Position endPosition)
        {
            Position[] possiblePosition = PossibleMoves.FindPossibleMoves(board, startPosition);

            if (!possiblePosition.Contains(endPosition)) return false;
            
            endPosition = Array.Find(possiblePosition, x => x.Equals(endPosition));
            ChessPiece movingPiece = board.GetPieceFromPosition(startPosition);
            
            if (endPosition.IsMoveOnThisPositionEnPassant())
            { 
                Position capturedPiecePosition = new Position(startPosition.row, endPosition.column);
                board.SetSquareEmpty(capturedPiecePosition);
            }
            else if (endPosition.IsMoveOnThisPositionCastling())
            {
                if(endPosition.column == 2)
                {
                    ChessPiece rook = board.GetPieceFromPosition(new Position(endPosition.row, 0));
                    board.SetSquareEmpty(new Position(endPosition.row,0));
                    board.SetPieceOnPosition(new Position(endPosition.row, 3), rook);
                }
                else
                {
                    ChessPiece rook = board.GetPieceFromPosition(new Position(endPosition.row, 7));
                    board.SetSquareEmpty(new Position(endPosition.row, 7));
                    board.SetPieceOnPosition(new Position(endPosition.row, 5), rook);
                }
            }

            board.SetPieceOnPosition(endPosition, movingPiece);
            board.SetSquareEmpty(startPosition);
           
            // en passant
            for (int i = 0; i <= 7; i++)
            {
                for (int j = 0; j <= 7; j++)
                {
                    Position pos = new Position(i, j);

                    if (!board.IsSquareEmpty(pos))
                    {
                        board.GetPieceFromPosition(pos).isEnPassantPossibleWest = false;
                        board.GetPieceFromPosition(pos).isEnPassantPossibleEast = false;
                    }
                }
            }

            if (board.GetPieceFromPosition(endPosition).figure == ChessPiece.Figure.Pawn && Math.Abs(endPosition.row- startPosition.row) == 2 )
            {
                if(board.GetPieceFromPosition(endPosition).color == ChessPiece.Color.Black && endPosition.row == 4)
                {
                    Position a = new Position(endPosition.row, endPosition.column - 1);
                    if(a.IsPositionOnTheBoard() && !board.IsSquareEmpty(a) && board.GetPieceFromPosition(a).figure == ChessPiece.Figure.Pawn && board.GetPieceFromPosition(a).color != board.GetPieceFromPosition(endPosition).color)
                        board.GetPieceFromPosition(a).isEnPassantPossibleEast = true;

                    a = new Position(endPosition.row, endPosition.column + 1);
                    if (a.IsPositionOnTheBoard() && !board.IsSquareEmpty(a) && board.GetPieceFromPosition(a).figure == ChessPiece.Figure.Pawn && board.GetPieceFromPosition(a).color != board.GetPieceFromPosition(endPosition).color)
                        board.GetPieceFromPosition(a).isEnPassantPossibleWest = true;
                }

                if (board.GetPieceFromPosition(endPosition).color == ChessPiece.Color.White && endPosition.row == 3)
                {
                    Position a = new Position(endPosition.row, endPosition.column - 1);
                    if (a.IsPositionOnTheBoard() && !board.IsSquareEmpty(a) && board.GetPieceFromPosition(a).figure == ChessPiece.Figure.Pawn && board.GetPieceFromPosition(a).color != board.GetPieceFromPosition(endPosition).color)
                        board.GetPieceFromPosition(a).isEnPassantPossibleEast = true;

                    a = new Position(endPosition.row, endPosition.column + 1);
                    if (a.IsPositionOnTheBoard() && !board.IsSquareEmpty(a) && board.GetPieceFromPosition(a).figure == ChessPiece.Figure.Pawn && board.GetPieceFromPosition(a).color != board.GetPieceFromPosition(endPosition).color)
                        board.GetPieceFromPosition(a).isEnPassantPossibleWest = true;
                }

            }
           
            // castling

            if(board.GetPieceFromPosition(endPosition).figure == ChessPiece.Figure.King)
            {
                board.GetPieceFromPosition(endPosition).isCastlingPossibleEast = false;
                board.GetPieceFromPosition(endPosition).isCastlingPossibleWest = false;
            }
            else if(board.GetPieceFromPosition(endPosition).figure == ChessPiece.Figure.Rook)
            {
                if ((startPosition.row == 0 || startPosition.row == 7) && startPosition.column == 0)
                {
                    Position kingPosition = FindKing(board, board.GetPieceFromPosition(endPosition).color);
                    board.GetPieceFromPosition(kingPosition).isCastlingPossibleWest = false;
                }
                else if ((startPosition.row == 0 || startPosition.row == 7) && startPosition.column == 7)
                {
                    Position kingPosition = FindKing(board, board.GetPieceFromPosition(endPosition).color);
                    board.GetPieceFromPosition(kingPosition).isCastlingPossibleEast = false;
                }
            }

            return true;  
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
