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

namespace BankClientWindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string _ip = String.Empty;
        int _port = 0;
        BankClientControl.BankClient client;

        public MainWindow(string ip, int port, int delayMs, int cycles)
        {
            InitializeComponent();
            _ip = ip;
            _port = port;
            client = new BankClientControl.BankClient();
            bankControl.BankClient = client;
            bool ret = client.Connect(_ip, _port, delayMs, cycles);
            if (ret)
            {
                client.TcpClient().Start();
            }
        }

        public void AddOutputMsg(string msg)
        {
            string msgOut = bankControl.ReceiveMsgs;
            msgOut += msg + Environment.NewLine;
        }

        public void ClearOutput()
        {
            bankControl.ReceiveMsgs = String.Empty;
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            client.Close();
        }
    }
}
