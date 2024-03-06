using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

namespace Timers.ToDoList
{
    public class ToDoViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private bool _editableTextBox = false;
        public bool EditableTextBox
        {
            get => _editableTextBox;
            set
            {
                _editableTextBox = value;
                OnPropertyChanged(nameof(EditableTextBox));
            }
        }
        private ToDo _toDo;
        private Brush _textBrush = new SolidColorBrush(Color.FromArgb(44, 255, 255, 255));
        private Visibility _cancelFinishVisibility = Visibility.Visible;
        public Visibility CancelFinishVisibility
        {
            get => _cancelFinishVisibility;
            set
            {
                _cancelFinishVisibility = value;
                OnPropertyChanged(nameof(CancelFinishVisibility));
            }
        }
        public string Content
        {
            get => _toDo.Content; set
            {
                _toDo.Content = value;
                OnPropertyChanged(nameof(Content));
            }
        }
        public bool Created
        {
            get => _toDo.Created;
            set
            {
                _toDo.Created = value;
                if (_toDo.Created)
                {
                    CancelFinishVisibility = Visibility.Collapsed;
                    EditableTextBox = true;
                }
            }
        }
        public delegate void RemoveToDoEventHandler(object sender, RemoveToDo e);
        public event RemoveToDoEventHandler ToDoRemoved;
        public Brush ParentBrush { get; set; }
        public Brush TextBrush
        {
            get => _textBrush; set
            {
                _textBrush = value;
                OnPropertyChanged(nameof(TextBrush));
            }
        }
        public ToDoViewModel(ToDo toDo, Brush parentBrush)
        {
            _toDo = toDo;
            ParentBrush = parentBrush;
        }
        public void Remove()
        {
            ToDoRemoved?.Invoke(this, new RemoveToDo { ToDoToRemove = this });
        }
    }
}
