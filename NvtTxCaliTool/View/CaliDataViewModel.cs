using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Media;

namespace NvtTxCaliTool
{
    class CaliDataViewModel : BaseViewModel
    {
        private static readonly SolidColorBrush ColorPass = new SolidColorBrush(Colors.LightGreen);
        private static readonly SolidColorBrush ColorFail = new SolidColorBrush(Colors.LightPink);
        private static readonly SolidColorBrush ColorTesting = new SolidColorBrush(Colors.LightGoldenrodYellow);
        private const string DataFolder = "Data";

        private SettingsWindowViewModel settings;

        public string Product
        {
            get => this.settings == null ? string.Empty : this.settings.Product;
        }

        private string qrCode = string.Empty;
        public string QRCode
        {
            get => this.qrCode;
            set => SetProperty(ref this.qrCode, value);
        }

        private string caliInput = string.Empty;
        public string CaliInput
        {
            get => this.caliInput;
            set => SetProperty(ref this.caliInput, value);
        }
        private bool? caliInputPass;//= null;
        public bool? CaliInputPass
        {
            get => this.caliInputPass;
            set => SetProperty(ref this.caliInputPass, value, nameof(this.CaliInputColor));
        }

        public Brush CaliInputColor
        {
            get => this.caliInputPass == null ? ColorTesting : this.caliInputPass == true ? ColorPass : ColorFail;
        }
        private string caliVsens = string.Empty;
        public string CaliVsens
        {
            get => this.caliVsens;
            set => SetProperty(ref this.caliVsens, value);
        }
        private bool? caliVsensPass;//= null;
        public bool? CaliVsensPass
        {
            get => this.caliVsensPass;
            set => SetProperty(ref this.caliVsensPass, value, nameof(this.CaliVsensColor));
        }
        public Brush CaliVsensColor
        {
            get => this.caliVsensPass == null ? ColorTesting : this.caliVsensPass == true ? ColorPass : ColorFail;
        }

        private string caliTemp = string.Empty;
        public string CaliTemp
        {
            get => this.caliTemp;
            set => SetProperty(ref this.caliTemp, value);
        }
        private bool? caliTempPass;//= null;
        public bool? CaliTempPass
        {
            get => this.caliTempPass;
            set => SetProperty(ref this.caliTempPass, value, nameof(this.CaliTempColor));
        }
        public Brush CaliTempColor
        {
            get => this.caliTempPass == null ? ColorTesting : this.caliTempPass == true ? ColorPass : ColorFail;
        }

        private string caliVcoil = string.Empty;
        public string CaliVcoil
        {
            get => this.caliVcoil;
            set => SetProperty(ref this.caliVcoil, value);
        }
        private bool? caliVcoilPass;//= null;
        public bool? CaliVcoilPass
        {
            get => this.caliVcoilPass;
            set => SetProperty(ref this.caliVcoilPass, value, nameof(this.CaliVcoilColor));
        }
        public Brush CaliVcoilColor
        {
            get => this.caliVcoilPass == null ? ColorTesting : this.caliVcoilPass == true ? ColorPass : ColorFail;
        }

        private string caliQ = string.Empty;
        public string CaliQ
        {
            get => this.caliQ;
            set => SetProperty(ref this.caliQ, value);
        }
        private bool? caliQPass;//= null;
        public bool? CaliQPass
        {
            get => this.caliQPass;
            set => SetProperty(ref this.caliQPass, value, nameof(this.CaliQColor));
        }
        public Brush CaliQColor
        {
            get => this.caliQPass == null ? ColorTesting : this.caliQPass == true ? ColorPass : ColorFail;
        }

        private string caliIsens = string.Empty;
        public string CaliIsens
        {
            get => this.caliIsens;
            set => SetProperty(ref this.caliIsens, value);
        }
        private bool? caliIsensPass;//= null;
        public bool? CaliIsensPass
        {
            get => this.caliIsensPass;
            set => SetProperty(ref this.caliIsensPass, value, nameof(this.CaliIsensColor));
        }
        public Brush CaliIsensColor
        {
            get => this.caliIsensPass == null ? ColorTesting : this.caliIsensPass == true ? ColorPass : ColorFail;
        }

        private string caliResult = string.Empty;
        public string CaliResult
        {
            get => this.caliResult;
            set => SetProperty(ref this.caliResult, value);
        }
        private bool? caliResultPass;//= null;
        public bool? CaliResultPass
        {
            get => this.caliResultPass;
            set => SetProperty(ref this.caliResultPass, value, nameof(this.CaliResultColor));
        }
        public Brush CaliResultColor
        {
            get => this.caliResultPass == null ? ColorTesting : this.caliResultPass == true ? ColorPass : ColorFail;
        }

        private bool inputReceived;
        private bool vsensReceived;
        private bool tempReceived;
        private bool vcoilReceived;
        private bool isensReceived;
        private bool qReceived;
        public bool AllReceived { private set; get; }
        public void Reset()
        {
            this.CaliInput = string.Empty;
            this.inputReceived = false;
            this.CaliInputPass = null;

            this.CaliVsens = string.Empty;
            this.vsensReceived = false;
            this.CaliVsensPass = null;

            this.CaliTemp = string.Empty;
            this.tempReceived = false;
            this.CaliTempPass = null;

            this.CaliVcoil = string.Empty;
            this.vcoilReceived = false;
            this.CaliVcoilPass = null;

            this.CaliQ = string.Empty;
            this.qReceived = false;
            this.CaliQPass = null;

            this.CaliIsens = string.Empty;
            this.isensReceived = false;
            this.CaliIsensPass = null;

            this.CaliResult = "Testing";
            this.AllReceived = false;
            this.CaliResultPass = null;
        }
        public void LoadMsg(UartMsgModel msg)
        {
            switch (msg.Type)
            {
                case UartMsgModel.MsgType.CALIINPUT:
                    if (!this.inputReceived)
                    {
                        this.CaliInput = msg.Content[0];
                        this.inputReceived = true;
                        this.CaliInputPass = CheckPass(this.CaliInput, this.settings.InputLow, this.settings.InputHigh);
                    }
                    break;
                case UartMsgModel.MsgType.CALIVSENS:
                    if (!this.vsensReceived)
                    {
                        this.CaliVsens = msg.Content[0];
                        this.vsensReceived = true;
                        this.CaliVsensPass = CheckPass(this.CaliVsens, this.settings.VsensLow, this.settings.VsensHigh);
                    }
                    break;
                case UartMsgModel.MsgType.CALITEMP:
                    if (!this.tempReceived)
                    {
                        this.CaliTemp = msg.Content[0];
                        this.tempReceived = true;
                        this.CaliTempPass = CheckPass(this.CaliTemp, this.settings.TempLow, this.settings.TempHigh);
                    }
                    break;
                case UartMsgModel.MsgType.CALIVCOIL:
                    if (!this.vcoilReceived)
                    {
                        this.CaliVcoil = msg.Content[0];
                        this.vcoilReceived = true;
                        this.CaliVcoilPass = CheckPass(this.CaliVcoil, this.settings.VcoilLow, this.settings.VcoilHigh);
                    }
                    break;
                case UartMsgModel.MsgType.CALIQ:
                    if (!this.qReceived)
                    {
                        this.CaliQ = msg.Content[0];
                        this.qReceived = true;
                        this.CaliQPass = CheckPass(this.CaliQ, this.settings.QLow, this.settings.QHigh);
                    }
                    break;
                case UartMsgModel.MsgType.CALIISENS:
                    if (!this.isensReceived)
                    {
                        this.CaliIsens = msg.Content[0];
                        this.isensReceived = true;
                        this.CaliIsensPass = CheckPass(this.CaliIsens, this.settings.IsensLow, this.settings.IsensHigh);
                    }
                    break;
                default:
                    break;
            }
            if (this.inputReceived && this.vsensReceived && this.tempReceived
                && this.vcoilReceived && this.isensReceived && this.qReceived)
            {
                this.AllReceived = true;
            }
        }

        public void UpdateResult()
        {
            if (!this.inputReceived)
            {
                this.CaliInput = "No Data";
                this.CaliInputPass = false;
            }
            if (!this.vsensReceived)
            {
                this.CaliVsens = "No Data";
                this.CaliVsensPass = false;
            }
            if (!this.tempReceived)
            {
                this.CaliTemp = "No Data";
                this.CaliTempPass = false;
            }
            if (!this.vcoilReceived)
            {
                this.CaliVcoil = "No Data";
                this.CaliVcoilPass = false;
            }
            if (!this.isensReceived)
            {
                this.CaliIsens = "No Data";
                this.CaliIsensPass = false;
            }
            if (!this.qReceived)
            {
                this.CaliQ = "No Data";
                this.CaliQPass = false;
            }
            if ((this.CaliInputPass == true) && (this.CaliVsensPass == true)
                && (this.CaliTempPass == true) && (this.CaliVcoilPass == true)
                && (this.CaliIsensPass == true) && (this.CaliQPass == true))
            {
                this.CaliResult = "Pass";
                this.CaliResultPass = true;
            }
            else
            {
                this.CaliResult = "Fail";
                this.CaliResultPass = false;
            }
        }

        public void LoadSettings(SettingsWindowViewModel s)
        {
            this.settings = s;
            OnPropertyChanged(nameof(this.Product));
        }

        private static bool CheckPass(string valueStr, int lower, int upper)
        {
            try
            {
                int value = Convert.ToInt32(valueStr, CultureInfo.InvariantCulture);
                return (value >= lower) && (value <= upper);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
                return false;
            }
        }

        public void SaveData()
        {
            try
            {
                string fileName = this.settings.Product + "-" +
                    DateTime.Today.ToString("yyyyMMdd", CultureInfo.InvariantCulture) + ".csv";
                string currentPath = Environment.CurrentDirectory;
                string fullPath = Path.Combine(currentPath, DataFolder);
                if (!Directory.Exists(fullPath))
                {
                    Directory.CreateDirectory(fullPath);
                }
                string file = Path.Combine(fullPath, fileName);

                bool flagFileExist = File.Exists(file);
                using (FileStream fs = new FileStream(file, flagFileExist ? FileMode.Append : FileMode.CreateNew))
                {
                    using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                    {
                        if (!flagFileExist)
                        {
                            sw.WriteLine("Time,QR Code,Input,Vsens,Temp,Vcoil,Q,Isens,Result");
                        }
                        StringBuilder sb = new StringBuilder()
                            .Append(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.ff", CultureInfo.InvariantCulture)).Append(',')
                            .Append(this.QRCode).Append(',')
                            .Append(this.CaliInput).Append(',')
                            .Append(this.CaliVsens).Append(',')
                            .Append(this.CaliTemp).Append(',')
                            .Append(this.CaliVcoil).Append(',')
                            .Append(this.CaliQ).Append(',')
                            .Append(this.CaliIsens).Append(',')
                            .Append(this.CaliResult);
                        sw.WriteLine(sb.ToString());
                        sw.Flush();
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
            }
        }
    }
}
