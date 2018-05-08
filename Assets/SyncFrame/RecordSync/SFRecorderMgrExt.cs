using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProtoBuf;

namespace SyncFrame
{


    /// <summary>
    /// SFMgr的扩展类，用于支持保存记录
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="ActionType"></typeparam>
    /// <typeparam name="ParamType"></typeparam>
	public partial class SFMgr<T, ActionType, ParamType> : MonoBehaviour where T : SFMgr<T, ActionType, ParamType> where ActionType : IComparable
    {

        public class SFRecorderAgent : SFRecorderAgent<T, ActionType, ParamType>
        {
            public SFRecorderAgent(T mgr) : base(mgr)
            {
            }
        }

        public void EnableRecord(bool bRecord = true)
        {
            if (bRecord)
            {
                AddActionMachine(new SFRecorderActionSyncMachine<T, ActionType, ParamType>(this as T));
            }
            else
            {
                var m = FindMachine<SFRecorderActionSyncMachine<T, ActionType, ParamType>>();
                if (m != null)
                {
                    RemoveActionMachine(m);
                }
            }
        }

        /// <summary>
        /// 保存到文件
        /// </summary>
        /// <param name="pathName"></param>
        public void SaveRecord(string pathName)
        {
            var recordMachine = FindMachine<SFRecorderActionSyncMachine<T, ActionType, ParamType>>();

            if (recordMachine != null)
            {
                recordMachine.SaveActionsJson(pathName);
            }
        }


    }

}

