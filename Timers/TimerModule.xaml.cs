using System.Windows.Controls;
using System.Windows.Input;

namespace Timers
{
    public partial class TimerModule : UserControl
    {


        public TimerModule()
        {
            InitializeComponent();
        }
        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            ((TimerViewModel)((Grid)sender).DataContext).Hovered = true;
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            ((TimerViewModel)((Grid)sender).DataContext).Hovered = false;
        }
        private void CancelTimer_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ((TimerViewModel)((Border)sender).DataContext).Cancelled = true;
        }

        private void PlayPauseButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ((TimerViewModel)((Border)sender).DataContext).IsPaused = !((TimerViewModel)((Border)sender).DataContext).IsPaused;
        }

        private void RepeatButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ((TimerViewModel)((Border)sender).DataContext).IsRepeated = !((TimerViewModel)((Border)sender).DataContext).IsRepeated;
        }

        private void CLoseButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            TimerViewModel dataContextTimer = ((TimerViewModel)((Border)sender).DataContext);
            dataContextTimer.Remove();
        }
    }
}
