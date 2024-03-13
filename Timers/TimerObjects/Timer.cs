using System.Collections.Generic;
using System.IO;
using System.Media;
using Timers.ToDoList;

namespace Timers.TimerObjects
{
    /// <summary>
    /// This class represents the timer class and holds all the neccessary properties
    /// relating to the successful creation and termination of a timer
    /// </summary>
    public class Timer
    {
        private bool _cancelled = false;
        private int _repeatedValue = 0;
        private SoundPlayer _player;
        /// <summary>
        /// Specifies how many time a timer has already been repeated
        /// </summary>
        public int RepeatedValue { get => _repeatedValue; set => _repeatedValue = value; }
        /// <summary>
        /// Defines wether a timer is to repeat indefinetly
        /// </summary>
        public bool IsRepeated { get; set; }
        /// <summary>
        /// The current value of the timer
        /// </summary>
        public long CurrentValue { get; set; }
        /// <summary>
        /// Wether the timer has been created
        /// </summary>
        public bool Created { get; set; }
        /// <summary>
        /// Wether the timer has finished running
        /// </summary>
        public bool Finished { get; set; }
        /// <summary>
        /// Wether the timer is paused
        /// </summary>
        public bool IsPaused { get; set; }
        /// <summary>
        /// The original value of the timer.
        /// Used for repeating the timer
        /// </summary>
        public long OriginalValue { get; set; }
        /// <summary>
        /// The list of todos that this timer holds
        /// </summary>
        public List<ToDo> ToDos { get; set; } = new List<ToDo>();
        /// <summary>
        /// Determines wether the timer was canceled
        /// </summary>
        public bool Cancelled
        {
            get => _cancelled;
            set
            {
                _cancelled = value;
                if (_cancelled)
                {
                    _player.Stop();
                }
            }
        }
        public Timer()
        {
            // The sound to play
            _player = new SoundPlayer(@$"{Directory.GetCurrentDirectory()}\soft.wav");
        }
        /// <summary>
        /// When a timer finishes this function is called to play a sound
        /// </summary>
        public void FinishTimer()
        {
            _player.Play();
        }
    }
}
