using System;
using System.Collections.Generic;
using System.Text;

namespace CheckersLogic
{
    public class CheckersPiece
    {
        public enum Type
        {
            Dame,
            Stone
        }
        public enum Color
        {
            White,
            Black
        }
        public Type type { get; set; }
        public Color color { get; set; }
        public CheckersPiece(Type type, Color color)
        {
            this.type = type;
            this.color = color;
        }
        public CheckersPiece(CheckersPiece piece)
        {
            this.color = piece.color;
            this.type = piece.type;
        }
        public CheckersPiece GetCheckersPiece()
        {
            return new CheckersPiece(this.type, this.color);
        }
        public override bool Equals(object obj)
        {
            CheckersPiece other = (CheckersPiece)obj;

            return type == other.type && color == other.color;
        }
        public CheckersPiece GetPieceWithOtherColor()
        {
            if(this.color == CheckersPiece.Color.Black)
                return new CheckersPiece(this.type, CheckersPiece.Color.White);
            else
                return new CheckersPiece(this.type, CheckersPiece.Color.Black);
        }
        public void ChangeColor()
        {
            if (this.color == Color.Black)
                this.color = Color.White;
            else
                this.color = Color.Black;
        }
        public void Crownig()
        {
            this.type = Type.Dame;
        }
    }
}
