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

    [XmlInclude(typeof(ReportTargetAllTask))]
    [XmlInclude(typeof(ReportTargetSpecifyTask))]
    public abstract class ReportTarget
    {
        public string Title { get; set; }
        public Period Period { get; set; }
        public abstract TaskReport CreateReport(TaskLogs logs);
    }

    public class ReportTargetAllTask: ReportTarget
    {
        public TaskSearchMethod TaskSearchMethod { get; set; }

        public ReportTargetAllTask() { }
        public ReportTargetAllTask(string title, Period period)
        {
            this.Title = title;
            this.Period = period;
            this.TaskSearchMethod = null;
        }

        public ReportTargetAllTask(string title, Period period, TaskSearchMethod taskSearchMethod)
        {
            this.Title = title;
            this.Period = period;
            this.TaskSearchMethod = taskSearchMethod;
        }

        private ReportTargetSpecifyTask CreateSpecify(TaskLogs logs)
        {
            var target = logs.Logs
                            .Where(x => (TaskSearchMethod == null ? true : TaskSearchMethod.IsMatched(x)))
                            .Select(x =>  x.TaskName)
                            .Distinct()
                            .Select(x => new TaskSearchMethod() { TaskKeyword = x, SearchMethod = TaskSearchMethodType.PerfectMatch })
                            .ToList();
            return new ReportTargetSpecifyTask(Title, Period, target);
        }

        public override TaskReport CreateReport(TaskLogs logs)
        {
            return CreateSpecify(logs).CreateReport(logs);
        }
    }

    public class ReportTargetSpecifyTask : ReportTarget
    {
        public List<TaskSearchMethod> TargetTasks {get; set;}

        public ReportTargetSpecifyTask() { }
        public ReportTargetSpecifyTask(
            string title,
            Period period,
            List<TaskSearchMethod> targetTasks)
        {
            this.Title = title;
            this.Period = period;
            this.TargetTasks = targetTasks;
        }

        public override TaskReport CreateReport(TaskLogs logs)
        {
            return logs.CreateReport(TargetTasks);
        }
    }
}
