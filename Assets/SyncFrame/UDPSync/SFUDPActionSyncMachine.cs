using System;
using System.Collections;
using System.Collections.Generic;
using UDPZ;
using UniRx;
using UnityEngine;

namespace SyncFrame
{
    public class SFUDPActionSyncMachine<T, ActionType, ParamType> :
        ISFActionSyncMachine<ActionType, ParamType> 
        where ActionType : IComparable 
        where T : SFMgr<T, ActionType, ParamType>
    {
        public T mgr;

		public  Func<int, SFUDPSyncAgent<T, ActionType, ParamType> > AgentCreator;

		public Action<SFUDPSyncAgent<T, ActionType, ParamType>> OnAgentEnter;
		public Action<SFUDPSyncAgent<T, ActionType, ParamType>> OnAgentLeave;

		private UDPRoom<SFUDPSyncMsg> room = new UDPRoom<SFUDPSyncMsg>();

		private int curFrameID = -1;

		private Subject<int> onResponse = new Subject<int>();

		private SFUDPSyncFrameData<ActionType, ParamType> frameDataPoolObj = new SFUDPSyncFrameData<ActionType, ParamType>();

		private List<SFUDPSyncAgent<T, ActionType, ParamType> > agentList = new List<SFUDPSyncAgent<T, ActionType, ParamType>>();


        /// <summary>
        /// Initializes a new instance of the <see cref="SyncFrame.SFUDPActionSyncMachine`3"/> class.
        /// </summary>
        /// <param name="mgr">Mgr.</param>
        /// <param name="port">Port.</param>
		public SFUDPActionSyncMachine(T mgr, int port, int userID)
        {
            this.mgr = mgr;

			room.Init(port);
			room.CurUserID = userID;	//client to set the userID

			//room.Connect("192.168.43.46", 3001);
			room.Connect("127.0.0.1", 3001);

			room.OnConnected += () => Debug.Log ("OnConnected");
			room.OnDisConnected += () => Debug.Log ("OnDisConnected");

			room.OnUserEnter += AddAgent;

			room.OnUserLeave += DelAgent;

			room.OnData += (x, reader) =>
			{
				var header =  reader.GetHeader();
				SFUDPSyncMsg msg = (SFUDPSyncMsg)(header.Code);
				switch (msg)
				{
				case SFUDPSyncMsg.SyncEnd:
					SFUDPSyncFrameDataResponse data = reader.Get<SFUDPSyncFrameDataResponse>();
					if (data.FrameID == curFrameID)
					{
						//Debug.Log("SyncEnd " + data.FrameID + " ");

						onResponse.OnNext(1);
					}
					break;

				case SFUDPSyncMsg.FrameDataRemote:
				case SFUDPSyncMsg.FrameData:
					SFUDPSyncFrameData<ActionType, ParamType> frame = 
						reader.Get<SFUDPSyncFrameData<ActionType, ParamType>>();
					
					if (frame != null)
					{
						UpdateAgent(frame);
					}
					break;

				default:
					Debug.Log("nohown msg " + msg.ToString() + " ");
					break;

				}
			};

			room.Run();

            // ObserveOnMainThread return to mainthread
            //Observable.Start(() =>
            //{
            //    Run();
            //}) // asynchronous work
            //    .ObserveOnMainThread()
            //    .Subscribe(x => Debug.Log(x));
        }

		/// <summary>
		/// Adds the agent.
		/// </summary>
		/// <param name="AgentID">Agent I.</param>
		private void AddAgent(int AgentID)
		{
			//SFUDPSyncAgent<T, ActionType, ParamType> agent = new SFUDPSyncAgent<T, ActionType, ParamType>(this.mgr);
			if (AgentCreator != null) {

				var agent = AgentCreator (AgentID);
				if (agent != null) {
					agent.AgentID = AgentID;

					agentList.Add(agent);

					if (OnAgentEnter != null)
						OnAgentEnter (agent);
				}

			}
		}

		/// <summary>
		/// Dels the agent.
		/// </summary>
		/// <param name="AgentID">Agent I.</param>
		private void DelAgent(int AgentID)
		{
			var agent = agentList.Find( a => a.AgentID == AgentID );

			if (agent != null) {
				agentList.Remove(agent);

				if (OnAgentEnter != null)
					OnAgentEnter (agent);
			}

		}

		/// <summary>
		/// Updates the agent.
		/// </summary>
		/// <param name="frame">Frame.</param>
		private void UpdateAgent(SFUDPSyncFrameData<ActionType, ParamType> frame)
		{
			Debug.Log ("UpdateAgent " + room.CurUserID);
			int AgentID = frame.UID;
			var agent = agentList.Find( a => a.AgentID == AgentID );

			if (agent != null) {
				agent.UpdateFrame (frame.FrameData);
			}
		}

		/// <summary>
		/// Dispatchs the actions.
		/// </summary>
		/// <param name="onEnd">On end.</param>
        public void DispatchActions(Action onEnd)
        {
			//Debug.Log ("DispatchActions");
			onResponse = new Subject<int> ();

			onResponse.Subscribe(_ => 
				{
					//Debug.Log("onEnd");
					onEnd ();
				});

			if (frameDataPoolObj != null)
				room.Send<SFUDPSyncFrameData<ActionType, ParamType> > (frameDataPoolObj, SFUDPSyncMsg.FrameData);
        }

        public void PostAction(SFAction<ActionType, ParamType> action)
        {
            throw new NotImplementedException();
        }

        public void PostActions(List<SFAction<ActionType, ParamType>> actions)
        {
            var frame = SFFrameJson<ActionType, ParamType>.CreateFrame(actions);
            frame.FrameID = mgr.CurFrameID + 1;

			curFrameID = frame.FrameID;
			//Debug.Log ("PostActions current frame id is " + curFrameID);
//			SFFrameJson<ActionType, ParamType> frame1 = new SFFrameJson<ActionType, ParamType>();
//			frame1.FrameID = 100;
//			frame1.Duration = 10;
//			frame1.StartTime = 21;
			//room.Send<SFFrameJson<ActionType, ParamType> > (frame1, SFUDPSyncMsg.FrameData);

			// = new SFUDPSyncFrameData<ActionType, ParamType> ();
			frameDataPoolObj.FrameData = frame;
			frameDataPoolObj.UID = room.CurUserID;

			//onResponse = new Subject<int> ();
            //udpConnor.Send<SFFrameJson<ActionType, ParamType>>(frame);
        }


		public void Dispose()
		{
			if (room != null)
				room.Dispose();
		}

    }
}

