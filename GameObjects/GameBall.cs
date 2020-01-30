using System;
using System.Numerics;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Pong
{
    class GameBall
    {
        private Rectangle ball;
        private Vector2 position;
        private Vector2 velocity = new Vector2(5, 5);
        private Vector2 direction;
        private double canvasWidth;
        private double canvasHeight;

        public GameBall(Rectangle ball, double canvasWidth, double canvasHeight)
        {
            this.ball = ball;
            this.canvasWidth = canvasWidth;
            this.canvasHeight = canvasHeight;
            position = new Vector2((float)canvasWidth / 2, (float)canvasHeight / 2 - 100);
            ball.SetValue(Canvas.LeftProperty, (double)position.X);
            ball.SetValue(Canvas.TopProperty, (double)position.Y);
            Random random = new Random();
            int randomDirX = -100 + random.Next(0, 201);
            int randomDirY = -100 + random.Next(0, 201);
            direction = new Vector2(randomDirX, randomDirY);
            direction = Vector2.Normalize(direction);
        }

        public void move()
        {
            if (position.X <= 0)
            {
                velocity.X = -velocity.X;
            }
            if (position.X >= (canvasWidth - (ball.Width + 5)))
            {
                velocity.X = -velocity.X;
            }
            if (position.Y <= 0)
            {
                velocity.Y = -velocity.Y;
            }
            if (position.Y >= (canvasHeight - ball.Height))
            {
                velocity.Y = -velocity.Y;
            }
            position += direction * velocity;
            ball.SetValue(Canvas.LeftProperty, (double)position.X);
            ball.SetValue(Canvas.TopProperty, (double)position.Y);
        }

        internal void inverse(Rectangle paddle)
        {
            velocity.Y = Math.Abs(velocity.Y);
            velocity.X = Math.Abs(velocity.X);
            double paddleMiddlePos = (double)paddle.GetValue(Canvas.LeftProperty) + (paddle.ActualWidth / 2);
            double ballMiddlePos = (double)ball.GetValue(Canvas.LeftProperty) + (ball.ActualWidth / 2);
            double positionDifference = ballMiddlePos - paddleMiddlePos;
            direction = new Vector2((1 * (float)positionDifference), ((float)-Math.Abs(1000 / positionDifference)));
            direction = Vector2.Normalize(direction);
            position += direction * velocity;
            ball.SetValue(Canvas.TopProperty, (double)position.Y);
        }

        internal void speedUp()
        {
            if (velocity.X > 0)
            {
                velocity.X += 1;
            }
            else if (velocity.X < 0)
            {
                velocity.X -= 1;
            }
            if (velocity.Y > 0)
            {
                velocity.Y += 1;
            }
            else if (velocity.Y < 0)
            {
                velocity.Y -= 1;
            }
        }

    }
}
