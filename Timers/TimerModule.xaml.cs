using System.Windows.Controls;
using System.Windows.Input;

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

        private void PlayPauseButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ((Timer)((Border)sender).DataContext).IsPaused = !((Timer)((Border)sender).DataContext).IsPaused;
        }
    }
}
