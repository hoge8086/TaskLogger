using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskLogger.Business.Domain.Model;

namespace TaskLogger.Business.Application
{
    public class TaskLogApplicationService
    {
        private ITaskLogRepository taskLogRepository;
        private IReportTargetRepository reportTargetRepository;
        private TaskLogFactory taskLogFactory; 

        public TaskLogApplicationService(ITaskLogRepository taskLogRepository, IReportTargetRepository reportTargetRepository)
        {
            this.taskLogRepository = taskLogRepository;
            this.taskLogFactory = new TaskLogFactory(taskLogRepository);
            this.reportTargetRepository = reportTargetRepository;
        }
        public TaskLog CreateTaskLog(DateTime logDate)
        {
            //var recentlyLogs = taskLogRepository.FindWithinPeriod(RecentlyPeriod);
            //var newTaskLog = new TaskLog(recentlyLogs);
            var newTaskLog = taskLogFactory.Create(logDate);
            taskLogRepository.Add(newTaskLog);
            taskLogRepository.Save();
            return newTaskLog;
        }

        public void ChangeTaskLogName(int logId, string taskName)
        {
            var log = taskLogRepository.FindByID(logId);
            if (log == null)
                throw new KeyNotFoundException();

            log.ChangeTaskName(taskName);
            taskLogRepository.Save();
        }

        public void SaveReportTarget(List<ReportTarget> targets)
        {
            reportTargetRepository.Save(targets);
        }

        public void ChangeTaskLogStart(int logId, DateTime start)
        {
            var log = taskLogRepository.FindByID(logId);
            if (log == null)
                throw new KeyNotFoundException();

            log.ChangeStart(start);
            taskLogRepository.Save();
        }
        public void ChangeTaskLogDownTime(int logId, int DownTimeMinutes)
        {
            var log = taskLogRepository.FindByID(logId);
            if (log == null)
                throw new KeyNotFoundException();

            log.ChangeDownTime(DownTimeMinutes);
            taskLogRepository.Save();
        }

        public void StartTaskNow(int logId)
        {
            var log = taskLogRepository.FindByID(logId);
            if (log == null)
                throw new KeyNotFoundException();

            log.StartNow();
            taskLogRepository.Save();
        }

        public void DeleteTaskLog(int logId)
        {
            var log = taskLogRepository.FindByID(logId);
            taskLogRepository.Remove(log);
            taskLogRepository.Save();
        }

        public void EndTaskNow(int logId)
        {
            var log = taskLogRepository.FindByID(logId);
            if (log == null)
                throw new KeyNotFoundException();

            log.EndNow();
            taskLogRepository.Save();
        }

        public void ChangeTaskLogEnd(int logId, DateTime end)
        {
            var log = taskLogRepository.FindByID(logId);
            if (log == null)
                throw new KeyNotFoundException();

            log.ChangeEnd(end);
            taskLogRepository.Save();
        }

        public IList<TaskLog> AllTaskLogs()
        {
            return taskLogRepository.FindAll().Logs.AsReadOnly();
        }

        public IList<ReportTarget> AllReportTargets()
        {
            return reportTargetRepository.FindAll();
        }

        public IList<TaskLog> TaskLogs(Period period)
        {
            return taskLogRepository.FindWithinPeriod(period).Logs.AsReadOnly();
        }

        public TaskLog TaskLog(int id)
        {
            return taskLogRepository.FindByID(id);
        }

        //public TaskReport CreateReportForAllTask(Period period)
        //{
        //    var logs = taskLogRepository.FindWithinPeriod(period);
        //    return logs.CreateReport();
        //}

        //public TaskReport CreateReport(Period period, List<TaskSearchMethod> targets)
        //{
        //    var logs = taskLogRepository.FindWithinPeriod(period);
        //    return logs.CreateReport(targets);
        //}

        public TaskReport CreateReport(Period period, TaskSpecify taskSpecify)
        {
            var logs = taskLogRepository.FindWithinPeriod(period);
            return logs.CreateReport(taskSpecify.CreateTaskSearchMethods(logs));
        }

        public List<string> RecentlyTaskNames()
        {
            var logs = taskLogRepository.FindWithinPeriod(new PartialPeriod() { EndDay = DateTime.Today, StartDay = DateTime.Today.AddDays(-30)});
            return logs.TaskNamesByRecentlyOrder();
        }
    }
}
