using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaseCollect
{
    public class StandardData
    {
        private int _calc_date;
        private int _cur_shift;
        private int _machine_id;
        private List<int> _slave_mac_id;
        private int _mac_run_state;
        private int _mac_alarm_state;
        private int _mac_pause_state;
        private string _mis_visual;
        private List<int> _slave_run_state;
        private List<int> _slave_alarm_state;
        private List<int> _slave_pause_state;
        private int _run_state;
        private int _part_cate;
        private int _slcount;
        private int _xlcount;
        private int _acp_count;
        private int _ng_count;
        private int _sum_count;
        private List<int> _list_slcount;
        private List<int> _list_xlcount;
        private string _main_program;
        private string _running_program;
        private double _spindle_speed;
        private double _spindle_override;
        private double _feed_speed;
        private double _feed_override;
        private double _axis_pos_x;
        private double _axis_pos_y;
        private double _axis_pos_z;
        private double _spload_x;
        private double _spload_y;
        private double _spload_z;
        private double _svload_x;
        private double _svload_y;
        private double _svload_z;
        private List<double> _list_spd_speed;
        private List<double> _list_spd_override;
        private List<double> _list_feed_speed;
        private List<double> _list_feed_override;
        private List<double> _list_axis_posi;
        private int _alarm_no;
        private List<ushort> _list_alarm_no = new List<ushort>();
        private List<string> _list_alarm_message = new List<string>();
        private string _e2d_stage;
        private string _e2d_code;
        private List<string> _list_e2d_stage;
        private List<string> _list_e2d_code;
        private DateTime _read_time;

        public int calc_date
        {
            get
            {
                return _calc_date;
            }
            set
            {
                _calc_date = value;
            }
        }
        public int cur_shift
        {
            get
            {
                return _cur_shift;
            }
            set
            {
                _cur_shift = value;
            }
        }
        public int machine_id
        {
            get
            {
                return _machine_id;
            }
            set
            {
                _machine_id = value;
            }
        }
        public List<int> slave_mac_id
        {
            get
            {
                return _slave_mac_id;
            }
            set
            {
                _slave_mac_id = value;
            }
        }
        public int mac_run_state
        {
            get
            {
                return _mac_run_state;
            }
            set
            {
                _mac_run_state = value;
            }
        }
        public int mac_alarm_state
        {
            get
            {
                return _mac_alarm_state;
            }
            set
            {
                _mac_alarm_state = value;
            }
        }
        public int mac_pause_state
        {
            get
            {
                return _mac_pause_state;
            }
            set
            {
                _mac_pause_state = value;
            }
        }
        public string mis_visual
        {
            get
            {
                return _mis_visual;
            }
            set
            {
                _mis_visual = value;
            }
        }
        public List<int> slave_run_state
        {
            get
            {
                return _slave_run_state;
            }
            set
            {
                _slave_run_state = value;
            }
        }
        public List<int> slave_alarm_state
        {
            get
            {
                return _slave_alarm_state;
            }
            set
            {
                _slave_alarm_state = value;
            }
        }
        public List<int> slave_pause_state
        {
            get
            {
                return _slave_pause_state;
            }
            set
            {
                _slave_pause_state = value;
            }
        }
        public int run_state
        {
            get
            {
                return _run_state;
            }
            set
            {
                _run_state = value;
            }
        }
        public int part_cate
        {
            get
            {
                return _part_cate;
            }
            set
            {
                _part_cate = value;
            }
        }
        public int slcount
        {
            get
            {
                return _slcount;
            }
            set
            {
                _slcount = value;
            }
        }
        public int xlcount
        {
            get
            {
                return _xlcount;
            }
            set
            {
                _xlcount = value;
            }
        }
        public int acp_count
        {
            get
            {
                return _acp_count;
            }
            set
            {
                _acp_count = value;
            }
        }
        public int ng_count
        {
            get
            {
                return _ng_count;
            }
            set
            {
                _ng_count = value;
            }
        }
        public int sum_count
        {
            get
            {
                return _sum_count;
            }
            set
            {
                _sum_count = value;
            }
        }
        public List<int> list_slcount
        {
            get
            {
                return _list_slcount;
            }
            set
            {
                _list_slcount = value;
            }
        }
        public List<int> list_xlcount
        {
            get
            {
                return _list_xlcount;
            }
            set
            {
                _list_xlcount = value;
            }
        }
        public string main_program
        {
            get
            {
                return _main_program;
            }
            set
            {
                _main_program = value;
            }
        }
        public string running_program
        {
            get
            {
                return _running_program;
            }
            set
            {
                _running_program = value;
            }
        }
        public double spindle_speed
        {
            get
            {
                return _spindle_speed;
            }
            set
            {
                _spindle_speed = value;
            }
        }
        public double spindle_override
        {
            get
            {
                return _spindle_override;
            }
            set
            {
                _spindle_override = value;
            }
        }
        public double feed_speed
        {
            get
            {
                return _feed_speed;
            }
            set
            {
                _feed_speed = value;
            }
        }
        public double feed_override
        {
            get
            {
                return _feed_override;
            }
            set
            {
                _feed_override = value;
            }
        }
        public double axis_pos_x
        {
            get
            {
                return _axis_pos_x;
            }
            set
            {
                _axis_pos_x = value;
            }
        }
        public double axis_pos_y
        {
            get
            {
                return _axis_pos_y;
            }
            set
            {
                _axis_pos_y = value;
            }
        }
        public double axis_pos_z
        {
            get
            {
                return _axis_pos_z;
            }
            set
            {
                _axis_pos_z = value;
            }
        }
        public double spload_x
        {
            get
            {
                return _spload_x;
            }
            set
            {
                _spload_x = value;
            }
        }
        public double spload_y
        {
            get
            {
                return _spload_y;
            }
            set
            {
                _spload_y = value;
            }
        }
        public double spload_z
        {
            get
            {
                return _spload_z;
            }
            set
            {
                _spload_z = value;
            }
        }
        public double svload_x
        {
            get
            {
                return _svload_x;
            }
            set
            {
                _svload_x = value;
            }
        }
        public double svload_y
        {
            get
            {
                return _svload_y;
            }
            set
            {
                _svload_y = value;
            }
        }
        public double svload_z
        {
            get
            {
                return _svload_z;
            }
            set
            {
                _svload_z = value;
            }
        }
        public List<double> list_spd_speed
        {
            get
            {
                return _list_spd_speed;
            }
            set
            {
                _list_spd_speed = value;
            }
        }
        public List<double> list_spd_override
        {
            get
            {
                return _list_spd_override;
            }
            set
            {
                _list_spd_override = value;
            }
        }
        public List<double> list_feed_speed
        {
            get
            {
                return _list_feed_speed;
            }
            set
            {
                _list_feed_speed = value;
            }
        }
        public List<double> list_feed_override
        {
            get
            {
                return _list_feed_override;
            }
            set
            {
                _list_feed_override = value;
            }
        }
        public List<double> list_axis_posi
        {
            get
            {
                return _list_axis_posi;
            }
            set
            {
                _list_axis_posi = value;
            }
        }
        public int alarm_no
        {
            get
            {
                return _alarm_no;
            }
            set
            {
                _alarm_no = value;
            }
        }
        public List<ushort> list_alarm_no
        {
            get
            {
                return _list_alarm_no;
            }
            set
            {
                _list_alarm_no = value;
            }
        }
        public List<string> list_alarm_message
        {
            get
            {
                return _list_alarm_message;
            }
            set
            {
                _list_alarm_message = value;
            }
        }
        public string e2d_stage
        {
            get
            {
                return _e2d_stage;
            }
            set
            {
                _e2d_stage = value;
            }
        }
        public string e2d_code
        {
            get
            {
                return _e2d_code;
            }
            set
            {
                _e2d_code = value;
            }
        }
        public List<string> list_e2d_stage
        {
            get
            {
                return _list_e2d_stage;
            }
            set
            {
                _list_e2d_stage = value;
            }
        }
        public List<string> list_e2d_code
        {
            get
            {
                return _list_e2d_code;
            }
            set
            {
                _list_e2d_code = value;
            }
        }
        public DateTime read_time
        {
            get
            {
                return _read_time;
            }
            set
            {
                _read_time = value;
            }
        }
    }
}
