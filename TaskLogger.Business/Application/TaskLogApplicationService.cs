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
        public ITaskLogRepository TaskLogRepository { get; set; } = null;
        //public TaskLogApplicationService(ITaskLogRepository taskLogRepository)
        //{
        //    this.taskLogRepository = taskLogRepository;
        //}

        public IList<TaskLog> AllTaskLogs()
        {
            return TaskLogRepository.FindAll().Logs.AsReadOnly();
        }

        public TaskReport CreateReport(ReportTarget reportTarget)
        {
            var logs = TaskLogRepository.FindAll();
            return logs.CreateReport(reportTarget);
        }

        public List<string> AllTaskNames(Period period)
        {
            var logs = TaskLogRepository.FindAll();
            return logs.FindAllTaskName(period);
        }
    }
}
