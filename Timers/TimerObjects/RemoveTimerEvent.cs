using System;

namespace Timers.TimerObjects
{
    public class RemoveTimerEvent : EventArgs
    {
        public TimerViewModel TimerToRemove { get; set; }
    }

}
