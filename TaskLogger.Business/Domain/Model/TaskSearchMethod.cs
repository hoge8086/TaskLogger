using System.ComponentModel;
using System.Linq;

namespace TaskLogger.Business.Domain.Model
{
    public enum TaskSearchMethodType
    {

        [Description("先頭一致")]
        FirstMatch,
        [Description("完全一致")]
        PerfectMatch,
        [Description("部分一致")]
        PartialMatch,
        [Description("正規表現")]
        RegexpMatch
    }
    public class TaskSearchMethod
    {
        public TaskSearchMethod() { }

        public TaskSearchMethod(
            string TaskKeyword,
            TaskSearchMethodType SearchMethod)
        {
            this.TaskKeyword = TaskKeyword;
            this.SearchMethod = SearchMethod;
        }
        public string TaskKeyword { get; set; }
        public TaskSearchMethodType SearchMethod { get; set; }

        public bool IsMatched(TaskLog log)
        {
            if (SearchMethod == TaskSearchMethodType.PerfectMatch)
                return log.TaskName == TaskKeyword;
            if (SearchMethod == TaskSearchMethodType.FirstMatch)
                return log.TaskName.StartsWith(TaskKeyword);
            if (SearchMethod == TaskSearchMethodType.PartialMatch)
                return log.TaskName.Contains(TaskKeyword);
            if (SearchMethod == TaskSearchMethodType.RegexpMatch)
                return System.Text.RegularExpressions.Regex.IsMatch(log.TaskName, TaskKeyword);

            return false;
        }
    }
}
