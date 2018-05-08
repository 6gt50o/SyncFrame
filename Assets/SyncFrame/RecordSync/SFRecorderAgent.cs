using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SyncFrame
{
    public class SFRecorderAgent<T, ActionType, ParamType> : SFActionsAgent<T, ActionType, ParamType> 
		where T : SFMgr<T, ActionType, ParamType> where ActionType : IComparable
    {

        private SFFramesJson<ActionType, ParamType> framesFile;
        private int curFrameID = 1;     //从第一帧开始回放

        public SFRecorderAgent(T mgr) : base(mgr)
        {
            mgr.OnSyncFrameEnd += OnPreSyncFrame;
        }

        /// <summary>
        /// 加载Action同步文件
        /// </summary>
        /// <param name="strData"></param>
        public void LoadActionsJson(string pathName)
        {
            //template 's template can't tojson
            //this.framesFile = JsonUtility.FromJson<SFFramesJson<ActionType, ParamType>>(strData);

            using (var file = File.OpenRead(pathName))
            {
                framesFile = Serializer.Deserialize<SFFramesJson<ActionType, ParamType>>(file);
                //Debug.Log("frames is " + framesFile.ToString());
            }
        }

        /// <summary>
        /// 获取动作的参数
        /// </summary>
        /// <param name="act"></param>
        /// <returns></returns>
        public new ParamType GetAction(ActionType act)
        {
            var curFrame = framesFile.FindFrame(curFrameID);
            if (curFrame != null)
            {
                return curFrame.FindAction(act);
            }

            return default(ParamType);
        }

        public new bool TryGetAction(ActionType act, out ParamType param)
        {
            var curFrame = framesFile.FindFrame(curFrameID);
            if (curFrame != null)
            {
				return curFrame.TryFindAction(act, out param);
                //return true;
            }

            param = default(ParamType);
            return false;
        }

        private void OnPreSyncFrame(int frameID)
        {
            //提前回放一帧，这样才能同步，差一帧都不行。特别是对于刚刚开始的第一帧
            curFrameID = frameID + 2;
        }
    }
}

