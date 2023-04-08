using System;
using System.Collections.Generic;
using System.Text;

namespace CheckersLogic
{
    public class PossibleMoves
    {
        public enum Type
        {
            capturing,
            normalMove
        }

        public Position position;
        public int howManyPieceWillBeCaptured;
        public Type typeOfMove;
        public List<PossibleMoves> nextPosition;
        public PossibleMoves(Position position,Type typeOfMove)
        {
            this.position = position;
            this.howManyPieceWillBeCaptured = 0;
            this.nextPosition = new List<PossibleMoves>();
            this.typeOfMove = typeOfMove;
        }
        public override bool Equals(object obj)
        {
            bool isEquals = true;
            bool isElementExist;
            PossibleMoves other = (PossibleMoves)obj;
            Console.WriteLine("{0} - {1}", this.position, other.position);

            if (other.nextPosition.Count != this.nextPosition.Count)
                return false;
            
            foreach (PossibleMoves otherNextMove in other.nextPosition)
            {
                isElementExist = false;
                foreach(PossibleMoves nextMove in this.nextPosition)
                {
                    Console.WriteLine("{0} - {1}", otherNextMove.position, nextMove.position);
                    if (otherNextMove.position.Equals(nextMove.position))
                    {
                        isElementExist = true;
                        break;
                    }
                }

                if (isElementExist == false)
                    return false;
                
            }

            foreach (PossibleMoves otherNextMove in other.nextPosition)
            {
                foreach (PossibleMoves nextMove in this.nextPosition)
                {
                    if (otherNextMove.position.Equals(nextMove.position))
                    {
                        isEquals = nextMove.Equals(otherNextMove);
                        break;
                    }
                }
                if (isEquals == false)
                    return false;
            }

            return isEquals;
        }
        public static PossibleMoves FindPossibleMove(CheckersBoard board, Position position)
        {
            if (!position.IsPositionOnTheBoard() || board.IsSquareEmpty(position))
                return null;

            CheckersPiece piece = board.GetPieceFromPosition(position);
            List<PossibleMoves> listOfTreesWithPossibleMoves = ListOfTreesWithAllPossibleMovesForColor.FindPossibleMovesForColor(board,piece.color);

            int maximumCaptured = 0;

            foreach(PossibleMoves tree in listOfTreesWithPossibleMoves)
            {
                maximumCaptured = Math.Max(maximumCaptured, tree.howManyPieceWillBeCaptured);
            }

            foreach (PossibleMoves treeRoot in listOfTreesWithPossibleMoves)
            {
                if (treeRoot.position.Equals(position))
                {
                    // if capturing for this position exist
                    if (treeRoot.howManyPieceWillBeCaptured == maximumCaptured && maximumCaptured!=0)
                        return RemoveWrongEdges(treeRoot);
                    // for normal move and when move is not exist
                    else
                        return treeRoot;
                }
            }

            // only if none of the pieces in color exist
            return null;
        }
        private static PossibleMoves RemoveWrongEdges(PossibleMoves currentMove)
        {
            int index = 0;

            while(index < currentMove.nextPosition.Count)
            {
                if (currentMove.nextPosition[index].howManyPieceWillBeCaptured != currentMove.howManyPieceWillBeCaptured - 1)
                    currentMove.nextPosition.RemoveAt(index);
                else
                    index++;
            }

            index = 0;

            while (index < currentMove.nextPosition.Count)
            {
                currentMove.nextPosition[index] = RemoveWrongEdges(currentMove.nextPosition[index]);
                index++;
            }

            return currentMove;
        } 
    }
    
    public static class ListOfTreesWithAllPossibleMovesForColor
    {
        private enum Directions
        {
            northWest,
            northEast,
            southWest,
            southEast

        }
        private static Position Move(this Directions direction, Position position)
        {
          
            if (direction == Directions.northEast)
                return new Position(position.row + 1, position.column + 1);
            else if (direction == Directions.northWest)
                return new Position(position.row + 1, position.column - 1);
            else if (direction == Directions.southWest)
                return new Position(position.row - 1, position.column - 1);
            else
                return new Position(position.row - 1, position.column + 1);
        }
        public static List<PossibleMoves> FindPossibleMovesForColor(CheckersBoard board, CheckersPiece.Color color)
        {
            List<PossibleMoves> possibleMoves = new List<PossibleMoves>();

            for(int row = 0; row < board.board.GetLength(1); row++)
            {
                for(int column = 0; column < board.board.GetLength(0) ;column++)
                {
                    Position square = new Position(row, column);
                    if(!board.IsSquareEmpty(square) && board.GetPieceFromPosition(square).color == color)
                    {
                        if (board.GetPieceFromPosition(square).type == CheckersPiece.Type.Stone)
                            possibleMoves.Add(FindPossibleMoveForStone(board, square));
                        else
                            possibleMoves.Add(FindPossibleMoveForDame(board, square));
                    }
                }
            }
            
             

            return possibleMoves;
        }
        private static PossibleMoves FindPossibleMoveForStone(CheckersBoard board, Position startPosition)
        {

            PossibleMoves firstMove;
            if(DoesCapturedExistForStone(board,startPosition))
            {
                firstMove = new PossibleMoves(startPosition, PossibleMoves.Type.capturing);
                // new array for simulation moves
                CheckersBoard workingBoard = board.GetCheckersBoard(); 
                
                ExploreCapturingForStone(workingBoard, board, startPosition, startPosition, firstMove);
            }
            else
            {
                firstMove = new PossibleMoves(startPosition, PossibleMoves.Type.normalMove);
                ExploreNormalMoveForStone(board, startPosition, firstMove);
            }

            return firstMove;
        }
        private static bool DoesCapturedExistForStone(CheckersBoard board, Position position)
        {
            Position[] capturedPiecePositions =
           {
                new Position(position.row+1,position.column-1),
                new Position(position.row + 1, position.column + 1),
                new Position(position.row - 1, position.column - 1),
                new Position(position.row - 1, position.column + 1)
            };
            Position[] possiblePositions =
            {
                new Position(position.row + 2, position.column - 2),
                new Position(position.row + 2, position.column + 2),
                new Position(position.row - 2, position.column - 2),
                new Position(position.row - 2, position.column + 2)
            };

            // possiblePosition.Length == capturedPiecePosition.Length
            for (int i = 0; i < possiblePositions.Length; i++)
            {
                // if possible position is on the board, capturedPiecePosition is on the board too. 
                if (possiblePositions[i].IsPositionOnTheBoard() && board.IsSquareEmpty(possiblePositions[i])) 
                {
                    if (!board.IsSquareEmpty(capturedPiecePositions[i]) && board.IsWhitePieceOnSquare(capturedPiecePositions[i]) != board.IsWhitePieceOnSquare(position))
                    {
                        return true;
                    }
                }
            }
            return false;

        }
        private static void ExploreCapturingForStone(CheckersBoard workingBoard, CheckersBoard board, Position position, Position startPosition, PossibleMoves currentMove)
        {

            Position[] capturedPiecePosition =
            {
                new Position(position.row + 1 ,position.column - 1),
                new Position(position.row + 1, position.column + 1),
                new Position(position.row - 1, position.column - 1),
                new Position(position.row - 1, position.column + 1)
            };
            Position[] possiblePositions =
            {
                new Position(position.row + 2, position.column - 2),
                new Position(position.row + 2, position.column + 2),
                new Position(position.row - 2, position.column - 2),
                new Position(position.row - 2, position.column + 2)
            };

            for(int i=0;i<possiblePositions.Length;i++)
            {
                // if possible position is on the board, capturedPiecePosition is on the board too. 
                if (possiblePositions[i].IsPositionOnTheBoard() && workingBoard.IsSquareEmpty(possiblePositions[i]))
                {
                    if (!workingBoard.IsSquareEmpty(capturedPiecePosition[i]) && workingBoard.IsWhitePieceOnSquare(capturedPiecePosition[i]) != workingBoard.IsWhitePieceOnSquare(position))
                    {
                        // simulation move on the wokring board
                        workingBoard.SetSquareEmpty(capturedPiecePosition[i]);
                        workingBoard.SetPieceOnPosition(possiblePositions[i], workingBoard.GetPieceFromPosition(position));
                        workingBoard.SetSquareEmpty(position);

                        // creation new possible move and connected it with currentMove. 
                        PossibleMoves nextMove = new PossibleMoves(possiblePositions[i], PossibleMoves.Type.capturing); 
                        currentMove.nextPosition.Add(nextMove);
                        //Console.WriteLine("Przed:");
                        //Console.WriteLine(workingBoard.ToString());
                        ExploreCapturingForStone(workingBoard, board, possiblePositions[i], startPosition, nextMove);

                        // recursion is back, so we're fixing the working board.
                        workingBoard.SetPieceOnPosition(capturedPiecePosition[i], board.GetPieceFromPosition(capturedPiecePosition[i]));
                        workingBoard.SetPieceOnPosition(position, board.GetPieceFromPosition(startPosition));
                        workingBoard.SetSquareEmpty(possiblePositions[i]);
                        //Console.WriteLine("Po:");
                        //Console.WriteLine(workingBoard.ToString());

                        currentMove.howManyPieceWillBeCaptured = Math.Max(nextMove.howManyPieceWillBeCaptured+1, currentMove.howManyPieceWillBeCaptured);
                    }
                }
            }
        }
        private static void ExploreNormalMoveForStone(CheckersBoard board, Position position, PossibleMoves currentMove)
        {
            Position[] possiblePositions = new Position[2];
            if(board.IsBlackPieceOnSquare(position))
            { 
                possiblePositions[0] = new Position(position.row - 1, position.column - 1);
                possiblePositions[1] = new Position(position.row - 1, position.column + 1);
            }
            else
            {
                possiblePositions[0] = new Position(position.row + 1, position.column - 1);
                possiblePositions[1] = new Position(position.row + 1, position.column + 1);
            }

            foreach (Position pos in possiblePositions)
            {
                if (pos.IsPositionOnTheBoard() && board.IsSquareEmpty(pos))
                {
                    PossibleMoves nextMove = new PossibleMoves(pos, PossibleMoves.Type.normalMove);
                    currentMove.nextPosition.Add(nextMove);
                }
            }

        }
        private static PossibleMoves FindPossibleMoveForDame(CheckersBoard board, Position startPosition)
        {
            PossibleMoves fisrtMove;
            if(DoesCapturedExistForDame(board, startPosition))
            {
                fisrtMove = new PossibleMoves(startPosition, PossibleMoves.Type.capturing);
                CheckersBoard workingBoard = board.GetCheckersBoard();
                
                ExploreCapturingForDame(workingBoard, board, startPosition, startPosition, fisrtMove);
            }
            else
            {
                fisrtMove = new PossibleMoves(startPosition, PossibleMoves.Type.normalMove);
                ExploreNormalMoveForDame(board, startPosition, fisrtMove);
            }

            return fisrtMove;
        }
        private static bool DoesCapturedExistForDame(CheckersBoard board, Position position)
        {
            Directions[] posssibleDirections =
            {
                Directions.northEast,
                Directions.northWest,
                Directions.southEast,
                Directions.southWest
            };


            foreach(Directions direction in posssibleDirections)
            {
                Position currentPosition = Move(direction, position);
               
                // simulation move for empty squares
                while (currentPosition.IsPositionOnTheBoard() && board.IsSquareEmpty(currentPosition))
                    currentPosition = Move(direction, currentPosition);

                // verification of the color of the first squares after empty squares
                if (Move(direction, currentPosition).IsPositionOnTheBoard() && board.IsBlackPieceOnSquare(currentPosition) != board.IsBlackPieceOnSquare(position) 
                    && board.IsSquareEmpty(Move(direction, currentPosition))) 
                    return true;
            }

            return false;


        }
        private static void ExploreCapturingForDame(CheckersBoard workingBoard, CheckersBoard board, Position position, Position startPositon, PossibleMoves currentMove)
        {
            Directions[] posssibleDirections =
            {
                Directions.northEast,
                Directions.northWest,
                Directions.southEast,
                Directions.southWest
            };

            foreach(Directions direction in posssibleDirections)
            {
                // move simulation
                Position currentPosition = Move(direction, position);
                
                // move simulation for empty squares 
                while (currentPosition.IsPositionOnTheBoard() && workingBoard.IsSquareEmpty(currentPosition))
                {
                    currentPosition = Move(direction, currentPosition);
                }
                
                Position capturedPiecePosition = new Position(currentPosition.row, currentPosition.column);
                currentPosition = Move(direction, currentPosition);
                
                // if capturing possible
                while (currentPosition.IsPositionOnTheBoard() && workingBoard.IsBlackPieceOnSquare(capturedPiecePosition) != workingBoard.IsBlackPieceOnSquare(position) && workingBoard.IsSquareEmpty(currentPosition))
                {
                    // move simulation for empty squares after capturing
                    while (currentPosition.IsPositionOnTheBoard() && workingBoard.IsSquareEmpty(currentPosition) )
                    {
                        // on capturedPiecPosition insert Piece in other color, we want to avoid standing on the square with a previously captured piece  
                        workingBoard.SetPieceOnPosition(capturedPiecePosition, workingBoard.GetPieceFromPosition(capturedPiecePosition).GetPieceWithOtherColor()); 
                        workingBoard.SetPieceOnPosition(currentPosition, workingBoard.GetPieceFromPosition(position));
                        workingBoard.SetSquareEmpty(position);
                       
                        PossibleMoves nextMove = new PossibleMoves(currentPosition, PossibleMoves.Type.capturing);
                        currentMove.nextPosition.Add(nextMove);

                        Console.WriteLine("Przed:");
                        Console.WriteLine(workingBoard.ToString());

                        ExploreCapturingForDame(workingBoard, board, currentPosition, startPositon, nextMove);

                        workingBoard.SetSquareEmpty(currentPosition);
                        workingBoard.SetPieceOnPosition(position, board.GetPieceFromPosition(startPositon));
                        workingBoard.SetPieceOnPosition(capturedPiecePosition, board.GetPieceFromPosition(capturedPiecePosition));

                        Console.WriteLine("Po:");
                        Console.WriteLine(workingBoard.ToString());
                
                        currentMove.howManyPieceWillBeCaptured = Math.Max(currentMove.howManyPieceWillBeCaptured, nextMove.howManyPieceWillBeCaptured + 1);
                        currentPosition = Move(direction, currentPosition);

                    }
                }
            }

        }
        private static void ExploreNormalMoveForDame(CheckersBoard board, Position position, PossibleMoves currentMove)
        {
            Directions[] posssibleDirections =
            {
                Directions.northEast,
                Directions.northWest,
                Directions.southEast,
                Directions.southWest
            };

            foreach (Directions direction in posssibleDirections)
            {
                Position nextPosition = Move(direction, position);
                while (nextPosition.IsPositionOnTheBoard() && board.IsSquareEmpty(nextPosition))
                {
                    PossibleMoves nextMove = new PossibleMoves(nextPosition, PossibleMoves.Type.normalMove);
                    currentMove.nextPosition.Add(nextMove);
                    nextPosition = Move(direction, nextPosition);
                }
            }   
          
        }
        
    }

}
