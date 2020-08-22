using System.Collections.ObjectModel;
using System.IO.Ports;

namespace NvtTxCaliTool
{
    class ComPortConfigViewModel : BaseViewModel
    {
        private string selectedPort;
        public string SelectedPort
        {
            get => this.selectedPort;
            set => SetProperty(ref this.selectedPort, value);
        }
        public ObservableCollection<string> PortList { get; private set; }
            = new ObservableCollection<string>();
        private bool btnConnect;//= false;
        public bool BtnConnect
        {
            get => this.btnConnect;
            set
            {
                SetProperty(ref this.btnConnect, value, nameof(this.BtnConnectTxt));
                OnPropertyChanged(nameof(this.PortEnable));
            }
        }
        public string BtnConnectTxt => this.btnConnect ? "Disconnect" : "Connect";
        public bool PortEnable => !this.btnConnect;

        public void RefreshPortList()
        {
            this.PortList.Clear();
            string[] portNameList = SerialPort.GetPortNames();
            foreach (string p in portNameList)
            {
                this.PortList.Add(p);
            }
            this.SelectedPort = this.PortList.Count > 0 ? this.PortList[0] : string.Empty;
        }
    }
}
