using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace NvtTxCaliTool
{
    public class UartMsgModel
    {
        public enum MsgType
        {
            UNKNOWN = 0,
            CALIINPUT = 1,
            CALIVSENS = 2,
            CALITEMP = 3,
            CALIVCOIL = 4,
            CALIQ = 5,
            CALIISENS = 6
        };

        private static readonly List<(string Header, int Length, MsgType Type)> typeList = new List<(string Header, int length, MsgType Type)>()
        {
            ("Input",1,MsgType.CALIINPUT),
            ("StandbyVsens",1,MsgType.CALIVSENS),
            ("Temperature",1,MsgType.CALITEMP),
            ("Calibre_V_coil",1,MsgType.CALIVCOIL),
            ("Calibre_Q",1,MsgType.CALIQ),
            ("Calibre_I",1,MsgType.CALIISENS)
        };

        public MsgType Type { get; private set; } = MsgType.UNKNOWN;
        public string Header { get; private set; } = string.Empty;
        public List<string> Content { get; private set; } = new List<string>();
        public int ContentLength => this.Content.Count;
        public string RawMsg { get; private set; } = string.Empty;
        public string Msg { get; private set; } = string.Empty;

        public UartMsgModel(string rawMsg)
        {
            if (!string.IsNullOrEmpty(rawMsg))
            {
                this.RawMsg = rawMsg;
                this.Msg = CleanMsg(rawMsg);
                string[] tmpStr = this.Msg.Split(' ');
                this.Header = tmpStr.Length > 0 ? tmpStr[0] : string.Empty;
                if (tmpStr.Length > 1)
                {
                    for (int ii = 1; ii < tmpStr.Length; ii++)
                    {
                        this.Content.Add(tmpStr[ii]);
                    }
                }
                foreach ((string Header, int Length, MsgType Type) t in typeList)
                {
                    if (this.Header == t.Header && this.ContentLength == t.Length)
                    {
                        this.Type = t.Type;
                        break;
                    }
                }
            }

        }

        private static string CleanMsg(string msg)
        {
            if (!string.IsNullOrEmpty(msg))
            {
                msg = msg.Trim();
            }
            if (!string.IsNullOrEmpty(msg))
            {
                msg = new Regex("->").Replace(msg, " ");
            }
            if (!string.IsNullOrEmpty(msg))
            {
                msg = new Regex("=").Replace(msg, " ");
            }
            if (!string.IsNullOrEmpty(msg))
            {
                msg = new Regex("[\\s]+").Replace(msg, " ");
            }
            return msg;
        }
    }
}