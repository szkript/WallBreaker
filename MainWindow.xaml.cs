using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using System.Collections.ObjectModel;
using System.Numerics;

namespace WallBreaker
{
    // TODO: Make it work in MvvM pattern
    public partial class MainWindow : Window
    {
        private Paddle paddle;
        private GameBall ball;
        private DispatcherTimer _timer;
        private DispatcherTimer _gameloopTimer;
        private bool Paused { set; get; } = false;
        private int score;

        private readonly int startingBallSpeed = 3;
        private readonly int rowOfBricks = 5;
        public ObservableCollection<Brick> Bricks { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Bricks = new ObservableCollection<Brick>();
        }
        private void InitBricks(int NumOfRows)
        {
            Bricks.Clear();
            double posTop = 20;
            for (int i = 0; i < NumOfRows; i++)
            {
                double posLeft = 10;
                while (PongCanvas.Width > posLeft + 50)
                {
                    Vector2 position = new Vector2((int)posLeft, (int)posTop);
                    Brick brick = new Brick(position);
                    Canvas.SetLeft(brick.brick, position.X);
                    Canvas.SetTop(brick.brick, position.Y);
                    Bricks.Add(brick);
                    PongCanvas.Children.Add(brick.brick);
                    posLeft += brick.brick.Width + 5;
                }
                posTop += 30;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                ExitGame();
            }
            switch (e.Key)
            {
                case Key.Left:
                    paddle.MoveLeft = true;
                    break;
                case Key.Right:
                    paddle.MoveRight = true;
                    break;
                case Key.Space:
                    TogglePause(GameState.SimplePause);
                    break;
            }
        }

        private void ExitGame()
        {
            TogglePause(GameState.Exit);
        }

        private void TogglePause()
        {
            if (Paused == false)
            {
                Paused = true;
                MessageBox.Show("Your Score :" + score);
                Paused = false;
            }
            else if (Paused == true)
            {
                Paused = false;
            }
        }

        private void TogglePause(GameState pauseState)
        {
            switch (pauseState)
            {
                case GameState.Exit:
                    Paused = true;
                    ExitPopupWindow exitWindow = new ExitPopupWindow();
                    exitWindow.ShowDialog();
                    Paused = false;
                    if (exitWindow.exitConfirmed())
                    {
                        Close();
                    }
                    if (exitWindow.restartGame())
                    {
                        StopGame();
                        StartGame();
                    }
                    break;
                case GameState.SimplePause:
                    TogglePause();
                    break;
                case GameState.GameOver:
                    Paused = true;
                    Game_overWindow gameOver = new Game_overWindow(GameState.GameOver, score);
                    gameOver.ShowDialog();
                    Paused = false;
                    if (gameOver.exitConfirmed())
                    {
                        StopGame();
                        Close();
                    }
                    if (gameOver.restartGame())
                    {
                        StopGame();
                        StartGame();
                    }
                    break;
                case GameState.Win:
                    Paused = true;
                    Game_overWindow game_Over = new Game_overWindow(GameState.Win, score);
                    game_Over.ShowDialog();
                    Paused = false;
                    if (game_Over.exitConfirmed())
                    {
                        StopGame();
                        Close();
                    }
                    if (game_Over.restartGame())
                    {
                        StopGame();
                        StartGame();
                    }
                    break;
            }
        }

        private void StopGame()
        {
            _timer.Stop();
            _gameloopTimer.Stop();
        }

        private void PongCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            StartGame();
        }

        private void StartGame()
        {
            score = 0;
            InitBricks(rowOfBricks);
            int ballStartingVerticalPosition = rowOfBricks * 40;
            ball = new GameBall(Ball, PongCanvas.ActualWidth, PongCanvas.ActualHeight, startingBallSpeed, ballStartingVerticalPosition);            
            paddle = new Paddle(Paddle, PongCanvas.ActualWidth);

            _timer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 1)
            };
            _timer.Tick += new EventHandler(DispatcherTimer_Tick);
            _timer.Start();

            _gameloopTimer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 0, 0, 8)
            };
            _gameloopTimer.Tick += new EventHandler(GameLoop);
            _gameloopTimer.Start();
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (!Paused)
            {
                Mouse.OverrideCursor = Cursors.None;
            }
            else
            {
                Mouse.OverrideCursor = Cursors.AppStarting;
            }
        }

        private void GameWon()
        {
            TogglePause(GameState.Win);
        }

        private void GameLoop(object sender, EventArgs e)
        {
            if (Paused) { return; }

            CheckCollusion();
            ball.Move();
            paddle.MovePaddle();
            UpdateLiveScore();
        }

        private void UpdateLiveScore()
        {
            Score.Content = "score: " + score;
        }

        private void CheckCollusion()
        {
            double paddlePosHorizontal = (double)Paddle.GetValue(Canvas.LeftProperty);
            double paddlePosVertical = (double)Paddle.GetValue(Canvas.TopProperty);
            double goloPosHorizontal = (double)Ball.GetValue(Canvas.LeftProperty);
            double goloPosVertical = (double)Ball.GetValue(Canvas.TopProperty);

            if (paddlePosVertical - Paddle.ActualHeight <= goloPosVertical)
            {
                if (paddlePosHorizontal <= goloPosHorizontal + Ball.ActualWidth && paddlePosHorizontal + Paddle.ActualWidth >= goloPosHorizontal)
                {
                    ball.Inverse(Paddle);
                }
            }
            if (paddlePosVertical <= goloPosVertical)
            {
                TogglePause(GameState.GameOver);
            }

            Brick removeAble = null;
            foreach (Brick brick in Bricks)
            {
                if (ball.ContactsWith(brick))
                {
                    score += 100;
                    removeAble = brick;
                }
            }

            if (Bricks.Count == 0) { GameWon(); }

            if (removeAble != null)
            {
                Bricks.Remove(removeAble);
                PongCanvas.Children.Clear();
                foreach (Brick brick in Bricks)
                {
                    PongCanvas.Children.Add(brick.brick);
                }
                PongCanvas.Children.Add(Score);
                PongCanvas.Children.Add(Paddle);
                PongCanvas.Children.Add(Ball);
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    paddle.MoveLeft = false;
                    break;
                case Key.Right:
                    paddle.MoveRight = false;
                    break;
            }
        }

    }
}
