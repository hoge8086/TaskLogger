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
            PeriodType = PeriodType.DatePeriod;
            SpecifyTaskMethod = SpecifyTaskMethod.AllTasks;

            ReportCommand = new DelegateCommand(
                    (_) =>
                    {
                        TaskReport report = null;
                        if(SpecifyTaskMethod == SpecifyTaskMethod.AllTasks)
                        {
                            report = service.CreateReportForAllTask(Period.Create());
                        }
                        else if(SpecifyTaskMethod == SpecifyTaskMethod.SpecifyTask)
                        {
                            report = service.CreateReport(Period.Create(), CreateTaskSearhMethods());
                        }
                        Update(report);
                    });
            //Update();
        }
        private List<TaskSearchMethod> CreateTaskSearhMethods()
        {
            return TaskReports
                .Select(x => {
                        return new TaskSearchMethod() { TaskKeyword = x.TaskName, SearchMethod = x.TaskSearchMethodType };
                    })
                .ToList();
        }

        public void Update(TaskReport report)
        {
            TaskReports.Clear();
            foreach(var item in report.Items)
            {
                TaskReports.Add(new TaskReportItemViewModel(item.TaskSearchMethod.TaskKeyword, item.TaskSearchMethod.SearchMethod, item.TotalMinutes));
            }
        }
    }


    public class TaskReportItemViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged([CallerMemberName]string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private string _TaskName;
        private TaskSearchMethodType _TaskSearchMethodType;
        private int? _WorkingMinutes;

        public TaskReportItemViewModel()
        {
            this.TaskName = "";
            this.TaskSearchMethodType = TaskSearchMethodType.FirstMatch;
            this.WorkingMinutes = 0;
        }
        public TaskReportItemViewModel(
            string TaskName,
            TaskSearchMethodType TaskSearchMethod,
            int? WorkingMinutes)
        {
            this.TaskName = TaskName;
            this.TaskSearchMethodType = TaskSearchMethod;
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

        public TaskSearchMethodType TaskSearchMethodType
        {
            get { return _TaskSearchMethodType; }
            set
            {
                if (value == _TaskSearchMethodType)
                    return;
                _TaskSearchMethodType = value;
                RaisePropertyChanged(nameof(TaskSearchMethodType));
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
