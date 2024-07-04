using System;
using Timers.ToDoList;

namespace Timers.TimerObjects
{
    public class ToDoAdded : EventArgs
    {
        public ToDoViewModel AddedToDo { get; set; }

    }
}
