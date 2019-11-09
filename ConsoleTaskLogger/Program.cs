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
                string[] cmd = line.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                if (cmd.Length < 0)
                    continue;

                if(string.Compare(cmd[0], "create", true) == 0)
                {
                    service.CreateTaskLog();
                }
                else if(string.Compare(cmd[0], "all", true) == 0)
                {
                    var logs = service.AllTaskLogs();
                    foreach (var log in logs)
                        Console.WriteLine(log.ToString());
                }
                else if(string.Compare(cmd[0], "changelogname", true) == 0)
                {
                    try
                    {
                        service.ChangeTaskLogName(Int32.Parse(cmd[1]), cmd[2]);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
                else if(string.Compare(cmd[0], "endnow", true) == 0)
                {
                    try
                    {
                        service.EndTaskNow(Int32.Parse(cmd[1]));
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
                else if(string.Compare(cmd[0], "quit", true) == 0)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Unkown");
                }
            }
        }
    }
}
