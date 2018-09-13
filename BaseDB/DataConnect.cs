using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace BaseCollect
{
    public class DataConnect
    {
        log4net.ILog log = log4net.LogManager.GetLogger("DataConnect");

        public static string DataSource = "localhost";
        public static string DataBase = "JnrsDB";
        public static string DataAccount = "sa";
        public static string DataPassword = "1`q";

        DataSet ds = new DataSet();
        DataTable dtInfo = new DataTable();

        //数据库初始化
        public DataConnect()
        {
            //log.Info("DataConnect Init!");
        }
        //数据库初始化
        public DataConnect(string dataSource, string dataBase, string dataAccount, string dataPassword)
        {
            //log.Info("DataConnect Init!");
            DataSource = dataSource;
            DataBase = dataBase;
            DataAccount = dataAccount;
            DataPassword = dataPassword;
        }

        private static string ConnectionString
        {
            get
            {
                string _conn = "Data Source = " + DataSource + "; DataBase = " + DataBase + "; User Id = " + DataAccount + "; Password = " + DataPassword + ";Integrated Security=false;Enlist=true;Pooling=true;Max Pool Size=5000;Min Pool Size=5;Connection Lifetime=3000;packet size=5000";
                return _conn;
            }
        }
        public SqlConnection sqlconn
        {
            get
            {
                log4net.ILog log = log4net.LogManager.GetLogger("DataConnect");
                SqlConnection conn = new SqlConnection(ConnectionString);

                try
                {
                    if (conn.State == ConnectionState.Closed)//增加一个判断语句
                    {
                        conn.Close(); conn.Open();
                    }
                    //if (conn == null)
                    //{
                    //    conn.Open();
                    //}
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    else if (conn.State == ConnectionState.Broken)
                    {
                        conn.Close();
                        conn.Open();
                    }
                    //log.Info("数据库初始化连接成功！" + "Data Source = " + DataSource + "; DataBase = " + DataBase + " ServerVersion: " + conn.ServerVersion + "  State: " + conn.State);
                }
                catch (InvalidOperationException cep)
                {
                    log.Error("数据库初始化连接失败！" + "Data Source = " + DataSource + "; DataBase = " + DataBase + "\n" + cep.Message);
                    return null;
                }
                catch (SqlException cep)
                {
                    log.Error("数据库初始化连接失败！" + "Data Source = " + DataSource + "; DataBase = " + DataBase + "\n" + cep.Message);
                    return null;
                }
                catch (SystemException ex)
                {
                    log.Error("数据库初始化连接失败！" + "Data Source = " + DataSource + "; DataBase = " + DataBase + "\n" + ex.Message);
                    return null;
                }
                catch (Exception ex)
                {
                    log.Error("数据库初始化连接失败！" + "Data Source = " + DataSource + "; DataBase = " + DataBase + "\n" + ex.Message);
                    return null;
                }
                return conn;
            }
        }
        public bool initDBConnect()
        {
            if (reOpenConnect())
            {
                return true;
            }

            return false;
        }

        public bool reOpenConnect()
        {
            try
            {
                if (sqlconn.State == ConnectionState.Closed)//增加一个判断语句
                {
                    sqlconn.Close();
                    sqlconn.Open();
                }
                if (sqlconn == null)
                {
                    sqlconn.Open();
                }
                else if (sqlconn.State == ConnectionState.Closed)
                {
                    sqlconn.Open();
                }
                else if (sqlconn.State == ConnectionState.Broken)
                {
                    sqlconn.Close();
                    sqlconn.Open();
                }
            }
            catch (InvalidOperationException ex)
            {
                log.Error("reOpenConnect is Error!" + "\n" + ex.Message);
                return false;
            }
            catch (SqlException cep)
            {
                log.Error("reOpenConnect is Error!" + "\n" + cep.Message);
                return false;
            }
            catch (SystemException ex)
            {
                log.Error("reOpenConnect is Error!" + "\n" + ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                log.Error("未配置数据源连接信息! " + "\n" + ex.Message);
                return false;
            }

            return true;
        }
        
        public bool disConnect()
        {
            try
            {
                sqlconn.Close();
            }
            catch (InvalidOperationException ex)
            {
                log.Error("disConnect is Error!" + "\n" + ex.Message);
                return false;
            }
            catch (SqlException ex)
            {
                log.Error("disConnect is Error!" + "\n" + ex.Message);
                return false;
            }
            catch (SystemException ex)
            {
                log.Error("disConnect is Error!" + "\n" + ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                log.Error("disConnect is Error!" + "\n" + ex.Message);
                return false;
            }
            return true;
        }

        public void endDBConnect()
        {
            try
            {
                //关闭数据源 
                sqlconn.Close();
            }
            catch (InvalidOperationException ex)
            {
                log.Error("数据库未正确关闭! " + "\n" + ex.Message);
            }
            catch (SqlException ex)
            {
                log.Error("数据库未正确关闭! " + "\n" + ex.Message);
            }
            catch (SystemException ex)
            {
                log.Error("数据库未正确关闭! " + "\n" + ex.Message);
            }
            catch (Exception ex)
            {
                log.Error("数据库未正确关闭! " + "\n" + ex.Message);
            }
            log.Info("数据库结束动作!");
        }
        //数据库读取
        public DataTable getStrSqlData(string strSql)
        {
            try
            {
                ds.Clear();
                dtInfo.Clear();

                using (SqlCommand sqlcmd = new SqlCommand(strSql, sqlconn))
                {
                    sqlcmd.CommandTimeout = 2000;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(ds);
                    dtInfo = ds.Tables[0];
                }
            }
            catch (NullReferenceException cep)
            {
                log.Error("Read Record is Error!\n" + strSql + "\n" + cep.Message);
                return null;
            }
            catch (InvalidOperationException cep)
            {
                log.Error("Read Record is Error!\n" + strSql + "\n" + cep.Message);
                return null;
            }
            catch (SqlException cep)
            {
                log.Error("Read Record is Error!\n" + strSql + "\n" + cep.Message);
                return null;
            }
            catch (SystemException cep)
            {
                log.Error("Read Record is Error!\n" + strSql + "\n" + cep.Message);
                return null;
            }
            catch (Exception cep)
            {
                log.Error("Read Record is Error!\n" + strSql + "\n" + cep.Message);
                return null;
            }
            return dtInfo;
        }
        //数据库读取
        public DataTable getExStrSqlData(string strSql)
        {
            try
            {
                ds.Clear();
                dtInfo.Clear();
            }
            catch (Exception cep)
            {
                log.Error("Clear Record is Error!\n" + cep.Message);
            }
            ///////////////////////////////
            try
            {
                using (SqlCommand sqlcmd = new SqlCommand(strSql, sqlconn))
                {
                    sqlcmd.CommandTimeout = 2000;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(ds);
                    dtInfo = ds.Tables[0];
                }
            }
            catch (NullReferenceException cep)
            {
                log.Error("Read Record is Error!" + strSql + "\n" + cep.Message);
                throw new Exception("Read Record is Error!" + strSql + "\n" + cep.Message);
            }
            catch (InvalidOperationException cep)
            {
                log.Error("Read Record is Error!" + strSql + "\n" + cep.Message);
                throw new Exception("Read Record is Error!" + strSql + "\n" + cep.Message);
            }
            catch (SqlException cep)
            {
                log.Error("Read Record is Error!" + strSql + "\n" + cep.Message);
                throw new Exception("Read Record is Error!" + strSql + "\n" + cep.Message);
            }
            catch (SystemException cep)
            {
                log.Error("Read Record is Error!\n" + strSql + "\n" + cep.Message);
                throw new Exception("Read Record is Error!" + strSql + "\n" + cep.Message);
            }
            catch (Exception cep)
            {
                log.Error("Read Record is Error!" + strSql + "\n" + cep.Message);
                throw new Exception("Read Record is Error!" + strSql + "\n" + cep.Message);
            }
            return dtInfo;
        }
        //数据库写入
        public bool putStrSqlData(string strSql)
        {
            try
            {
                using (SqlCommand sqlcmd = new SqlCommand(strSql, sqlconn))
                {
                    sqlcmd.ExecuteNonQuery();
                }
            }
            catch (NullReferenceException cep)
            {
                log.Error("Insert Record is Error!\n" + strSql + "\n" + cep.Message);
                return false;
            }
            catch (InvalidOperationException cep)
            {
                log.Error("Insert Record is Error!\n" + strSql + "\n" + cep.Message);
                return false;
            }
            catch (SqlException cep)
            {
                log.Error("Insert Record is Error!\n" + strSql + "\n" + cep.Message);
                return false;
            }
            catch (SystemException cep)
            {
                log.Error("Insert Record is Error!\n" + strSql + "\n" + cep.Message);
                return false;
            }
            catch (Exception cep)
            {
                log.Error("Insert Record is Error!\n" + strSql + "\n" + cep.Message);
                return false;
            }
            return true;
        }
        public void putStandardData(StandardData sd, DateTime dtime)
        {
            //write database
            string strSql = "select wshift_id,wshift_date from tb_shift_date where start_dtime = '" + dtime + "'" 
                            + " and wshift_date >= " + DateTime.Today.AddDays(-1).ToString("yyyyMMdd");
            try
            {
                //exe write
                dtInfo = getExStrSqlData(strSql);

                if (dtInfo != null && dtInfo.Rows.Count > 0 && !Convert.IsDBNull(dtInfo.Rows[0]["wshift_id"]))
                {
                    ShiftInfo.ShiftId = Convert.ToInt32(dtInfo.Rows[0]["wshift_id"]);
                    ShiftInfo.ShiftDate = dtInfo.Rows[0]["wshift_date"].ToString();
                }

                _putStandardData(sd, dtime);
                _putStandardDataHis(sd, dtime);
            }
            catch (NullReferenceException cep)
            {
                log.Error("putStandardData is Error!\n" + strSql + "\n" + cep.Message);
            }
            catch (InvalidOperationException cep)
            {
                log.Error("putStandardData is Error!\n" + strSql + "\n" + cep.Message);
            }
            catch (SqlException cep)
            {
                log.Error("putStandardData is Error!\n" + strSql + "\n" + cep.Message);
            }
            catch (SystemException cep)
            {
                log.Error("putStandardData is Error!\n" + strSql + "\n" + cep.Message);
            }
            catch (Exception cep)
            {
                log.Error("putStandardData is Error!\n" + strSql + "\n" + cep.Message);
            }
        }
        private void _putStandardData(StandardData sd, DateTime dtime)
        {
            //write database
            string strSql = "insert into tb_standard_data(calc_date,machine_id,wshift_id,wshift_date,mac_run_state,mac_alarm_state,mac_pause_state, "
                            + " run_state,slcount,xlcount,acp_count,ng_count,sum_count,main_program,running_program,spindle_speed,spindle_override, "
                            + " feed_speed,feed_override,axis_pos_x,axis_pos_y,axis_pos_z,spload_x,spload_y,spload_z,svload_x,svload_y,svload_z, "
                            + " alarm_no,read_time)values ( " 
                            + dtime.ToString("yyyyMMdd") + "," + sd.machine_id + "," + ShiftInfo.ShiftId + ",'" + ShiftInfo.ShiftDate + "'," 
                            + sd.mac_run_state + "," + sd.mac_alarm_state + "," + sd.mac_pause_state + "," + sd.run_state + "," + sd.slcount + "," 
                            + sd.xlcount + "," + sd.acp_count + "," + sd.ng_count + "," + sd.sum_count + ",'" + sd.main_program + "','" + sd.running_program + "'," 
                            + sd.spindle_speed + "," + sd.spindle_override + "," + sd.feed_speed + "," + sd.feed_override + "," + sd.axis_pos_x + "," 
                            + sd.axis_pos_y + "," + sd.axis_pos_z + "," + sd.spload_x + "," + sd.spload_y + "," + sd.spload_z + "," + sd.svload_x + "," 
                            + sd.svload_y + "," + sd.svload_z + "," + sd.alarm_no + ",'" + dtime + "')";
            //exe write
            try
            {
                if (!putStrSqlData(strSql))
                {
                    log.Info("标准化数据写入失败!执行putStandardData!ID" + sd.machine_id);
                    delayTime(1);
                }
            }
            catch (NullReferenceException cep)
            {
                log.Error("标准化数据错误!执行putStandardData!ID:" + sd.machine_id + "\n" + cep.Message);
            }
            catch (InvalidOperationException cep)
            {
                log.Error("标准化数据错误!执行putStandardData!ID:" + sd.machine_id + "\n" + cep.Message);
            }
            catch (SqlException cep)
            {
                log.Error("标准化数据错误!执行putStandardData!ID:" + sd.machine_id + "\n" + cep.Message);
            }
            catch (SystemException cep)
            {
                log.Error("标准化数据错误!执行putStandardData!ID:" + sd.machine_id + "\n" + cep.Message);
            }
            catch (Exception cep)
            {
                log.Error("标准化数据错误!执行putStandardData!ID:" + sd.machine_id + "\n" + cep.Message);
            }
        }
        private void _putStandardDataHis(StandardData sd, DateTime dtime)
        {
            //write database
            string strSql = "insert into tb_standard_data_his(calc_date,machine_id,wshift_id,wshift_date,mac_run_state,mac_alarm_state,mac_pause_state, "
                            + " run_state,slcount,xlcount,acp_count,ng_count,sum_count,main_program,running_program,spindle_speed,spindle_override, "
                            + " feed_speed,feed_override,axis_pos_x,axis_pos_y,axis_pos_z,spload_x,spload_y,spload_z,svload_x,svload_y,svload_z, "
                            + " alarm_no,read_time)values ( "
                            + dtime.ToString("yyyyMMdd") + "," + sd.machine_id + "," + ShiftInfo.ShiftId + ",'" + ShiftInfo.ShiftDate + "'," 
                            + sd.mac_run_state + "," + sd.mac_alarm_state + "," + sd.mac_pause_state + "," + sd.run_state + "," + sd.slcount + "," 
                            + sd.xlcount + "," + sd.acp_count + "," + sd.ng_count + "," + sd.sum_count + ",'" + sd.main_program + "','" + sd.running_program + "'," 
                            + sd.spindle_speed + "," + sd.spindle_override + "," + sd.feed_speed + "," + sd.feed_override + "," + sd.axis_pos_x + "," 
                            + sd.axis_pos_y + "," + sd.axis_pos_z + "," + sd.spload_x + "," + sd.spload_y + "," + sd.spload_z + "," + sd.svload_x + "," 
                            + sd.svload_y + "," + sd.svload_z + "," + sd.alarm_no + ",'" + dtime + "')";
            //exe write
            try
            {
                if (!putStrSqlData(strSql))
                {
                    log.Info("标准化数据写入失败!执行putStandardDataHis!ID" + sd.machine_id);
                    delayTime(1);
                }
            }
            catch (NullReferenceException cep)
            {
                log.Error("标准化数据错误!执行putStandardDataHis!ID:" + sd.machine_id + "\n" + cep.Message);
            }
            catch (InvalidOperationException cep)
            {
                log.Error("标准化数据错误!执行putStandardDataHis!ID:" + sd.machine_id + "\n" + cep.Message);
            }
            catch (SqlException cep)
            {
                log.Error("标准化数据错误!执行putStandardDataHis!ID:" + sd.machine_id + "\n" + cep.Message);
            }
            catch (SystemException cep)
            {
                log.Error("标准化数据错误!执行putStandardDataHis!ID:" + sd.machine_id + "\n" + cep.Message);
            }
            catch (Exception cep)
            {
                log.Error("标准化数据错误!执行putStandardDataHis!ID:" + sd.machine_id + "\n" + cep.Message);
            }
        }
        public void putAlarmHistory(MachineInfo mi, List<ushort> list_alarm_no, int alarm_arg, List<string> list_alarm_message, DateTime dtime)
        {
            try
            {
                _putAlarmHistory(mi, list_alarm_no, alarm_arg, list_alarm_message, dtime);
            }
            catch (Exception ex)
            {
                log.Info("设备报警信息写入失败，请检查:" + ex.Message);
            }
        }
        private void _putAlarmHistory(MachineInfo mi, List<ushort> list_alarm_no, int alarm_arg, List<string> list_alarm_message, DateTime dtime)
        {
            string strSql = "";

            try
            {
                AlarmInfo ai = new AlarmInfo();
                for (int i = 0; i < list_alarm_no.Count; i++)
                {
                    ai.machine_id = mi.machine_id;
                    ai.axis_no = 0;
                    ai.axis_num = 0;
                    ai.alarm_no = list_alarm_no[i];
                    ai.alarm_group = alarm_arg;
                    if (list_alarm_message != null && i < list_alarm_message.Count)
                    {
                        ai.alarm_msg = list_alarm_message[i];
                    }
                    else
                    {
                        ai.alarm_msg = "";
                    }
                    ai.read_time = dtime;

                    strSql = "select count(1) as alarm_num from dbo.tb_alarm_history where machine_id = " + mi.machine_id
                                + " and alarm_grp = " + ai.alarm_group + " and alarm_no = " + ai.alarm_no
                                + " and axis_no = " + ai.axis_no + " and axis_num = " + ai.axis_num + " and read_time = \'" + ai.read_time + "\'";

                    dtInfo = getStrSqlData(strSql);

                    int count = 1;

                    if (dtInfo.Rows.Count > 0 && !Convert.IsDBNull(dtInfo.Rows[0]["alarm_num"]))
                    {
                        count = Convert.ToInt32(dtInfo.Rows[0]["alarm_num"]);
                    }

                    //log.Info("已存在该报警记录数:" + count);
                    if (count == 0)
                    {
                        strSql = "insert into dbo.tb_alarm_history(calc_date,machine_id,wshift_id,wshift_date,alarm_start_no,alarm_end_no,alarm_grp, "
                                 + " alarm_no,alarm_msg,axis_no,axis_num,read_time)" + "values (" + DateTime.Now.ToString("yyyyMMdd") + "," + mi.machine_id + ","
                                 + ShiftInfo.ShiftId + ",'" + ShiftInfo.ShiftDate + "'," + 0 + "," + 0 + "," + ai.alarm_group + "," + ai.alarm_no + ",\'"
                                 + ai.alarm_msg + "\'," + ai.axis_no + "," + ai.axis_num + ",'" + ai.read_time + "')";

                        putStrSqlData(strSql);

                        strSql = "insert into dbo.tb_alarm_history_his(calc_date,machine_id,wshift_id,wshift_date,alarm_start_no,alarm_end_no,alarm_grp, "
                                 + " alarm_no,alarm_msg,axis_no,axis_num,read_time)" + "values (" + DateTime.Now.ToString("yyyyMMdd") + "," + mi.machine_id + ","
                                 + ShiftInfo.ShiftId + ",'" + ShiftInfo.ShiftDate + "'," + 0 + "," + 0 + "," + ai.alarm_group + "," + ai.alarm_no + ",\'"
                                 + ai.alarm_msg + "\'," + ai.axis_no + "," + ai.axis_num + ",'" + ai.read_time + "')";

                        putStrSqlData(strSql);
                    }
                }
            }
            catch (NullReferenceException ex)
            {
                log.Error("设备报警信息写入失败，请检查SQL:" + strSql + "---" + ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                log.Error("设备报警信息写入失败，请检查SQL:" + strSql + "---" + ex.Message);
            }
            catch (SqlException ex)
            {
                log.Error("设备报警信息写入失败，请检查SQL:" + strSql + "---" + ex.Message);
            }
            catch (SystemException ex)
            {
                log.Error("设备报警信息写入失败，请检查SQL:" + strSql + "---" + ex.Message);
            }
            catch (Exception ex)
            {
                log.Error("设备报警信息写入失败，请检查SQL:" + strSql + "---" + ex.Message);
            }
        }
        public void putSDToRunInfo(MachineInfo tmi, DateTime dtime)
        {
            try
            {
                _putSDToRunInfo(tmi, dtime);

                string calc_date = dtime.ToString("yyyyMMdd");
                //exe write

                exespDeviceUsedTime("sp_init_run_seque", ShiftInfo.ShiftDate, ShiftInfo.ShiftDate, tmi.machine_id);
                exespDeviceUsedTime("sp_init_state_seque", ShiftInfo.ShiftDate, ShiftInfo.ShiftDate, tmi.machine_id);
            }
            catch (NullReferenceException ep)
            {
                log.Error("运行sp_init_run_seque错误!ID:" + tmi.machine_id + "\n" + ep.Message);
            }
            catch (InvalidOperationException ep)
            {
                log.Error("运行sp_init_run_seque错误!ID:" + tmi.machine_id + "\n" + ep.Message);
            }
            catch (SqlException ep)
            {
                log.Error("运行sp_init_run_seque错误!ID:" + tmi.machine_id + "\n" + ep.Message);
            }
            catch (SystemException ep)
            {
                log.Error("运行sp_init_run_seque错误!ID:" + tmi.machine_id + "\n" + ep.Message);
            }
            catch (Exception ep)
            {
                log.Error("运行sp_init_run_seque错误!ID:" + tmi.machine_id + "\n" + ep.Message);
            }
        }
        private void _putSDToRunInfo(MachineInfo tmi, DateTime dtime)
        {
            string strSql = "";
            strSql = " insert into tb_run_info(calc_date,machine_id,wshift_id,wshift_date,main_prog_num,running_prog_num,run_state,act_spindle_speed_0,"
                     + "      act_spindle_override_0,act_feed_speed_0,act_feed_override_0,sp_load_0,sp_load_1,sp_load_2,sv_load_0,sv_load_1,sv_load_2,read_time) "
                     + " select top 1 tsd.calc_date,tsd.machine_id,tsd.wshift_id,tsd.wshift_date,tsd.main_program,tsd.running_program,tsd.run_state,"
                     + "        tsd.spindle_speed,tsd.spindle_override,tsd.feed_speed,tsd.feed_override,spload_x,spload_y,spload_z,svload_x,svload_y, "
                     + "        svload_z,tsd.read_time "
                     + "   from tb_standard_data tsd where tsd.machine_id = " + tmi.machine_id + " and tsd.read_time = '" + dtime + "'"
                     + "    and tsd.wshift_date = " + ShiftInfo.ShiftDate;

            //exe write
            try
            {
                if (!putStrSqlData(strSql))
                {
                    log.Info("运行信息转换数据写入失败!ID" + tmi.machine_id);
                    delayTime(1);
                }
            }
            catch (NullReferenceException ep)
            {
                log.Error("运行信息转换数据错误!ID:" + tmi.machine_id + "\n" + ep.Message);
            }
            catch (InvalidOperationException ep)
            {
                log.Error("运行信息转换数据错误!ID:" + tmi.machine_id + "\n" + ep.Message);
            }
            catch (SqlException ep)
            {
                log.Error("运行信息转换数据错误!ID:" + tmi.machine_id + "\n" + ep.Message);
            }
            catch (SystemException ep)
            {
                log.Error("运行信息转换数据错误!ID:" + tmi.machine_id + "\n" + ep.Message);
            }
            catch (Exception ep)
            {
                log.Error("运行信息转换数据错误!ID:" + tmi.machine_id + "\n" + ep.Message);
            }
        }
        public void putSDToRunData(MachineInfo tmi, DateTime dtime)
        {
            try
            {
                _putSDToRunData(tmi, dtime);
            }
            catch (Exception ep)
            {
                log.Error("putSDToRunData!ID:" + tmi.machine_id + "\n" + ep.Message);
            }
        }
        private void _putSDToRunData(MachineInfo tmi, DateTime dtime)
        {
            string calc_date = dtime.ToString("yyyyMMdd");
            //exe write
            try
            {
                exespDeviceProdNum("sp_init_prod_seque", ShiftInfo.ShiftDate, ShiftInfo.ShiftDate, tmi.machine_id);
                exespDeviceProdNum("sp_pool_data", ShiftInfo.ShiftDate, ShiftInfo.ShiftDate, tmi.machine_id);
            }
            catch (NullReferenceException ep)
            {
                log.Error("putSDToRunData!ID:" + tmi.machine_id + "\n" + ep.Message);
            }
            catch (InvalidOperationException ep)
            {
                log.Error("putSDToRunData!ID:" + tmi.machine_id + "\n" + ep.Message);
            }
            catch (SqlException ep)
            {
                log.Error("putSDToRunData!ID:" + tmi.machine_id + "\n" + ep.Message);
            }
            catch (SystemException ep)
            {
                log.Error("putSDToRunData!ID:" + tmi.machine_id + "\n" + ep.Message);
            }
            catch (Exception ep)
            {
                log.Error("putSDToRunData!ID:" + tmi.machine_id + "\n" + ep.Message);
            }
        }
        public void putDeviceRunToExt(MachineInfo tmi, DateTime dtime)
        {
            try
            {
                _putDeviceRunToExt(tmi, dtime);
            }
            catch (Exception ep)
            {
                log.Error("putDeviceRunToExt!ID:" + tmi.machine_id + "\n" + ep.Message);
            }
        }
        private void _putDeviceRunToExt(MachineInfo tmi, DateTime dtime)
        {
            string calc_date = dtime.ToString("yyyyMMdd");
            //exe write
            try
            {
                exespDeviceDataExt("sp_init_run_seque_ext", ShiftInfo.ShiftDate, ShiftInfo.ShiftDate, tmi.machine_id, dtime);
                exespDeviceDataExt("sp_init_state_seque_ext", ShiftInfo.ShiftDate, ShiftInfo.ShiftDate, tmi.machine_id, dtime);
            }
            catch (NullReferenceException ep)
            {
                log.Error("putDeviceRunToExt!ID:" + tmi.machine_id + "\n" + ep.Message);
            }
            catch (InvalidOperationException ep)
            {
                log.Error("putDeviceRunToExt!ID:" + tmi.machine_id + "\n" + ep.Message);
            }
            catch (SqlException ep)
            {
                log.Error("putDeviceRunToExt!ID:" + tmi.machine_id + "\n" + ep.Message);
            }
            catch (SystemException ep)
            {
                log.Error("putDeviceRunToExt!ID:" + tmi.machine_id + "\n" + ep.Message);
            }
            catch (Exception ep)
            {
                log.Error("putDeviceRunToExt!ID:" + tmi.machine_id + "\n" + ep.Message);
            }
        }
        public void putDeviceDataToExt(MachineInfo tmi, DateTime dtime)
        {
            try
            {
                _putDeviceDataToExt(tmi, dtime);
            }
            catch (Exception ep)
            {
                log.Error("putDeviceDataToExt!ID:" + tmi.machine_id + "\n" + ep.Message);
            }
        }
        private void _putDeviceDataToExt(MachineInfo tmi, DateTime dtime)
        {
            string calc_date = dtime.ToString("yyyyMMdd");
            //exe write
            try
            {
                exespDeviceDataExt("sp_init_prod_seque_ext", ShiftInfo.ShiftDate, ShiftInfo.ShiftDate, tmi.machine_id, dtime);
                exespDeviceDataExt("sp_pool_data_ext", ShiftInfo.ShiftDate, ShiftInfo.ShiftDate, tmi.machine_id, dtime);
            }
            catch (NullReferenceException ep)
            {
                log.Error("putDeviceDataToExt!ID:" + tmi.machine_id + "\n" + ep.Message);
            }
            catch (InvalidOperationException ep)
            {
                log.Error("putDeviceDataToExt!ID:" + tmi.machine_id + "\n" + ep.Message);
            }
            catch (SqlException ep)
            {
                log.Error("putDeviceDataToExt!ID:" + tmi.machine_id + "\n" + ep.Message);
            }
            catch (SystemException ep)
            {
                log.Error("putDeviceDataToExt!ID:" + tmi.machine_id + "\n" + ep.Message);
            }
            catch (Exception ep)
            {
                log.Error("putDeviceDataToExt!ID:" + tmi.machine_id + "\n" + ep.Message);
            }
        }
        public void putSDToAxisPos(MachineInfo tmi, DateTime dtime)
        {
            try
            {
                //_putSDToAxisPos(tmi, dtime);
            }
            catch (Exception ep)
            {
                log.Error("putSDToAxisPos!ID:" + tmi.machine_id + "\n" + ep.Message);
            }
        }
        private void _putSDToAxisPos(MachineInfo tmi, DateTime dtime)
        {
            string strSql = "";
            strSql = " insert into tb_pos_axis(calc_date,machine_id,wshift_id,wshift_date,axis_pos_a,axis_pos_b,axis_pos_c,read_time) "
                     + " select top 1 tsd.calc_date,tsd.machine_id,tsd.wshift_id,tsd.wshift_date,axis_pos_x,axis_pos_y,axis_pos_z,tsd.read_time from tb_standard_data tsd "
                     + " where tsd.machine_id = " + tmi.machine_id + " and tsd.read_time = '" + dtime + "' and tsd.wshift_date = " + ShiftInfo.ShiftDate;

            //exe write
            try
            {
                if (!putStrSqlData(strSql))
                {
                    log.Info("putSDToAxisPos!ID" + tmi.machine_id);
                    delayTime(1);
                }
            }
            catch (NullReferenceException ep)
            {
                log.Error("putSDToAxisPos!ID:" + tmi.machine_id + "\n" + ep.Message);
            }
            catch (InvalidOperationException ep)
            {
                log.Error("putSDToAxisPos!ID:" + tmi.machine_id + "\n" + ep.Message);
            }
            catch (SqlException ep)
            {
                log.Error("putSDToAxisPos!ID:" + tmi.machine_id + "\n" + ep.Message);
            }
            catch (SystemException ep)
            {
                log.Error("putSDToAxisPos!ID:" + tmi.machine_id + "\n" + ep.Message);
            }
            catch (Exception ep)
            {
                log.Error("putSDToAxisPos!ID:" + tmi.machine_id + "\n" + ep.Message);
            }
        }
        public void putUOToAxisPos(MachineInfo tmi, int curShift, List<float> listAxisPos)
        {
            DateTime dtime = DateTime.Now;
            try
            {
                _putUOToAxisPos(tmi, curShift, listAxisPos, dtime);
            }
            catch (Exception ep)
            {
                log.Error("putUOToAxisPos!ID:" + tmi.machine_id + "\n" + ep.Message);
            }
        }
        private void _putUOToAxisPos(MachineInfo tmi, int curShift, List<float> listAxisPos, DateTime dtime)
        {
            DateTime wtime = dtime.AddHours(-8);
            string strSql = "";

            strSql = "select count(1) as axis_num from tb_pos_axis where read_time = '" + dtime + "' and machine_id = " + tmi.machine_id
                     + " and wshift_date = " + wtime.ToString("yyyyMMdd");

            try
            {
                //dtInfo = getStrSqlData(strSql).Copy();

                int count = 1;

                //if (dtInfo.Rows.Count > 0 && !Convert.IsDBNull(dtInfo.Rows[0]["axis_num"]))
                //{
                //    count = Convert.ToInt32(dtInfo.Rows[0]["axis_num"]);
                //}

                count = 0;
                if (count == 0 && listAxisPos != null && listAxisPos.Count > 0)
                {
                    string vstrSql = " insert into tb_pos_axis(calc_date,machine_id,wshift_id,wshift_date,axis_pos_a,axis_pos_b,axis_pos_c,read_time) "
                             + " values (" + wtime.ToString("yyyyMMdd") + "," + tmi.machine_id + "," + curShift + ",'" + ShiftInfo.ShiftDate + "',"
                             + listAxisPos[0] + "," + listAxisPos[1] + "," + listAxisPos[2] + ",'" + dtime + "')";

                    if (!putStrSqlData(vstrSql))
                    {
                        log.Info("坐标信息数据写入失败!ID" + tmi.machine_id + " " + vstrSql);
                        delayTime(1);
                    }
                }
            }
            catch (NullReferenceException ep)
            {
                log.Error("putSDToAxisPos!ID:" + tmi.machine_id + "\n" + ep.Message);
            }
            catch (InvalidOperationException ep)
            {
                log.Error("putSDToAxisPos!ID:" + tmi.machine_id + "\n" + ep.Message);
            }
            catch (SqlException ep)
            {
                log.Error("putSDToAxisPos!ID:" + tmi.machine_id + "\n" + ep.Message);
            }
            catch (SystemException ep)
            {
                log.Error("putSDToAxisPos!ID:" + tmi.machine_id + "\n" + ep.Message);
            }
            catch (Exception ep)
            {
                log.Error("putSDToAxisPos!ID:" + tmi.machine_id + "\n" + ep.Message);
            }
        }
        public void putSDToProdHis(MachineInfo mi, StandardData sd, string prod_class, DateTime dtime)
        {
            DateTime wtime = dtime.AddHours(-8);
            string strSql = "";

            strSql = "select count(1) as ecode_num from tb_product_his where product_ecode = '" + sd.e2d_code + "' and stage_group_id = "
                     + mi.group_id + " and stage_mac_id = " + mi.machine_id + " and calc_date = " + wtime.ToString("yyyyMMdd");

            try
            {
                dtInfo = getStrSqlData(strSql);

                int count = 1;

                if (dtInfo.Rows.Count > 0 && !Convert.IsDBNull(dtInfo.Rows[0]["ecode_num"]))
                {
                    count = Convert.ToInt32(dtInfo.Rows[0]["ecode_num"]);
                }

                int product_id = getProductInfo(mi, prod_class);
                if (count == 0)
                {
                    strSql = " insert into tb_product_his(calc_date,wshift_id,wshift_date,product_id,product_ecode,stage_name,stage_desp, "
                             + "      stage_group_id,stage_mac_id,stage_emp_id,stage_time) values(" + wtime.ToString("yyyyMMdd") + ","
                             + ShiftInfo.ShiftId + ",'" + ShiftInfo.ShiftDate + "'," + product_id + ",'" + sd.e2d_code + "','" + sd.e2d_stage + "','"
                             + mi.machine_id + "'," + mi.group_id + "," + mi.machine_id + "," + "10" + ",'" + dtime + "')";

                    if (!putStrSqlData(strSql))
                    {
                        log.Info("加工历史信息写入失败!ID" + mi.machine_id);
                    }
                }
            }
            catch (NullReferenceException ep)
            {
                log.Error("putSDToProdHis!ID:" + mi.machine_id + "\n" + ep.Message);
            }
            catch (InvalidOperationException ep)
            {
                log.Error("putSDToProdHis!ID:" + mi.machine_id + "\n" + ep.Message);
            }
            catch (SqlException ep)
            {
                log.Error("putSDToProdHis!ID:" + mi.machine_id + "\n" + ep.Message);
            }
            catch (SystemException ep)
            {
                log.Error("putSDToProdHis!ID:" + mi.machine_id + "\n" + ep.Message);
            }
            catch (Exception ep)
            {
                log.Error("putSDToProdHis!ID:" + mi.machine_id + "\n" + ep.Message);
            }
        }
        private int getProductInfo(MachineInfo mi, string prod_class)
        {
            string strSql = "";
            int product_id = 0;

            strSql = " select product_id from tb_product_info where product_no = '" + prod_class + "'";

            //exe write
            try
            {
                dtInfo = getStrSqlData(strSql);

                if (dtInfo.Rows.Count > 0 && !Convert.IsDBNull(dtInfo.Rows[0]["product_id"]))
                {
                    product_id = Convert.ToInt32(dtInfo.Rows[0]["product_id"]);
                }
            }
            catch (NullReferenceException ep)
            {
                log.Error("getProductInfo!ID:" + mi.machine_id + "\n" + ep.Message);
            }
            catch (InvalidOperationException ep)
            {
                log.Error("getProductInfo!ID:" + mi.machine_id + "\n" + ep.Message);
            }
            catch (SqlException ep)
            {
                log.Error("getProductInfo!ID:" + mi.machine_id + "\n" + ep.Message);
            }
            catch (SystemException ep)
            {
                log.Error("getProductInfo!ID:" + mi.machine_id + "\n" + ep.Message);
            }
            catch (Exception ep)
            {
                log.Error("getProductInfo!ID:" + mi.machine_id + "\n" + ep.Message);
            }
            return product_id;
        }
        //执行存储过程
        public bool exeStoreProcedure(string sp_name, string calc_date)
        {
            try
            {
                using (SqlCommand sqlcom = new SqlCommand())
                {
                    sqlcom.Connection = sqlconn;
                    sqlcom.CommandType = CommandType.StoredProcedure; //设置调用的类型为存储过程  
                    sqlcom.CommandText = sp_name;
                    sqlcom.Parameters.Clear();

                    SqlParameter sqlParme = new SqlParameter();
                    //参数1  
                    sqlParme = sqlcom.Parameters.Add("@calc_date", SqlDbType.VarChar);
                    sqlParme.Direction = ParameterDirection.Input;
                    sqlParme.Value = calc_date.Trim();

                    sqlcom.ExecuteNonQuery();
                }
            }
            catch (NullReferenceException ep)
            {
                log.Error("Store Procedure is Error!" + sp_name + "\n" + ep.Message);
                return false;
            }
            catch (InvalidOperationException ep)
            {
                log.Error("Store Procedure is Error!" + sp_name + "\n" + ep.Message);
                return false;
            }
            catch (SqlException ep)
            {
                log.Error("Store Procedure is Error!" + sp_name + "\n" + ep.Message);
                return false;
            }
            catch (SystemException ep)
            {
                log.Error("Store Procedure is Error!" + sp_name + "\n" + ep.Message);
                return false;
            }
            catch (Exception ep)
            {
                log.Error("Store Procedure is Error!" + sp_name + "\n" + ep.Message);
                return false;
            }
            return true;
        }
        //执行存储过程
        public bool exespDeviceUsedTime(string sp_name, string calc_date, string wshift_date, int mac_id)
        {
            try
            {
                using (SqlCommand sqlcom = new SqlCommand())
                {
                    sqlcom.Connection = sqlconn;
                    sqlcom.CommandType = CommandType.StoredProcedure; //设置调用的类型为存储过程  
                    sqlcom.CommandText = sp_name;
                    sqlcom.Parameters.Clear();

                    SqlParameter sqlParme = new SqlParameter();
                    //参数1  
                    sqlParme = sqlcom.Parameters.Add("@calc_date", SqlDbType.VarChar);
                    sqlParme.Direction = ParameterDirection.Input;
                    sqlParme.Value = calc_date.Trim();
                    //参数1  
                    sqlParme = sqlcom.Parameters.Add("@wshift_date", SqlDbType.VarChar);
                    sqlParme.Direction = ParameterDirection.Input;
                    sqlParme.Value = wshift_date.Trim();
                    //参数2
                    sqlParme = sqlcom.Parameters.Add("@mac_id", SqlDbType.VarChar);
                    sqlParme.Direction = ParameterDirection.Input;
                    sqlParme.Value = mac_id;

                    sqlcom.ExecuteNonQuery();
                }
            }
            catch (NullReferenceException ep)
            {
                log.Error("Store Procedure is Error!" + sp_name + "\n" + ep.Message);
                return false;
            }
            catch (InvalidOperationException ep)
            {
                log.Error("Store Procedure is Error!" + sp_name + "\n" + ep.Message);
                return false;
            }
            catch (SqlException ep)
            {
                log.Error("Store Procedure is Error!" + sp_name + "\n" + ep.Message);
                return false;
            }
            catch (SystemException ep)
            {
                log.Error("Store Procedure is Error!" + sp_name + "\n" + ep.Message);
                return false;
            }
            catch (Exception cep)
            {
                log.Error("Store Procedure is Error!" + sp_name + "\n" + cep.Message);

                return false;
            }
            return true;
        }
        //执行存储过程
        public bool exespDeviceProdNum(string sp_name, string calc_date, string wshift_date, int mac_id)
        {
            try
            {
                using (SqlCommand sqlcmd = new SqlCommand())
                {
                    sqlcmd.Connection = sqlconn;
                    sqlcmd.CommandType = CommandType.StoredProcedure; //设置调用的类型为存储过程  
                    sqlcmd.CommandText = sp_name;
                    sqlcmd.Parameters.Clear();

                    SqlParameter sqlParme = new SqlParameter();
                    //参数1  
                    sqlParme = sqlcmd.Parameters.Add("@calc_date", SqlDbType.VarChar);
                    sqlParme.Direction = ParameterDirection.Input;
                    sqlParme.Value = calc_date.Trim();
                    //参数1  
                    sqlParme = sqlcmd.Parameters.Add("@wshift_date", SqlDbType.VarChar);
                    sqlParme.Direction = ParameterDirection.Input;
                    sqlParme.Value = wshift_date.Trim();
                    //参数2
                    sqlParme = sqlcmd.Parameters.Add("@mac_id", SqlDbType.VarChar);
                    sqlParme.Direction = ParameterDirection.Input;
                    sqlParme.Value = mac_id;

                    sqlcmd.ExecuteNonQuery();
                }
            }
            catch (NullReferenceException ep)
            {
                log.Error("Store Procedure is Error!" + sp_name + "\n" + ep.Message);
                return false;
            }
            catch (InvalidOperationException ep)
            {
                log.Error("Store Procedure is Error!" + sp_name + "\n" + ep.Message);
                return false;
            }
            catch (SqlException ep)
            {
                log.Error("Store Procedure is Error!" + sp_name + "\n" + ep.Message);
                return false;
            }
            catch (SystemException ep)
            {
                log.Error("Store Procedure is Error!" + sp_name + "\n" + ep.Message);
                return false;
            }
            catch (Exception cep)
            {
                log.Error("Store Procedure is Error!" + sp_name + "\n" + cep.Message);

                return false;
            }
            return true;
        }
        //执行存储过程
        public bool exespDeviceDataExt(string sp_name, string calc_date, string wshift_date, int mac_id, DateTime dtime)
        {
            try
            {
                using (SqlCommand sqlcmd = new SqlCommand())
                {
                    sqlcmd.Connection = sqlconn;
                    sqlcmd.CommandType = CommandType.StoredProcedure; //设置调用的类型为存储过程  
                    sqlcmd.CommandText = sp_name;
                    sqlcmd.Parameters.Clear();

                    SqlParameter sqlParme = new SqlParameter();
                    //参数1  
                    sqlParme = sqlcmd.Parameters.Add("@calc_date", SqlDbType.VarChar);
                    sqlParme.Direction = ParameterDirection.Input;
                    sqlParme.Value = calc_date.Trim();
                    //参数1  
                    sqlParme = sqlcmd.Parameters.Add("@wshift_date", SqlDbType.VarChar);
                    sqlParme.Direction = ParameterDirection.Input;
                    sqlParme.Value = wshift_date.Trim();
                    //参数2
                    sqlParme = sqlcmd.Parameters.Add("@mac_id", SqlDbType.VarChar);
                    sqlParme.Direction = ParameterDirection.Input;
                    sqlParme.Value = mac_id;
                    //参数3
                    sqlParme = sqlcmd.Parameters.Add("@read_time", SqlDbType.DateTime);
                    sqlParme.Direction = ParameterDirection.Input;
                    sqlParme.Value = dtime;

                    sqlcmd.ExecuteNonQuery();
                }
            }
            catch (NullReferenceException ep)
            {
                log.Error("Store Procedure is Error!" + sp_name + "\n" + ep.Message);
                return false;
            }
            catch (InvalidOperationException ep)
            {
                log.Error("Store Procedure is Error!" + sp_name + "\n" + ep.Message);
                return false;
            }
            catch (SqlException ep)
            {
                log.Error("Store Procedure is Error!" + sp_name + "\n" + ep.Message);
                return false;
            }
            catch (SystemException ep)
            {
                log.Error("Store Procedure is Error!" + sp_name + "\n" + ep.Message);
                return false;
            }
            catch (Exception cep)
            {
                log.Error("Store Procedure is Error!" + sp_name + "\n" + cep.Message);

                return false;
            }
            return true;
        }
        //执行存储过程
        public bool exespProdEcodeSeque(string sp_name, string calc_date, string wshift_date, int mac_id)
        {
            try
            {
                using (SqlCommand sqlcmd = new SqlCommand())
                {
                    sqlcmd.Connection = sqlconn;
                    sqlcmd.CommandType = CommandType.StoredProcedure; //设置调用的类型为存储过程  
                    sqlcmd.CommandText = sp_name;
                    sqlcmd.Parameters.Clear();

                    SqlParameter sqlParme = new SqlParameter();
                    //参数1  
                    sqlParme = sqlcmd.Parameters.Add("@calc_date", SqlDbType.VarChar);
                    sqlParme.Direction = ParameterDirection.Input;
                    sqlParme.Value = calc_date.Trim();
                    //参数1  
                    sqlParme = sqlcmd.Parameters.Add("@wshift_date", SqlDbType.VarChar);
                    sqlParme.Direction = ParameterDirection.Input;
                    sqlParme.Value = wshift_date.Trim();
                    //参数3
                    sqlParme = sqlcmd.Parameters.Add("@mac_id", SqlDbType.Int);
                    sqlParme.Direction = ParameterDirection.Input;
                    sqlParme.Value = mac_id;

                    sqlcmd.ExecuteNonQuery();
                }
            }
            catch (NullReferenceException ep)
            {
                log.Error("Store Procedure is Error!" + sp_name + "\n" + ep.Message);
                return false;
            }
            catch (InvalidOperationException ep)
            {
                log.Error("Store Procedure is Error!" + sp_name + "\n" + ep.Message);
                return false;
            }
            catch (SqlException ep)
            {
                log.Error("Store Procedure is Error!" + sp_name + "\n" + ep.Message);
                return false;
            }
            catch (SystemException ep)
            {
                log.Error("Store Procedure is Error!" + sp_name + "\n" + ep.Message);
                return false;
            }
            catch (Exception cep)
            {
                log.Error("Store Procedure is Error!" + sp_name + "\n" + cep.Message);
                return false;
            }
            return true;
        }
        //执行存储过程
        public bool exespInitAlarmSeque(string sp_name, string calc_date, string wshift_date, int mac_id)
        {
            try
            {
                using (SqlCommand sqlcmd = new SqlCommand())
                {
                    sqlcmd.Connection = sqlconn;
                    sqlcmd.CommandType = CommandType.StoredProcedure; //设置调用的类型为存储过程  
                    sqlcmd.CommandText = sp_name;
                    sqlcmd.Parameters.Clear();

                    SqlParameter sqlParme = new SqlParameter();
                    //参数1  
                    sqlParme = sqlcmd.Parameters.Add("@calc_date", SqlDbType.VarChar);
                    sqlParme.Direction = ParameterDirection.Input;
                    sqlParme.Value = calc_date.Trim();
                    //参数1  
                    sqlParme = sqlcmd.Parameters.Add("@wshift_date", SqlDbType.VarChar);
                    sqlParme.Direction = ParameterDirection.Input;
                    sqlParme.Value = wshift_date.Trim();
                    //参数3
                    sqlParme = sqlcmd.Parameters.Add("@mac_id", SqlDbType.Int);
                    sqlParme.Direction = ParameterDirection.Input;
                    sqlParme.Value = mac_id;

                    sqlcmd.ExecuteNonQuery();
                }
            }
            catch (NullReferenceException ep)
            {
                log.Error("Store Procedure is Error!" + sp_name + "\n" + ep.Message);
                return false;
            }
            catch (InvalidOperationException ep)
            {
                log.Error("Store Procedure is Error!" + sp_name + "\n" + ep.Message);
                return false;
            }
            catch (SqlException ep)
            {
                log.Error("Store Procedure is Error!" + sp_name + "\n" + ep.Message);
                return false;
            }
            catch (SystemException ep)
            {
                log.Error("Store Procedure is Error!" + sp_name + "\n" + ep.Message);
                return false;
            }
            catch (Exception cep)
            {
                log.Error("Store Procedure is Error!" + sp_name + "\n" + cep.Message);
                return false;
            }
            return true;
        }
        public static bool delayTime(int delayTime)
        {
            for (int i = 0; i < delayTime * 10; i++)
            {
                System.Threading.Thread.Sleep(10);
            }
            return true;
        }
    }
}

