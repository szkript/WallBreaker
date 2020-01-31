using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Pong
{
    public class Brick
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        private Rectangle brick;
        public Brick()
        {
            Rectangle rect = new Rectangle();
            rect.Name = "Brick";
            rect.Stroke = Brushes.Red;
            rect.Width = 50;
            rect.Height = 20;
            rect.Fill = Brushes.Black;
            rect.VerticalAlignment = VerticalAlignment.Center;
            brick = rect;
        }
    }
}
