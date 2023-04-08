using System;
using System.Collections.Generic;
using System.Text;

namespace CheckersLogic
{
    public class CheckersBoard
    {
        public CheckersPiece[,] board { get; set; }
        public CheckersBoard()
        {
            board = new CheckersPiece[8, 8];

            for(int i=0;i<=2;i++)
                for(int j=0;j<=7;j++)
                {
                    if( (j+i)%2==0 )
                    {
                        board[j, i] = new CheckersPiece(CheckersPiece.Type.Stone, CheckersPiece.Color.White);
                    }
                }

            for (int i = 5; i <= 7; i++)
                for (int j = 0; j <= 7; j++)
                {
                    if ((j + i) % 2 == 0)
                    {
                        board[j, i] = new CheckersPiece(CheckersPiece.Type.Stone, CheckersPiece.Color.Black);
                    }
                }
        }
        public CheckersPiece this[Position position]
        {
            get => board[position.column, position.row];
            set => board[position.column, position.row] = value;
        }
        public CheckersPiece GetPieceFromPosition(Position position)
        {
            return board[position.column, position.row];
        }
        public CheckersPiece GetPieceFromPosition(int row, int column)
        {
            return board[column, row]; 
        }
        public CheckersBoard GetCheckersBoard()
        {
            CheckersBoard newBoard = new CheckersBoard();
            for (int i = 0; i <= 7; i++)
                for (int j = 0; j <= 7; j++)
                {
                    Position pos = new Position(i, j);
                    if (this.IsSquareEmpty(pos))
                        newBoard.SetSquareEmpty(pos);
                    else
                    {
                        CheckersPiece piece = this.GetPieceFromPosition(i, j).GetCheckersPiece();
                        newBoard.SetPieceOnPosition(i, j, piece);
                    }
                }
            return newBoard;
        }
        public bool IsSquareEmpty(Position position)
        {
            return board[position.column, position.row] == null;
        }
        public bool IsWhitePieceOnSquare(Position position)
        {
            return board[position.column, position.row] != null && board[position.column, position.row].color == CheckersPiece.Color.White;
        }
        public bool IsBlackPieceOnSquare(Position position)
        {
            return board[position.column, position.row] != null && board[position.column, position.row].color == CheckersPiece.Color.Black;
        }
        public bool SetPieceOnPosition(Position position, CheckersPiece piece)
        {
            if (!position.IsPositionOnTheBoard()) return false;

            board[position.column, position.row] = piece;
            return true;

        }
        public bool SetPieceOnPosition(int row, int column, CheckersPiece piece)
        {
            if (!new Position(row, column).IsPositionOnTheBoard()) return false;

            board[column, row] = piece;
            return true;
        }
        public void SetSquareEmpty(Position position)
        {
            board[position.column, position.row] = null;
        }
        public override bool Equals(object obj)
        {
            CheckersBoard other = (CheckersBoard)obj;

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
        public override string ToString()
        {
            string board = "";

            for(int i=7;i>=0;i--)
            {
                for(int j=0;j<=7;j++)
                {
                    if(this.board[j,i] == null)
                    {
                        board += ".";
                    }
                    else if(this.board[j,i].color == CheckersPiece.Color.Black)
                    {
                        if (this.board[j, i].type == CheckersPiece.Type.Dame)
                            board += "D";
                        else
                            board += "S";
                    }
                    else
                    {
                        if (this.board[j, i].type == CheckersPiece.Type.Stone)
                            board += "s";
                        else
                            board += "d";
                    }
                }
                board += "\n";
            }

            return board;
        }

    }
}
