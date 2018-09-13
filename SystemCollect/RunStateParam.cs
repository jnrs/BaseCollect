using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaseCollect
{
    public class RunStateParam
    {
        public static int StopState { get; set; }
        public static int RunState { get; set; }
        public static int AlarmState { get; set; }
        public static int PauseState { get; set; }
        public static int ReadyState { get; set; }
        public static int EditState { get; set; }
        public static int ExcpState { get; set; }
        public static int ReadState { get; set; }
        public static bool NotEmergState = false;
        public static bool EmergState = true;
        public static int OnState = 1;
        public static int OffState = 0;

    }
}
