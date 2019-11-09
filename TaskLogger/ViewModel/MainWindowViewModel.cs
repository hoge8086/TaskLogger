using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskLogger.Business.Application;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

namespace TaskLogger.ViewModel
{

    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged([CallerMemberName]string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public DelegateCommand AddLogCommand { get; set; }
        private DateTime _Date;
        public DateTime Date
        {
            get { return _Date; }
            set
            {
                if (value == _Date)
                    return;
                _Date = value;
                RaisePropertyChanged(nameof(Date));
            }
        }

        public ObservableCollection<TaskLogViewModel> TaskLogs { get; set; }
        private TaskLogApplicationService service;
        public MainWindowViewModel(TaskLogApplicationService service)
        {
            this.service = service;
            AddLogCommand = new DelegateCommand(
                    (_) =>
                    {
                        var log = service.CreateTaskLog();
                        TaskLogs.Add(new TaskLogViewModel(log.Id, service));
                    });
            Update();
        }

        public void Update()
        {
            TaskLogs = new ObservableCollection<TaskLogViewModel>();
            var logs = service.AllTaskLogs();
            foreach (var log in logs)
            {
                TaskLogs.Add(new TaskLogViewModel(log.Id, service));
            }
        }
    }
}
