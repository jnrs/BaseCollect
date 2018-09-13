using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace JDataCollect.Common
{
    /// <summary>
    /// UdpClientWithTimeout 用来设置一个带连接超时功能的类
    /// 使用者可以设置毫秒级的等待超时时间 (1000=1second)
    /// 例如:
    /// UdpClient connection = new UdpClientWithTimeout('127.0.0.1',80,1000).Connect();
    /// </summary>
    public class UdpClientTimeout
    {
        protected string _hostname;
        protected int _port;
        protected int _timeout_milliseconds;
        protected UdpClient connection;
        protected bool connected;
        protected Exception exception;

        public UdpClientTimeout(string hostname, int port, int timeout_milliseconds)
        {
            _hostname = hostname;
            _port = port;
            _timeout_milliseconds = timeout_milliseconds;
        }
        public UdpClient Connect()
        {
            try
            {
                // kick off the thread that tries to connect
                connected = false;
                exception = null;
                Thread thread = new Thread(new ThreadStart(BeginConnect));
                thread.IsBackground = true; // 作为后台线程处理
                // 不会占用机器太长的时间
                thread.Start();

                // 等待如下的时间
                thread.Join(_timeout_milliseconds);

                if (connected == true)
                {
                    // 如果成功就返回TcpClient对象
                    thread.Abort();
                    return connection;
                }
                if (exception != null)
                {
                    // 如果失败就抛出错误
                    thread.Abort();
                    throw exception;
                }
                else
                {
                    // 同样地抛出错误
                    thread.Abort();
                    string message = string.Format("UdpClient connection to {0}:{1} timed out", _hostname, _port);
                    throw new TimeoutException(message);
                }
            }
            catch (ThreadInterruptedException ex)
            {
                exception = ex;
                throw ex;
            }
            catch (ThreadAbortException ex)
            {
                exception = ex;
                throw ex;
            }
            catch (TimeoutException ex)
            {
                exception = ex;
                throw ex;
            }
            catch (Exception ex)
            {
                exception = ex;
                throw ex;
            }
        }
        protected void BeginConnect()
        {
            try
            {
                connection = new UdpClient(_hostname, _port);
                // 标记成功，返回调用者
                connected = true;
            }
            catch (Exception ex)
            {
                // 标记失败
                exception = ex;
            }
        }
    }
}
