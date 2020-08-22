using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace NvtTxCaliTool
{
    public class SettingsWindowViewModel : BaseViewModel
    {
        [Newtonsoft.Json.JsonIgnore]
        private int inputLow;
        public int InputLow
        {
            get => this.inputLow;
            set => SetProperty(ref this.inputLow, value, nameof(this.InputLowText));
        }
        [Newtonsoft.Json.JsonIgnore]
        public string InputLowText
        {
            get => this.inputLow.ToString(CultureInfo.InvariantCulture);
            set
            {
                try
                {
                    this.inputLow = Convert.ToInt32(value, CultureInfo.InvariantCulture);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.ToString());
                    this.inputLow = 0;
                }
            }
        }

        [Newtonsoft.Json.JsonIgnore]
        private int inputHigh = 99999;
        public int InputHigh
        {
            get => this.inputHigh;
            set => SetProperty(ref this.inputHigh, value, nameof(this.InputHighText));
        }
        [Newtonsoft.Json.JsonIgnore]
        public string InputHighText
        {
            get => this.inputHigh.ToString(CultureInfo.InvariantCulture);
            set
            {
                try
                {
                    this.inputHigh = Convert.ToInt32(value, CultureInfo.InvariantCulture);
                    ;
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.ToString());
                    this.inputHigh = 99999;
                }
            }
        }

        [Newtonsoft.Json.JsonIgnore]
        private int vsensLow;
        public int VsensLow
        {
            get => this.vsensLow;
            set => SetProperty(ref this.vsensLow, value, nameof(this.VsensLowText));
        }
        [Newtonsoft.Json.JsonIgnore]
        public string VsensLowText
        {
            get => this.vsensLow.ToString(CultureInfo.InvariantCulture);
            set
            {
                try
                {
                    this.vsensLow = Convert.ToInt32(value, CultureInfo.InvariantCulture);
                    ;
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.ToString());
                    this.vsensLow = 0;
                }
            }
        }

        [Newtonsoft.Json.JsonIgnore]
        private int vsensHigh = 99999;
        public int VsensHigh
        {
            get => this.vsensHigh;
            set => SetProperty(ref this.vsensHigh, value, nameof(this.VsensHighText));
        }
        [Newtonsoft.Json.JsonIgnore]
        public string VsensHighText
        {
            get => this.vsensHigh.ToString(CultureInfo.InvariantCulture);
            set
            {
                try
                {
                    this.vsensHigh = Convert.ToInt32(value, CultureInfo.InvariantCulture);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.ToString());
                    this.vsensHigh = 99999;
                }
            }
        }

        [Newtonsoft.Json.JsonIgnore]
        private int isensLow;
        public int IsensLow
        {
            get => this.isensLow;
            set => SetProperty(ref this.isensLow, value, nameof(this.IsensLowText));
        }
        [Newtonsoft.Json.JsonIgnore]
        public string IsensLowText
        {
            get => this.isensLow.ToString(CultureInfo.InvariantCulture);
            set
            {
                try
                {
                    this.isensLow = Convert.ToInt32(value, CultureInfo.InvariantCulture);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.ToString());
                    this.isensLow = 0;
                }
            }
        }

        [Newtonsoft.Json.JsonIgnore]
        private int isensHigh = 99999;
        public int IsensHigh
        {
            get => this.isensHigh;
            set => SetProperty(ref this.isensHigh, value, nameof(this.IsensHighText));
        }
        [Newtonsoft.Json.JsonIgnore]
        public string IsensHighText
        {
            get => this.isensHigh.ToString(CultureInfo.InvariantCulture);
            set
            {
                try
                {
                    this.isensHigh = Convert.ToInt32(value, CultureInfo.InvariantCulture);
                    ;
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.ToString());
                    this.isensHigh = 99999;
                }
            }
        }

        [Newtonsoft.Json.JsonIgnore]
        private int vcoilLow;
        public int VcoilLow
        {
            get => this.vcoilLow;
            set => SetProperty(ref this.vcoilLow, value, nameof(this.VcoilLowText));
        }
        [Newtonsoft.Json.JsonIgnore]
        public string VcoilLowText
        {
            get => this.vcoilLow.ToString(CultureInfo.InvariantCulture);
            set
            {
                try
                {
                    this.vcoilLow = Convert.ToInt32(value, CultureInfo.InvariantCulture);
                    ;
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.ToString());
                    this.vcoilLow = 0;
                }
            }
        }

        [Newtonsoft.Json.JsonIgnore]
        private int vcoilHigh = 99999;
        public int VcoilHigh
        {
            get => this.vcoilHigh;
            set => SetProperty(ref this.vcoilHigh, value, nameof(this.VcoilHighText));
        }
        [Newtonsoft.Json.JsonIgnore]
        public string VcoilHighText
        {
            get => this.vcoilHigh.ToString(CultureInfo.InvariantCulture);
            set
            {
                try
                {
                    this.vcoilHigh = Convert.ToInt32(value, CultureInfo.InvariantCulture);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.ToString());
                    this.vcoilHigh = 99999;
                }
            }
        }

        [Newtonsoft.Json.JsonIgnore]
        private int qLow;
        public int QLow
        {
            get => this.qLow;
            set => SetProperty(ref this.qLow, value, nameof(this.QLowText));
        }
        [Newtonsoft.Json.JsonIgnore]
        public string QLowText
        {
            get => this.qLow.ToString(CultureInfo.InvariantCulture);
            set
            {
                try
                {
                    this.qLow = Convert.ToInt32(value, CultureInfo.InvariantCulture);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.ToString());
                    this.qLow = 0;
                }
            }
        }

        [Newtonsoft.Json.JsonIgnore]
        private int qHigh = 99999;
        public int QHigh
        {
            get => this.qHigh;
            set => SetProperty(ref this.qHigh, value, nameof(this.QHighText));
        }
        [Newtonsoft.Json.JsonIgnore]
        public string QHighText
        {
            get => this.qHigh.ToString(CultureInfo.InvariantCulture);
            set
            {
                try
                {
                    this.qHigh = Convert.ToInt32(value, CultureInfo.InvariantCulture);
                    ;
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.ToString());
                    this.qHigh = 99999;
                }
            }
        }

        [Newtonsoft.Json.JsonIgnore]
        private int tempLow;
        public int TempLow
        {
            get => this.tempLow;
            set => SetProperty(ref this.tempLow, value, nameof(this.TempLowText));
        }
        [Newtonsoft.Json.JsonIgnore]
        public string TempLowText
        {
            get => this.tempLow.ToString(CultureInfo.InvariantCulture);
            set
            {
                try
                {
                    this.tempLow = Convert.ToInt32(value, CultureInfo.InvariantCulture);
                    ;
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.ToString());
                    this.tempLow = 0;
                }
            }
        }

        [Newtonsoft.Json.JsonIgnore]
        private int tempHigh = 99999;
        public int TempHigh
        {
            get => this.tempHigh;
            set => SetProperty(ref this.tempHigh, value, nameof(this.TempHighText));
        }
        [Newtonsoft.Json.JsonIgnore]
        public string TempHighText
        {
            get => this.tempHigh.ToString(CultureInfo.InvariantCulture);
            set
            {
                try
                {
                    this.tempHigh = Convert.ToInt32(value, CultureInfo.InvariantCulture);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.ToString());
                    this.tempHigh = 99999;
                }
            }
        }

        public void SaveConfig(string file)
        {
            try
            {
                using (FileStream fs = new FileStream(file, FileMode.Create))
                {
                    using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                    {
                        sw.Write(JsonConvert.SerializeObject(this, Formatting.Indented));
                        sw.Flush();
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
            }
        }
        public void LoadConfig(string file)
        {
            try
            {
                SettingsWindowViewModel view = null;
                using (StreamReader sr = new StreamReader(file, Encoding.UTF8))
                {
                    view = JsonConvert.DeserializeObject(sr.ReadToEnd(), typeof(SettingsWindowViewModel)) as SettingsWindowViewModel;
                }
                if (view != null)
                {
                    this.InputHigh = view.InputHigh;
                    this.InputLow = view.InputLow;
                    this.VsensHigh = view.VsensHigh;
                    this.VsensLow = view.VsensLow;
                    this.TempHigh = view.TempHigh;
                    this.TempLow = view.TempLow;
                    this.VcoilHigh = view.VcoilHigh;
                    this.VcoilLow = view.VcoilLow;
                    this.IsensHigh = view.IsensHigh;
                    this.IsensLow = view.IsensLow;
                    this.QHigh = view.QHigh;
                    this.QLow = view.QLow;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
            }
        }
    }
}
