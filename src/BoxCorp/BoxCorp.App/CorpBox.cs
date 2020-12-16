using System;
using System.Drawing;
using QuadTrees.QTreeRect;

namespace BoxCorp.App
{
    public class CorpBox : IRectQuadStorable, IComparable<CorpBox>
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public double Rank { get; set; }

        private Rectangle? _rectangle;

        public Rectangle Rect
        {
            get
            {
                _rectangle ??= new Rectangle(X, Y, Width, Height);
                return (Rectangle) _rectangle;
            }
        }

        public int CompareTo(CorpBox other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            var xComparison = X.CompareTo(other.X);
            if (xComparison != 0) return xComparison;
            var yComparison = Y.CompareTo(other.Y);
            if (yComparison != 0) return yComparison;
            var widthComparison = Width.CompareTo(other.Width);
            if (widthComparison != 0) return widthComparison;
            var heightComparison = Height.CompareTo(other.Height);
            if (heightComparison != 0) return heightComparison;
            return Rank.CompareTo(other.Rank);
        }
    }
}