using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace WallBreaker
{
    public static class GameTimeManager
    {
        private static DispatcherTimer _gameloopTimer;
        private static DispatcherTimer _slowMotionCooldownTimer;
        private static DispatcherTimer _slowMotionTimer;
        public static DispatcherTimer _timer;

        internal static void StartGame(Action<object, EventArgs> gameLoop)
        {
            _gameloopTimer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 0, 0, 8)
            };
            _gameloopTimer.Tick += new EventHandler(gameLoop);
            _gameloopTimer.Start();
        }
        internal static void StopGame()
        {
            _gameloopTimer.Stop();
        }

        internal static void GameTime(Action<object, EventArgs> dispatcherTimer_Tick)
        {
            _timer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 1)
            };
            _timer.Tick += new EventHandler(dispatcherTimer_Tick);
            _timer.Start();
        }

        internal static void SlowMotionCooldownStart(int slowMotionCooldownTime, Action<object, EventArgs> slowMotion_tick)
        {
            _slowMotionCooldownTimer = new DispatcherTimer();
            _slowMotionCooldownTimer.Interval = TimeSpan.FromSeconds(slowMotionCooldownTime);
            _slowMotionCooldownTimer.Tick += new EventHandler(slowMotion_tick);
            _slowMotionCooldownTimer.Start();
        }
        internal static void SlowMotionCoolDownStop()
        {
            _slowMotionCooldownTimer.Stop();
        }

        internal static void SlowMotionTimeStart(Action<object, EventArgs> slowMotionTimer_Tick, double slowTimeAmmount)
        {
            _slowMotionTimer = new DispatcherTimer();
            _slowMotionTimer.Interval = TimeSpan.FromSeconds(slowTimeAmmount);
            _slowMotionTimer.Tick += new EventHandler(slowMotionTimer_Tick);
            _slowMotionTimer.Start();
            Console.WriteLine($"{slowTimeAmmount} sec");
        }
        internal static void SlowMotionTimeStop() 
        {
            _slowMotionTimer.Stop();
        }

    }
}
