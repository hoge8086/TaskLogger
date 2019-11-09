using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskLogger.Business.Domain.Model;
using TaskLogger.Business.Application;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

namespace TaskLogger.ViewModel
{

    public class TaskLogViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged([CallerMemberName]string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private int _Id;
        private string _TaskName;
        private DateTime? _Start;
        private DateTime? _End;
        private int _DownTimeMinutes;
        private int? _WorkingMinutes;

        public TaskLogViewModel(TaskLog taskLog)
        {
            Update(taskLog);
        }
        public void Update(TaskLog taskLog)
        {
            this._Id = taskLog.Id;
            this._TaskName = taskLog.TaskName;
            this._Start = taskLog.Start;
            this._End = taskLog.End;
            this._DownTimeMinutes = taskLog.DownTimeMinutes;
            this._WorkingMinutes = taskLog.WorkingMinutes;
        }

        public int Id
        {
            get { return _Id; }
            set
            {
                if (value == _Id)
                    return;
                _Id = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(Id));
            }
        }
        public string TaskName
        {
            get { return _TaskName; }
            set
            {
                if (value == _TaskName)
                    return;
                _TaskName = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(TaskName));
            }
        }

        public DateTime? Start
        {
            get { return _Start; }
            set
            {
                if (value == _Start)
                    return;
                _Start = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(Start));
                RaisePropertyChanged(nameof(WorkingMinutes));
            }
        }
        public DateTime? End
        {
            get { return _End; }
            set
            {
                if (value == _End)
                    return;
                _End = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(End));
                RaisePropertyChanged(nameof(WorkingMinutes));
            }
        }
        public int DownTimeMinutes
        {
            get { return _DownTimeMinutes; }
            set
            {
                if (value == _DownTimeMinutes)
                    return;
                _DownTimeMinutes = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(DownTimeMinutes));
            }
        }
        public int? WorkingMinutes
        {
            get { return _WorkingMinutes; }
            set
            {
                if (value == _WorkingMinutes)
                    return;
                _WorkingMinutes = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(WorkingMinutes));
            }
        }
    }

    public class MainWindowViewModel
    {
        public ObservableCollection<TaskLogViewModel> TaskLogs { get; set; }
        private TaskLogApplicationService service;
        public MainWindowViewModel(TaskLogApplicationService service)
        {
            this.service = service;
            Update();
        }

        public void Update()
        {
            TaskLogs = new ObservableCollection<TaskLogViewModel>();
            var logs = service.AllTaskLogs();
            foreach (var log in logs)
                TaskLogs.Add(new TaskLogViewModel(log));
        }
    }
}
