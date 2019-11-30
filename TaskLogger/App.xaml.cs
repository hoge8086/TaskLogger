﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
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
        public TaskLogApplicationService TaskLogApplicationService{ get; set; }

        void App_Startup(object sender, StartupEventArgs e)
        {
            TaskLogContext = new TaskLogContext();
            TaskLogRepository = new TaskLogRepository(TaskLogContext);
            TaskLogApplicationService = new TaskLogApplicationService(TaskLogRepository);
        }
    }
}
