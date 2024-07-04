using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Timers.ToDoList;

namespace Timers.Messengers
{
    public class ToDoMessenger
    {
        private static readonly Lazy<ToDoMessenger> _instance = new Lazy<ToDoMessenger>(() => new ToDoMessenger());
        private Dictionary<DateTime, ObservableCollection<ToDoViewModel>> _allDaysTodos;

        private ToDoMessenger()
        {
            _allDaysTodos = new Dictionary<DateTime, ObservableCollection<ToDoViewModel>>();
        }

        public static ToDoMessenger Instance => _instance.Value;
        public IReadOnlyList<ToDoViewModel> TodaysTodos =>
            _allDaysTodos.ContainsKey(DateTime.Today)
            ? _allDaysTodos[DateTime.Today]
            : new List<ToDoViewModel>();
        public void AddItem(DateTime theDateAdded, ToDoViewModel todo)
        {
            if (_allDaysTodos.ContainsKey(theDateAdded.Date))
            {
                _allDaysTodos[theDateAdded.Date].Add(todo);
            }
            else
            {
                _allDaysTodos.Add(theDateAdded.Date, new ObservableCollection<ToDoViewModel> { todo });
            }
        }

        internal ObservableCollection<ToDoViewModel> GetListForDay(DateTime theDay)
        {
            if (!_allDaysTodos.ContainsKey(theDay.Date))
            {
                _allDaysTodos.Add(theDay.Date, new ObservableCollection<ToDoViewModel>());
            }
            return _allDaysTodos[theDay.Date];
        }
    }
}
