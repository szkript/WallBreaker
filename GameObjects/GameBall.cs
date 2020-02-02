﻿using System;
using System.Collections.Generic;
using System.Linq;
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

        public GameBall(Rectangle ball, double canvasWidth, double canvasHeight, int ballStartingSpeed)
        {
            this.ball = ball;
            this.canvasWidth = canvasWidth;
            this.canvasHeight = canvasHeight;
            position = new Vector2((float)canvasWidth / 2, (float)canvasHeight / 2 - 100);
            ball.SetValue(Canvas.LeftProperty, (double)position.X);
            ball.SetValue(Canvas.TopProperty, (double)position.Y);
            Random random = new Random();
            int randomDirX = -100 + random.Next(0, 201);
            int randomDirY = 0 + random.Next(0, 201);
            direction = new Vector2(randomDirX, randomDirY);
            direction = Vector2.Normalize(direction);
            speedMultiplier(ballStartingSpeed);
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
        private void speedMultiplier(int speedUpXTimes)
        {
            for (int i = 0; i < speedUpXTimes; i++)
            {
                speedUp();
            }
        }

        public bool ContactsWith(Brick rectangle)
        {
            int ballX1 = Convert.ToInt32(ball.GetValue(Canvas.LeftProperty));
            if (BallInRange(rectangle))
            {
                List<int> ballTop = Enumerable.Range((int)ballX1, (int)ball.Width).ToList();

                if (ballTop.Any(x => rectangle.sides[Side.Bottom].Contains(x)))
                {
                    Console.WriteLine("brickBotY1-> " + rectangle.sides[Side.Left].First());
                    Console.WriteLine("brickBotY2-> " + rectangle.sides[Side.Left].Last());
                    Console.WriteLine($"brickY2 pos-> {rectangle.Position.Y + rectangle.brick.Height}");
                    Console.WriteLine("---------------------------------------");
                    direction.Y = Math.Abs(direction.Y);
                    return true;
                }
            }
            return false;
        }
        private bool BallInRange(Brick brick)
        {
            int yRange = 3;
            int ballY1 = Convert.ToInt32(ball.GetValue(Canvas.TopProperty));
            Console.WriteLine("ball " + ballY1);
            Console.WriteLine("ball " + (int)position.Y);
            Console.WriteLine("brick " + brick.Position.Y);
            Console.WriteLine("brickBySide " + brick.sides[Side.Left].Last());
            if ((int)position.Y <= brick.sides[Side.Left].Last() + yRange
                && (int)position.Y >= brick.sides[Side.Left].First() - yRange)
            {
                return true;
            }
            return false;
        }
    }
}
