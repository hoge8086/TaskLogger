using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskLogger.Business.Application;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

using TaskLogger.Business.Domain.Model;

namespace TaskLogger.ViewModel
{

    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged([CallerMemberName]string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public DelegateCommand AddLogCommand { get; set; }
        public DelegateCommand DeleteLogCommand { get; set; }
        public DelegateCommand NextDayCommand { get; set; }
        public DelegateCommand PrevDayCommand { get; set; }
        private DateTime _Date;
        public DateTime Date
        {
            get { return _Date; }
            set
            {
                if (value == _Date)
                    return;
                _Date = value;
                Update();
                RaisePropertyChanged(nameof(Date));
            }
        }
        public ObservableCollection<string> RecentlyTaskNames { get; set; }
        public ObservableCollection<TaskLogViewModel> TaskLogs { get; set; }
        private TaskLogApplicationService service;
        public MainWindowViewModel(TaskLogApplicationService service)
        {
            this.service = service;
            this.TaskLogs = new ObservableCollection<TaskLogViewModel>();
            this.RecentlyTaskNames = new ObservableCollection<string>();
            this.Date = DateTime.Today;
            AddLogCommand = new DelegateCommand(
                    (_) =>
                    {
                        var log = service.CreateTaskLog(Date);
                        //TaskLogs.Add(new TaskLogViewModel(log.Id, service));
                        Update();
                    });
            DeleteLogCommand = new DelegateCommand(
                    (x) =>
                    {
                        var log = x as TaskLogViewModel;
                        if (log == null) return;
                        service.DeleteTaskLog(log.Id);
                        Update();
                    });
            NextDayCommand = new DelegateCommand(
                    (_) =>
                    {
                        Date = Date.AddDays(1);
                    });
            PrevDayCommand = new DelegateCommand(
                    (_) =>
                    {
                        Date = Date.AddDays(-1);
                    });
            Update();
        }

        public void Update()
        {
            //TaskLogs = new ObservableCollection<TaskLogViewModel>();
            TaskLogs.Clear();
            var logs = service.TaskLogs(new DatePeriod() { Date = _Date });
            foreach (var log in logs)
            {
                TaskLogs.Add(new TaskLogViewModel(log.Id, service));
            }
            RecentlyTaskNames.Clear();
            foreach(var tn in service.RecentlyTaskNames())
                RecentlyTaskNames.Add(tn);
        }
    }
}
