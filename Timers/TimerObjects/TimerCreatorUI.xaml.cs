using System;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace Timers.TimerObjects
{
    /// <summary>
    /// The UI for the timer creator
    /// </summary>
    public partial class TimerCreatorUI : UserControl
    {
        private int _currStage = 0;
        public TimerCreatorUI()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Only allow numbers in the text box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        /// <summary>
        /// Go back a stage so either end up in hh or mm
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PreviousButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            TimerViewModel currentTimer = (TimerViewModel)DataContext;
            currentTimer.CurrentStage = currentTimer.Stages[--_currStage];
        }
        /// <summary>
        /// Go forward a stage so either in mm or ss
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NextButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            TimerViewModel currentTimer = (TimerViewModel)DataContext;
            if (currentTimer.CurrentStage.Value.Length == 0) { return; }
            currentTimer.CurrentStage = currentTimer.Stages[++_currStage];
        }
        /// <summary>
        /// confirm once ss is set
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfirmButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            TimerViewModel currentTimer = (TimerViewModel)DataContext;
            if (currentTimer.CurrentStage.Value.Length == 0) { return; }
            currentTimer.CreateTimer();
        }
        /// <summary>
        /// Ensure that pasted value satisfies number constraints
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
