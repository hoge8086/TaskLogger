using System.Collections.Generic;

namespace TaskLogger.Business.Domain.Model
{
    public class TaskReport
    {
        public List<TaskReportItem> Items { get; set; }
    }
    public class TaskReportItem
    {
        public TaskSearchMethod TaskSearchMethod { get; set; }
        public TaskLogs Logs { get; set; }
        public int TotalMinutes { get; set; }
    }

    public class ReportTarget
    {
        public TaskSearchMethod TaskSearchMethod { get;  }
        public string Keyword { get; }

        public ReportTarget(TaskSearchMethod TaskSearchMethod, string Keyword)
        {
            this.TaskSearchMethod = TaskSearchMethod;
            this.Keyword = Keyword;
        }
    }
}
