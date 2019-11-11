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

        //TODO:内部表現を時刻だけにするよう修正したほがよい
        private DateTime _Date; //DataTimePickerとバインドするとに時刻のみ入力したときに、日付が勝手に変わってしまうので、日付けを持つ
        private int _Id;
        private string _TaskName;
        private DateTime _Start;
        private DateTime? _End;
        private int _DownTimeMinutes;
        private int? _WorkingMinutes;
        public DelegateCommand StartNowCommand { get; }
        public DelegateCommand EndNowCommand { get; }
        private TaskLogApplicationService service;

        public TaskLogViewModel(int id, DateTime date, TaskLogApplicationService service)
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
            _Date = date;
            Update();
        }

        public void Update()
        {
            //TODO:効率が悪いので何とかする
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
        }

        public int Id
        {
            get { return _Id; }
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
            }
        }

        public DateTime Start
        {
            get { return _Start; }
            set
            {
                if (value == _Start)
                    return;
                _Start = CorrectionDate(value);
                service.ChangeTaskLogStart(_Id, _Start);
                Update();
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
                    _End = CorrectionDate(value.Value);
                else
                    _End = null;
                service.ChangeTaskLogEnd(_Id, _End.Value);
                Update();
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
            }
        }
        public int? WorkingMinutes
        {
            get { return _WorkingMinutes; }
        }

        private DateTime CorrectionDate(DateTime dateTime)
        {
            return new DateTime(_Date.Year, _Date.Month, _Date.Day, dateTime.Hour, dateTime.Minute, 0);
        }
    }
}
