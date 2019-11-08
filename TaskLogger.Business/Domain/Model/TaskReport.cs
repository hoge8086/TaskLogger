using System.Collections.Generic;

namespace TaskLogger.Business.Domain.Model
{
    public class TaskReport
    {
        public List<TaskReportItem> Items { get; set; }
        public TaskReportItem FindItem(string TaskKeyword)
        {
            return Items.Find(x => x.TaskKeyword == TaskKeyword);
        }
    }
    public class TaskReportItem
    {
        public string TaskKeyword { get; set; }
        public TaskLogs Logs { get; set; }
        public int TotalMinutes { get; set; }
    }
    public class ReportTarget
    {
        public Period Period { get; set;}
        public List<TaskSearchMethod> TargetTasks { get; set; }
    }
}
