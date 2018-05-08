using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using ProtoBuf;

namespace SyncFrame
{
    //[System.Serializable]
    [ProtoContract]
    public class SFFramesJson<ActionType, ParamType> where ActionType : IComparable
    {
        [ProtoMember(1)]
        public List<SFFrameJson<ActionType, ParamType>> Frames = new List<SFFrameJson<ActionType, ParamType>>();

        public SFFrameJson<ActionType, ParamType> FindFrame(int frameID)
        {
            return Frames.Find((a) => a.FrameID == frameID);
        }
    }


    //[System.Serializable]
    [ProtoContract]
    public class SFActionJson<ActionType, ParamType>
    {
        [ProtoMember(1)]
		public ActionType ActionID = default(ActionType);

        [ProtoMember(2)]
		public ParamType Param = default(ParamType);

        [ProtoMember(3)]
        public int FrameID = 0;

        public static SFActionJson<ActionType, ParamType> CreateActionJson(SFAction<ActionType, ParamType> action)
        {
            SFActionJson<ActionType, ParamType> json = new SFActionJson<ActionType, ParamType>();
            json.ActionID = action.ActionID;
            json.FrameID = action.FrameID;
            json.Param = action.Param;
            return json;
        }
    }




    //[System.Serializable]
    [ProtoContract]
    public class SFFrameJson<ActionType, ParamType> where ActionType : IComparable
    {
        [ProtoMember(1)]
        public int FrameID  = 0;

        [ProtoMember(2)]
        public float StartTime = 0;

        [ProtoMember(3)]
        public float Duration = 0;

        [ProtoMember(4)]
        public List<SFActionJson<ActionType, ParamType>> Actions = new List<SFActionJson<ActionType, ParamType>>();

        public ParamType FindAction(ActionType act)
        {
            var action =  Actions.Find((a) => a.ActionID.CompareTo(act) == 0);
            if (action != null)
                return action.Param;

            return default(ParamType);
        }

		public bool TryFindAction(ActionType act, out ParamType data)
		{
			var action =  Actions.Find((a) => a.ActionID.CompareTo(act) == 0);
			if (action != null) {
				data = action.Param;
				return true;
			}

			data = default(ParamType);
			return false;
		}

        /// <summary>
        /// 用于快速进行Frame的构造
        /// </summary>
        /// <param name="actions"></param>
        /// <returns></returns>
        public static SFFrameJson<ActionType, ParamType> CreateFrame(List<SFAction<ActionType, ParamType>> actions)
        {
            SFFrameJson<ActionType, ParamType> rets = new SFFrameJson<ActionType, ParamType>();

            foreach (var a in actions)
            {
                rets.Actions.Add(SFActionJson<ActionType, ParamType>.CreateActionJson(a));
            }

            return rets;
        }

        public static  string ToJson(SFFrameJson<ActionType, ParamType> frame)
        {
            return null;
        }

        public static SFFrameJson<ActionType, ParamType> ToObj(string jsonStr)
        {
            return null;
        }
    }


}

