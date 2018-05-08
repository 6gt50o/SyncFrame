using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniRx;
using UnityEngine;

namespace UDPZ
{


	/// <summary>
	/// UDP room.
	/// </summary>
	public class UDPRoom<Msg>
		: UDPConnor<UIDHeader<RoomMsgMapper<Msg> > >, IDisposable
		where Msg : IConvertible
    {
        public Action<int> OnUserEnter;
        public Action<int> OnUserLeave;

		public Action OnConnected;
		public Action OnDisConnected;

		public Action<int, ProtoDataReader<UIDHeader<RoomMsgMapper<Msg> > > > OnData;

        private List<int> userList = new List<int>();
        private IDisposable runThread;

		private int curUserID = 1;	//for test
		public int CurUserID
		{
			get{
				
				return curUserID;
			}

			set{
				curUserID = value;
			}
		}

		private ProtoDataReader<UIDHeader<RoomMsgMapper<Msg> > > recvReader = new ProtoDataReader<UIDHeader<RoomMsgMapper<Msg> > >();

        public void Run()
        {
            runThread = Observable.Start(() =>
            {
                innerRun();
				}).Subscribe(_ => Debug.Log(" thread end user id is " + curUserID) );
        }

        public void Dispose()
        {
			Debug.Log ("Dispose room user id is " + curUserID);
			//Send<NullMsg>(new NullMsg(), RoomMsg.Leave);
			SendRoomMsg(RoomMsg.Disconnect);
            if (runThread != null)
            {
                runThread.Dispose();
            }
        }

        private void innerRun()
        {
            //Send(RoomMsg.Enter);
            //Send<NullMsg>(new NullMsg(), RoomMsg.Enter);
			SendRoomMsg(RoomMsg.Connect);	//connect request

            while (true)
            {
				UIDHeader<RoomMsgMapper<Msg> > header;
                if (RecvHeader(out header) <= 0)
                {
                    continue;
                }
				if (curUserID == 2) {
					int i = 0;
				}
				//connect request return msg
                if ((RoomMsg)header.Code == RoomMsg.Connect)
                {
                    if (!IsInroom(header.ID))
                    {
						if (OnConnected != null)
							OnConnected();
						
						curUserID = header.ID;
                        //userList.Add(header.ID);
                    }
                }
                else if ((RoomMsg)header.Code == RoomMsg.Disconnect)
                {
                    if (IsInroom(header.ID))
                    {
						if (OnDisConnected != null)
							OnDisConnected();

                        //userList.Remove(header.ID);
                    }
                }


				//user enter msg be recv, when a user connect
				else if ((RoomMsg)header.Code == RoomMsg.UserEnter)
				{
					if (!IsInroom(header.ID))
					{
						if (OnUserEnter != null)
							OnUserEnter(header.ID);

						userList.Add(header.ID);
					}
				}
				else if ((RoomMsg)header.Code == RoomMsg.UserLeave)
				{
					if (IsInroom(header.ID))
					{
						if (OnUserLeave != null)
							OnUserLeave(header.ID);

						userList.Remove(header.ID);
					}
				}


				else //if ((RoomMsg)header.Code == RoomMsg.Disconnect)
                {
                    //if (IsInroom(header.ID))
                    {
                        if (OnData != null)
                        {
							recvReader.RawData = GetCurrentRecvBuf();

							MainThreadDispatcher.Post(_ => OnData(header.ID, recvReader), null);
                            //OnData(header.ID, recvReader);
                        }
                    }
                }
                //break;
            }

			Debug.Log ("inner run end ");
        }

        private bool IsInroom(int id)
        {
            int iret = userList.Find( a => a == id );

            return iret != 0;
        }

		public int Send<SendData>(SendData data, Msg msg)
		{
			UIDHeader<RoomMsgMapper<Msg>> header = new UIDHeader<RoomMsgMapper<Msg>> (msg);
			header.ID = curUserID;
			return Send<SendData> (data, header);
		}

		private int SendRoomMsg(RoomMsg rMsg)
		{
			UIDHeader<RoomMsgMapper<Msg>> header = new UIDHeader<RoomMsgMapper<Msg>> (rMsg);
			header.ID = curUserID;
			return Send<NullMsg> (new NullMsg(), header);
		}
    }


    
}
