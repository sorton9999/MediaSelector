using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BankClientWindow
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            string ip = String.Empty;
            int port = CliServLib.CliServDefaults.DfltPort;
            int cycles = 20;
            int delayMs = 5000;
            for (int i = 0; i < e.Args.Length; ++i)
            {
                if (e.Args[i] == "-p")
                {
                    port = Convert.ToInt32(e.Args[i + 1]);
                }
                else if (e.Args[i] == "-h")
                {
                    ip = e.Args[i + 1];
                }
                else if (e.Args[i] == "-c")
                {
                    cycles = Convert.ToInt32(e.Args[i + 1]);
                }
                else if (e.Args[i] == "-d")
                {
                    delayMs = Convert.ToInt32(e.Args[i + 1]);
                }
            }
            MainWindow window = new BankClientWindow.MainWindow(ip, port, delayMs, cycles);
            window.AddOutputMsg("Connecting IP: " + ip + " port: " + port + " Delay: " + delayMs + " Max Cycles: " + cycles);
            window.Show();
            //BankClient client = new BankClient();
            //bool ret = new BankClient().Connect(ip, 7001, delayMs, cycles);
        }
    }
}
