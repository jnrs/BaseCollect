using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BaseCollect
{
    public class AlarmInfo
    {
        DataConnect dbc = new DataConnect();

        public int group_id { get; set; }
        public string group_name { get; set; }
        public int machine_id { get; set; }
        public string machine_name { get; set; }
        public string machine_series { get; set; }
        public string machine_number { get; set; }
        public int axis_num { get; set; }    //报警轴
        public int axis_no { get; set; }     //轴号
        public int alarm_no { get; set; }
        public int alarm_group { get; set; }  //报警类别
        public int urge_info { get; set; }     //紧急程度:info 提示；warn 警告；merg 严重
        public int urge_warn { get; set; }
        public int urge_criti { get; set; }
        public string alarm_msg { get; set; }
        public int alarm_number { get; set; } //数量
        public int read_date { get; set; }
        public DateTime read_time { get; set; }

        public bool initAlarmInfo()
        {
            String strSql = "select ta.machine_id,ta.machine_name,ta.machine_number,tb.alarm_no,tb.alarm_text, "
                            + " count(tb.alarm_no) over(partition by ta.machine_id) as alarm_num from tb_machine_info ta,tb_alarm_info tb "
                            + " where ta.machine_id = tb.machine_id and ta.comm_protocol = \'Libnodev2\' and ta.comm_interface = \'TCPIP\' and ta.enabled = 1";

            try
            {
                DataTable dtInfo = dbc.getStrSqlData(strSql);

                if (dtInfo != null && dtInfo.Rows.Count > 0)
                {
                    //LibndParameter.AlarmLen = Convert.ToInt32(dtInfo.Compute("Min(alarm_num)", ""));
                }
            }
            catch (Exception ep)
            {
                Console.WriteLine("数据读取失败，请检查数据库连接！" + ep.Message);
                return false;
            }

            return true;
        }
    }
}
