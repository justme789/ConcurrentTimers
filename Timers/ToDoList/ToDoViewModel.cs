using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Timers.ToDoList
{
    public class ToDoViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private ToDo _toDo;
        public string Content { get => _toDo.Content; set => _toDo.Content = value; }
        public ToDoViewModel(ToDo toDo)
        {
            _toDo = toDo;
        }
    }
}
