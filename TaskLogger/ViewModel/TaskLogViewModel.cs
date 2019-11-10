using System;
using TaskLogger.Business.Domain.Model;
using TaskLogger.Business.Application;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TaskLogger.ViewModel
{
    public class TaskLogViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged([CallerMemberName]string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private int _Id;
        private string _TaskName;
        private DateTime _Start;
        private DateTime? _End;
        private int _DownTimeMinutes;
        private int? _WorkingMinutes;
        public DelegateCommand StartNowCommand { get; }
        public DelegateCommand EndNowCommand { get; }
        private TaskLogApplicationService service;

        public TaskLogViewModel(int id, TaskLogApplicationService service)
        {
            this.service = service;
            StartNowCommand = new DelegateCommand(
                (_) => {
                    service.StartTaskNow(id);
                    Update();
                });
            EndNowCommand = new DelegateCommand(
                (_) => {
                    service.EndTaskNow(id);
                    Update();
                });

            _Id = id;
            Update();
        }

        public void Update()//TaskLog taskLog)
        {
            var log = service.TaskLog(_Id);
            //this._Id = log.Id;
            this._TaskName = log.TaskName;
            this._Start = log.Start;
            this._End = log.End;
            this._DownTimeMinutes = log.DownTimeMinutes;
            this._WorkingMinutes = log.WorkingMinutes;
            RaisePropertyChanged(nameof(TaskName));
            RaisePropertyChanged(nameof(Start));
            RaisePropertyChanged(nameof(End));
            RaisePropertyChanged(nameof(DownTimeMinutes));
            RaisePropertyChanged(nameof(WorkingMinutes));

            //RaisePropertyChanged();
        }

        public int Id
        {
            get { return _Id; }
            //set
            //{
            //    if (value == _Id)
            //        return;
            //    _Id = value;
            //    RaisePropertyChanged(nameof(Id));
            //}
        }
        public string TaskName
        {
            get { return _TaskName; }
            set
            {
                if (value == _TaskName)
                    return;
                _TaskName = value;
                service.ChangeTaskLogName(_Id, _TaskName);
                Update();
                //RaisePropertyChanged(nameof(TaskName));
            }
        }

        public DateTime Start
        {
            get { return _Start; }
            set
            {
                if (value == _Start)
                    return;
                _Start = value;
                service.ChangeTaskLogStart(_Id, _Start);
                Update();
                //RaisePropertyChanged(nameof(Start));
                //RaisePropertyChanged(nameof(WorkingMinutes));
            }
        }
        public DateTime? End
        {
            get { return _End; }
            set
            {
                if (value == _End)
                    return;
                if (value != null)
                    _End = new DateTime(Start.Year, Start.Month, Start.Day, value.Value.Hour, value.Value.Minute, 0);
                else
                    _End = null;
                service.ChangeTaskLogEnd(_Id, _End.Value);
                Update();
                //RaisePropertyChanged(nameof(End));
                //RaisePropertyChanged(nameof(WorkingMinutes));
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
                service.ChangeTaskLogDownTime(_Id, _DownTimeMinutes);
                Update();
                //service.ChangeTaskLogDownTime(_Id, _DownTimeMinutes);
                //RaisePropertyChanged(nameof(DownTimeMinutes));
            }
        }
        public int? WorkingMinutes
        {
            get { return _WorkingMinutes; }
            //set
            //{
            //    if (value == _WorkingMinutes)
            //        return;
            //    _WorkingMinutes = value;
            //    RaisePropertyChanged(nameof(WorkingMinutes));
            //}
        }
    }
}
