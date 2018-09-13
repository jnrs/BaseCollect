using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace BaseCollect
{
    public class TcpServerTimeout
    {
        private static Socket dog = null;
        private static byte[] bytes = new byte[256];
        private static Thread threadwatch, threadprocess;
        public void initialize()
        {
            dog = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress address = IPAddress.Any;
            IPEndPoint point = new IPEndPoint(address, 51000);
            dog.Bind(point);
            dog.Listen(20);
            //threadwatch = new Thread(Connection);
            //threadwatch.IsBackground = true;
            //threadwatch.Start();
            Connection("@D&");
            //Connection("@D&");
            //Thread.Sleep(1000);
            //Connection("@C&");
            //Thread.Sleep(1000);
            Sclose();
        }
        public string Get_M()
        {
            if (bytes == null)
                return "NULL";
            else
                return Encoding.ASCII.GetString(bytes);
        }
        public void Sclose()
        {
            threadprocess.Abort();
            //threadwatch.Abort();
        }
        private void Connection(string send_msg)
        {
            byte[] msg;
            try
            {
                Socket C0 = dog.Accept();
                threadprocess = new Thread(Watching);
                threadprocess.Start(C0);

                //Thr                                  ead.Sleep(1000);
                msg = Encoding.UTF8.GetBytes(send_msg);
                C0.Send(msg);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void Watching(object obj)
        {
            Socket sniffer = obj as Socket;
            while (true)
            {
                try
                {
                    sniffer.Receive(bytes);
                    string getm = Get_M();
                    Console.WriteLine(getm.Substring(0, 2));
                    if (getm != null && getm.Substring(0, 2).Equals("@D"))
                    {
                        Console.WriteLine(getm);
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    break;
                }
            }
        }
    }
}
