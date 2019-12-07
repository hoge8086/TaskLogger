using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TaskLogger.Business.Domain.Model;

namespace TaskLogger.ViewModel
{
    public abstract class TaskSpecifyViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([CallerMemberName]string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public abstract TaskSpecify Create(List<TaskSearchMethod> targetTasks);
    }

    [Description("全てのタスク")]
    public class AllTaskSpecifyViewModel : TaskSpecifyViewModel
    {
        public override TaskSpecify Create(List<TaskSearchMethod> targetTasks)
        {
            return new AllTaskSpecify();
        }
    }

    [Description("全てのタスク(検索)")]
    public class AllTaskSpecifyByKeywordViewModel : TaskSpecifyViewModel
    {
        private string _TaskName;
        private TaskSearchMethodType _TaskSearchMethodType;

        public AllTaskSpecifyByKeywordViewModel()
        {
            _TaskName = "";
            _TaskSearchMethodType = TaskSearchMethodType.FirstMatch;
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

        public override TaskSpecify Create(List<TaskSearchMethod> targetTasks)
        {
            return new AllTaskSpecify(new TaskSearchMethod(TaskName, TaskSearchMethodType));
        }
    }

    [Description("タスク指定")]
    public class IndividualTaskSpecifyViewModel : TaskSpecifyViewModel
    {
        public override TaskSpecify Create(List<TaskSearchMethod> targetTasks)
        {
            return new IndividualTaskSpecify(targetTasks);
        }
    }

}
