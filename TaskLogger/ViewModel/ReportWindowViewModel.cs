using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TaskLogger.Business.Application;
using TaskLogger.Business.Domain.Model;

namespace TaskLogger.ViewModel
{
    public enum SpecifyTaskMethod
    {
        [Description("タスク指定")]
        SpecifyTask,
        [Description("すべてのタスク")]
        AllTasks
    }

    public class ReportWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged([CallerMemberName]string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private TaskLogApplicationService service;

        public DelegateCommand ReportCommand { get; set; }

        private PeriodViewModel _Period;
        public PeriodViewModel Period
        {
            get { return _Period; }
            set
            {
                if (value == _Period)
                    return;
                _Period = value;
                RaisePropertyChanged(nameof(Period));
            }
        }
        private SpecifyTaskMethod _SpecifyTaskMethod;
        public SpecifyTaskMethod SpecifyTaskMethod
        {
            get { return _SpecifyTaskMethod; }
            set
            {
                if (value == _SpecifyTaskMethod)
                    return;
                _SpecifyTaskMethod = value;
                RaisePropertyChanged(nameof(SpecifyTaskMethod));
            }
        }

        private PeriodType _PeriodType;
        public PeriodType PeriodType
        {
            get { return _PeriodType; }
            set
            {
                if (value == _PeriodType)
                    return;
                _PeriodType = value;
                switch(_PeriodType)
                {
                    case PeriodType.DatePeriod:
                        Period = new DatePeriodViewModel();
                        break;
                    case PeriodType.PartialPeriod:
                        Period = new PartialPeriodViewModel();
                        break;
                    case PeriodType.WholePeriod:
                        Period = new WholePeriodViewModel();
                        break;
                }
                RaisePropertyChanged(nameof(PeriodType));
            }
        }

        public ObservableCollection<TaskReportItemViewModel> TaskReports { get; set; }
        public ReportWindowViewModel(TaskLogApplicationService service)
        {
            this.service = service;
            this.TaskReports = new ObservableCollection<TaskReportItemViewModel>();
            ReportCommand = new DelegateCommand(
                    (_) =>
                    {
                        var log = service.CreateReport(new ReportTarget());
                        Update();
                    });
            PeriodType = PeriodType.DatePeriod;
            SpecifyTaskMethod = SpecifyTaskMethod.AllTasks;
            Update();
        }

        public void Update()
        {
            //TaskLogs = new ObservableCollection<TaskLogViewModel>();
            //TaskLogs.Clear();
            //var logs = service.TaskLogs(new DatePeriod() { Date = _Date });
            //foreach (var log in logs)
            //{
            //    TaskLogs.Add(new TaskLogViewModel(log.Id, _Date, service));
            //}
            //RecentlyTaskNames.Clear();
            //foreach(var tn in service.RecentlyTaskNames())
            //    RecentlyTaskNames.Add(tn);
        }
    }


    public class TaskReportItemViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged([CallerMemberName]string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private string _TaskName;
        private TaskSearchMethod _TaskSearchMethod;
        private int? _WorkingMinutes;

        public TaskReportItemViewModel(
            string TaskName,
            TaskSearchMethod TaskSearchMethod,
            int? WorkingMinutes)
        {
            this.TaskName = TaskName;
            this.TaskSearchMethod = TaskSearchMethod;
            this.WorkingMinutes = WorkingMinutes;
        }

        public string TaskName
        {
            get { return _TaskName; }
            set
            {
                if (value == _TaskName)
                    return;
                _TaskName = value;
                RaisePropertyChanged(nameof(TaskName));
            }
        }

        public TaskSearchMethod TaskSearchMethod
        {
            get { return _TaskSearchMethod; }
            set
            {
                if (value == _TaskSearchMethod)
                    return;
                _TaskSearchMethod = value;
                RaisePropertyChanged(nameof(TaskSearchMethod));
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
                RaisePropertyChanged(nameof(WorkingMinutes));
            }
        }
    }


}
