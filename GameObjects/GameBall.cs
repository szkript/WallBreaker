using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Windows.Controls;
using System.Windows.Shapes;
using WallBreaker.GameObjects;

namespace WallBreaker
{
    class GameBall : GameObject
    {
        private Rectangle ball;
        private Vector2 velocity;
        private Vector2 direction;
        private double canvasWidth;
        private double canvasHeight;

        public GameBall(Rectangle ball, double canvasWidth, double canvasHeight, int ballStartingSpeed, int startingHeightPosition)
        {
            velocity = new Vector2(ballStartingSpeed, ballStartingSpeed);
            this.ball = ball;
            this.canvasWidth = canvasWidth;
            this.canvasHeight = canvasHeight;
            Position = new Vector2((float)canvasWidth / 2, startingHeightPosition);
            ball.SetValue(Canvas.LeftProperty, (double)Position.X);
            ball.SetValue(Canvas.TopProperty, (double)Position.Y);
            Random random = new Random();
            int randomDirX = -100 + random.Next(0, 201);
            int randomDirY = 0 + random.Next(0, 201);
            direction = new Vector2(randomDirX, randomDirY);
            direction = Vector2.Normalize(direction);
            Width = ball.ActualWidth;
            Height = ball.ActualHeight;
            //SpeedMultiplier(ballStartingSpeed);
        }

        public void Move()
        {
            if (Position.X <= 0)
            {
                velocity.X = -velocity.X;
            }
            if (Position.X >= (canvasWidth - (ball.Width + 5)))
            {
                velocity.X = -velocity.X;
            }
            if (Position.Y <= 0)
            {
                velocity.Y = -velocity.Y;
            }
            if (Position.Y >= (canvasHeight - ball.Height))
            {
                velocity.Y = -velocity.Y;
            }
            Position += direction * velocity;
            ball.SetValue(Canvas.LeftProperty, (double)Position.X);
            ball.SetValue(Canvas.TopProperty, (double)Position.Y);
        }
        internal void Inverse(Rectangle paddle)
        {
            velocity.Y = Math.Abs(velocity.Y);
            velocity.X = Math.Abs(velocity.X);
            double paddleMiddlePos = (double)paddle.GetValue(Canvas.LeftProperty) + (paddle.ActualWidth / 2);
            double ballMiddlePos = (double)ball.GetValue(Canvas.LeftProperty) + (ball.ActualWidth / 2);
            double positionDifference = ballMiddlePos - paddleMiddlePos;
            direction = new Vector2((1 * (float)positionDifference), ((float)-Math.Abs(1000 / positionDifference)));
            direction = Vector2.Normalize(direction);
            Position += direction * velocity;
            ball.SetValue(Canvas.TopProperty, (double)Position.Y);
        }
        internal void SpeedUp()
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
        private void SpeedMultiplier(int speedUpXTimes)
        {
            for (int i = 0; i < speedUpXTimes; i++)
            {
                SpeedUp();
            }
        }

        public bool ContactsWith(Brick brick)
        {
            // TODO: collusion must be more accurate
            // TODO: dynammically check all side and make inversion based on side
            //Dictionary<Side, List<int>> ballSides;
            if (BallInRange(brick))
            {
                List<int> ballTop = Enumerable.Range((int)Position.X, (int)ball.Width).ToList();

                if (ballTop.Any(ballPosition => brick.sides[Side.Bottom].Contains(ballPosition)))
                {
                    InverseDirection(Axis.Y);
                    return true;
                }
            }
            return false;
        }

        private void InverseDirection(Axis axis)
        {
            switch (axis)
            {
                case Axis.X:
                    direction.X = direction.X > direction.X + 1 ? direction.X : -direction.X;
                    break;
                case Axis.Y:
                    direction.Y = direction.Y > direction.Y + 1 ? direction.Y : -direction.Y;
                    break;
                default:
                    break;
            }
        }

        private bool BallInRange(Brick brick)
        {
            Console.WriteLine($"{brick.Position.Y}, ball y: {Position.Y}");
            if ((int)Position.Y <= brick.sides[Side.Left].Last()
                && (int)Position.Y >= brick.sides[Side.Left].First())
            {
                return true;
            }
            return false;
        }
    }

}
