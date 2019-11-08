using System.Linq;

namespace TaskLogger.Business.Domain.Model
{
    public class TaskSearchMethod
    {
        public enum Method
        {
            FirstMatch,
            PerfectMatch,
            PartialMatch,
            RegexpMatch
        }
        public string TaskKeyword { get; set; }
        public Method searchMethod { get; set; }

        public bool IsMatched(TaskLog log)
        {
            if (searchMethod == Method.PerfectMatch)
                return log.TaskName == TaskKeyword;
            if (searchMethod == Method.FirstMatch)
                return log.TaskName.StartsWith(TaskKeyword);
            if (searchMethod == Method.PartialMatch)
                return log.TaskName.Contains(TaskKeyword);
            if (searchMethod == Method.RegexpMatch)
                return System.Text.RegularExpressions.Regex.IsMatch(log.TaskName, TaskKeyword);

            return false;
        }
    }
}
