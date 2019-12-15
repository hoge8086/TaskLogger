using System;
using System.Linq;

using Windows.UI.Notifications;
using System.Windows.Threading;

namespace TaskLogger
{
    //TODO:DDD的に扱うための検討
    public class ToastCurrentTaskService
    {
        private DateTime lastDeactiveDateTime;
        private DispatcherTimer timer;
        private double intervalMinutes;

        public ToastCurrentTaskService(double intervalMinutes)
        {
            DeactiveNow();
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 30);
            timer.Tick += new EventHandler(WatchCurrentTask);
            this.intervalMinutes = intervalMinutes;
        }

        public void Toast(string message)
        {
            var type = ToastTemplateType.ToastText01;
            var content = ToastNotificationManager.GetTemplateContent(type);
            var text = content.GetElementsByTagName("text").First();
            text.AppendChild(content.CreateTextNode(message));
            var notifier = ToastNotificationManager.CreateToastNotifier("Microsoft.Windows.Computer");
            notifier.Show(new ToastNotification(content));
        }
        public void DeactiveNow()
        {
            lastDeactiveDateTime = DateTime.Now;
        }
        public void StartMonitor()
        {
            timer.Start();
        }
        private void WatchCurrentTask(object sender, EventArgs e)
        {
            if((DateTime.Now -  lastDeactiveDateTime).TotalMinutes >= intervalMinutes)
            {
                var currentTask = ((App)App.Current).TaskLogApplicationService.GetCurrentWorkingTask();
                if(currentTask == null)
                {
                    Toast("現在のタスク：\n タスクが開始されていません");

                }else
                {
                    Toast("現在のタスク：\n " + currentTask.TaskName + "\n " + currentTask.Start.ToString("t") + "～");
                }
                DeactiveNow();
            }
        }
    }
}
