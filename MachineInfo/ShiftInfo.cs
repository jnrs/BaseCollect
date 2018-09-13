using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace BaseCollect
{
    public class ShiftInfo
    {
        public static int ShiftId;
        public static string ShiftName;
        public static string ShiftDate;
        public static DateTime StartTime;
        public static DateTime EndTime;

        DataTable dtInfo = new DataTable();
        DataConnect dbc = new DataConnect();

        public ShiftInfo()
        {
            ShiftId = 0;
            ShiftName = "";
            ShiftDate = DateTime.Today.ToString("yyyyMMdd");
            StartTime = DateTime.Today;
            EndTime = DateTime.Today.AddDays(1);
        }

        public bool initShiftInfo()
        {
            String strSql = "select wshift_id,wshift_name,wshift_date,wshift_type,start_dtime,end_dtime "
                            + " from tb_shift_date where start_dtime <= getdate() and end_dtime > getdate() "
                            + "  and wshift_date >= " + DateTime.Today.AddDays(-1).ToString("yyyyMMdd");

            try
            {
                dtInfo = dbc.getStrSqlData(strSql);

                for (int i = 0; i < dtInfo.Rows.Count; i++)
                {
                    ShiftInfo.ShiftId = Convert.ToInt32(dtInfo.Rows[i]["wshift_id"].ToString());
                    ShiftInfo.ShiftName = dtInfo.Rows[i]["wshift_name"].ToString();
                    ShiftInfo.ShiftDate = dtInfo.Rows[i]["wshift_date"].ToString();
                    ShiftInfo.StartTime = Convert.ToDateTime(dtInfo.Rows[i]["start_dtime"]);
                    ShiftInfo.EndTime = Convert.ToDateTime(dtInfo.Rows[i]["end_dtime"]);
                }
            }
            catch (Exception ep)
            {
                Console.WriteLine("班次日期读取失败！" + ep.Message);
                return false;
            }
            return true;
        }
        public bool initShiftInfo_bk()
        {
            String strSql = "select si.wshift_id,si.wshift_name,si.wshift_type, Convert(varchar(12),si.start_time,108) as start_time, "
                            + "     Convert(varchar(12),si.end_time,108) as end_time "
                            + " from vw_shift_info si where convert(varchar(12),si.start_time,108) <= convert(varchar(12),si.end_time,108) "
                            + "  and si.start_time <= convert(varchar(10),getdate(),108) "
                            + "  and si.end_time > convert(varchar(10),getdate(),108) "
                            + " union all "
                            + " select si.wshift_id,si.wshift_name,si.wshift_type, Convert(varchar(12),si.start_time,108) as start_time, "
                            + "     Convert(varchar(12),si.end_time,108) as end_time "
                            + " from vw_shift_info si where convert(varchar(12),si.start_time,108) > convert(varchar(12),si.end_time,108) "
                            + "  and ((si.start_time <= convert(varchar(10),getdate(),108) and convert(varchar(10),getdate(),108) < '24:00:00') "
                            + "    or (si.end_time > convert(varchar(10),getdate(),108) and convert(varchar(10),getdate(),108) >= '00:00:00')) ";

            try
            {
                dtInfo = dbc.getStrSqlData(strSql);

                for (int i = 0; i < dtInfo.Rows.Count; i++)
                {
                    ShiftInfo.ShiftId = Convert.ToInt32(dtInfo.Rows[i]["wshift_id"].ToString());
                    ShiftInfo.ShiftName = dtInfo.Rows[i]["wshift_name"].ToString();
                    //ShiftInfo.StartTime = dtInfo.Rows[i]["start_time"].ToString();
                    //ShiftInfo.EndTime = dtInfo.Rows[i]["end_time"].ToString();
                }
            }
            catch (Exception ep)
            {
                Console.WriteLine("数据读取失败，请检查数据库连接！" + ep.Message);
                return false;
            }
            return true;
        }
        public bool initShiftDate()
        {
            String strSql = " select count(1) as row_cnt from tb_shift_date where calc_date = convert(varchar(10),getdate(),112) ";

            try
            {
                dtInfo = dbc.getStrSqlData(strSql);

                int count = 0;
                if (dtInfo != null && dtInfo.Rows.Count > 0)
                {
                    count = Convert.ToInt32(dtInfo.Rows[0]["row_cnt"]);
                }

                if (count == 0)
                {
                    strSql = "insert into tb_shift_date(calc_date,wshift_id,wshift_no,wshift_date,wshift_name,wshift_type,start_time,end_time,start_dtime,end_dtime) "
                            + " select convert(varchar(10),getdate(),112) as calc_date,wshift_id,wshift_no, "
                            + "        case when start_time < end_time then convert(varchar(10),dateadd(day,wshift_days,getdate()),112) "
                            + "             else convert(varchar(10),getdate(),112) end as wshift_date, "
                            + "         wshift_name,wshift_type,start_time,end_time, "
                            + "        cast(convert(varchar(10),getdate(),112) + ' ' + start_time as datetime) as start_dtime, "
                            + "        case when start_time < end_time then cast(convert(varchar(10),getdate(),112) + ' ' + end_time as datetime) "
                            + "             else cast(convert(varchar(10),dateadd(day,abs(wshift_days),getdate()),112) + ' ' + end_time as datetime) end as end_dtime "
                            + "   from vw_shift_info ";

                    dbc.putStrSqlData(strSql);
                }
            }
            catch (Exception ep)
            {
                Console.WriteLine("班次日期更新失败！" + ep.Message);
                return false;
            }
            
            return true;
        }
        public bool initEmpShiftDate()
        {
            String strSql = " select count(1) as row_cnt from tb_shift_date where calc_date = convert(varchar(10),getdate(),112) ";

            try
            {
                dtInfo = dbc.getStrSqlData(strSql);

                int count = 0;
                if (dtInfo != null && dtInfo.Rows.Count > 0)
                {
                    count = Convert.ToInt32(dtInfo.Rows[0]["row_cnt"]);
                }

                if (count == 0)
                {
                    strSql = "insert into tb_emp_shift_date(calc_date,wshift_id,wshift_no,wshift_date,wshift_name,wshift_type,start_time,end_time,start_dtime,end_dtime) "
                            + " select convert(varchar(10),getdate(),112) as calc_date,wshift_id,wshift_no, "
                            + "        case when start_time < end_time then convert(varchar(10),dateadd(day,wshift_days,getdate()),112) "
                            + "             else convert(varchar(10),getdate(),112) end as wshift_date, "
                            + "         wshift_name,wshift_type,start_time,end_time, "
                            + "        cast(convert(varchar(10),getdate(),112) + ' ' + start_time as datetime) as start_dtime, "
                            + "        case when start_time < end_time then cast(convert(varchar(10),getdate(),112) + ' ' + end_time as datetime) "
                            + "             else cast(convert(varchar(10),dateadd(day,abs(wshift_days),getdate()),112) + ' ' + end_time as datetime) end as end_dtime "
                            + "   from vw_shift_info ";

                    dbc.putStrSqlData(strSql);
                }
            }
            catch (Exception ep)
            {
                Console.WriteLine("班次日期更新失败！" + ep.Message);
                return false;
            }

            return true;
        }
    }
}
