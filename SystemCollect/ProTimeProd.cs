using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaseCollect
{
    public class ProTime
    {
        public int MachineId { get; set; }
        public string StartTime { get; set; }
        public string CurTime { get; set; }
        public int LastTimeProd { get; set; }
        public int CurTimeProd { get; set; }
        public int CurdayInitProd { get; set; }
        public int CurShiftInitPord { get; set; }
    }
    public class ProTimeProd
    {
        public static DateTime last_time { get; set; }
        public static DateTime cur_time { get; set; }
        public static int LastHour { get; set; }
        public static int CurHour { get; set; }

        public string StartTime { get; set; }
        public string CurTime { get; set; }

        public static bool IsIntHour { get; set; }

        public static List<ProTime> ListCurdayInitProd { get; set; }
        public static List<ProTime> ListCurShiftInitProd { get; set; }
        public static List<ProTime> ListLastTimeProd { get; set; }
        public static List<ProTime> ListCurTimeProd { get; set; }
    }
}
