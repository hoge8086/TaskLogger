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

        public ObservableCollection<ReportViewModel> Reports { get; set; }
        public DelegateCommand CloseCommand { get; set; }

        public ReportWindowViewModel(TaskLogApplicationService service)
        {
            Reports = new ObservableCollection<ReportViewModel>();

            var targets = service.AllReportTargets();
            foreach (var t in targets)
                Reports.Add(new ReportViewModel(service, t));

            CloseCommand = new DelegateCommand(
                    (_) =>
                    {
                        var t = Reports.Select(x => x.CreateModel()).ToList();
                        service.SaveReportTarget(t);
                    });
        }
    }

    public class ReportViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged([CallerMemberName]string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private TaskLogApplicationService service;

        public DelegateCommand ReportCommand { get; set; }
        public DelegateCommand AddRowCommand { get; set; }

        private string _Title;
        public string Title
        {
            get { return _Title; }
            set
            {
                if (value == _Title)
                    return;
                _Title = value;
                RaisePropertyChanged(nameof(Title));
            }
        }
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
        public ReportViewModel(TaskLogApplicationService service, ReportTarget reportTarget)
        {
            var periodType = PeriodType.DatePeriod;
            PeriodViewModel periodViewModel = null;
            if (reportTarget.Period is DatePeriod)
            {
                periodType = PeriodType.DatePeriod;
                periodViewModel = new DatePeriodViewModel() { Date = ((DatePeriod)reportTarget.Period).Date };
            }
            if (reportTarget.Period is WholePeriod)
            {
                periodType = PeriodType.WholePeriod;
                periodViewModel = new WholePeriodViewModel();
            }
            if (reportTarget.Period is PartialPeriod)
            {
                periodType = PeriodType.PartialPeriod;
                periodViewModel = new PartialPeriodViewModel()
                {
                    Start = ((PartialPeriod)reportTarget.Period).StartDay,
                    End = ((PartialPeriod)reportTarget.Period).EndDay
                };
            }

            var targets = new ObservableCollection<TaskReportItemViewModel>();
            var specifyTaskMethod = SpecifyTaskMethod;
            if (reportTarget is ReportTargetAllTask)
            {
                specifyTaskMethod = SpecifyTaskMethod.AllTasks;
            }
            if (reportTarget is ReportTargetSpecifyTask)
            {
                specifyTaskMethod = SpecifyTaskMethod.SpecifyTask;
                foreach(var x in ((ReportTargetSpecifyTask)reportTarget).TargetTasks)
                    targets.Add(new TaskReportItemViewModel(x.TaskKeyword, x.SearchMethod, 0));
            }
            Init(service, reportTarget.Title, periodType,  periodViewModel, specifyTaskMethod, targets);
        }

        public ReportTarget CreateModel()
        {
            if(SpecifyTaskMethod == SpecifyTaskMethod.AllTasks)
            {
                return new ReportTargetAllTask(Title, Period.Create());
            }
            else if(SpecifyTaskMethod == SpecifyTaskMethod.SpecifyTask)
            {
                return new ReportTargetSpecifyTask(Title, Period.Create(), CreateTaskSearhMethods());
            }
            return null;
        }

        public ReportViewModel(TaskLogApplicationService service)
        {
            Init(service, "新規", PeriodType.DatePeriod, new DatePeriodViewModel(), SpecifyTaskMethod.AllTasks,  new ObservableCollection<TaskReportItemViewModel>());
        }

        public void Init(TaskLogApplicationService service, string title, PeriodType periodType, PeriodViewModel periodViewModel, SpecifyTaskMethod specifyTaskMethod, ObservableCollection<TaskReportItemViewModel> targets)
        {
            this.service = service;
            this.TaskReports = targets;
            Title = title;
            PeriodType = periodType;
            Period = periodViewModel;
            SpecifyTaskMethod = specifyTaskMethod;

            ReportCommand = new DelegateCommand(
                    (_) =>
                    {
                        var report = service.CreateReport(CreateModel());
                        Update(report);
                    });
            AddRowCommand = new DelegateCommand(
                (_) =>
                {
                    TaskReports.Add(new TaskReportItemViewModel());
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
