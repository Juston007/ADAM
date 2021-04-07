using System;
using System.Collections.Generic;
using System.Configuration;
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

using ADAM;
using System.Threading;

namespace AdamDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private String adamPort = "";

        private ADAM4017 adam4017;
        private ADAM4150 adam4150;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            //从配置文件当中读取信息
            adamPort = ConfigurationManager.AppSettings["ADAMPort"];
            if (adamPort.Trim() == String.Empty)
            {
                adamPort = "COM2";
            }

            //Init
            ComSettingModel comSettingModel = new ComSettingModel() { AnalogCOM = adamPort, DigitalCOM = adamPort };
            adam4017 = new ADAM4017(comSettingModel);
            adam4150 = new ADAM4150(comSettingModel);

            //Connect （要都打开！）
            adam4150.Connect();
            adam4017.Connect();

            //GetData
            new Thread(() =>
            {
                while (true)
                {
                    //读取数据
                    adam4017.setData();
                    //显示数据
                    this.Dispatcher.Invoke(() =>
                    {
                        lblTemp.Content = ConvertHelper.Temperature(adam4017.Vin0).ToString("F2");
                        lblHumdity.Content = ConvertHelper.Humidity(adam4017.Vin2).ToString("F2");
                        lblFire.Content = adam4150.DI1 ? "有火" : "无火";
                        lblSmork.Content = adam4150.DI2 ? "有烟" : "无烟";
                    });
                }
            }).Start();
        }

        private void Window_Closed_1(object sender, EventArgs e)
        {
            //关闭端口
            adam4150.Close();
            adam4017.Close();
        }
    }
}
