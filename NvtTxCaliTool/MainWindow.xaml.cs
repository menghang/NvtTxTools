using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace NvtTxCaliTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel view;
        private SettingsWindowViewModel settingsView;
        private UartUtil uart;
        private const string DefaultConfigFile = "Config.json";

        public MainWindow()
        {
            InitializeComponent();
            this.view = new MainWindowViewModel();
            this.DataContext = this.view;
        }

        private void ButtonRefreshComPortList_Click(object sender, RoutedEventArgs e)
        {
            this.view.ComPortConfigView.RefreshPortList();
        }

        private void ButtonConnectComPort_Click(object sender, RoutedEventArgs e)
        {
            ComPortConfigViewModel tmpView = this.view.ComPortConfigView;
            if (!tmpView.BtnConnect)
            {
                this.uart = new UartUtil(tmpView.SelectedPort);
                this.uart.UartMsgReceived += UartPort_UartMsgReceived;
                if (this.uart.OpenPort())
                {
                    tmpView.BtnConnect = true;
                }
            }
            else
            {
                if (this.uart != null)
                {
                    this.uart.ClosePort();
                    this.uart.Dispose();
                }
                tmpView.BtnConnect = false;
            }
        }

        private void UartPort_UartMsgReceived(object sender, UartUtil.UartMsgEventArgs e)
        {
            Queue<UartMsgModel> msgQueue = e.MsgQueue;
            while (msgQueue.Count > 0)
            {
                UartMsgModel msg = msgQueue.Dequeue();
                switch (msg.Type)
                {
                    case UartMsgModel.MsgType.CALIINPUT:
                    case UartMsgModel.MsgType.CALIVSENS:
                    case UartMsgModel.MsgType.CALITEMP:
                    case UartMsgModel.MsgType.CALIVCOIL:
                    case UartMsgModel.MsgType.CALIQ:
                    case UartMsgModel.MsgType.CALIISENS:
                        this.view.CaliDataView.LoadMsg(msg);
                        break;
                    case UartMsgModel.MsgType.UNKNOWN:
                    default:
                        break;
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.view.ComPortConfigView.RefreshPortList();
            this.settingsView = new SettingsWindowViewModel();
            this.settingsView.LoadConfig(DefaultConfigFile);
            this.view.CaliDataView.LoadSettings(this.settingsView);
        }

        private async void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            this.uart.ClearBuf();
            this.uart.StartReceiving();
            this.uart.SendCaliCmd();
            this.view.CaliDataView.Reset();
            int count = 0;
            while ((!this.view.CaliDataView.AllReceived) && (count < 10))
            {
                await Task.Delay(1000).ConfigureAwait(false);
                count++;
            }
            this.uart.StopReceiving();
            this.view.CaliDataView.UpdateResult();

        }

        private void MenuItemSettings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow setting = new SettingsWindow(DefaultConfigFile);
            if (setting.ShowDialog() == true)
            {
                this.settingsView = setting.View;
                this.settingsView.SaveConfig(DefaultConfigFile);
                this.view.CaliDataView.LoadSettings(this.settingsView);
            }
        }
    }
}
