using System;
using System.Collections.Generic;
using System.Text;

namespace ChessLogic
{
    public class CheckMate
    {
        public static bool IsCheckMate(ChessBoard board, ChessPiece.Color color) // sprawdzam czy figury w danym kolorze są szachowane
        {
            Position [] possibleMovesForAllPiecesInColor;
            for (int i = 0; i <= 7; i++)
            {
                for (int j = 0; j <= 7; j++)
                {
                    Position pos = new Position(i, j);
                    if(!board.IsSquareEmpty(pos) && board.GetPieceFromPosition(pos).color == color )
                    {
                        possibleMovesForAllPiecesInColor = PossibleMoves.FindPossibleMoves(board, pos);
                        if(possibleMovesForAllPiecesInColor.Length > 0)
                            return false;
                    }
                }
            }
            return true;
        }

    }

}
