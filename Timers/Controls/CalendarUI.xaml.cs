using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace Timers.Controls
{
    /// <summary>
    /// Interaction logic for CalendarUI.xaml
    /// </summary>
    public class SelectedDateChanged : EventArgs
    {
        public CalendarItemModel SelectedDate { get; set; }
    }
    public partial class CalendarUI : UserControl
    {
        public CalendarItemModel SelectedDate { get; set; }
        public event EventHandler<SelectedDateChanged> ItemClicked;

        protected virtual void OnItemClicked(CalendarItemModel itemInfo)
        {
            ItemClicked?.Invoke(this, new SelectedDateChanged { SelectedDate = itemInfo });
            itemInfo.IsSelected = true;
        }
        public CalendarUI()
        {
            InitializeComponent();
            DateTime today = DateTime.Today;

            MonthName.Text = today.ToString("MMMM");
            List<CalendarItemModel> theDays = Enumerable.Range(1, DateTime.DaysInMonth(today.Year, today.Month))
                    .Select(day => new CalendarItemModel(new DateTime(today.Year, today.Month, day)))
                    .ToList();
            TheCalendar.ItemsSource = theDays;
            // Not cool
            int offset = Math.Abs(theDays[0].TheDate.DayOfWeek - DayOfWeek.Monday) * 84;
            theDays[0].TheMargin = new System.Windows.Thickness(offset, 5, 10, 5);
            SelectedDate = theDays[today.Day - 1];
            theDays[today.Day - 1].IsSelected = true;
        }

        private void CalendarItem_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            CalendarItem item = (CalendarItem)sender;
            if (SelectedDate != null)
            {
                SelectedDate.IsSelected = false;
            }
            SelectedDate = (CalendarItemModel)item.DataContext;
            OnItemClicked(SelectedDate);
        }
    }
}
