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
        public ToDo ToDo { get; set; }
        private Brush _textBrush = new SolidColorBrush(Color.FromArgb(44, 255, 255, 255));
        private Visibility _cancelVisibility = Visibility.Visible;
        public Visibility CancelVisibility
        {
            get => _cancelVisibility;
            set
            {
                _cancelVisibility = value;
                OnPropertyChanged(nameof(CancelVisibility));
            }
        }
        private Visibility _finishVisibility = Visibility.Collapsed;
        public Visibility FinishVisibility
        {
            get => _finishVisibility;
            set
            {
                _finishVisibility = value;
                OnPropertyChanged(nameof(FinishVisibility));
            }
        }
        public string Content
        {
            get => ToDo.Content;
            set
            {
                ToDo.Content = value;
                OnPropertyChanged(nameof(Content));
            }
        }
        public bool Created
        {
            get => ToDo.Created;
            set
            {
                ToDo.Created = value;
                if (ToDo.Created)
                {
                    CancelVisibility = Visibility.Collapsed;
                    FinishVisibility = Visibility.Collapsed;
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
            ToDo = toDo;
            ParentBrush = parentBrush;
            if (Created)
            {
                Created = true;
                TextBrush = parentBrush;
            }
        }
        public void Remove()
        {
            ToDoRemoved?.Invoke(this, new RemoveToDo { ToDoToRemove = this });
        }
    }
}
