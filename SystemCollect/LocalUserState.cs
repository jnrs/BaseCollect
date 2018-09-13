using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaseCollect
{
    public class LocalUserState
    {
        public static int _group_id;
        public static int _machine_id;
        public static int _user_id;
        public static string _user_name;
        public static string _access_type;
        public static DateTime _last_time;

        public int group_id
        {
            get
            {
                return _group_id;
            }
            set
            {
                _group_id = value;
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
        public int user_id
        {
            get
            {
                return _user_id;
            }
            set
            {
                _user_id = value;
            }
        }
        public string user_name
        {
            get
            {
                return _user_name;
            }
            set
            {
                _user_name = value;
            }
        }
        public string access_type
        {
            get
            {
                return _access_type;
            }
            set
            {
                _access_type = value;
            }
        }
        public DateTime last_time
        {
            get
            {
                return _last_time;
            }
            set
            {
                _last_time = value;
            }
        }
    }
}
