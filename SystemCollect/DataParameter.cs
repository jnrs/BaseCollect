using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace BaseCollect
{
    public enum DirFloatData
    {
        Act_Spindle_Speed_0_index = 0,
        Act_Feedrate_0_index,
        Act_Speed_0_index,
        Act_Axis_0_index,
        Act_Spindle_Speed_1_index,
        Act_Feedrate_1_index,
        Act_Speed_1_index,
        Act_Axis_1_index,
        Act_Spindle_Speed_2_index,
        Act_Feedrate_2_index,
        Act_Speed_2_index,
        Act_Axis_2_index,
        Sp_Load_0,
        Sp_Load_1,
        Sp_Load_2,
        Sv_Load_0,
        Sv_Load_1,
        Sv_Load_2,
        Run_Time
    }
    public class DataParameter
    {
        log4net.ILog log = log4net.LogManager.GetLogger("DataParameter");

        public static bool DebugFlag = false;
        public static bool DebugCountFlag = false;
        public static int DebugCountNum = 0;
        public static int SLCount = 0;
        public static int XLCount = 0;
        public static int DebugRunState = 0;

        public static Dictionary<int, int> DicSLCount = new Dictionary<int, int>();
        public static Dictionary<int, int> DicXLCount = new Dictionary<int, int>();
        public static Dictionary<int, int> DicDebugRunState = new Dictionary<int, int>();
        public static Dictionary<int, bool> DicDebugCountFlag = new Dictionary<int, bool>();

        public static int RunStateLen = 8;
        public static int ByteLen = 8;
        public static int DataLen = 16;
        public static int DWordLen = 32;
        public static int OneByte = 1;
        public static int TwoByte = 2;
        public static int FouByte = 4;

        public static int DefaultAlarm_grp = 0;
        public static int ReHisDataCounter = 0;
        public static int ReWShiftCounter = 0;

        public static int tmFanucToolCounter = 1;
        public static int tmBRToolCounter = 0;
        public static int tmSimenToolCounter = 0;
        public static int tmOmronToolCounter = 0;
        public static int tmWimiToolCounter = 0;
        public static int tmMITToolCounter = 0;

        public static Dictionary<int, DateTime> RunInfoTime = new Dictionary<int, DateTime>();
        public static Dictionary<int, DateTime> RunDataTime = new Dictionary<int, DateTime>();
        public static Dictionary<int, DateTime> AlarmInfoTime = new Dictionary<int, DateTime>();

        private int _MachineId;
        private int _ParamId;
        private string _ParamName;
        private string _ParamType;
        private string _PRWType;
        private string _PFieldsName;
        private string _PFieldsType;
        private int _PFieldsLen;
        private string _PDataAddr;
        private object _PDataSetup;
        private string _PDataType;
        private int _PDataLen;
        private bool _PDataIsVisual;
        private bool _PDataIsHot;

        DataSet ds = new DataSet();
        DataTable dtInfo = new DataTable();
        DataConnect dbc = new DataConnect();
        // ------------------------------------------------------------------------
        // Button read coils//状态转换
        // ------------------------------------------------------------------------
        public bool readInitRunState()
        {
            String strSql = "select param_name,data_setup from dbo.tb_dict_parameter where param_type = \'RunState\'";

            //取设备 
            try
            {
                dtInfo = dbc.getStrSqlData(strSql);

                Console.WriteLine("dtInfo:" + dtInfo.Rows.Count);

                for (int i = 0; i < dtInfo.Rows.Count; i++)
                {
                    if (dtInfo.Rows[i]["param_name"].ToString().Equals("RunState"))
                    {
                        RunStateParam.RunState = Convert.ToInt32(dtInfo.Rows[i]["data_setup"]);
                    }
                    if (dtInfo.Rows[i]["param_name"].ToString().Equals("AlarmState"))
                    {
                        RunStateParam.AlarmState = Convert.ToInt32(dtInfo.Rows[i]["data_setup"]);
                    }
                    if (dtInfo.Rows[i]["param_name"].ToString().Equals("PauseState"))
                    {
                        RunStateParam.PauseState = Convert.ToInt32(dtInfo.Rows[i]["data_setup"]);
                    }
                    if (dtInfo.Rows[i]["param_name"].ToString().Equals("ReadyState"))
                    {
                        RunStateParam.ReadyState = Convert.ToInt32(dtInfo.Rows[i]["data_setup"]);
                    }
                    if (dtInfo.Rows[i]["param_name"].ToString().Equals("EditState"))
                    {
                        RunStateParam.EditState = Convert.ToInt32(dtInfo.Rows[i]["data_setup"]);
                    }
                    if (dtInfo.Rows[i]["param_name"].ToString().Equals("StopState"))
                    {
                        RunStateParam.StopState = Convert.ToInt32(dtInfo.Rows[i]["data_setup"]);
                    }
                    if (dtInfo.Rows[i]["param_name"].ToString().Equals("ExcpState"))
                    {
                        RunStateParam.ExcpState = Convert.ToInt32(dtInfo.Rows[i]["data_setup"]);
                    }
                    if (dtInfo.Rows[i]["param_name"].ToString().Equals("ReadState"))
                    {
                        RunStateParam.ReadState = Convert.ToInt32(dtInfo.Rows[i]["data_setup"]);
                    }
                }
                log.Info("RunStateParam:" + RunStateParam.RunState);
            }
            catch (NullReferenceException ep)
            {
                log.Error("初始化运行状态失败！" + ep.Message);
                return false;
            }
            catch (InvalidOperationException ep)
            {
                log.Error("初始化运行状态失败！" + ep.Message);
                return false;
            }
            catch (FormatException ep)
            {
                log.Error("初始化运行状态失败！" + ep.Message);
                return false;
            }
            catch (SqlException ep)
            {
                log.Error("初始化运行状态失败！" + ep.Message);
                return false;
            }
            catch (SystemException ep)
            {
                log.Error("初始化运行状态失败！" + ep.Message);
                return false;
            }
            catch (Exception ep)
            {
                log.Error("初始化运行状态失败！" + ep.Message);
                return false;
            }
            return false;
        }

        public static int FormatAsRunState(MachineInfo mi, string strdata)
        {
            int runState = RunStateParam.StopState;

            int pos = 0;
            bool run = false;
            bool alarm = false;
            bool pause = false;

            for (int i = 0; i < strdata.Length; i++)
            {
                pos = strdata.Length - i - 1;
                if (i == mi.RunBit && strdata[pos] == '1') run = true;
                if (i == mi.AlarmBit && strdata[pos] == '1') alarm = true;
                if (i == mi.PauseBit && strdata[pos] == '1') pause = true;
            }

            if (alarm)
            {
                runState = RunStateParam.AlarmState;
            }
            if (pause)
            {
                runState = RunStateParam.PauseState;
            }
            if (run)
            {
                runState = RunStateParam.RunState;
            }

            if (!run && !alarm && !pause)
            {
                runState = RunStateParam.ReadyState;
            }
            if (run && alarm) runState = RunStateParam.RunState;

            return runState;
        }
        public static int CellsFormatAsRunState(MachineInfo mi, List<bool> listCells)
        {
            int runState = RunStateParam.StopState;

            int pos = 0;
            bool run = false;
            bool alarm = false;
            bool pause = false;

            for (int i = 0; i < listCells.Count; i++)
            {
                pos = listCells.Count - i - 1;
                if (i == mi.RunBit && listCells[pos]) run = true;
                if (i == mi.AlarmBit && listCells[pos]) alarm = true;
                if (i == mi.PauseBit && listCells[pos]) pause = true;
            }

            if (alarm)
            {
                runState = RunStateParam.AlarmState;
            }
            if (pause)
            {
                runState = RunStateParam.PauseState;
            }
            if (run)
            {
                runState = RunStateParam.RunState;
            }

            if (!run && !alarm && !pause)
            {
                runState = RunStateParam.ReadyState;
            }
            if (run && alarm) runState = RunStateParam.RunState;

            return runState;
        }
        public static int BitFormatAsRunState(int run_value, int alarm_value, int pause_value)
        {
            int runState = RunStateParam.StopState;

            bool run = run_value == 1;
            bool alarm = alarm_value == 1;
            bool pause = pause_value == 1;

            if (pause)
            {
                runState = RunStateParam.PauseState;
            }
            if (alarm)
            {
                runState = RunStateParam.AlarmState;
            }
            if (run)
            {
                runState = RunStateParam.RunState;
            }
            if (!run && !alarm && !pause)
            {
                runState = RunStateParam.ReadyState;
            }
            //if (run && alarm) runState = RunStateParam.EditState;

            Console.WriteLine("BitFormatAsRunState:" + " " + run_value + " " + alarm_value + " " + pause_value + " " + runState);
            return runState;
        }
        // ------------------------------------------------------------------------
        // Button read holding register//保持寄存器
        // ------------------------------------------------------------------------
        public static float FormatAsFloat(int sindex, ushort[] udata)
        {
            try
            {
                return _FormatAsFloat(sindex, udata);
            }
            catch (Exception ex)
            {
                throw new Exception("FormatAsFloat is Error:" + ex.Message);
            }
        }
        private static float _FormatAsFloat(int sindex, ushort[] udata)
        {
            float fdata = 0f;
            byte[] tdata = new Byte[4];

            try
            {
                if (udata.Length - sindex < 2)
                {
                    return 0f;
                }

                //ushort[] sd = new ushort[2];
                //sd[1] = 17096;
                //sd[0] = 16174;
                //Console.WriteLine("sd" + sd + " " + (byte)(sd[0]) + " " + (byte)(sd[0] >> 8));
                //Console.WriteLine("sd" + sd + " " + (byte)(sd[1]) + " " + (byte)(sd[1] >> 8));

                //tdata[0] = (byte)(sd[0] >> 8);
                //tdata[1] = (byte)(sd[0]);
                //tdata[2] = (byte)(sd[1] >> 8);
                //tdata[3] = (byte)(sd[1]);

                tdata[0] = (byte)(udata[sindex] >> 8);
                tdata[1] = (byte)(udata[sindex]);
                tdata[2] = (byte)(udata[sindex + 1] >> 8);
                tdata[3] = (byte)(udata[sindex + 1]);

                fdata = ByteToFloat(tdata);
            }
            catch (OverflowException ex)
            {
                throw new Exception("FormatAsFloat is Error!" + ex.Message);
            }
            catch (FormatException ex)
            {
                throw new Exception("FormatAsFloat is Error!" + ex.Message);
            }
            return fdata;
        }
        public static float ByteToFloat(byte[] bResponse)
        {
            try
            {
                return _ByteToFloat(bResponse);
            }
            catch (Exception ex)
            {
                throw new Exception("ByteToFloat is Error!" + ex.Message);
            }
        }
        private static float _ByteToFloat(byte[] bResponse)
        {
            try
            {
                if (bResponse.Length < 4 || bResponse.Length > 4)
                {
                    //throw new NotEnoughDataInBufferException(data.length(), 8);
                    return 0;
                }
                else
                {
                    byte[] intBuffer = new byte[4];
                    //将byte数组的前后两个字节的高低位换过来
                    intBuffer[0] = bResponse[1];
                    intBuffer[1] = bResponse[0];
                    intBuffer[2] = bResponse[3];
                    intBuffer[3] = bResponse[2];
                    return BitConverter.ToSingle(intBuffer, 0);
                }
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new Exception("ByteToFloat is Error!" + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("ByteToFloat is Error!" + ex.Message);
            }
        }
        /*取登陆状态*/
        public LocalUserState getUsedOnLineState(MachineInfo mi)
        {
            LocalUserState lus = null;
            List<LocalUserState> llus = new List<LocalUserState>();

            string strSql = "select group_id,machine_id,employee_id,access_type,last_time "
                            + " from (select vmi.group_id,tea.machine_id,tea.employee_id,tea.access_type, "
                            + "              row_number() over(partition by vmi.group_id,tea.machine_id order by tea.last_time desc) as row_ord,tea.last_time "
                            + "         from tb_employee_access tea,vw_machine_info vmi where tea.machine_id = vmi.machine_id and tea.machine_id = " + mi.machine_id
                            + "          and vmi.group_id = " + mi.group_id + ") ta where row_ord = 1";

            try
            {
                dtInfo = dbc.getStrSqlData(strSql).Copy();
                if (dtInfo.Rows.Count == 0)
                {
                    lus = new LocalUserState();

                    lus.group_id = mi.group_id;
                    lus.machine_id = mi.machine_id;
                    lus.user_id = 0;
                    lus.access_type = "LogOut";
                    lus.last_time = DateTime.Now;
                }

                for (int i = 0; i < dtInfo.Rows.Count; i++)
                {
                    lus = new LocalUserState();

                    lus.group_id = Convert.ToInt32(dtInfo.Rows[i]["group_id"]);
                    lus.machine_id = Convert.ToInt32(dtInfo.Rows[i]["machine_id"]);
                    lus.user_id = Convert.ToInt32(dtInfo.Rows[i]["employee_id"]);
                    lus.access_type = dtInfo.Rows[i]["access_type"].ToString();
                    lus.last_time = Convert.ToDateTime(dtInfo.Rows[i]["last_time"]);

                    llus.Add(lus);
                }
            }
            catch (SqlException ex)
            {
                log.Error("getUsedOnLineState is Error！" + ex.Message);
                return new LocalUserState();
            }
            catch (FormatException ex)
            {
                log.Error("getUsedOnLineState is Error！" + ex.Message);
                return new LocalUserState();
            }
            catch (InvalidCastException ex)
            {
                log.Error("getUsedOnLineState is Error！" + ex.Message);
                return new LocalUserState();
            }
            catch (SystemException ex)
            {
                log.Error("getUsedOnLineState is Error！" + ex.Message);
                return new LocalUserState();
            }
            catch (Exception ex)
            {
                log.Error("getUsedOnLineState is Error！" + ex.Message);
                return new LocalUserState();
            }
            return lus;
        }
        /*用时分析*/
        public void getUsedTimeSeq_Curday()
        {
            DateTime start_time = DateTime.Today;
            DateTime end_time = DateTime.Today.AddDays(1).AddSeconds(-1);
            //int dur_length = getDiffTotalSeconds(start_time, end_time);
            int dur_length = 24 * 3600;

            Console.WriteLine("TimeSeque:" + start_time + " " + end_time + " " + DateTime.Now);
            log.Info("TimeSeque:" + start_time + " " + end_time);

            try
            {
                _getUsedTimeSeq_Curday(start_time, end_time, dur_length);
            }
            catch (Exception ex)
            {
                log.Error("getUsedTimeSeq_Curday is Error!" + ex.Message);
            }
            
        }
        /*用时分析*/
        private void _getUsedTimeSeq_Curday(DateTime start_time, DateTime end_time, int dur_length)
        {
            MachineInfo mi = null;
            List<MachineInfo> listUsedTime = new List<MachineInfo>();

            string strSql = "select mi.group_id,mi.group_name,mi.machine_id,mi.machine_name,mi.machine_series,mi.machine_number,1 as mac_id "
                            + " from vw_machine_info mi where 1 = 1 ";
            try
            {
                dtInfo = dbc.getStrSqlData(strSql);
                DataTable cdt = dtInfo.Copy();

                for (int i = 0; i < cdt.Rows.Count; i++)
                {
                    mi = new MachineInfo();

                    mi.group_id = Convert.ToInt32(cdt.Rows[i]["group_id"]);
                    mi.group_name = cdt.Rows[i]["group_name"].ToString();
                    mi.machine_id = Convert.ToInt32(cdt.Rows[i]["machine_id"]);
                    mi.machine_name = cdt.Rows[i]["machine_name"].ToString();
                    mi.machine_series = cdt.Rows[i]["machine_series"].ToString();
                    mi.machine_number = cdt.Rows[i]["machine_number"].ToString();
                    int mac_id = Convert.ToInt32(cdt.Rows[i]["mac_id"]);

                    _getDeviceSequeUsedTime_Curday(mi, mac_id, start_time, end_time, dur_length);
                }
            }
            catch (SqlException ex)
            {
                log.Error("getUsedTimeSeq_Curday is Error！" + ex.Message);
            }
            catch (FormatException ex)
            {
                log.Error("getUsedTimeSeq_Curday is Error！" + ex.Message);
            }
            catch (InvalidCastException ex)
            {
                log.Error("getUsedOnLineState is Error！" + ex.Message);
            }
            catch (SystemException ex)
            {
                log.Error("getUsedTimeSeq_Curday is Error！" + ex.Message);
            }
            catch (Exception ex)
            {
                log.Error("getUsedTimeSeq_Curday is Error！" + ex.Message);
            }
        }
        /*取设备用时分析*/
        private void _getDeviceSequeUsedTime_Curday(MachineInfo mi, int mac_id, DateTime start_time, DateTime end_time, int dur_length)
        {
            try
            {
                if (mac_id > 0)
                {
                    string wshift_date = start_time.ToString("yyyyMMdd");
                    dbc.exespDeviceUsedTime("sp_init_run_seque", wshift_date, wshift_date, mi.machine_id);
                    dbc.exespDeviceUsedTime("sp_init_state_seque", wshift_date, wshift_date, mi.machine_id);
                }
            }
            catch (SqlException ex)
            {
                log.Error("getUsedTimeSeq_Curday is Error！" + ex.Message);
            }
            catch (FormatException ex)
            {
                log.Error("getUsedTimeSeq_Curday is Error！" + ex.Message);
            }
            catch (InvalidCastException ex)
            {
                log.Error("getUsedTimeSeq_Curday is Error！" + ex.Message);
            }
            catch (SystemException ex)
            {
                log.Error("getUsedTimeSeq_Curday is Error！" + ex.Message);
            }
            catch (Exception ex)
            {
                log.Error("getUsedTimeSeq_Curday is Error！" + ex.Message);
            }
        }
        /*用时分析*/
        public void getUsedTimeSeq(DateTime startDate, DateTime endDate)
        {
            DateTime start_time = startDate.Date;
            DateTime end_time = endDate.Date.AddDays(1).AddSeconds(-1);
            //int dur_length = getDiffTotalSeconds(start_time, end_time);
            int dur_length = 24 * 3600;

            Console.WriteLine("TimeSeque:" + start_time + " " + end_time);
            log.Info("TimeSeque:" + start_time + " " + end_time);

            try
            {
                _getUsedTimeSeq(start_time, end_time, dur_length);
            }
            catch (Exception ex)
            {
                log.Error("getUsedTimeSeq is Error!" + ex.Message);
            }
        }
        
        /*用时分析*/
        private void _getUsedTimeSeq(DateTime start_time, DateTime end_time, int dur_length)
        {
            MachineInfo mi = null;
            List<MachineInfo> listUsedTime = new List<MachineInfo>();

            string strSql = "select mi.group_id,mi.group_name,mi.machine_id,mi.machine_name,mi.machine_series,mi.machine_number,isnull(ri.machine_id,0) as mac_id "
                            + " from vw_machine_info mi left join (select distinct machine_id from tb_run_info "
                            + "        where calc_date >= " + start_time.ToString("yyyyMMdd") + " and calc_date <= " + end_time.ToString("yyyyMMdd") + ") ri "
                            + " on mi.machine_id = ri.machine_id where 1=1 ";

            try
            {
                dtInfo = dbc.getStrSqlData(strSql);
                DataTable cdt = dtInfo.Copy();

                for (int i = 0; i < cdt.Rows.Count; i++)
                {
                    mi = new MachineInfo();

                    mi.group_id = Convert.ToInt32(cdt.Rows[i]["group_id"]);
                    mi.group_name = cdt.Rows[i]["group_name"].ToString();
                    mi.machine_id = Convert.ToInt32(cdt.Rows[i]["machine_id"]);
                    mi.machine_name = cdt.Rows[i]["machine_name"].ToString();
                    mi.machine_series = cdt.Rows[i]["machine_series"].ToString();
                    mi.machine_number = cdt.Rows[i]["machine_number"].ToString();
                    //int mac_id = Convert.ToInt32(cdt.Rows[i]["mac_id"]);
                    int mac_id = mi.machine_id;

                    _getDeviceSequeUsedTime(mi, mac_id, start_time, end_time, dur_length);
                }
            }
            catch (SqlException ex)
            {
                log.Error("getUsedTimeSeq_Curday is Error！" + ex.Message);
            }
            catch (FormatException ex)
            {
                log.Error("getUsedTimeSeq_Curday is Error！" + ex.Message);
            }
            catch (InvalidCastException ex)
            {
                log.Error("getUsedTimeSeq_Curday is Error！" + ex.Message);
            }
            catch (SystemException ex)
            {
                log.Error("getUsedTimeSeq_Curday is Error！" + ex.Message);
            }
            catch (Exception ex)
            {
                log.Error("getUsedTimeSeq_Curday is Error！" + ex.Message);
            }
        }
        /*取设备用时分析*/
        private void _getDeviceSequeUsedTime(MachineInfo mi, int mac_id, DateTime start_time, DateTime end_time, int dur_length)
        {
            try
            {
                if (mac_id > 0)
                {
                    TimeSpan dd = end_time.Subtract(start_time);
                    for (int i = 0; i < dd.Days + 1; i++)
                    {
                        string wshift_date = start_time.AddDays(i).ToString("yyyyMMdd");
                        dbc.exespDeviceUsedTime("sp_init_run_seque_his", wshift_date, wshift_date, mi.machine_id);
                        dbc.exespDeviceUsedTime("sp_init_state_seque_his", wshift_date, wshift_date, mi.machine_id);

                        if (DateTime.Compare(start_time.AddDays(i).Date, DateTime.Today.AddDays(-3).Date) >= 0)
                        {
                            string strSql = "delete from tb_run_info_init where machine_id = " + mac_id + " and wshift_date = " + wshift_date;
                            if (dbc.putStrSqlData(strSql))
                            {
                                dbc.exespDeviceUsedTime("sp_init_run_seque", wshift_date, wshift_date, mi.machine_id);
                            }

                            strSql = "delete from tb_run_state_seque where machine_id = " + mac_id + " and wshift_date = " + wshift_date;
                            if (dbc.putStrSqlData(strSql))
                            {
                                dbc.exespDeviceUsedTime("sp_init_state_seque", wshift_date, wshift_date, mi.machine_id);
                            }
                        }
                    }
                    log.Info("DeviceSeque Machine:" + mi.machine_id + " " + mi.machine_name);
                }
            }
            catch (SqlException ex)
            {
                log.Error("getUsedTimeSeq_Curday is Error！" + ex.Message);
            }
            catch (FormatException ex)
            {
                log.Error("getUsedTimeSeq_Curday is Error！" + ex.Message);
            }
            catch (InvalidCastException ex)
            {
                log.Error("getUsedTimeSeq_Curday is Error！" + ex.Message);
            }
            catch (SystemException ex)
            {
                log.Error("getUsedTimeSeq_Curday is Error！" + ex.Message);
            }
            catch (Exception ex)
            {
                log.Error("getUsedTimeSeq_Curday is Error！" + ex.Message);
            }
        }
        public bool PutOperateRunInfo(string calc_date, string put_date)
        {
            return _PutOperateRunInfo(calc_date, put_date);
        }
        public bool PutOperateRunInfoInit(string calc_date, string put_date)
        {
            return _PutOperateRunInfoInit(calc_date, put_date);
        }
        public bool PutOperateRunStateSeque(string calc_date, string put_date)
        {
            return _PutOperateRunStateSeque(calc_date, put_date);
        }
        public bool PutOperateRunData(string calc_date, string put_date)
        {
            return _PutOperateRunData(calc_date, put_date);
        }
        public bool PutOperateRunDataInit(string calc_date, string put_date)
        {
            return _PutOperateRunDataInit(calc_date, put_date);
        }
        public bool PubOperateCurdayProdNum(string calc_date, string put_date)
        {
            return _PubOperateCurdayProdNum(calc_date, put_date);
        }
        public bool PutOperateAlarmHistory(string calc_date, string put_date)
        {
            return _PutOperateAlarmHistory(calc_date, put_date);
        }
        public bool PutOperateAlarmHistoryInit(string calc_date, string put_date)
        {
            //return _PutOperateAlarmHistoryInit(calc_date, put_date);
            return true;
        }
        public bool PutOperateStandardData(string calc_date, string put_date)
        {
            return _PutOperateStandardData(calc_date, put_date);
        }
        private bool _PutOperateRunInfo(string calc_date, string put_date)
        {
            string strSql = "delete from tb_run_info where wshift_date < " + put_date;
            try
            {
                if (dbc.putStrSqlData(strSql))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.Error("PutOperateRunInfo is Error!" + ex.Message);
            }
            return false;
        }
        private bool _PutOperateRunInfoInit(string calc_date, string put_date)
        {
            string strSql = "delete from tb_run_info_init where wshift_date < " + put_date;
            try
            {
                if (dbc.putStrSqlData(strSql))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.Error("PutOperateRunInfoInit is Error!" + ex.Message);
            }
            return false;
        }
        private bool _PutOperateRunStateSeque(string calc_date, string put_date)
        {
            string strSql = "delete from tb_run_state_seque where wshift_date < " + put_date;
            try
            {
                if (dbc.putStrSqlData(strSql))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.Error("PutOperateRunStateSeque is Error!" + ex.Message);
            }
            return false;
        }
        private bool _PutOperateRunData(string calc_dat, string put_date)
        {
            string strSql = "delete from tb_run_data where wshift_date < " + put_date;
            try
            {
                if (dbc.putStrSqlData(strSql))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.Error("PutOperateRunData is Error!" + ex.Message);
            }
            return false;
        }
        private bool _PutOperateRunDataInit(string calc_date, string put_date)
        {
            string strSql = "delete from tb_run_data_init where wshift_date < " + put_date;
            try
            {
                if (dbc.putStrSqlData(strSql))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.Error("PutOperateRunDataInit is Error!" + ex.Message);
            }
            return false;
        }
        private bool _PubOperateCurdayProdNum(string calc_date, string put_date)
        {
            string strSql = "delete from tb_curday_prod_num where wshift_date < " + put_date;
            try
            {
                if (dbc.putStrSqlData(strSql))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.Error("PubOperateCurdayProdNum is Error!" + ex.Message);
            }
            return false;
        }
        private bool _PutOperateAlarmHistory(string calc_date, string put_date)
        {
            string strSql = "delete from tb_alarm_history where wshift_date < " + put_date;
            try
            {
                if (dbc.putStrSqlData(strSql))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.Error("PutOperateAlarmHistory is Error!" + ex.Message);
            }
            return false;
        }
        private bool _PutOperateAlarmHistoryInit(string calc_date, string put_date)
        {
            string strSql = "delete from tb_alarm_history_init where wshift_date < " + put_date;
            try
            {
                if (dbc.putStrSqlData(strSql))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.Error("PutOperateAlarmHistoryInit is Error!" + ex.Message);
            }
            return false;
        }
        private bool _PutOperateStandardData(string calc_date, string put_date)
        {
            string strSql = "delete from tb_standard_data where wshift_date < " + put_date;
            try
            {
                if (dbc.putStrSqlData(strSql))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.Error("PutOperateStandardData is Error!" + ex.Message);
            }
            return false;
        }
        /*取时间差总秒数*/
        public int getDiffTotalSeconds(DateTime start_time, DateTime end_time)
        {
            try
            {
                if (DateTime.Compare(end_time, start_time) < 0)
                    return 0;

                TimeSpan ts1 = new TimeSpan(end_time.Ticks);
                TimeSpan ts2 = new TimeSpan(start_time.Ticks);
                TimeSpan ts = ts1.Subtract(ts2).Duration();

                int run_duration = Convert.ToInt32(Math.Round(ts.TotalSeconds, 0));

                return run_duration;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                log.Error("getDiffTotalSeconds is Error!" + ex.Message);
                return 0;
            }
            catch (OverflowException ex)
            {
                log.Error("getDiffTotalSeconds is Error!" + ex.Message);
                return 0;
            }
            catch (Exception ex)
            {
                log.Error("getDiffTotalSeconds is Error!" + ex.Message);
                return 0;
            }
        }
        //整点时间判断
        public bool TimeJudge()
        {
            DateTime dtime = DateTime.Now;
            int tmp_hour = ProTimeProd.cur_time.Hour;

            if (dtime.Hour > tmp_hour)
            {

                ProTimeProd.last_time = ProTimeProd.cur_time;
                ProTimeProd.cur_time = dtime;

                ProTimeProd.LastHour = ProTimeProd.last_time.Hour;
                ProTimeProd.CurHour = ProTimeProd.cur_time.Hour;

                return true;
            }
            return false;
        }
        /*用时分析*/
        public void getProdNumSeque(DateTime startDate, DateTime endDate)
        {
            DateTime start_time = startDate.Date;
            DateTime end_time = endDate.Date.AddDays(1).AddSeconds(-1);
            //int dur_length = getDiffTotalSeconds(start_time, end_time);
            int dur_length = 24 * 3600;

            Console.WriteLine("TimeSeque:" + start_time + " " + end_time);
            log.Info("TimeSeque:" + start_time + " " + end_time);

            try
            {
                _getProdNumSeque(start_time, end_time, dur_length);
            }
            catch (Exception ex)
            {
                log.Error("getProdNumSeque is Error!" + ex.Message);
            }
        }

        /*用时分析*/
        private void _getProdNumSeque(DateTime start_time, DateTime end_time, int dur_length)
        {
            MachineInfo mi = null;
            List<MachineInfo> listUsedTime = new List<MachineInfo>();

            string strSql = "select mi.group_id,mi.group_name,mi.machine_id,mi.machine_name,mi.machine_series,mi.machine_number,isnull(ri.machine_id,0) as mac_id "
                            + " from vw_machine_info mi left join (select distinct machine_id from tb_run_info "
                            + "        where calc_date >= " + start_time.ToString("yyyyMMdd") + " and calc_date <= " + end_time.ToString("yyyyMMdd") + ") ri "
                            + " on mi.machine_id = ri.machine_id where 1=1 and is_prod = 'SXL'";

            try
            {
                dtInfo = dbc.getStrSqlData(strSql);
                DataTable cdt = dtInfo.Copy();

                for (int i = 0; i < cdt.Rows.Count; i++)
                {
                    mi = new MachineInfo();

                    mi.group_id = Convert.ToInt32(cdt.Rows[i]["group_id"]);
                    mi.group_name = cdt.Rows[i]["group_name"].ToString();
                    mi.machine_id = Convert.ToInt32(cdt.Rows[i]["machine_id"]);
                    mi.machine_name = cdt.Rows[i]["machine_name"].ToString();
                    mi.machine_series = cdt.Rows[i]["machine_series"].ToString();
                    mi.machine_number = cdt.Rows[i]["machine_number"].ToString();
                    //int mac_id = Convert.ToInt32(cdt.Rows[i]["mac_id"]);
                    int mac_id = mi.machine_id;

                    _getDeviceProdNumSeque(mi, mac_id, start_time, end_time, dur_length);
                }
            }
            catch (SqlException ex)
            {
                log.Error("getProdNumSeque is Error！" + ex.Message);
            }
            catch (FormatException ex)
            {
                log.Error("getProdNumSeque is Error！" + ex.Message);
            }
            catch (InvalidCastException ex)
            {
                log.Error("getProdNumSeque is Error！" + ex.Message);
            }
            catch (SystemException ex)
            {
                log.Error("getProdNumSeque is Error！" + ex.Message);
            }
            catch (Exception ex)
            {
                log.Error("getProdNumSeque is Error！" + ex.Message);
            }
        }
        /*取设备用时分析*/
        private void _getDeviceProdNumSeque(MachineInfo mi, int mac_id, DateTime start_time, DateTime end_time, int dur_length)
        {
            try
            {
                if (mac_id > 0)
                {
                    TimeSpan dd = end_time.Subtract(start_time);
                    for (int i = 0; i < dd.Days + 1; i++)
                    {
                        string wshift_date = start_time.AddDays(i).ToString("yyyyMMdd");
                        dbc.exespDeviceUsedTime("sp_init_prod_seque_his", wshift_date, wshift_date, mi.machine_id);
                        dbc.exespDeviceUsedTime("sp_pool_data_his", wshift_date, wshift_date, mi.machine_id);
                        
                        if (DateTime.Compare(start_time.AddDays(i).Date, DateTime.Today.AddDays(-2).Date) >= 0)
                        {
                            string strSql = "delete from tb_run_data where machine_id = " + mac_id + " and wshift_date = " + wshift_date;
                            if (dbc.putStrSqlData(strSql))
                            {
                                dbc.exespDeviceUsedTime("sp_init_prod_seque", wshift_date, wshift_date, mi.machine_id);
                            }

                            string strSql1 = "delete from tb_run_data_init where machine_id = " + mac_id + " and wshift_date = " + wshift_date;
                            string strSql2 = "delete from tb_curday_prod_num where machine_id = " + mac_id + " and wshift_date = " + wshift_date;
                            if (dbc.putStrSqlData(strSql1) && dbc.putStrSqlData(strSql2))
                            {
                                dbc.exespDeviceUsedTime("sp_pool_data", wshift_date, wshift_date, mi.machine_id);
                            }
                        }
                    }
                    log.Info("ProdNumSeque Machine:" + mi.machine_id + " " + mi.machine_name);
                }
            }
            catch (SqlException ex)
            {
                log.Error("getDeviceProdNumSeque is Error！ID:" + mi.machine_id + "\n" + ex.Message);
            }
            catch (FormatException ex)
            {
                log.Error("getDeviceProdNumSeque is Error！ID:" + mi.machine_id + "\n" + ex.Message);
            }
            catch (InvalidCastException ex)
            {
                log.Error("getDeviceProdNumSeque is Error！ID:" + mi.machine_id + "\n" + ex.Message);
            }
            catch (SystemException ex)
            {
                log.Error("getDeviceProdNumSeque is Error！ID:" + mi.machine_id + "\n" + ex.Message);
            }
            catch (Exception ex)
            {
                log.Error("getDeviceProdNumSeque is Error！ID:" + mi.machine_id + "\n" + ex.Message);
            }
        }
        /*用时分析*/
        public void getAlarmNumSeque(DateTime startDate, DateTime endDate)
        {
            DateTime start_time = startDate.Date;
            DateTime end_time = endDate.Date.AddDays(1).AddSeconds(-1);
            //int dur_length = getDiffTotalSeconds(start_time, end_time);
            int dur_length = 24 * 3600;

            Console.WriteLine("TimeSeque:" + start_time + " " + end_time);
            log.Info("TimeSeque:" + start_time + " " + end_time);

            try
            {
                _getAlarmNumSeque(start_time, end_time, dur_length);
            }
            catch (Exception ex)
            {
                log.Error("getAlarmNumSeque is Error!" + ex.Message);
            }
        }

        /*用时分析*/
        private void _getAlarmNumSeque(DateTime start_time, DateTime end_time, int dur_length)
        {
            MachineInfo mi = null;
            List<MachineInfo> listUsedTime = new List<MachineInfo>();

            string strSql = "select mi.group_id,mi.group_name,mi.machine_id,mi.machine_name,mi.machine_series,mi.machine_number,isnull(ri.machine_id,0) as mac_id "
                            + " from vw_machine_info mi left join (select distinct machine_id from tb_run_info "
                            + "        where calc_date >= " + start_time.ToString("yyyyMMdd") + " and calc_date <= " + end_time.ToString("yyyyMMdd") + ") ri "
                            + " on mi.machine_id = ri.machine_id where 1=1";
            try
            {
                dtInfo = dbc.getStrSqlData(strSql);
                DataTable cdt = dtInfo.Copy();

                for (int i = 0; i < cdt.Rows.Count; i++)
                {
                    mi = new MachineInfo();

                    mi.group_id = Convert.ToInt32(cdt.Rows[i]["group_id"]);
                    mi.group_name = cdt.Rows[i]["group_name"].ToString();
                    mi.machine_id = Convert.ToInt32(cdt.Rows[i]["machine_id"]);
                    mi.machine_name = cdt.Rows[i]["machine_name"].ToString();
                    mi.machine_series = cdt.Rows[i]["machine_series"].ToString();
                    mi.machine_number = cdt.Rows[i]["machine_number"].ToString();
                    //int mac_id = Convert.ToInt32(cdt.Rows[i]["mac_id"]);
                    int mac_id = mi.machine_id;

                    _getDeviceAlarmNumSeque(mi, mac_id, start_time, end_time, dur_length);
                }
            }
            catch (SqlException ex)
            {
                log.Error("getAlarmNumSeque is Error！ID:" + mi.machine_id + "\n" + ex.Message);
            }
            catch (FormatException ex)
            {
                log.Error("getAlarmNumSeque is Error！ID:" + mi.machine_id + "\n" + ex.Message);
            }
            catch (InvalidCastException ex)
            {
                log.Error("getAlarmNumSeque is Error！ID:" + mi.machine_id + "\n" + ex.Message);
            }
            catch (SystemException ex)
            {
                log.Error("getAlarmNumSeque is Error！ID:" + mi.machine_id + "\n" + ex.Message);
            }
            catch (Exception ex)
            {
                log.Error("getAlarmNumSeque is Error！ID:" + mi.machine_id + "\n" + ex.Message);
            }
        }
        /*取设备用时分析*/
        private void _getDeviceAlarmNumSeque(MachineInfo mi, int mac_id, DateTime start_time, DateTime end_time, int dur_length)
        {
            try
            {
                if (mac_id > 0)
                {
                    TimeSpan dd = end_time.Subtract(start_time);
                    for (int i = 0; i < dd.Days + 1; i++)
                    {
                        string wshift_date = start_time.AddDays(i).ToString("yyyyMMdd");
                        dbc.exespDeviceUsedTime("sp_init_alarm_seque_his", wshift_date, wshift_date, mi.machine_id);

                        if (DateTime.Compare(start_time.AddDays(i).Date, DateTime.Today.AddDays(-2).Date) >= 0)
                        {
                            string strSql = "delete from tb_alarm_history_init where machine_id = " + mac_id + " and wshift_date = " + wshift_date;
                            if (dbc.putStrSqlData(strSql))
                            {
                                dbc.exespDeviceUsedTime("sp_init_alarm_seque", wshift_date, wshift_date, mi.machine_id);
                            }
                        }
                    }
                    log.Info("AlarmNumSeque Machine:" + mi.machine_id + " " + mi.machine_name);
                }
            }
            catch (SqlException ex)
            {
                log.Error("getDeviceAlarmNumSeque is Error！ID:" + mi.machine_id + "\n" + ex.Message);
            }
            catch (FormatException ex)
            {
                log.Error("getDeviceAlarmNumSeque is Error！ID:" + mi.machine_id + "\n" + ex.Message);
            }
            catch (InvalidCastException ex)
            {
                log.Error("getDeviceAlarmNumSeque is Error！ID:" + mi.machine_id + "\n" + ex.Message);
            }
            catch (SystemException ex)
            {
                log.Error("getDeviceAlarmNumSeque is Error！ID:" + mi.machine_id + "\n" + ex.Message);
            }
            catch (Exception ex)
            {
                log.Error("getDeviceAlarmNumSeque is Error！ID:" + mi.machine_id + "\n" + ex.Message);
            }
        }
        public int MachineId
        {
            get
            {
                return _MachineId;
            }
            set
            {
                _MachineId = value;
            }
        }
        public int ParamId
        {
            get
            {
                return _ParamId;
            }
            set
            {
                _ParamId = value;
            }
        }
        public string ParamName
        {
            get
            {
                return _ParamName;
            }
            set
            {
                _ParamName = value;
            }
        }
        public string ParamType
        {
            get
            {
                return _ParamType;
            }
            set
            {
                _ParamType = value;
            }
        }

        public string PRWType
        {
            get
            {
                return _PRWType;
            }
            set
            {
                _PRWType = value;
            }
        }
        public string PFieldsName
        {
            get
            {
                return _PFieldsName;
            }
            set
            {
                _PFieldsName = value;
            }
        }
        public string PFieldsType
        {
            get
            {
                return _PFieldsType;
            }
            set
            {
                _PFieldsType = value;
            }
        }
        public int PFieldsLen
        {
            get
            {
                return _PFieldsLen;
            }
            set
            {
                _PFieldsLen = value;
            }
        }
        public string PDataAddr
        {
            get
            {
                return _PDataAddr;
            }
            set
            {
                _PDataAddr = value;
            }
        }
        public object PDataSetup
        {
            get
            {
                return _PDataSetup;
            }
            set
            {
                _PDataSetup = value;
            }
        }
        public string PDataType
        {
            get
            {
                return _PDataType;
            }
            set
            {
                _PDataType = value;
            }
        }
        public int PDataLen
        {
            get
            {
                return _PDataLen;
            }
            set
            {
                _PDataLen = value;
            }
        }
        public bool PDataIsVisual
        {
            get
            {
                return _PDataIsVisual;
            }
            set
            {
                _PDataIsVisual = value;
            }
        }
        public bool PDataIsHot
        {
            get
            {
                return _PDataIsHot;
            }
            set
            {
                _PDataIsHot = value;
            }
        }
    }
    #region
    public class TimeSeque
    {
        public int run_state { get; set; }
        public int run_duration { get; set; }
        public double dur_rate { get; set; }
        public int last_run_state { get; set; }
        public int next_run_state { get; set; }
        public DateTime state_start_time { get; set; }
        public DateTime state_end_time { get; set; }
        public string read_short_time { get; set; }
        public string read_long_time { get; set; }
        public string read_univer_time { get; set; }
        public string read_formf_time { get; set; }
        public int wshift_id { get; set; }
        public DateTime read_time { get; set; }          //最后采集时间
    }
    public enum PosiClearCmd
    {
        ClearPartCountCmd,
        ClearAcceptedPartCountCmd,
        ClearNGPartCountCmd,
        ClearSumPartCmd,
    }
    public enum PosiRunState
    {
        RunState,
        AlarmState,
        HaltState
    }
    public enum PosiAxisInfo
    {
        Axis_01,
        Axis_02,
        Axis_03,
        Axis_04,
        Axis_05,
        Axis_06,
        Axis_07,
        Axis_08,
        Axis_09,
        Axis_10
    }
    public enum PosiProdCount
    {
        SlPartCount,
        XlPartCount,
        M1SlPartCount,
        M1XlPartCount,
        M2SlPartCount,
        M2XlPartCount,
        M3SlPartCount,
        M3XlPartCount,
        M4SlPartCount,
        M4XlPartCount
    }
    public enum PosiRunTime
    {
        RunTime,
        PowerOnTime
    }
    public enum LeakProdCount
    {
        PartCount,
        AcpPartCount,
        NGPartCount
    }
    public class GooGolConditionTyp
    {
        public int AlarmNo = 0;
        public string AlarmMessage = "";
        public DateTime AlarmTime = DateTime.Now;

        public double J1AxisPos = 0;
        public double J2AxisPos = 0;
        public double J3AxisPos = 0;
        public double J4AxisPos = 0;
        public double J5AxisPos = 0;
        public double J6AxisPos = 0;

        public int ProductName = 0;
        public int PassProdNum = 0;
        public int NPassProdNum = 0;

        public double FeedRate = 0;
        public double FeedSpeed = 0;
        public double SpindleRate = 0;
        public double SpindleSpeed = 0;
    }
    public enum GooGolRunState
    {
        EState_Ready = 0,
        EState_Run = 1,
        EState_Pause = 2,
        EState_Alarm = 3,
        EState_Stop = 4
    }
    public enum GooGolStateIndex
    {
        EState_DI0 = 0,
        EState_DI1 = 1,
        EState_DI2 = 2,
        EState_DO0 = 3,
        EState_DO1 = 4,
        EState_DO2 = 5
    }
    public enum GooGolDI0Index
    {
        EState_BitIn0p0 = 19,
        EState_BitIn0p1 = 18,
        EState_BitIn0p2 = 17,
        EState_BitIn0p3 = 16,
        EState_BitIn0p16 = 3
    }
    public enum GooGolDI1Index
    {
        EState_BitIn1p0 = 19,
        EState_BitIn1p1 = 18,
        EState_BitIn1p2 = 17,
        EState_BitIn1p3 = 16,
        EState_BitIn1p4 = 15,
        EState_BitIn1p5 = 14,
        EState_BitIn1p6 = 13,
        EState_BitIn1p7 = 12,
        EState_BitIn1p8 = 11,
        EState_BitIn1p9 = 10
    }
    public enum GooGolDI2Index
    {
        EState_BitIn2p0 = 19,
        EState_BitIn2p1 = 18,
        EState_BitIn2p2 = 17,
        EState_BitIn2p3 = 16,
        EState_BitIn2p4 = 15,
        EState_BitIn2p5 = 14,
        EState_BitIn2p6 = 13,
        EState_BitIn2p7 = 12
    }
    public enum GooGolDO0Index
    {
        EState_BitOut0p0 = 19,
        EState_BitOut0p1 = 18,
        EState_BitOut0p2 = 17,
        EState_BitOut0p3 = 16,
        EState_BitOut0p4 = 15,
        EState_BitOut0p5 = 14
    }
    public enum GooGolDO1Index
    {
        EState_BitOut1p0 = 19,
        EState_BitOut1p1 = 18,
        EState_BitOut1p2 = 17,
        EState_BitOut1p3 = 16,
        EState_BitOut1p4 = 15,
        EState_BitOut1p5 = 14,
        EState_BitOut1p6 = 13,
        EState_BitOut1p7 = 12,
        EState_BitOut1p8 = 11,
        EState_BitOut1p9 = 10
    }
    public enum GooGolDO2Index
    {
        EState_BitOut2p0 = 19,
        EState_BitOut2p1 = 18,
        EState_BitOut2p2 = 17,
        EState_BitOut2p4 = 15,
        EState_BitOut2p6 = 13,
        EState_BitOut2p7 = 12,
        EState_BitOut2p10 = 9,
        EState_BitOut2p11 = 8,
        EState_BitOut2p12 = 7,
        EState_BitOut2p13 = 6
    }
    public enum GooGolSystemState
    {
        EState_TeachBox = 2,
        EState_RobotState = 3,
        EState_ServoState = 4,
        EState_Coordinate = 8,
        EState_Emergency = 13
    }
    public enum GooGolTeachState
    {
        EState_ModeError = 1,
        EState_TeachingMode = 2,
        EState_PlaybackMode = 3,
        EState_RemoteMode = 4
    }
    public enum GooGolRobotState
    {
        EState_ReadyState = 0,
        EState_RunState = 1,
        EState_PauseState = 2,
        EState_AlarmState = 3,
        EState_InterruptState = 4
    }
    public enum GooGolServoState
    {
        EState_ServoOFF = 0,
        EState_ServoON = 1,
        EState_ServoAlarm = 2,
        EState_ServoAbnormal = 3
    }
    public enum GooGolCrdntState
    {
        EState_JointCrdnt = 0,
        EState_RightAngCrdnt = 1,
        EState_ToolCrdnt = 2,
        EState_WorldCrdnt = 3,
        EState_Cmpnt1Crdnt = 4,
        EState_Cmpnt2Crdnt = 5
    }
    public enum GooGolMonitorA
    {
        ModeError = 0,
        TeachingMode = 1,
        PlaybackMode = 2,
        RemoteMode = 3,
        ReadyState = 4,
        RunState = 5,
        PauseState = 6,
        AlarmState = 7,
        InterruptState = 8,
        ServoOFF = 9,
        ServoON = 10,
        ServoAlarm = 11,
        ServoAbnormal = 12,
        JointCrdnt = 13,
        RightAngCrdnt = 14,
        ToolCrdnt = 15,
        WorldCrdnt = 16,
        Tool1Crdnt = 17,
        Tool2Crdnt = 18,
        Emergency = 19
    }
    public enum GooGolMonitorB
    {
        BitIn0p0 = 0,
        BitIn0p1 = 1,
        BitIn0p2 = 2,
        BitIn0p3 = 3,
        BitIn0p16 = 4,
        BitIn1p0 = 5,
        BitIn1p1 = 6,
        BitIn1p2 = 7,
        BitIn1p3 = 8,
        BitIn1p4 = 9,
        BitIn1p5 = 10,
        BitIn1p6 = 11,
        BitIn1p7 = 12,
        BitIn1p8 = 13,
        BitIn1p9 = 14,
        BitIn2p0 = 15,
        BitIn2p1 = 16,
        BitIn2p2 = 17,
        BitIn2p3 = 18,
        BitIn2p4 = 19,
        BitIn2p5 = 20,
        BitIn2p6 = 21,
        BitIn2p7 = 22,
        BitOut1p0 = 23,
        BitOut1p1 = 24,
        BitOut1p2 = 25,
        BitOut1p3 = 26,
        BitOut1p4 = 27,
        BitOut1p5 = 28,
        BitOut1p6 = 29,
        BitOut1p7 = 30,
        BitOut1p8 = 31,
        BitOut1p9 = 32,
        BitOut2p0 = 33,
        BitOut2p1 = 34,
        BitOut2p2 = 35,
        BitOut2p4 = 36,
        BitOut2p6 = 37,
        BitOut2p7 = 38,
        BitOut2p10 = 39,
        BitOut2p11 = 40,
        BitOut2p12 = 41,
        BitOut2p13 = 42
    }
    #endregion
}
