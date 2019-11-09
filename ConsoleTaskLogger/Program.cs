using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskLogger.Business.Application;
using TaskLogger.Infrastructure;

namespace ConsoleTaskLogger
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new TaskLogContext();
            var rep = new TaskLogRepository(context);
            var service = new TaskLogApplicationService(rep);
            for(; ; )
            {
                Console.Write(">");
                var line = Console.ReadLine();

                if(string.Compare(line, "create", true) == 0)
                {
                    service.CreateTaskLog();

                }else if(string.Compare(line, "all", true) == 0)
                {
                    var logs = service.AllTaskLogs();
                    foreach (var log in logs)
                        Console.WriteLine(log.ToString());
                }else if(string.Compare(line, "quit", true) == 0)
                {
                    break;
                }
            }
        }
    }
}
