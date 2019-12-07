using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace TaskLogger.Business.Domain.Model
{
    public class TaskReport
    {
        public List<TaskReportItem> Items { get; set; }
        public Int64 TotalMinutes { get; set; }
    }
    public class TaskReportItem
    {
        public TaskSearchMethod TaskSearchMethod { get; set; }
        public TaskLogs Logs { get; set; }
        public int TotalMinutes { get; set; }
    }

    public class ReportTarget
    {
        public ReportTarget()
        {
            this.Title = "新規";
            this.Period = new WholePeriod();
            this.TaskSpecify = new AllTaskSpecify();
        }

        public ReportTarget(string title, Period period, TaskSpecify taskSpecify)
        {
            this.Title = title;
            this.Period = period;
            this.TaskSpecify = taskSpecify;
        }

        public string Title { get; set; }
        public Period Period { get; set; }
        public TaskSpecify TaskSpecify { get; set; }
    }

    [XmlInclude(typeof(AllTaskSpecify))]
    [XmlInclude(typeof(IndividualTaskSpecify))]
    public abstract class TaskSpecify
    {
        public abstract List<TaskSearchMethod> CreateTaskSearchMethods(TaskLogs logs);
    }

    public class AllTaskSpecify: TaskSpecify
    {
        public TaskSearchMethod TaskSearchMethod { get; set; }

        public AllTaskSpecify()
        {
            this.TaskSearchMethod = null;
        }

        public AllTaskSpecify(TaskSearchMethod taskSearchMethod)
        {
            this.TaskSearchMethod = taskSearchMethod;
        }

        public override List<TaskSearchMethod> CreateTaskSearchMethods(TaskLogs logs)
        {
            return logs.Logs
                            .Where(x => (TaskSearchMethod == null ? true : TaskSearchMethod.IsMatched(x)))
                            .Select(x =>  x.TaskName)
                            .Distinct()
                            .Select(x => new TaskSearchMethod() { TaskKeyword = x, SearchMethod = TaskSearchMethodType.PerfectMatch })
                            .ToList();
        }
    }

    public class IndividualTaskSpecify : TaskSpecify
    {
        public List<TaskSearchMethod> TargetTasks {get; set;}

        public IndividualTaskSpecify() { }
        public IndividualTaskSpecify(
            List<TaskSearchMethod> targetTasks)
        {
            this.TargetTasks = targetTasks;
        }

        public override List<TaskSearchMethod> CreateTaskSearchMethods(TaskLogs logs)
        {
            return TargetTasks;
        }
    }
}
