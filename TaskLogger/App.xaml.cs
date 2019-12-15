using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using TaskLogger.View;

using TaskLogger.Business.Application;
using TaskLogger.Infrastructure;
using TaskLogger.ViewModel;

namespace TaskLogger
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        public TaskLogContext TaskLogContext { get; set; }
        public TaskLogRepository TaskLogRepository { get; set; }
        public ReportTargetRepository ReportTargetRepository { get; set; }
        public TaskLogApplicationService TaskLogApplicationService{ get; set; }
        public ToastCurrentTaskService ToastService { get; set; }

        void App_Startup(object sender, StartupEventArgs e)
        {
            TaskLogContext = new TaskLogContext();
            TaskLogRepository = new TaskLogRepository(TaskLogContext);
            ReportTargetRepository = new ReportTargetRepository();
            TaskLogApplicationService = new TaskLogApplicationService(TaskLogRepository, ReportTargetRepository);

            //TODO:時刻を設定化
            ToastService = new ToastCurrentTaskService(45);
            ToastService.StartMonitor();
        }

        private void Application_Deactivated(object sender, EventArgs e)
        {
            ToastService.DeactiveNow();
        }
    }
}
