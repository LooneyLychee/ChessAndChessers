using System;
using System.Collections.Generic;
using System.Text;

namespace ChessLogic
{
    public class ChessBoard
    {
        public ChessPiece [,] board{ get; set; }

        public ChessBoard()
        {
            board = new ChessPiece[8, 8];
           
            board[0, 0] = new ChessPiece(ChessPiece.Figure.Rook, ChessPiece.Color.White); 
            board[1, 0] = new ChessPiece(ChessPiece.Figure.Knight, ChessPiece.Color.White);
            board[2, 0] = new ChessPiece(ChessPiece.Figure.Bishop, ChessPiece.Color.White);
            board[3, 0] = new ChessPiece(ChessPiece.Figure.Queen, ChessPiece.Color.White);
            board[4, 0] = new ChessPiece(ChessPiece.Figure.King, ChessPiece.Color.White);
            board[5, 0] = new ChessPiece(ChessPiece.Figure.Bishop, ChessPiece.Color.White);
            board[6, 0] = new ChessPiece(ChessPiece.Figure.Knight, ChessPiece.Color.White);
            board[7, 0] = new ChessPiece(ChessPiece.Figure.Rook, ChessPiece.Color.White);
            for (int i = 0; i < 8; i++)
            {
                board[i, 1] = new ChessPiece(ChessPiece.Figure.Pawn, ChessPiece.Color.White);
            }
         
            board[0, 7] = new ChessPiece(ChessPiece.Figure.Rook, ChessPiece.Color.Black);
            board[1, 7] = new ChessPiece(ChessPiece.Figure.Knight, ChessPiece.Color.Black);
            board[2, 7] = new ChessPiece(ChessPiece.Figure.Bishop, ChessPiece.Color.Black);
            board[3, 7] = new ChessPiece(ChessPiece.Figure.Queen, ChessPiece.Color.Black);
            board[4, 7] = new ChessPiece(ChessPiece.Figure.King, ChessPiece.Color.Black);
            board[5, 7] = new ChessPiece(ChessPiece.Figure.Bishop, ChessPiece.Color.Black);
            board[6, 7] = new ChessPiece(ChessPiece.Figure.Knight, ChessPiece.Color.Black);
            board[7, 7] = new ChessPiece(ChessPiece.Figure.Rook, ChessPiece.Color.Black);
            for (int i = 0; i < 8; i++)
            {
                board[i, 6] = new ChessPiece(ChessPiece.Figure.Pawn, ChessPiece.Color.Black);
            }

        }
        
    
        public ChessPiece this[Position position]
        {
            get => board[position.column, position.row];
            set => board[position.column, position.row] = value;         
        }

        public ChessPiece GetPieceFromPosition(Position position)
        {
            return board[position.column, position.row];
        }

        public bool IsSquareEmpty(Position position)
        {
            return board[position.column, position.row] == null; 
        }
        public bool IsWhitePieceOnSquare(Position position)
        {
            return board[position.column, position.row] != null && board[position.column, position.row].color == ChessPiece.Color.White;
        }

        public bool IsBlackPieceOnSquare(Position position)
        {
            return board[position.column, position.row] != null && board[position.column, position.row].color == ChessPiece.Color.Black;
        }

        public bool SetPieceOnPosition(Position position, ChessPiece piece)
        {
            if (!position.IsPositionOnTheBoard ()) return false;

            board[position.column, position.row] = piece;
            return true;

        }


        public void SetSquareEmpty(Position position)
        {
            board[position.column, position.row] = null;
        }

        public override bool Equals(object obj)
        {
            ChessBoard other = (ChessBoard)obj;

            for (int i = 0; i <= 7; i++)
                for (int j = 0; j <= 7; j++)
                    if (board[i, j] != other.board[i, j])
                        return false;
            return true;
        }

        public override int GetHashCode()
        {
            if (board != null)
            {
                unchecked
                {
                    int hash = 17;
                    // get hash code for all items in array
                    foreach (var item in board)
                    {
                        hash = hash * 23 + ((item != null) ? item.GetHashCode() : 0);
                    }

                    return hash;
                }
            }

            // if null, hash code is zero
            return 0;
        }


    }
}
