using System;
using System.Collections.Generic;
using System.Text;

namespace CheckersLogic
{
    class Moves
    {
        public enum Status
        {
            endOfMove,
            moveIsImpossible,
            moveExist
        }

        private Position currentPosition;
        private Status moveStatus;
        private PossibleMoves possibleMoves;
        public Moves(CheckersBoard board, Position startPosition)
        {
            this.currentPosition = startPosition;
            this.possibleMoves = PossibleMoves.FindPossibleMove(board, startPosition);
            if (possibleMoves == null || possibleMoves.nextPosition.Count == 0)
                this.moveStatus = Status.moveIsImpossible;
            else
                this.moveStatus = Status.moveExist;

        }
        private void MovesChange(CheckersBoard board, Position startPosition, PossibleMoves possibleMoves)
        {
            this.currentPosition = startPosition;
            this.possibleMoves = possibleMoves;
            if (possibleMoves == null || possibleMoves.nextPosition.Count == 0)
            {
                this.moveStatus = Status.endOfMove;
                if(possibleMoves.position.row == 7 && board.IsWhitePieceOnSquare(possibleMoves.position))
                {
                    board.GetPieceFromPosition(possibleMoves.position).Crownig();
                }
            }
            else
                this.moveStatus = Status.moveExist;

        }
        public bool Move(CheckersBoard board, Position endPosition)
        {
            if (moveStatus == Status.endOfMove || moveStatus == Status.moveIsImpossible)
                return false;

            bool isEndPositionIsPossible = false;
            foreach(PossibleMoves possibleMove in possibleMoves.nextPosition)
            {
                if(possibleMove.position == endPosition)
                {
                    isEndPositionIsPossible = true;
                    
                    // move
                    if(possibleMove.typeOfMove == PossibleMoves.Type.capturing)
                    {
                        Position capturedPiecePosition = CapturedPiecePosition(board, this.currentPosition, endPosition);
                        board.SetSquareEmpty(capturedPiecePosition);
                        board.SetPieceOnPosition(endPosition, board.GetPieceFromPosition(currentPosition));
                        board.SetSquareEmpty(currentPosition);
                    }
                    else
                    {
                        board.SetPieceOnPosition(endPosition, board.GetPieceFromPosition(currentPosition));
                        board.SetSquareEmpty(currentPosition);
                    }

                    // class actualization
                    MovesChange(board,endPosition,possibleMove);
                    break;
                }
            }

            if (!isEndPositionIsPossible)
                return false;
            return true;
        }

        private static Position CapturedPiecePosition(CheckersBoard board, Position startPosition, Position endPosition)
        {
            int columnChange;
            int rowChange;

            if (startPosition.column > endPosition.column)
            { columnChange = -1; }
            else
            { columnChange = 1; }

            if (startPosition.row > endPosition.row)
            { rowChange = -1; }
            else
            { rowChange = 1; }

            Position capturedPiecePosition = new Position(startPosition.row + rowChange, startPosition.column + columnChange);
            // we know that captured piece exist
            while (endPosition.row != capturedPiecePosition.row)
            {
                if (!board.IsSquareEmpty(capturedPiecePosition))
                    return capturedPiecePosition;

                capturedPiecePosition.row += rowChange;
                capturedPiecePosition.column += columnChange;
            }
            throw new Exception("captured piece was not found");
           
        }
    }

    
}
