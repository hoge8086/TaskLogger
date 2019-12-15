using System;
using System.Linq;

using Windows.UI.Notifications;
using System.Windows.Threading;

namespace TaskLogger
{
    //TODO:DDD的に扱うための検討
    public class ToastCurrentTaskService
    {
        private DateTime lastConfirmedDateTime;
        private DispatcherTimer timer;
        private double intervalMinutes;

        public ToastCurrentTaskService(double intervalMinutes)
        {
            lastConfirmedDateTime = DateTime.Now;
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
        public void StartMonitor()
        {
            timer.Start();
        }
        private void WatchCurrentTask(object sender, EventArgs e)
        {
            var currentTask = ((App)App.Current).TaskLogApplicationService.GetCurrentWorkingTask();

            //[基本的に通知は、intervalMinutes分毎に行う、その期間内に]
            //[作業中のタスクの開始時刻が最後の通知時刻以降に更新された場合のみ、通知タイマをリセットする]
            if (currentTask != null && currentTask.Start >= lastConfirmedDateTime)
                lastConfirmedDateTime = currentTask.Start;

            if((DateTime.Now -  lastConfirmedDateTime).TotalMinutes >= intervalMinutes)
            {
                if(currentTask == null)
                {
                    Toast("現在のタスク：\n タスクが開始されていません");

                }else
                {
                    Toast("現在のタスク：\n " + currentTask.TaskName + "\n " + currentTask.Start.ToString("t") + "～");
                }
                lastConfirmedDateTime = DateTime.Now;
            }
        }
    }
}
