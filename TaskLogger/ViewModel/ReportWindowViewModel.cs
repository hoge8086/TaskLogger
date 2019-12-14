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

        public ObservableCollection<TaskSpecifyViewModel> TaskSpecifies { get; set; }
        private TaskSpecifyViewModel _TaskSpecify;
        public TaskSpecifyViewModel TaskSpecify
        {
            get { return _TaskSpecify; }
            set
            {
                if (value == _TaskSpecify)
                    return;
                _TaskSpecify = value;
                RaisePropertyChanged(nameof(TaskSpecify));
            }
        }

        public ObservableCollection<TaskReportItemViewModel> TaskReports { get; set; }

        public ReportViewModel(TaskLogApplicationService service, ReportTarget reportTarget)
        {
            ObservableCollection<PeriodViewModel> periods = new ObservableCollection<PeriodViewModel>();
            PeriodViewModel period = PeriodViewModel.Create(reportTarget.Period);
            var targets = new ObservableCollection<TaskReportItemViewModel>();
            TaskSpecifyViewModel reportTargetVm = TaskSpecifyViewModel.Create(reportTarget.TaskSpecify);
            if (reportTarget.TaskSpecify is IndividualTaskSpecify)
            {
                foreach(var x in ((IndividualTaskSpecify)reportTarget.TaskSpecify).TargetTasks)
                    targets.Add(new TaskReportItemViewModel(x.TaskKeyword, x.SearchMethod, 0));
            }
            Init(service, reportTarget.Title, period, reportTargetVm, targets);
        }

        public ObservableCollection<PeriodViewModel> Periods { get; set; }
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

        public ReportTarget CreateModel()
        {
            return new Business.Domain.Model.ReportTarget(Title, Period.Create(), TaskSpecify.Create(CreateTaskSearhMethods()));
        }

        public ReportViewModel(TaskLogApplicationService service)
        {
            Init(service, "新規", new DatePeriodViewModel(), new AllTaskSpecifyViewModel(),  new ObservableCollection<TaskReportItemViewModel>());
        }

        private  static T ItselfOrDefault<T>(object obj) where T : new() { return  (obj != null && obj is T) ? (T)obj : new T(); }
        public void Init(TaskLogApplicationService service, string title, PeriodViewModel period,  TaskSpecifyViewModel taskSpecify, ObservableCollection<TaskReportItemViewModel> targets)
        {
            this.service = service;
            this.TaskReports = targets;
            this.Title = title;
            Periods = new ObservableCollection<PeriodViewModel>();
            Periods.Add(ItselfOrDefault<WholePeriodViewModel>(period));
            Periods.Add(ItselfOrDefault<PartialPeriodViewModel>(period));
            Periods.Add(ItselfOrDefault<DatePeriodViewModel>(period));
            this.Period = period != null ? period : Periods[0];

            TaskSpecifies = new ObservableCollection<TaskSpecifyViewModel>();
            TaskSpecifies.Add(ItselfOrDefault<AllTaskSpecifyViewModel>(taskSpecify));
            TaskSpecifies.Add(ItselfOrDefault<AllTaskSpecifyByKeywordViewModel>(taskSpecify));
            TaskSpecifies.Add(ItselfOrDefault<IndividualTaskSpecifyViewModel>(taskSpecify));
            this.TaskSpecify = taskSpecify != null ? taskSpecify :TaskSpecifies[0];

            ReportCommand = new DelegateCommand(
                    (_) =>
                    {
                        var report = service.CreateReport(Period.Create(), TaskSpecify.Create(CreateTaskSearhMethods()));
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
