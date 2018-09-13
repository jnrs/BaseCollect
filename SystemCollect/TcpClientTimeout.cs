using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace BaseCollect
{
    /// <summary>
    /// TcpClientWithTimeout 用来设置一个带连接超时功能的类
    /// 使用者可以设置毫秒级的等待超时时间 (1000=1second)
    /// 例如:
    /// TcpClient connection = new TcpClientWithTimeout('127.0.0.1',80,1000).Connect();
    /// </summary>
    /// 
    public class TcpClientTimeout
    {
        protected string _hostname;
        protected int _port;
        protected int _timeout_milliseconds;
        private static bool IsConnectionSuccessful = false;
        private static Exception socketexception;
        private static ManualResetEvent TimeoutObject = new ManualResetEvent(false);
        public TcpClientTimeout(string hostname, int port, int timeout_milliseconds)
        {
            _hostname = hostname;
            _port = port;
            _timeout_milliseconds = timeout_milliseconds;
        }
        public TcpClient Connect()
        {
            TimeoutObject.Reset();
            socketexception = null;
            //string serverip = Convert.ToString(remoteEndPoint.Address);
            //int serverport = remoteEndPoint.Port;

            TcpClient tcpclient = new TcpClient();
            try
            {
                tcpclient.BeginConnect(_hostname, _port, new AsyncCallback(CallBackMethod), tcpclient);
                if (TimeoutObject.WaitOne(_timeout_milliseconds, false))
                {
                    if (IsConnectionSuccessful)
                    {
                        return tcpclient;
                    }
                    else
                    {
                        throw socketexception;
                    }
                }
                else
                {
                    tcpclient.Close();
                    throw new TimeoutException("TimeOut Exception");
                }
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            catch (ObjectDisposedException ex)
            {
                throw ex;
            }
            catch (SocketException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private static void CallBackMethod(IAsyncResult asyncresult)
        {
            try
            {
                IsConnectionSuccessful = false;
                TcpClient tcpclient = asyncresult.AsyncState as TcpClient;
                if (tcpclient.Client != null)
                {
                    tcpclient.EndConnect(asyncresult);
                    IsConnectionSuccessful = true;
                }
            }
            catch (ArgumentNullException ex)
            {
                IsConnectionSuccessful = false;
                socketexception = ex;
            }
            catch (ObjectDisposedException ex)
            {
                IsConnectionSuccessful = false;
                socketexception = ex;
            }
            catch (SocketException ex)
            {
                IsConnectionSuccessful = false;
                socketexception = ex;
            }
            catch (Exception ex)
            {
                IsConnectionSuccessful = false;
                socketexception = ex;
            }
            finally
            {
                TimeoutObject.Set();
            }
        }
    }
}
