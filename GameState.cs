using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pong
{
    public enum GameState
    {
        [Description("SimplePause")]
        SimplePause,
        [Description("GameOver")]
        GameOver,
        [Description("Exit")]
        Exit,
        [Description("Win")]
        Win
    }
}


