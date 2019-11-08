using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskLogger.Business.Domain.Model;
using TaskLogger.Business.Application;

namespace TaskLogger
{
    public class MainWindowViewModel
    {
        public List<TaskLog> TaskLogs { get; set; }

        public MainWindowViewModel()
        {
            TaskLogs = new MockTaskLogRepository().FindAll().Logs;
        }
    }
}
