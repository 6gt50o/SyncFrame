using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;


namespace SyncFrame
{

    public partial class SFMgr<T, ActionType, ParamType> : MonoBehaviour where T : SFMgr<T, ActionType, ParamType> where ActionType : IComparable
    {

		public void InitUDP(int port, int userID = -1)
        {

			AddActionMachine(new SFUDPActionSyncMachine<T, ActionType, ParamType>(this as T, port, userID));

			gameObject.AddComponent<MainThreadDispatcher> ();
        }

		public SFUDPActionSyncMachine<T, ActionType, ParamType> GetUDPSyncMachine()
		{
			return this.FindMachine<SFUDPActionSyncMachine<T, ActionType, ParamType>> ();
		}


    }
}
