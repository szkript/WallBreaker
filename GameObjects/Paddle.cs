using System.Windows.Controls;
using System.Windows.Shapes;

namespace Pong
{
    class Paddle
    {
        private Rectangle paddle;
        private int speed;
        private double canvasWidth;
        public bool MoveLeft { get; set; } = false;
        public bool MoveRight { get; set; } = false;

        public Paddle(Rectangle paddle, double canvasWidth)
        {
            this.paddle = paddle;
            speed = 8;
            this.canvasWidth = canvasWidth;
            paddle.SetValue(Canvas.LeftProperty, (canvasWidth / 2) - (paddle.ActualWidth / 2));

        }
        public void movePaddle()
        {
            if (MoveLeft)
            {
                if ((double)paddle.GetValue(Canvas.LeftProperty) <= 0)
                {
                    paddle.SetValue(Canvas.LeftProperty, 0.0);
                }
                else
                {
                    paddle.SetValue(Canvas.LeftProperty, (double)paddle.GetValue(Canvas.LeftProperty) - speed);
                }
            }
            if (MoveRight)
            {
                if ((double)paddle.GetValue(Canvas.LeftProperty) + paddle.ActualWidth >= canvasWidth)
                {
                    paddle.SetValue(Canvas.LeftProperty, canvasWidth-paddle.ActualWidth);
                }
                else
                {
                    paddle.SetValue(Canvas.LeftProperty, (double)paddle.GetValue(Canvas.LeftProperty) + speed);
                }
            }
            
        }

    }
}
