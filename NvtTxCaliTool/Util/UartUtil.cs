using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Text.RegularExpressions;

namespace NvtTxCaliTool
{
    class UartUtil : IDisposable
    {
        private readonly SerialPort port;
        private string recBuf;

        public UartUtil(string portName)
        {
            this.port = new SerialPort()
            {
                PortName = portName,
                BaudRate = 115200,
                Parity = Parity.None,
                DataBits = 8,
                StopBits = StopBits.One
            };
            this.recBuf = string.Empty;
        }

        public void ClearBuf()
        {
            this.recBuf = null;
            this.port.ReadExisting();
        }

        public void StartReceiving()
        {
            this.port.DataReceived += SerialPort_DataReceived;
        }

        public void StopReceiving()
        {
            this.port.DataReceived -= SerialPort_DataReceived;
        }

        public bool OpenPort()
        {
            try
            {
                if (this.port.IsOpen)
                {
                    this.port.Close();
                }
                this.port.Open();
                return true;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
                return false;
            }
        }

        public void ClosePort()
        {
            this.port.Close();
            this.port.DataReceived -= SerialPort_DataReceived;
            this.port.Dispose();
        }

        public delegate void UartMsgRevceivedEventHandler(object sender, UartMsgEventArgs e);
        public event UartMsgRevceivedEventHandler UartMsgReceived;

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string buf = this.port.ReadExisting();
            this.recBuf += buf;
            Regex regex = new Regex("[\\S ]+?\n\r");
            MatchCollection mc = regex.Matches(this.recBuf);
            Queue<UartMsgModel> msgQueue = new Queue<UartMsgModel>();
            for (int ii = 0; ii < mc.Count; ii++)
            {
                string msg = mc[ii].Value.Replace("\n\r", "");
                msgQueue.Enqueue(new UartMsgModel(msg));
            }
            int p = this.recBuf.LastIndexOf("\r", StringComparison.InvariantCulture) + 1;
            this.recBuf = this.recBuf.Substring(p);
            if (msgQueue.Count > 0)
            {
                UartMsgReceived(this, new UartMsgEventArgs(msgQueue));
            }
        }

        public class UartMsgEventArgs : EventArgs
        {
            public Queue<UartMsgModel> MsgQueue { get; private set; }
            public UartMsgEventArgs(Queue<UartMsgModel> q)
                => this.MsgQueue = q;
        }

        private static readonly byte[] CaliCmd = new byte[3] { 0xAC, 0x5A, 0xCA };
        public void SendCaliCmd()
        {
            this.port.Write(CaliCmd, 0, 3);
        }

        #region IDisposable Support
        private bool disposedValue;//= false; // To detect redundant calls

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
                if (this.port != null)
                {
                    this.port.Dispose();
                }

                this.disposedValue = true;
            }
        }

        // override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        ~UartUtil()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
