namespace Pong
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Shapes;

    public class Brick
    {
        public double X { get; set; }

        public double Y { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        public Rectangle brick { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="Brick"/> class.
        /// </summary>
        public Brick()
        {
            Rectangle rect = new Rectangle();
            rect.Name = "Brick";
            rect.Stroke = Brushes.Red;
            rect.Width = 50;
            rect.Height = 20;
            rect.Fill = Brushes.Black;
            brick = rect;
            Height = rect.Height;
            Width = rect.Width;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Brick"/> class.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="brick"></param>
        public Brick(double x, double y, double width, double height, Rectangle brick)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            this.brick = brick;
        }
    }
}
