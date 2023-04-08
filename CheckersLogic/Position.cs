using System;
using System.Collections.Generic;
using System.Text;

namespace CheckersLogic
{
    public class Position
    {
        public int row { get; set; }
        public int column { get; set; }
        public Position(string position)
        {
            if (position.Length != 2)
                throw new Exception("Wrong Position");

            char row = position[1];
            char column = position[0];

            this.column = column - 'a';
            this.row = row - '1';
            
        }
        public Position(int row, int column)
        {
            this.row = row;
            this.column = column;
         
        }
        public Position(Position position)
        {
            this.row = position.row;
            this.column = position.column;
        }
        public bool IsPositionOnTheBoard()
        {
            return (row <= 7 && row >= 0 && column <= 7 && column >= 0);
        }
        public override string ToString()
        {
            char column = Convert.ToChar(this.column + 'a');
            char row = Convert.ToChar(this.row + 49);
            string a = Convert.ToString(column) + Convert.ToString(row);

            return a;
        }
        public override bool Equals(object obj)
        {
            Position other = (Position)obj;

            return column == other.column && row == other.row;
        }
        public override int GetHashCode()
        {
            return column * 8 + row;
        }
    }
}
