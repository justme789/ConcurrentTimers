using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Timers.ToDoList
{
    /// <summary>
    /// Interaction logic for ToDoItem.xaml
    /// </summary>
    public partial class ToDoItem : UserControl
    {
        private bool _gotFocus = false;
        public ToDoItem()
        {
            InitializeComponent();
        }
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            _gotFocus = true;
            TextBox theSender = sender as TextBox;
            if (!theSender.IsReadOnly)
            {
                // Hide the hint
                theSender.Text = "";
                if (theSender.DataContext is ToDoViewModel toDoItemViewModel)
                {
                    toDoItemViewModel.TextBrush = toDoItemViewModel.ParentBrush;
                }
            }
        }
        private void Cancel_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ToDoViewModel theDataContext = ((ToDoViewModel)((Border)sender).DataContext);
            theDataContext.Remove();
        }

        private void Finish_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ToDoViewModel theDataContext = ((Border)sender).DataContext as ToDoViewModel;
            theDataContext.Created = true;
            Keyboard.ClearFocus();
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            // Make sure there is something in todo
            ToDoViewModel theDataContext = ((TextBox)sender).DataContext as ToDoViewModel;
            if (_gotFocus && theDataContext.Content.Length > 0 && !theDataContext.Created)
            {
                theDataContext.FinishVisibility = Visibility.Visible;
            }
            else
            {
                theDataContext.FinishVisibility = Visibility.Collapsed;
            }
        }

        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            ToDoViewModel theDataContext = ((Border)sender).DataContext as ToDoViewModel;
            if (theDataContext.Created)
            {
                theDataContext.CancelVisibility = Visibility.Visible;
            }
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            ToDoViewModel theDataContext = ((Border)sender).DataContext as ToDoViewModel;
            if (theDataContext != null && theDataContext.Created)
            {
                theDataContext.CancelVisibility = Visibility.Collapsed;
            }
        }
    }
}
