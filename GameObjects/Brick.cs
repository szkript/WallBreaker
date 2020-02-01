namespace Pong
{
    using System.Numerics;
    using System.Windows.Media;
    using System.Windows.Shapes;

    public class Brick
    {
        public double Width { get; set; } = 50;
        public double Height { get; set; } = 20;
        public Rectangle brick { get; set; }
        public Vector2 Position;

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

        }
    }
}
