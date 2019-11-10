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
        public DateTime? End
        {
            get
            {
                if (Start == null || WorkingMinutes == null)
                    return null;

                return Start.Value.AddMinutes((WorkingMinutes ?? 0) + DownTimeMinutes);
            }
        }
        public void ChangeEnd(DateTime? end)
        {
            end = TruncateMinute(end);
            WorkingMinutes = (int)((end - Start).Value.TotalMinutes) - DownTimeMinutes;
        }
        public void ChangeDownTime(int downTimeMinutes)
        {
            int diff = this.DownTimeMinutes - downTimeMinutes;
            this.DownTimeMinutes = downTimeMinutes;
            this.WorkingMinutes += diff;
        }
        public int DownTimeMinutes { get; set; }
        public int? WorkingMinutes { get; set; }

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
            ChangeEnd(DateTime.Now);
        }
        public override string ToString()
        {
            return Id.ToString() + "、" + TaskName + "、期間:" + Start.ToString() + "-" + End.ToString() + "、中断時間:" + DownTimeMinutes.ToString() + "、作業時間:" + WorkingMinutes?.ToString();
        }

        public static DateTime? TruncateMinute(DateTime? dt)
        {
            if (dt == null) return null;
            return new DateTime(dt.Value.Year, dt.Value.Month, dt.Value.Day, dt.Value.Hour, dt.Value.Minute, 0);
        }

        public TaskLog()
        {
            Id = 0;
            TaskName = "";
            Start = DateTime.Now;
            //End = null;
            DownTimeMinutes = 0;
            WorkingMinutes = null;
        }

        public TaskLog(TaskLogs logs)
        {
            Id = 0;
            var recentTaskNames = logs.TaskNamesByRecentlyOrder();
            TaskName = recentTaskNames.Count > 0 ? recentTaskNames[0] : "";
            Start = DateTime.Now;
            //End = null;
            DownTimeMinutes = 0;
            WorkingMinutes = null;
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
    }
}
