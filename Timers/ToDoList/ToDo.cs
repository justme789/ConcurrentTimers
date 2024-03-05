using Timers.TimerObjects;

namespace Timers.ToDoList
{
    public class ToDo
    {
        public string Content { get; set; } = "Enter TODO Name here";
        public bool Created { get; set; } = false;
        public TimerViewModel Parent;
        public ToDo(TimerViewModel timer)
        {
            Parent = timer;
        }
    }
}
