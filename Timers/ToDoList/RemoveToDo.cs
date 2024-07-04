using System;

namespace Timers.ToDoList
{
    public class RemoveToDo : EventArgs
    {
        public ToDoViewModel ToDoToRemove { get; set; }
    }
}
