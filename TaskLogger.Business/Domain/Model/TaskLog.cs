using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskLogger.Business.Domain.Model
{
    [Table("TaskLogs")]
    public class TaskLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string TaskName { get; set; }
        public DateTime _start;
        public DateTime Start { get => _start; set { _start = TruncateMinute(new Nullable<DateTime>(value)).Value; } }
        public DateTime? _end;
        public DateTime? End { get => _end ; set { _end = TruncateMinute(value); } }
        public int DownTimeMinutes { get; set; }
        public int? WorkingMinutes
        {
            get
            {
                if (_start == null || _end == null)
                    return null;

                return (int)((_end - _start).Value.TotalMinutes) - DownTimeMinutes;
            }
        }
        //{
        //    get
        //    {
        //        if (Start == null || WorkingMinutes == null)
        //            return null;

        //        return Start.Value.AddMinutes((WorkingMinutes ?? 0) + DownTimeMinutes);
        //    }
        //}
        public void ChangeEnd(DateTime? end)
        {
            if (end != null && Start.Date != end.Value.Date)
                throw new InvalidOperationException("日にちをまたぐタスクログは作成できません.");

            if (end != null && Start > end.Value)
                throw new InvalidOperationException("終了時刻は開始時刻より早くなるようにしてください.");

            End = end;
        }
        public void ChangeDownTime(int downTimeMinutes)
        {
            DownTimeMinutes = downTimeMinutes;
        }

        public void ChangeTaskName(string taskName)
        {
            //トリムする
            this.TaskName = taskName;
        }

        public void ChangeStart(DateTime start)
        {
            if (End!= null && start.Date != End.Value.Date)
                throw new InvalidOperationException("日にちをまたぐタスクログは作成できません.");

            this.Start = start;
        }
        public void StartNow()
        {
            ChangeEnd(DateTime.Now);
        }
        public void EndNow()
        {
            this.End = DateTime.Now;
        }
        public override string ToString()
        {
            return Id.ToString() + "、" + TaskName + "、期間:" + Start.ToString() + "-" + End.ToString() + "、中断時間:" + DownTimeMinutes.ToString() + "、作業時間:" + WorkingMinutes?.ToString();
        }

        private static DateTime? TruncateMinute(DateTime? dt)
        {
            if (dt == null) return null;
            return new DateTime(dt.Value.Year, dt.Value.Month, dt.Value.Day, dt.Value.Hour, dt.Value.Minute, 0);
        }

        public TaskLog()
        {
            Id = 0;
            TaskName = "";
            Start = DateTime.Now;
            End = null;
            DownTimeMinutes = 0;
        }

        public TaskLog(string taskName, DateTime start)
        {
            Id = 0;
            TaskName = taskName;
            Start = start;
            End = null;
            DownTimeMinutes = 0;
        }
    }

    public class TaskLogFactory
    {
        private ITaskLogRepository taskLogRepository;
        public TaskLogFactory(ITaskLogRepository taskLogRepository)
        {
            this.taskLogRepository = taskLogRepository;
        }
        public TaskLog Create(DateTime logDate)
        {
            var recentlyTaskNames = taskLogRepository.FindWithinPeriod(new PartialPeriod() { EndDay = DateTime.Today, StartDay = DateTime.Today.AddDays(-14)}).TaskNamesByRecentlyOrder();
            string defaultTaskName= recentlyTaskNames.Count > 0 ? recentlyTaskNames[0] : "";
            DateTime defDateTime = logDate.Date.AddHours(8).AddMinutes(30);
            if(logDate.Date == DateTime.Today.Date)
            {
                defDateTime = DateTime.Now;
            }
            var logs = taskLogRepository.FindWithinPeriod(new DatePeriod() { Date = logDate.Date });
            return new TaskLog(defaultTaskName, logs.LastTime() ?? defDateTime);
        }
    }

    public class TaskLogs
    {
        public List<TaskLog> Logs { get; set; }

        public int TotalMinutes
        {
            get
            {
                return Logs.Sum(x => x.WorkingMinutes ?? 0);
            }
        }
        private TaskLogs FindAll(TaskSearchMethod searchMethod)
        {
            return new TaskLogs { Logs = Logs.Where(x => searchMethod.IsMatched(x)).ToList() };
        }
        //public TaskReport CreateReport()
        //{
        //    var targets = Logs
        //                    .Select(x => x.TaskName)
        //                    .Distinct()
        //                    .Select(x => new TaskSearchMethod() { TaskKeyword = x, SearchMethod = TaskSearchMethodType.PerfectMatch })
        //                    .ToList();
        //    return CreateReport(targets);
        //}
        public TaskReport CreateReport(List<TaskSearchMethod> targets)
        {
            return new TaskReport()
            {
                Items = targets.Select(x =>
                {
                    var logs = FindAll(x);
                    return new TaskReportItem()
                    {
                        Logs = logs,
                        //TaskKeyword = x.TaskKeyword,
                        TaskSearchMethod = x,
                        TotalMinutes = logs.TotalMinutes
                    };
                }).ToList()
            };
        }

        public List<string> TaskNamesByRecentlyOrder()
        {
            return Logs.OrderByDescending(x => x.Start)
                        .Select(x => x.TaskName)
                        .Distinct()
                        .ToList();
        }

        public DateTime? LastTime()
        {
            return Logs.Max(x => (x.End > x.Start ? x.End : x.Start));
        }
    }
}
