using System.Threading;

namespace Timers
{
    public class Timer
    {
        private Thread _soundThread;
        private bool _finished = false;
        private bool _created = false;
        private bool _isPaused = false;
        private int _repeatedValue = 0;


        public TimersContainer Container { get; set; }
        public int RepeatedValue { get => _repeatedValue; set => _repeatedValue = value; }
        public bool IsRepeated
        { get; set; }
        public long CurrentValue { get; set; }
        public bool Created { get; set; }
        public bool Finished { get; set; }
        public bool IsPaused { get; set; }
        public long OriginalValue { get; set; }
        public bool Cancelled { get; set; }
        public Timer()
        {
            _soundThread = new Thread(() =>
            {
                System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"C:\Users\nobod\source\repos\Timers\Timers\soft.wav");
                player.Play();
                while (!Cancelled) { }
                player.Stop();
            });
        }
        public void FinishTimer()
        {
            _soundThread.Start();
        }
    }
}
