using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace Pong
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer _timer;
        private GameBall ball;
        private DispatcherTimer _gameloopTimer;
        private bool paused { set; get; } = false;
        private Paddle paddle;
        private int score;

        private int startingBallSpeed = 1;
        private int rowOfBricks = 2;
        public ObservableCollection<Brick> bricks { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            bricks = new ObservableCollection<Brick>();
        }
        private void InitBricks(int NumOfRows)
        {
            double posTop = 20;
            for (int i = 0; i < NumOfRows; i++)
            {
                double posLeft = 10;
                while (PongCanvas.Width > posLeft + 50)
                {
                    Brick recti = new Brick();
                    recti.X = posLeft;
                    recti.Y = posTop;
                    Canvas.SetTop(recti.brick, posTop);
                    Canvas.SetLeft(recti.brick, posLeft);
                    PongCanvas.Children.Add(recti.brick);
                    bricks.Add(recti);
                    posLeft += recti.Width + 5;
                }
                posTop += 30;
            }
        }


        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                exitGame();
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
                    togglePause(GameState.SimplePause);
                    break;
            }
        }

        private void exitGame()
        {
            togglePause(GameState.Exit);
        }

        private void togglePause()
        {
            if (paused == false)
            {
                paused = true;
                MessageBox.Show("Your Score :" + score);
                paused = false;
            }
            else if (paused == true)
            {
                paused = false;
            }
        }

        private void togglePause(GameState pauseState)
        {
            switch (pauseState)
            {
                case GameState.Exit:
                    paused = true;
                    ExitPopupWindow exitWindow = new ExitPopupWindow();
                    exitWindow.ShowDialog();
                    paused = false;
                    if (exitWindow.exitConfirmed())
                    {
                        Close();
                    }
                    if (exitWindow.restartGame())
                    {
                        stopGame();
                        startGame();
                    }
                    break;
                case GameState.SimplePause:
                    togglePause();
                    break;
                case GameState.GameOver:
                    paused = true;
                    Game_overWindow gameOver = new Game_overWindow(GameState.GameOver, score);
                    gameOver.ShowDialog();
                    paused = false;
                    if (gameOver.exitConfirmed())
                    {
                        stopGame();
                        Close();
                    }
                    if (gameOver.restartGame())
                    {
                        stopGame();
                        startGame();
                    }
                    break;
                case GameState.Win:
                    paused = true;
                    Game_overWindow game_Over = new Game_overWindow(GameState.Win, score);
                    game_Over.ShowDialog();
                    paused = false;
                    if (game_Over.exitConfirmed())
                    {
                        stopGame();
                        Close();
                    }
                    if (game_Over.restartGame())
                    {
                        stopGame();
                        startGame();
                    }
                    break;
            }
        }

        private void stopGame()
        {
            _timer.Stop();
            _gameloopTimer.Stop();
        }

        private void PongCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            startGame();
        }

        private void startGame()
        {
            score = 0;
            InitBricks(rowOfBricks);
            ball = new GameBall(Ball, PongCanvas.ActualWidth, PongCanvas.ActualHeight, startingBallSpeed);
            paddle = new Paddle(Paddle, PongCanvas.ActualWidth);
            _timer = new DispatcherTimer();
            _timer.Interval = new TimeSpan(0, 0, 1);
            _timer.Tick += new EventHandler(dispatcherTimer_Tick);
            _timer.Start();
            _gameloopTimer = new DispatcherTimer();

            _gameloopTimer.Interval = new TimeSpan(0, 0, 0, 0, 8);
            _gameloopTimer.Tick += new EventHandler(gameLoop);
            _gameloopTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (!paused)
            {
                Mouse.OverrideCursor = Cursors.None;
            }
            else
            {
                Mouse.OverrideCursor = Cursors.AppStarting;
            }
        }

        private void gameWon()
        {
            togglePause(GameState.Win);
        }

        private void gameLoop(object sender, EventArgs e)
        {
            if (paused) { return; }

            checkCollusion();
            ball.move();
            paddle.movePaddle();
            updateLiveScore();
        }

        private void updateLiveScore()
        {
            Score.Content = "score: " + score;
        }

        private void checkCollusion()
        {
            double paddlePosHorizontal = (double)Paddle.GetValue(Canvas.LeftProperty);
            double paddlePosVertical = (double)Paddle.GetValue(Canvas.TopProperty);
            double goloPosHorizontal = (double)Ball.GetValue(Canvas.LeftProperty);
            double goloPosVertical = (double)Ball.GetValue(Canvas.TopProperty);

            if (paddlePosVertical - Paddle.ActualHeight <= goloPosVertical)
            {
                if (paddlePosHorizontal <= goloPosHorizontal + Ball.ActualWidth && paddlePosHorizontal + Paddle.ActualWidth >= goloPosHorizontal)
                {
                    ball.inverse(Paddle);
                    score += 100;
                }
            }
            if (paddlePosVertical <= goloPosVertical)
            {
                togglePause(GameState.GameOver);
            }
            // to show that you'll get an enumerable of rectangles.
            IEnumerable<Rectangle> rectangles = PongCanvas.Children.OfType<Rectangle>();

            int indexOfItemToRemove = -1;
            foreach (var rect in rectangles.Select((value, i) => new { i, value }))
            {
                Console.WriteLine(rect.value);
                // do something with the rectangle
                if (rect.value.Name == "Brick")
                {
                    if (ball.ContactsWith(rect.value))
                    {
                        
                        indexOfItemToRemove = rect.i;
                    }
                }
            }
            //PongCanvas.Children.Clear();
            //PongCanvas.Children.Add(bricks[1].brick);

            if (indexOfItemToRemove >= 0)
            {
                PongCanvas.Children.RemoveAt(indexOfItemToRemove);
            }
            //Trace.WriteLine("Found " + rectangles.Count() + " rectangles, height: " + maxHorizon);
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
