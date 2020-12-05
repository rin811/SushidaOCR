using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sushida
{
    public class Debug
    {
        public bool isRunning = false;
        public string ScanResult = "";
        public bool isMissedScan = false;
        public bool isDummyMissing = false;
        public bool isUseHighTrainData = false;
        //public string 

        public Debug()
        {
            //isRunning = false;
            //ScanResult = "";
            //isMissedScan = false;
        }
    }

    enum State
    {
        Stopped,
        Running
    }
}
