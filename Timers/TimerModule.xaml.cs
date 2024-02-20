using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Timers
{
    public partial class TimerModule
    {
        public TimerModule()
        {
            InitializeComponent();
        }
        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Timer)((Grid)sender).DataContext).Hovered = true;
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Timer)((Grid)sender).DataContext).Hovered = false;
        }
        private void CancelTimer_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ((Timer)((Border)sender).DataContext).Cancelled = true;
        }
    }
}
