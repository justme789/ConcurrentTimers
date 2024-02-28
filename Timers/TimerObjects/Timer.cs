using System.IO;
using System.Media;

namespace Timers.TimerObjects
{
    public class Timer
    {
        private bool _cancelled = false;
        private int _repeatedValue = 0;
        private SoundPlayer _player;
        public int RepeatedValue { get => _repeatedValue; set => _repeatedValue = value; }
        public bool IsRepeated
        { get; set; }
        public long CurrentValue { get; set; }
        public bool Created { get; set; }
        public bool Finished { get; set; }
        public bool IsPaused { get; set; }
        public long OriginalValue { get; set; }
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
            _player = new SoundPlayer(@$"{Directory.GetCurrentDirectory()}\soft.wav");
        }
        public void FinishTimer()
        {
            _player.Play();
        }
    }
}
