using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace BaseCollect
{
    public class MachineInfo
    {
        log4net.ILog log = log4net.LogManager.GetLogger("MachineInfo");

        DataConnect dbc = new DataConnect();

        public int group_id { get; set; }
        public string group_name { get; set; }
        public int machine_id { get; set; }
        public string machine_name { get; set; }
        public string machine_series { get; set; }
        public string machine_number { get; set; }
        public int rank_num { get; set; }
        public string category { get; set; }
        public string is_run_state { get; set; }
        public string is_prod { get; set; }
        public string mis_visual { get; set; }
        public string is_barcode { get; set; }
        public int station_cnt { get; set; }
        public string comm_protocol { get; set; }
        public string comm_interface { get; set; }
        public string machine_ip { get; set; }
        public ushort machine_port { get; set; }
        public ushort listen_port { get; set; }
        public bool connect_state { get; set; }

        protected int _RunBit = 0;
        protected int _AlarmBit = 0;
        protected int _PauseBit = 0;
        protected int _EditBit = 0;
        protected int _StopBit = 0;
        protected int _ReadyBit = 0;

        public MachineInfo()
        {
        }

        public List<MachineInfo> initMacCommProtocal(string machine_series)
        {
            MachineInfo mi = null;
            List<MachineInfo> lmi = new List<MachineInfo>();
            String strSql = " select ta.group_number,gi.group_name,ta.machine_id,ta.machine_name,ta.machine_series, "
                            + "      ta.machine_number,ta.comm_protocol,ta.comm_interface "
                            + " from vw_machine_info ta where machine_series = '" + machine_series + "'";

            try
            {
                DataTable dtInfo = dbc.getStrSqlData(strSql).Copy();

                for (int i = 0; i < dtInfo.Rows.Count; i++)
                {
                    mi = new MachineInfo();

                    mi.group_id = Convert.ToInt32(dtInfo.Rows[i]["group_number"]);
                    mi.group_name = dtInfo.Rows[i]["group_name"].ToString();
                    mi.machine_id = Convert.ToInt32(dtInfo.Rows[i]["machine_id"]);
                    mi.machine_name = dtInfo.Rows[i]["machine_name"].ToString();
                    mi.machine_series = dtInfo.Rows[i]["machine_series"].ToString();
                    mi.machine_number = dtInfo.Rows[i]["machine_number"].ToString();
                    mi.comm_protocol = dtInfo.Rows[i]["comm_protocol"].ToString();
                    mi.comm_interface = dtInfo.Rows[i]["comm_interface"].ToString();

                    lmi.Add(mi);
                }

            }
            catch (InvalidOperationException ep)
            {
                log.Error("执行initMacCommProtocal错误！" + ep.Message);
                return null;
            }
            catch (InvalidCastException ep)
            {
                log.Error("执行initMacCommProtocal错误！" + ep.Message);
                return null;
            }
            catch (OutOfMemoryException ep)
            {
                log.Error("执行initMacCommProtocal错误！" + ep.Message);
                return null;
            }
            catch (SqlException ep)
            {
                log.Error("执行initMacCommProtocal错误！" + ep.Message);
                return null;
            }
            catch (Exception ep)
            {
                log.Error("执行initMacCommProtocal错误！" + ep.Message);
                return null;
            }
            return lmi;
        }

        public List<MachineInfo> initMachineInfo()
        {
            DataTable dtInfo = null;
            MachineInfo mi = null;
            List<MachineInfo> lmi = new List<MachineInfo>();
            String strSql = " select ta.group_number,gi.group_name,ta.machine_id,ta.machine_name,ta.machine_series,ta.machine_number,ta.rank_num, "
                            + "      ta.category,ta.is_run_state,ta.is_prod,ta.is_barcode,ta.station_cnt,tb.ip_addr,tb.ip_com "
                            + " from tb_machine_info ta,tb_macgroup_info gi,tb_machine_ipaddr tb where ta.machine_id = tb.machine_id "
                            + "  and ta.group_number = gi.group_id "
                            + "  and ta.enabled = 1 and gi.enabled = 1 and tb.enabled = 1 ";

            try
            {
                dtInfo = dbc.getStrSqlData(strSql).Copy();

                for (int i = 0; i < dtInfo.Rows.Count; i++)
                {
                    mi = new MachineInfo();

                    mi.group_id = Convert.ToInt32(dtInfo.Rows[i]["group_number"]);
                    mi.group_name = dtInfo.Rows[i]["group_name"].ToString();
                    mi.machine_id = Convert.ToInt32(dtInfo.Rows[i]["machine_id"]);
                    mi.machine_name = dtInfo.Rows[i]["machine_name"].ToString();
                    mi.machine_series = dtInfo.Rows[i]["machine_series"].ToString();
                    mi.machine_number = dtInfo.Rows[i]["machine_number"].ToString();
                    mi.rank_num = Convert.ToInt32(dtInfo.Rows[i]["rank_num"]);
                    mi.category = dtInfo.Rows[i]["category"].ToString();
                    mi.is_run_state = dtInfo.Rows[i]["is_run_state"].ToString();
                    mi.is_prod = dtInfo.Rows[i]["is_prod"].ToString();
                    mi.is_barcode = dtInfo.Rows[i]["is_barcode"].ToString();
                    mi.station_cnt = Convert.ToInt32(dtInfo.Rows[i]["station_cnt"]);
                    mi.machine_ip = dtInfo.Rows[i]["ip_addr"].ToString();
                    mi.machine_port = ushort.Parse(dtInfo.Rows[i]["ip_com"].ToString());
                    mi.connect_state = false;

                    lmi.Add(mi);
                }

            }
            catch (InvalidOperationException ep)
            {
                log.Error("执行initMachineInfo错误！" + ep.Message);
                return null;
            }
            catch (InvalidCastException ep)
            {
                log.Error("执行initMachineInfo错误！" + ep.Message);
                return null;
            }
            catch (OutOfMemoryException ep)
            {
                log.Error("执行initMachineInfo错误！" + ep.Message);
                return null;
            }
            catch (SqlException ep)
            {
                log.Error("执行initMachineInfo错误！" + ep.Message);
                return null;
            }
            catch (Exception ep)
            {
                log.Error("执行initMachineInfo错误！" + ep.Message);
                return null;
            }
            return lmi;
        }

        public List<MachineInfo> initMachineInfo(string comm_protocol, string comm_interface,int hardware_id)
        {
            DataTable dtInfo = null;
            MachineInfo mi = null;
            List<MachineInfo> lmi = new List<MachineInfo>();
            String strSql = " select ta.group_number,gi.group_name,ta.machine_id,ta.machine_name,ta.machine_series,ta.machine_number,ta.rank_num, "
                            + "      ta.category,ta.is_run_state,ta.is_prod,ta.is_barcode,ta.station_cnt,tb.ip_addr,tb.ip_com "
                            + " from tb_machine_info ta,tb_macgroup_info gi,tb_machine_ipaddr tb where ta.machine_id = tb.machine_id "
                            + "  and ta.group_number = gi.group_id and tb.hardware_id = " + hardware_id
                            + "  and ta.comm_protocol = \'" + comm_protocol + "\' and ta.comm_interface = \'" + comm_interface + "\'"
                            + "  and ta.enabled = 1 and gi.enabled = 1 and tb.enabled = 1 ";
            
            try
            {
                dtInfo = dbc.getStrSqlData(strSql).Copy();

                for (int i = 0; i < dtInfo.Rows.Count; i++)
                {
                    mi = new MachineInfo();

                    mi.group_id = Convert.ToInt32(dtInfo.Rows[i]["group_number"]);
                    mi.group_name = dtInfo.Rows[i]["group_name"].ToString();
                    mi.machine_id = Convert.ToInt32(dtInfo.Rows[i]["machine_id"]);
                    mi.machine_name = dtInfo.Rows[i]["machine_name"].ToString();
                    mi.machine_series = dtInfo.Rows[i]["machine_series"].ToString();
                    mi.machine_number = dtInfo.Rows[i]["machine_number"].ToString();
                    mi.rank_num = Convert.ToInt32(dtInfo.Rows[i]["rank_num"]);
                    mi.category = dtInfo.Rows[i]["category"].ToString();
                    mi.is_run_state = dtInfo.Rows[i]["is_run_state"].ToString();
                    mi.is_prod = dtInfo.Rows[i]["is_prod"].ToString();
                    mi.is_barcode = dtInfo.Rows[i]["is_barcode"].ToString();
                    mi.station_cnt = Convert.ToInt32(dtInfo.Rows[i]["station_cnt"]);
                    mi.machine_ip = dtInfo.Rows[i]["ip_addr"].ToString();
                    mi.machine_port = ushort.Parse(dtInfo.Rows[i]["ip_com"].ToString());
                    mi.connect_state = false;

                    lmi.Add(mi);
                }

            }
            catch (InvalidOperationException ep)
            {
                log.Error("执行initMachineInfo错误！" + ep.Message);
                return null;
            }
            catch (InvalidCastException ep)
            {
                log.Error("执行initMachineInfo错误！" + ep.Message);
                return null;
            }
            catch (OutOfMemoryException ep)
            {
                log.Error("执行initMachineInfo错误！" + ep.Message);
                return null;
            }
            catch (SqlException ep)
            {
                log.Error("执行initMachineInfo错误！" + ep.Message);
                return null;
            }
            catch (Exception ep)
            {
                log.Error("执行initMachineInfo错误！" + ep.Message);
                return null;
            }
            return lmi;
        }

        public MachineInfo initMachineInfo(string comm_protocol, string comm_interface, int hardware_id, string machine_no)
        {
            DataTable dtInfo = null;
            MachineInfo mi = null;
            String strSql = " select top 1 ta.group_number,gi.group_name,ta.machine_id,ta.machine_name,ta.machine_series,ta.machine_number, "
                            + "      ta.rank_num,ta.category,ta.is_run_state,ta.is_prod,ta.is_barcode,ta.station_cnt,tb.ip_addr,tb.ip_com "
                            + " from tb_machine_info ta,tb_macgroup_info gi,tb_machine_ipaddr tb where ta.machine_id = tb.machine_id "
                            + "  and ta.group_number = gi.group_id and tb.hardware_id = " + hardware_id
                            + "  and ta.comm_protocol = \'" + comm_protocol + "\' and ta.comm_interface = \'" + comm_interface + "\'"
                            + "  and ta.machine_number = '" + machine_no + "' and ta.enabled = 1 and gi.enabled = 1 and tb.enabled = 1 ";

            try
            {
                dtInfo = dbc.getStrSqlData(strSql).Copy();

                if(dtInfo != null && dtInfo.Rows.Count > 0)
                {
                    mi = new MachineInfo();

                    mi.group_id = Convert.ToInt32(dtInfo.Rows[0]["group_number"]);
                    mi.group_name = dtInfo.Rows[0]["group_name"].ToString();
                    mi.machine_id = Convert.ToInt32(dtInfo.Rows[0]["machine_id"]);
                    mi.machine_name = dtInfo.Rows[0]["machine_name"].ToString();
                    mi.machine_series = dtInfo.Rows[0]["machine_series"].ToString();
                    mi.machine_number = dtInfo.Rows[0]["machine_number"].ToString();
                    mi.rank_num = Convert.ToInt32(dtInfo.Rows[0]["rank_num"]);
                    mi.category = dtInfo.Rows[0]["category"].ToString();
                    mi.is_run_state = dtInfo.Rows[0]["is_run_state"].ToString();
                    mi.is_prod = dtInfo.Rows[0]["is_prod"].ToString();
                    mi.is_barcode = dtInfo.Rows[0]["is_barcode"].ToString();
                    mi.station_cnt = Convert.ToInt32(dtInfo.Rows[0]["station_cnt"]);
                    mi.machine_ip = dtInfo.Rows[0]["ip_addr"].ToString();
                    mi.machine_port = ushort.Parse(dtInfo.Rows[0]["ip_com"].ToString());
                    mi.connect_state = false;
                }
            }
            catch (InvalidOperationException ep)
            {
                log.Error("执行initMachineInfo错误！" + ep.Message);
                return null;
            }
            catch (InvalidCastException ep)
            {
                log.Error("执行initMachineInfo错误！" + ep.Message);
                return null;
            }
            catch (OutOfMemoryException ep)
            {
                log.Error("执行initMachineInfo错误！" + ep.Message);
                return null;
            }
            catch (SqlException ep)
            {
                log.Error("执行initMachineInfo错误！" + ep.Message);
                return null;
            }
            catch (Exception ep)
            {
                log.Error("执行initMachineInfo错误！" + ep.Message);
                return null;
            }
            return mi;
        }

        public List<MachineInfo> initSlaveInfo(string comm_protocol, string comm_interface, int hardware_id)
        {
            DataTable dtInfo = null;
            MachineInfo mi = null;
            List<MachineInfo> lmi = new List<MachineInfo>();
            String strSql = " select ta.group_number,gi.group_name,ta.machine_id,ta.machine_name,ta.machine_series,ta.machine_number,ta.rank_num, "
                            + "      ta.is_prod,ta.station_cnt,ta.mis_visual,tb.ip_addr,tb.ip_com,tb.listen_port "
                            + " from tb_machine_info ta,tb_macgroup_info gi,tb_machine_ipaddr tb where ta.machine_id = tb.machine_id "
                            + "  and ta.group_number = gi.group_id and tb.hardware_id = " + hardware_id
                            + "  and ta.enabled = 1 and gi.enabled = 1 and tb.enabled = 1 ";

            try
            {
                dtInfo = dbc.getStrSqlData(strSql).Copy();

                for (int i = 0; i < dtInfo.Rows.Count; i++)
                {
                    mi = new MachineInfo();

                    mi.group_id = Convert.ToInt32(dtInfo.Rows[i]["group_number"]);
                    mi.group_name = dtInfo.Rows[i]["group_name"].ToString();
                    mi.machine_id = Convert.ToInt32(dtInfo.Rows[i]["machine_id"]);
                    mi.machine_name = dtInfo.Rows[i]["machine_name"].ToString();
                    mi.machine_series = dtInfo.Rows[i]["machine_series"].ToString();
                    mi.machine_number = dtInfo.Rows[i]["machine_number"].ToString();
                    mi.rank_num = Convert.ToInt32(dtInfo.Rows[i]["rank_num"]);
                    mi.is_prod = dtInfo.Rows[i]["is_prod"].ToString();
                    mi.mis_visual = dtInfo.Rows[i]["mis_visual"].ToString();
                    mi.machine_ip = dtInfo.Rows[i]["ip_addr"].ToString();
                    mi.machine_port = ushort.Parse(dtInfo.Rows[i]["ip_com"].ToString());
                    mi.listen_port = ushort.Parse(dtInfo.Rows[i]["listen_port"].ToString());
                    mi.connect_state = false;

                    lmi.Add(mi);
                }

            }
            catch (InvalidOperationException ep)
            {
                log.Error("执行initSlaveInfo错误！" + ep.Message);
                return null;
            }
            catch (InvalidCastException ep)
            {
                log.Error("执行initSlaveInfo错误！" + ep.Message);
                return null;
            }
            catch (OutOfMemoryException ep)
            {
                log.Error("执行initSlaveInfo错误！" + ep.Message);
                return null;
            }
            catch (SqlException ep)
            {
                log.Error("执行initSlaveInfo错误！" + ep.Message);
                return null;
            }
            catch (Exception ep)
            {
                log.Error("执行initSlaveInfo错误！" + ep.Message);
                return null;
            }
            return lmi;
        }

        public MachineInfo initMachineStateBit(MachineInfo mi)
        {
            String strSql = "select run_bit,stop_bit,alarm_bit,edit_bit,pause_bit,ready_bit from dbo.tb_pers_data_info "
                            + " where data_name = \'RunState\' and machine_id = " + mi.machine_id;

            //取设备 
            try
            {
                DataTable dtInfo = dbc.getStrSqlData(strSql);

                if (dtInfo.Rows.Count > 0)
                {
                    mi.RunBit = Convert.ToInt32(dtInfo.Rows[0]["run_bit"]);
                    mi.AlarmBit = Convert.ToInt32(dtInfo.Rows[0]["alarm_bit"]);
                    mi.PauseBit = Convert.ToInt32(dtInfo.Rows[0]["pause_bit"]);
                    mi.ReadyBit = Convert.ToInt32(dtInfo.Rows[0]["ready_bit"]);
                    mi.StopBit = Convert.ToInt32(dtInfo.Rows[0]["stop_bit"]);
                    mi.EditBit = Convert.ToInt32(dtInfo.Rows[0]["edit_bit"]);
                }
                return mi;
            }
            catch (IndexOutOfRangeException ep)
            {
                log.Error("执行initMachineStateBit错误！" + ep.Message);
            }
            catch (InvalidOperationException ep)
            {
                log.Error("执行initMachineStateBit错误！" + ep.Message);
            }
            catch (InvalidCastException ep)
            {
                log.Error("执行initMachineStateBit错误！" + ep.Message);
            }
            catch (OutOfMemoryException ep)
            {
                log.Error("执行initMachineStateBit错误！" + ep.Message);
                return null;
            }
            catch (SqlException ep)
            {
                log.Error("执行initMachineStateBit错误！" + ep.Message);
            }
            catch (Exception ep)
            {
                log.Error("执行initMachineStateBit错误！" + ep.Message);
            }
            return null;
        }

        public List<MachineInfo> initMachineStateBit(List<MachineInfo> lmi)
        {
            string strMac = "";

            if (lmi == null || lmi.Count == 0)
            {
                return lmi;
            }

            for (int i = 0; i < lmi.Count; i++)
            {

                if (i < lmi.Count - 1)
                {
                    strMac += lmi[i].machine_id + ",";
                }
                else
                {
                    strMac += lmi[i].machine_id;
                }
            }

            MachineInfo mi = null;
            String strSql = "select machine_id,run_bit,stop_bit,alarm_bit,edit_bit,pause_bit,ready_bit from dbo.tb_pers_data_info "
                            + " where data_name = \'RunState\' and machine_id in (" + strMac + ")";

            //取设备 
            try
            {
                DataTable dtInfo = dbc.getStrSqlData(strSql);

                if (dtInfo.Rows.Count > 0)
                {
                    for (int i = 0; i < dtInfo.Rows.Count; i++)
                    {
                        int machine_id = Convert.ToInt32(dtInfo.Rows[i]["machine_id"]);
                        mi = lmi.Find(delegate(MachineInfo fmi)
                            {
                                return fmi.machine_id == machine_id;
                            });

                        log.Info(mi != null ? mi.machine_id + System.Environment.NewLine + mi.machine_name : "没有查找到:" + machine_id);

                        mi.RunBit = Convert.ToInt32(dtInfo.Rows[0]["run_bit"]);
                        mi.AlarmBit = Convert.ToInt32(dtInfo.Rows[0]["alarm_bit"]);
                        mi.PauseBit = Convert.ToInt32(dtInfo.Rows[0]["pause_bit"]);
                        mi.ReadyBit = Convert.ToInt32(dtInfo.Rows[0]["ready_bit"]);
                        mi.StopBit = Convert.ToInt32(dtInfo.Rows[0]["stop_bit"]);
                        mi.EditBit = Convert.ToInt32(dtInfo.Rows[0]["edit_bit"]);
                    }
                }
                return lmi;
            }
            catch (IndexOutOfRangeException ep)
            {
                log.Error("执行initMachineStateBit错误！" + ep.Message);
            }
            catch (InvalidOperationException ep)
            {
                log.Error("执行initMachineStateBit错误！" + ep.Message);
            }
            catch (InvalidCastException ep)
            {
                log.Error("执行initMachineStateBit错误！" + ep.Message);
            }
            catch (OutOfMemoryException ep)
            {
                log.Error("执行initMachineStateBit错误！" + ep.Message);
                return null;
            }
            catch (SqlException ep)
            {
                log.Error("执行initMachineStateBit错误！" + ep.Message);
            }
            catch (Exception ep)
            {
                log.Error("执行initMachineStateBit错误！" + ep.Message);
            }
            return null;
        }

        public List<DataParameter> initParamAddrInfo(List<MachineInfo> lmi)
        {
            DataTable dtInfo = null;
            DataParameter dp = null;
            List<DataParameter> ldp = new List<DataParameter>();

            try
            {
                foreach (MachineInfo mi in lmi)
                {
                    String strSql = " select tdp.param_id,tdp.param_name,tdp.param_type,tdp.prw_type,tdp.fields_name,tdp.fields_type,tdp.fields_len,tmp.data_addr,tmp.data_setup,tmp.data_type,tmp.data_len,tmp.data_isvisual,tmp.data_ishot "
                                + " from tb_dict_parameter tdp,tb_machine_param tmp where tdp.param_type in ('ProdData','RunStateData','ProgramData','DeviceData','AlarmData','RunParamData','UserState') "
                                + " and tdp.param_id = tmp.param_id and tmp.machine_id = " + mi.machine_id + " and tmp.enabled = 1 ";

                    dtInfo = dbc.getStrSqlData(strSql).Copy();

                    for (int i = 0; i < dtInfo.Rows.Count; i++)
                    {
                        dp = new DataParameter();

                        dp.MachineId = mi.machine_id;
                        dp.ParamId = Convert.ToInt32(dtInfo.Rows[i]["param_id"]);
                        dp.ParamName = dtInfo.Rows[i]["param_name"].ToString();
                        dp.ParamType = dtInfo.Rows[i]["param_type"].ToString();
                        dp.PRWType = dtInfo.Rows[i]["prw_type"].ToString();
                        if (!Convert.IsDBNull(dtInfo.Rows[i]["fields_name"]))
                        {
                            dp.PFieldsName = dtInfo.Rows[i]["fields_name"].ToString();
                        }
                        else
                        {
                            dp.PFieldsName = "";
                        }
                        if (!Convert.IsDBNull(dtInfo.Rows[i]["fields_type"]))
                        {
                            dp.PFieldsType = dtInfo.Rows[i]["fields_type"].ToString();
                        }
                        else
                        {
                            dp.PFieldsType = "";
                        }
                        if (!Convert.IsDBNull(dtInfo.Rows[i]["fields_len"]))
                        {
                            dp.PFieldsLen = Convert.ToInt32(dtInfo.Rows[i]["fields_len"]);
                        }
                        else
                        {
                            dp.PFieldsLen = 0;
                        }
                        dp.PDataAddr = dtInfo.Rows[i]["data_addr"].ToString();
                        dp.PDataSetup = dtInfo.Rows[i]["data_setup"];
                        dp.PDataType = dtInfo.Rows[i]["data_type"].ToString();
                        dp.PDataLen = Convert.ToInt32(dtInfo.Rows[i]["data_len"]);
                        dp.PDataIsVisual = Convert.ToBoolean(dtInfo.Rows[i]["data_isvisual"]);
                        dp.PDataIsHot = Convert.ToBoolean(dtInfo.Rows[i]["data_ishot"]);

                        ldp.Add(dp);
                    }
                }
            }
            catch (InvalidOperationException ep)
            {
                log.Error("执行initParamAddrInfo错误！" + ep.Message);
                return null;
            }
            catch (InvalidCastException ep)
            {
                log.Error("执行initParamAddrInfo错误！" + ep.Message);
                return null;
            }
            catch (OutOfMemoryException ep)
            {
                log.Error("执行initParamAddrInfo错误！" + ep.Message);
                return null;
            }
            catch (SqlException ep)
            {
                log.Error("执行initParamAddrInfo错误！" + ep.Message);
                return null;
            }
            catch (Exception ep)
            {
                log.Error("执行initParamAddrInfo错误！" + ep.Message);
                return null;
            }
            return ldp;
        }
        public List<DataParameter> initParamAddrInfo(MachineInfo tmi)
        {
            DataTable dtInfo = null;
            DataParameter dp = null;
            List<DataParameter> ldp = new List<DataParameter>();

            try
            {
                String strSql = " select tdp.param_id,tdp.param_name,tdp.param_type,tdp.prw_type,tdp.fields_name,tdp.fields_type,tdp.fields_len,tmp.data_addr,tmp.data_setup,tmp.data_type,tmp.data_len,tmp.data_isvisual,tmp.data_ishot "
                                + " from tb_dict_parameter tdp,tb_machine_param tmp where tdp.param_type in ('ProdData','RunStateData','ProgramData','DeviceData','AlarmData','RunParamData','UserState') "
                                + " and tdp.param_id = tmp.param_id and tmp.machine_id = " + tmi.machine_id + " and tmp.enabled = 1 ";

                dtInfo = dbc.getStrSqlData(strSql).Copy();

                for (int i = 0; i < dtInfo.Rows.Count; i++)
                {
                    dp = new DataParameter();

                    dp.MachineId = tmi.machine_id;
                    dp.ParamId = Convert.ToInt32(dtInfo.Rows[i]["param_id"]);
                    dp.ParamName = dtInfo.Rows[i]["param_name"].ToString();
                    dp.ParamType = dtInfo.Rows[i]["param_type"].ToString();
                    dp.PRWType = dtInfo.Rows[i]["prw_type"].ToString();
                    if (!Convert.IsDBNull(dtInfo.Rows[i]["fields_name"]))
                    {
                        dp.PFieldsName = dtInfo.Rows[i]["fields_name"].ToString();
                    }
                    else
                    {
                        dp.PFieldsName = "";
                    }
                    if (!Convert.IsDBNull(dtInfo.Rows[i]["fields_type"]))
                    {
                        dp.PFieldsType = dtInfo.Rows[i]["fields_type"].ToString();
                    }
                    else
                    {
                        dp.PFieldsType = "";
                    }
                    if (!Convert.IsDBNull(dtInfo.Rows[i]["fields_len"]))
                    {
                        dp.PFieldsLen = Convert.ToInt32(dtInfo.Rows[i]["fields_len"]);
                    }
                    else
                    {
                        dp.PFieldsLen = 0;
                    }
                    dp.PDataAddr = dtInfo.Rows[i]["data_addr"].ToString();
                    dp.PDataSetup = dtInfo.Rows[i]["data_setup"];
                    dp.PDataType = dtInfo.Rows[i]["data_type"].ToString();
                    dp.PDataLen = Convert.ToInt32(dtInfo.Rows[i]["data_len"]);
                    dp.PDataIsVisual = Convert.ToBoolean(dtInfo.Rows[i]["data_isvisual"]);
                    dp.PDataIsHot = Convert.ToBoolean(dtInfo.Rows[i]["data_ishot"]);

                    ldp.Add(dp);
                }
            }
            catch (InvalidOperationException ep)
            {
                log.Error("执行initParamAddrInfo错误！" + ep.Message);
                return null;
            }
            catch (InvalidCastException ep)
            {
                log.Error("执行initParamAddrInfo错误！" + ep.Message);
                return null;
            }
            catch (OutOfMemoryException ep)
            {
                log.Error("执行initParamAddrInfo错误！" + ep.Message);
                return null;
            }
            catch (SqlException ep)
            {
                log.Error("执行initParamAddrInfo错误！" + ep.Message);
                return null;
            }
            catch (Exception ep)
            {
                log.Error("执行initParamAddrInfo错误！" + ep.Message);
                return null;
            }
            return ldp;
        }
        public void readFailParamsData(MachineInfo mi)
        {
            DateTime dtime = DateTime.Now;
            StandardData sd = new StandardData();

            sd.machine_id = mi.machine_id;
            sd.mac_run_state = 0;
            sd.mac_alarm_state = 0;
            sd.mac_pause_state = 0;
            sd.run_state = RunStateParam.StopState;
            sd.slcount = 0;
            sd.xlcount = 0;
            sd.main_program = "";
            sd.running_program = "";
            sd.alarm_no = 0;

            //Output Database
            dbc.putStandardData(sd, dtime);
            dbc.putSDToRunInfo(mi, dtime);

            dbc.putDeviceRunToExt(mi, dtime);
        }
        public void readDebugParamsData(MachineInfo mi, List<DataParameter> ldp)
        {
            Random rd = new Random();
            DateTime dtime = DateTime.Now;
            StandardData sd = new StandardData();

            if (!DataParameter.DicDebugRunState.ContainsKey(mi.machine_id))
            {
                DataParameter.DicDebugRunState.Add(mi.machine_id, RunStateParam.StopState);
            }
            if (!DataParameter.DicSLCount.ContainsKey(mi.machine_id))
            {
                DataParameter.DicSLCount.Add(mi.machine_id, 0);
            }
            if (!DataParameter.DicXLCount.ContainsKey(mi.machine_id))
            {
                DataParameter.DicXLCount.Add(mi.machine_id, 0);
            }
            if (!DataParameter.DicDebugCountFlag.ContainsKey(mi.machine_id))
            {
                DataParameter.DicDebugCountFlag.Add(mi.machine_id, true);
            }

            int alarm_num = 0;
            if (DateTime.Now.Minute % 5 == 0 && DataParameter.DicDebugCountFlag[mi.machine_id])
            {
                DataParameter.DebugRunState = rd.Next(30, 75) % 5;
                DataParameter.SLCount += rd.Next(0, 2);
                DataParameter.XLCount += rd.Next(0, 2);
                alarm_num = rd.Next(0, 10);

                if (DataParameter.DicDebugRunState.ContainsKey(mi.machine_id))
                {
                    DataParameter.DicDebugRunState[mi.machine_id] = rd.Next(30, 75) % 5;
                }

                int slCount = 0;
                if (DataParameter.DicSLCount.ContainsKey(mi.machine_id))
                {
                    slCount = DataParameter.DicSLCount[mi.machine_id];
                    slCount += rd.Next(0, 2);
                    DataParameter.DicSLCount[mi.machine_id] = slCount;
                }

                int xlCount = 0;
                if (DataParameter.DicXLCount.ContainsKey(mi.machine_id))
                {
                    xlCount = DataParameter.DicXLCount[mi.machine_id];
                    xlCount += rd.Next(0, 2);
                    DataParameter.DicXLCount[mi.machine_id] = xlCount;
                }
                
                DataParameter.DicDebugCountFlag[mi.machine_id] = false;
            }
            if (DateTime.Now.Minute % 5 > 0)
            {
                DataParameter.DicDebugCountFlag[mi.machine_id] = true;
            }

            sd.machine_id = mi.machine_id;
            sd.mac_run_state = 0;
            sd.mac_alarm_state = 0;
            sd.mac_pause_state = 0;
            sd.run_state = RunStateParam.StopState;
            sd.slcount = 0;
            sd.xlcount = 0;
            sd.main_program = "";
            sd.running_program = "";
            sd.alarm_no = 0;

            foreach (DataParameter dp in ldp)
            {
                if (dp.PRWType != null && dp.PRWType.Equals("ReadOut"))
                {
                    switch (dp.PFieldsName)
                    {
                        case "mac_run_state":
                            sd.mac_run_state = DataParameter.DicDebugRunState[mi.machine_id] == 1 ? 1 : 0; break;
                        case "mac_alarm_state":
                            sd.mac_alarm_state = DataParameter.DicDebugRunState[mi.machine_id] == 2 ? 1 : 0; break;
                        case "mac_pause_state":
                            sd.mac_pause_state = DataParameter.DicDebugRunState[mi.machine_id] == 3 ? 1 : 0; break;
                        case "run_state":
                            sd.run_state = DataParameter.DicDebugRunState[mi.machine_id];
                            break;
                        case "slcount":
                            sd.slcount = DataParameter.DicSLCount[mi.machine_id];
                            break;
                        case "xlcount":
                            sd.xlcount = DataParameter.DicXLCount[mi.machine_id];
                            break;
                        case "alarm_no":
                            List<ushort> lan = new List<ushort>();
                            List<string> lam = new List<string>();
                            for (int i = 0; i < alarm_num; i++)
                            {
                                ushort no = (ushort)rd.Next(0, 20);
                                lan.Add(no);
                                lam.Add(no + "#alarm");
                            }
                            sd.list_alarm_no = lan;
                            sd.list_alarm_message = lam;
                            dbc.putAlarmHistory(mi, sd.list_alarm_no, DataParameter.DefaultAlarm_grp, sd.list_alarm_message, dtime);
                            break;
                        default: break;
                    }
                }
            }
            sd.run_state = DataParameter.DicDebugRunState[mi.machine_id];
            //if (sd.run_state == RunStateParam.ReadyState)
            //{
            //    sd.run_state = RunStateParam.StopState;
            //}
            //Output Database
            dbc.putStandardData(sd, dtime);
            dbc.putSDToRunInfo(mi, dtime);
            dbc.putSDToRunData(mi, dtime);

            dbc.putDeviceRunToExt(mi, dtime);
            dbc.putDeviceDataToExt(mi, dtime);
        }
        public int RunBit
        {
            get
            {
                return _RunBit;
            }
            set
            {
                _RunBit = value;
            }
        }

        public int AlarmBit
        {
            get
            {
                return _AlarmBit;
            }
            set
            {
                _AlarmBit = value;
            }
        }

        public int PauseBit
        {
            get
            {
                return _PauseBit;
            }
            set
            {
                _PauseBit = value;
            }
        }

        public int EditBit
        {
            get
            {
                return _EditBit;
            }
            set
            {
                _EditBit = value;
            }
        }

        public int StopBit
        {
            get
            {
                return _StopBit;
            }
            set
            {
                _StopBit = value;
            }
        }

        public int ReadyBit
        {
            get
            {
                return _ReadyBit;
            }
            set
            {
                _ReadyBit = value;
            }
        }
    }
}
