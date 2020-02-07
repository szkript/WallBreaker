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
        public int NitroSpeed = 10;


        public bool NitroIsOn { get; internal set; }

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
        public Vector2 PeekingMove()
        {
            Vector2 fake = new Vector2(velocity.X, velocity.Y);
            if (Position.X <= 0)
            {
                fake.X = -velocity.X;
            }
            if (Position.X >= (canvasWidth - (ball.Width + 5)))
            {
                fake.X = -velocity.X;
            }
            if (Position.Y <= 0)
            {
                fake.Y = -velocity.Y;
            }
            if (Position.Y >= (canvasHeight - ball.Height))
            {
                fake.Y = -velocity.Y;
            }
            return Position + (direction * fake);
            //return fake;
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
        public void SpeedUp(int speed)
        {
            if (NitroIsOn) return;
            if (velocity.X > 0)
            {
                velocity.X += speed;
            }
            else if (velocity.X < 0)
            {
                velocity.X -= speed;
            }
            if (velocity.Y > 0)
            {
                velocity.Y += speed;
            }
            else if (velocity.Y < 0)
            {
                velocity.Y -= speed;
            }
            NitroIsOn = true;
        }
        public void SpeedDown(int speed)
        {
            // input is minus value
            if (velocity.X > 0)
            {
                velocity.X += speed;
            }
            else if (velocity.X < 0)
            {
                velocity.X -= speed;
            }
            if (velocity.Y > 0)
            {
                velocity.Y += speed;
            }
            else if (velocity.Y < 0)
            {
                velocity.Y -= speed;
            }
            NitroIsOn = false;
        }

        public bool ContactsWith(Brick brick)
        {
            // TODO: collusion must be more accurate
            // TODO: dynammically check all side and make inversion based on side
            if (BallInRange(brick))
            {
                foreach (Side side in Enum.GetValues(typeof(Side)))
                {
                    this.sides.Clear();
                    this.CalculateSides();
                    foreach (var ballSide in this.sides)
                    {
                        Side opposite = GetOppositeSide(ballSide.Key);
                        if (ballSide.Value.Any(val => brick.sides[opposite].Contains(val)))
                        {
                            Console.WriteLine($"ball side -> {ballSide.Key}, brick side -> {opposite}");
                            if (side == Side.Bottom || side == Side.Top)
                            {
                                InverseDirection(Axis.Y);
                                return true;
                            }
                            if (side == Side.Left || side == Side.Right)
                            {
                                InverseDirection(Axis.X);
                                return true;
                            }
                        }
                    }
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
            this.sides.Clear();
            this.CalculateSides();
            // get next position without modifying actual move
            Vector2 nextMove = PeekingMove();

            //Console.WriteLine("-----------------------------------------");
            //Console.WriteLine($"X: {(int)Position.X}, Y: {(int)Position.Y}");
            //Console.WriteLine($"nextX: {(int)nextMove.X}, nextY: {(int)nextMove.Y}");

            if (brick.Position.Y <= nextMove.Y && nextMove.Y <= brick.Position.Y + Height)
            {
                if (brick.Position.X <= nextMove.X && nextMove.X <= brick.Position.X + brick.Width)
                {
                    return true;
                }
            }
            return false;
        }
    }

}
