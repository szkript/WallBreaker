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
        public static DispatcherTimer _timer;

        static GameTimeManager()
        {

        }

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
    }
}
