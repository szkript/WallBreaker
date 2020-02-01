namespace Pong
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Windows.Media;
    using System.Windows.Shapes;

    public class Brick
    {
        public double Width { get; set; } = 50;
        public double Height { get; set; } = 20;
        public Rectangle brick { get; set; }
        public Vector2 Position;
        public Dictionary<Side, List<int>> sides = new Dictionary<Side, List<int>>();

        public Brick(Vector2 position)
        {
            Rectangle rect = new Rectangle();
            rect.Name = "Brick";
            rect.Stroke = Brushes.Red;
            rect.Fill = Brushes.Black;
            rect.Width = Width;
            rect.Height = Height;
            brick = rect;
            Position = position;
            CalculateSides();
        }

        private void CalculateSides()
        {
            List<int> topSide = Enumerable.Range((int)Position.Y, (int)Width).ToList();
            List<int> rightSide = Enumerable.Range((int)Position.Y + (int)Width, (int)Height).ToList();
            List<int> bottomSide = Enumerable.Range((int)Position.Y + (int)Height, (int)Width).ToList();
            List<int> leftSide = Enumerable.Range((int)Position.Y, (int)Height).ToList();

            sides.Add(Side.Top, topSide);
            sides.Add(Side.Right, rightSide);
            sides.Add(Side.Bottom, bottomSide);
            sides.Add(Side.Left, leftSide);
        }
    }
    public enum Side
    {
        Top,
        Right,
        Bottom,
        Left
    }
}
