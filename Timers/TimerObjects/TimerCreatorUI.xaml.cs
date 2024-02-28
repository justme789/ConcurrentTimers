using System;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace Timers.TimerObjects
{
    /// <summary>
    /// Interaction logic for TimerCreatorUI.xaml
    /// </summary>
    public partial class TimerCreatorUI : UserControl
    {
        private int _currStage = 0;
        public TimerCreatorUI()
        {
            InitializeComponent();
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void PreviousButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            TimerViewModel currentTimer = (TimerViewModel)DataContext;
            currentTimer.CurrentStage = currentTimer.Stages[--_currStage];
        }

        private void NextButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            TimerViewModel currentTimer = (TimerViewModel)DataContext;
            currentTimer.CurrentStage = currentTimer.Stages[++_currStage];
        }

        private void ConfirmButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            TimerViewModel currentTimer = (TimerViewModel)DataContext;
            currentTimer.CreateTimer();
        }

        private void TextBox_Pasting(object sender, System.Windows.DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                Regex regex = new Regex("^[0-9]+$");
                if (!regex.IsMatch(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }
    }
}
