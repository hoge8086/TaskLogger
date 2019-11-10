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
        public DateTime? _start;
        public DateTime? Start { get => _start; set { _start = TruncateMinute(value); } }
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
            this.Start = start;
        }
        public void StartNow()
        {
            this.Start = DateTime.Now;
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
            Start = null;
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
        public TaskLog Create(DateTime logDateTime)
        {
            var recentlyTaskNames = taskLogRepository.FindWithinPeriod(new PartialPeriod() { StartDay = DateTime.Today, EndDay = DateTime.Today.AddDays(14)}).TaskNamesByRecentlyOrder();
            string defaultTaskName= recentlyTaskNames.Count > 0 ? recentlyTaskNames[0] : "";
            if(logDateTime.Date == DateTime.Today.Date)
            {
                return new TaskLog(defaultTaskName, DateTime.Now);
            }
            var logs = taskLogRepository.FindWithinPeriod(new DatePeriod() { Date = logDateTime });
            return new TaskLog(defaultTaskName, logs.LastEndTime() ?? logDateTime.AddHours(8).AddMinutes(30));
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
        private TaskLogs FindAll(Period period)
        {
            return new TaskLogs { Logs = Logs.Where(x => period.IsIn(x.Start) || period.IsIn(x.End)).ToList() };
        }
        private TaskLogs FindAll(TaskSearchMethod searchMethod)
        {
            return new TaskLogs { Logs = Logs.Where(x => searchMethod.IsMatched(x)).ToList() };
        }

        //public List<string> FindAllTaskName(Period period)
        //{
        //    return Logs.Select(x => x.TaskName).Distinct().ToList();
        //}

        public TaskReport CreateReport(ReportTarget target)
        {
            //Periodが全期間以外の場合は、その他をレポートに追加する????DownTimeはその他に含めるか??
            var logsInPeriod = FindAll(target.Period);
            return new TaskReport() {
                Items = target.TargetTasks
                        .Select(x => {
                            var logs = logsInPeriod.FindAll(x);
                            return new TaskReportItem() {
                                Logs = logs,
                                TaskKeyword = x.TaskKeyword,
                                TotalMinutes = logs.TotalMinutes //メモ:日付をまたぐログはないものとして処理する(計算がやっかいなので)
                            };
                        })
                        .ToList()
                };
        }

        //public TaskLog NewTaskLog()
        //{
        //    var recently = RecentlyTaskNames();
        //    return new TaskLog(recently.Count > 0 ? recently[0] : "");
        //}

        public List<string> TaskNamesByRecentlyOrder()
        {
            return Logs.OrderByDescending(x => x.Start)
                        .Select(x => x.TaskName)
                        .Distinct()
                        .ToList();
        }

        public DateTime? LastEndTime()
        {
            return Logs.Max(x => x.End);
        }
    }
}
