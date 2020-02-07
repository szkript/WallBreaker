namespace WallBreaker
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Windows.Media;
    using System.Windows.Shapes;
    using WallBreaker.GameObjects;

    public class Brick : GameObject
    {
        public Rectangle brick { get; set; }
        private const int Brick_Width = 50;
        private const int Brick_Height = 20;
        public readonly List<int[]> allocatedPosition = new List<int[]>();
        public Brick(Vector2 position)
        {
            Rectangle rect = new Rectangle
            {
                Name = "Brick",
                Stroke = Brushes.Red,
                Fill = Brushes.Black,
                Width = Brick_Width,
                Height = Brick_Height
            };
            Width = Brick_Width;
            Height = Brick_Height;
            brick = rect;
            Position = position;
            CalculateSides();
            AllocatePosition();

        }
        private void AllocatePosition()
        {
            for (int i = (int)Position.Y; i <= Position.Y + Height; i++)
            {
                allocatedPosition.Add(Enumerable.Range((int)Position.X + i, (int)Width).ToArray());
            }
        }
    }
}
