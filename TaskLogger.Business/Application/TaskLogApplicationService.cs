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
        public ITaskLogRepository taskLogRepository;

        public TaskLogApplicationService(ITaskLogRepository taskLogRepository)
        {
            this.taskLogRepository = taskLogRepository;
        }
        public void CreateTaskLog()
        {
            var newTaskLog = new TaskLog();
            taskLogRepository.Add(newTaskLog);
            taskLogRepository.Save();
        }

        public void ChangeTaskLogName(int logId, string taskName)
        {
            var log = taskLogRepository.FindByID(logId);
            if (log == null)
                throw new KeyNotFoundException();

            log.ChangeTaskName(taskName);
            taskLogRepository.Save();
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

        public TaskReport CreateReport(ReportTarget reportTarget)
        {
            var logs = taskLogRepository.FindAll();
            return logs.CreateReport(reportTarget);
        }

        public List<string> AllTaskNames(Period period)
        {
            var logs = taskLogRepository.FindAll();
            return logs.FindAllTaskName(period);
        }
    }
}
