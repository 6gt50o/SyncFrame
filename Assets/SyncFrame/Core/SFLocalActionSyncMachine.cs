using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SyncFrame
{
    /// <summary>
    /// 用于进行本地同步操作
    /// </summary>
    /// <typeparam name="ActionType"></typeparam>
    /// <typeparam name="ParamType"></typeparam>
    public class SFLocalActionSyncMachine<T, ActionType, ParamType> : ISFActionSyncMachine<ActionType, ParamType>
    {
        public T mgr;

        //private SFMgr<T, ActionType, ParamType> mgr;

        //public SFLocalActionSyncMachine(SFMgr<T, ActionType, ParamType> mgr)
        //{
        //    this.mgr = mgr;
        //}

        public SFLocalActionSyncMachine(T mgr)
        {
            this.mgr = mgr;
        }

        public void DispatchActions(Action onEnd)
        {
            onEnd();
        }

        public void PostActions(List<SFAction<ActionType, ParamType>> actions)
        {
            //donothing
        }

        //IEnumerator ISFActionSyncMachine<ActionType, ParamType>.DispatchActions()
        //{
        //    yield return null;
        //}

        void ISFActionSyncMachine<ActionType, ParamType>.PostAction(SFAction<ActionType, ParamType> action)
        {

        }

		public void Dispose()
		{
			
		}
    }
}

