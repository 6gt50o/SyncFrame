using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;


namespace UDPZ
{


    public class UDPConnor<Header>
    {
        private int localPort;
        private IPEndPoint localEndPoint;

        private EndPoint curRemoteEndPoint;

        private Socket socket;

        public Socket Soc
        {
            get
            {
                return socket;
            }
        }

       private byte[] recvBuf = new byte[1000];

        public bool Init(int port)
        {
            string curIP = Network.player.ipAddress;
            this.localPort = port;

            localEndPoint = new IPEndPoint(IPAddress.Parse(curIP), localPort);

            // 定义套接字类型,在主线程中定义
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socket.ReceiveTimeout = 3000;

            return true;
        }

        /// <summary>
        /// 监听一个端口
        /// </summary>
        /// <returns></returns>
        public bool Listen()
        {
            try
            {
                socket.Bind(localEndPoint);
            }
            catch (Exception exception)
            {
                Debug.Log("The error is " + exception.ToString());
                return false;
            }

            return true;
        }


        public void Connect(string ip, int port)
        {
            curRemoteEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);

            //SendProtoBuf()
        }

        public int Send(Header header = default(Header))
        {
            return Send<NullMsg>(new NullMsg(), header);
        }

        public int Send<SendData>(SendData data, Header header = default(Header))
        {
            ////清空发送缓存
            //var buf = new byte[1024];
            int count = 0;
            using (MemoryStream memory = new MemoryStream())//初始化MemoryStream类
            {
                var package = new UDPProtocal<Header, SendData>();
                package.Header = header;
                package.Data = data;
                
                Serializer.Serialize(memory, package);

                //数据类型转换，把Json字符转转为字节数组（网络通信以字节形式进行传输）
                //int count = Encoding.ASCII.GetBytes(memory.ToArray());

                byte[] arrayNew = memory.ToArray();
                count = socket.SendTo(arrayNew, arrayNew.Length, SocketFlags.None, curRemoteEndPoint);
				//Debug.Log("socket SendTo after remote ip is " + socket.RemoteEndPoint.ToString());

            }
            return count;
            //Debug.Log("temp=" + temp);
        }

        public int Recv(out  byte[] retBuf)
        {
            EndPoint endPoint = curRemoteEndPoint;
            int count = 0;
            retBuf = default(byte[]);

			Array.Clear (recvBuf, 0, 1000);
            try
            {
                //count = socket.ReceiveFrom(recvBuf, ref endPoint);
				count = socket.Receive(recvBuf);
                retBuf = recvBuf;

				Debug.Log("socket remote ip is " + socket.RemoteEndPoint.ToString());
            }
            catch
            {

            }

            return count;
        }
        public int RecvHeader(out Header header)
        {
            //System.Runtime.InteropServices.Marshal.SizeOf(UDPProtocal<Header, RecvData>)
            EndPoint endPoint = curRemoteEndPoint;
            int count = 0;
            header = default(Header);
			Array.Clear (recvBuf, 0, 1000);
            try
            {
				count = socket.Receive(recvBuf);
            }
            catch
            {
				//time out
            }

			try
			{
				if (count > 0)
				{
					using (MemoryStream memory = new MemoryStream(recvBuf))
					{
						var ret = Serializer.Deserialize<UDPProtocalRet<Header>>(memory);

						header = ret.Header;
					}
				}
			}
			catch (Exception e) {
				Debug.Log ("when Deserialize head error " + e.ToString ());
				count = 0;

			}



            return count;
        }

        public int Recv<RecvData>(out RecvData data, out Header header)
        {
            //System.Runtime.InteropServices.Marshal.SizeOf(UDPProtocal<Header, RecvData>)
            EndPoint endPoint = curRemoteEndPoint;
            int count = 0;
            data = default(RecvData);
            header = default(Header);

			Array.Clear (recvBuf, 0, 1000);

            try
            {
				count = socket.Receive(recvBuf);//, ref endPoint);
            }
            catch
            {

            }


            if (count > 0)
            {
                using (MemoryStream memory = new MemoryStream(recvBuf))
                {
                    //var ret = Serializer.Deserialize<UDPProtocalRet<Header>>(memory);
                    var ret = Serializer.Deserialize<UDPProtocal<Header, RecvData>>(memory);
                    //data = ret.Data;
                    header = ret.Header;
                    //Debug.Log("frames is " + framesFile.ToString());
                }
            }

            return count;
        }

		private byte[] CreateBuf(int count)
		{
			return new byte[1000];
		}

		public byte[] GetCurrentRecvBuf()
        {
			var dest = new byte[1000];
			Array.Copy(recvBuf, dest, recvBuf.Length);
			return dest;
        }
    }
}

