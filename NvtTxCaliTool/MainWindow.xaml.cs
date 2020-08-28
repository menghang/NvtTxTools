using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace NvtTxCaliTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IDisposable
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
            if (!tmpView.PortConnect)
            {
                if (!string.IsNullOrEmpty(tmpView.SelectedPort))
                {
                    this.uart = new UartUtil(tmpView.SelectedPort);
                    this.uart.UartMsgReceived += UartPort_UartMsgReceived;
                    if (this.uart.OpenPort())
                    {
                        tmpView.PortConnect = true;
                    }
                }
            }
            else
            {
                if (this.uart != null)
                {
                    this.uart.ClosePort();
                    this.uart.Dispose();
                }
                tmpView.PortConnect = false;
            }
        }

        private void UartPort_UartMsgReceived(object sender, UartUtil.UartMsgEventArgs e)
        {
            Queue<UartMsgModel> msgQueue = e.MsgQueue;
            while ((msgQueue.Count > 0) && (!this.view.CaliDataView.AllReceived))
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
            this.view.CaliDataView.QRCode = "Not Available";
            await RunCaliTest().ConfigureAwait(false);
        }

        private async Task RunCaliTest()
        {
            DateTime dt0 = DateTime.Now;
            this.view.CaliDataView.Reset();
            this.uart.ClearBuf();
            this.uart.StartReceiving();
            this.uart.SendCaliCmd();
            while ((!this.view.CaliDataView.AllReceived) && (dt0.AddSeconds(8) > DateTime.Now))
            {
                await Task.Run(() => Thread.Sleep(100)).ConfigureAwait(false);
            }
            this.uart.StopReceiving();
            this.view.CaliDataView.UpdateResult();
            this.view.CaliDataView.SaveData();
            double period = (DateTime.Now - dt0).TotalMilliseconds;
            Trace.WriteLine("Time Escape: " + period.ToString("0.00", CultureInfo.InvariantCulture));
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

        private void MenuItemAbout_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow about = new AboutWindow();
            about.ShowDialog();
        }

        private string inputBuf = string.Empty;

        private async void Window_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (this.view.ComPortConfigView.PortConnect)
            {
                char[] tmp = e.Text.ToCharArray();
                foreach (char c in tmp)
                {
                    if (c == '\r')
                    {
                        this.view.CaliDataView.QRCode = this.inputBuf;
                        this.inputBuf = string.Empty;
                        if (string.IsNullOrEmpty(this.view.CaliDataView.QRCode))
                        {
                            this.view.CaliDataView.QRCode = "Not Available";
                        }
                        await RunCaliTest().ConfigureAwait(false);
                    }
                    else
                    {
                        this.inputBuf += c;
                    }
                }
            }
        }


        #region IDisposable Support
        private bool disposedValue;// = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    // dispose managed state (managed objects).
                }

                // free unmanaged resources (unmanaged objects) and override a finalizer below.
                // set large fields to null.
                if (this.uart != null)
                {
                    this.uart.Dispose();
                }

                this.disposedValue = true;
            }
        }

        // override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        ~MainWindow()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}
