using System;

namespace globals {
    /// <summary>
    /// define any constants that might be needed
    /// 'globally' throughout the game.
    /// </summary>
    ///     


    public enum Dir {
        NORTH,
        SOUTH,
        EAST,
        WEST,
        UP,
        DOWN
    }

    public enum State { // to set object states - e.g. monitor.state = State.ON
        ACTIVATED,
        NORMAL,
        ON,
        OFF
    }

    public enum TFN { // three-state alternative to bool
        TRUE,
        FALSE,
        NOTFOUND
    }
}
