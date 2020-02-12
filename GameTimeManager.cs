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
        private static DispatcherTimer gameloopTimer;
        private static DispatcherTimer slowMotionCooldownTimer;
        private static DispatcherTimer slowMotionTimer;
        public static DispatcherTimer timer;
        private static DispatcherTimer gameStartDelay;

        internal static void StartGame(Action<object, EventArgs> gameLoop)
        {
            gameloopTimer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 0, 0, 8)
            };
            gameloopTimer.Tick += new EventHandler(gameLoop);
            gameloopTimer.Start();
        }
        internal static void StopGame()
        {
            gameloopTimer.Stop();
        }

        internal static void GameTime(Action<object, EventArgs> dispatcherTimer_Tick)
        {
            timer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 1)
            };
            timer.Tick += new EventHandler(dispatcherTimer_Tick);
            timer.Start();
        }

        internal static void SlowMotionCooldownStart(int slowMotionCooldownTime, Action<object, EventArgs> slowMotion_tick)
        {
            slowMotionCooldownTimer = new DispatcherTimer();
            slowMotionCooldownTimer.Interval = TimeSpan.FromSeconds(slowMotionCooldownTime);
            slowMotionCooldownTimer.Tick += new EventHandler(slowMotion_tick);
            slowMotionCooldownTimer.Start();
        }
        internal static void SlowMotionCoolDownStop()
        {
            slowMotionCooldownTimer.Stop();
        }

        internal static void SlowMotionTimeStart(Action<object, EventArgs> slowMotionTimer_Tick, double slowTimeAmmount)
        {
            slowMotionTimer = new DispatcherTimer();
            slowMotionTimer.Interval = TimeSpan.FromSeconds(slowTimeAmmount);
            slowMotionTimer.Tick += new EventHandler(slowMotionTimer_Tick);
            slowMotionTimer.Start();
            Console.WriteLine($"{slowTimeAmmount} sec");
        }
        internal static void SlowMotionTimeStop() 
        {
            slowMotionTimer.Stop();
        }

    }
}
