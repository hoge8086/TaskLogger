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
using System.Windows.Shapes;
using TaskLogger.Business.Application;
using TaskLogger.ViewModel;

namespace TaskLogger.View
{
    /// <summary>
    /// ReportWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class ReportWindow : Window
    {
        public ReportWindow()
        {
            InitializeComponent();
            this.DataContext = new ReportWindowViewModel(((App)App.Current).TaskLogApplicationService);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //TODO:これに直す
            //https://codeday.me/jp/qa/20190208/209713.html
            var vm = this.DataContext as ReportWindowViewModel;
            if (vm != null)
                vm.CloseCommand.Execute(null);

        }
    }
}
