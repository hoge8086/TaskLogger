using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using TaskLogger.Business.Application;
using TaskLogger.Infrastructure;

namespace TaskLogger
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var context = new TaskLogContext();
            var rep = new TaskLogRepository(context);
            var service = new TaskLogApplicationService(rep);
            this.DataContext = new MainWindowViewModel(service);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
