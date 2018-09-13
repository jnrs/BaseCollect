using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaseCollect
{
    public class BaseMethod
    {
        public static bool isNumberic(string message, out int result)
        {
            try
            {
                System.Text.RegularExpressions.Regex rex =
                new System.Text.RegularExpressions.Regex(@"^\d+$");
                result = -1;
                if (rex.IsMatch(message))
                {
                    result = int.Parse(message);
                    return true;
                }
                else
                    return false;
            }
            catch (ArgumentNullException ex)
            {
                throw new Exception("isNumberic is Error!" + ex.Message);
            }
            catch (ArgumentException ex)
            {
                throw new Exception("isNumberic is Error!" + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("isNumberic is Error!" + ex.Message);
            }
        }
        // ------------------------------------------------------------------------
        // Button read coils//状态转换
        // ------------------------------------------------------------------------
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
