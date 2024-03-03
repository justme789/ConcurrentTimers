﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Timers.ToDoList
{
    /// <summary>
    /// Interaction logic for ToDoItem.xaml
    /// </summary>
    public partial class ToDoItem : UserControl
    {
        public ToDoItem()
        {
            InitializeComponent();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox theSender = sender as TextBox;
            if (!theSender.IsReadOnly)
            {
                theSender.Text = "";
                theSender.Foreground = new SolidColorBrush(Colors.White);
            }
        }
        private void Cancel_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ToDoViewModel theDataContext = ((Border)sender).DataContext as ToDoViewModel;
            theDataContext.Remove();
        }

        private void Finish_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ToDoViewModel theDataContext = ((Border)sender).DataContext as ToDoViewModel;
            theDataContext.Created = true;

        }
    }
}
