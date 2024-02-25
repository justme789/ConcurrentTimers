using System;

namespace Timers
{
    public class RemoveTimerEvent : EventArgs
    {
        public TimerViewModel TimerToRemove { get; set; }
    }

}
