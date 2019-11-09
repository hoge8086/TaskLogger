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

        public void ChangeTaskLog(TaskLog newTaskLog)
        {
            taskLogRepository.Add(newTaskLog);
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
