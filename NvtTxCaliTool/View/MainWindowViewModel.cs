using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NvtTxCaliTool
{
    class MainWindowViewModel : BaseViewModel
    {
        public ComPortConfigViewModel ComPortConfigView { get; private set; }
            = new ComPortConfigViewModel();
        public CaliDataViewModel CaliDataView { get; private set; }
            = new CaliDataViewModel();
    }
}
