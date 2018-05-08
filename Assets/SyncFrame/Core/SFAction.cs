using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace SyncFrame
{
    public class ComparableException : SystemException
    {
        public ComparableException()
        {

        }
        public ComparableException(string message): base(message)
        {

        }
        public ComparableException(string message, Exception inner) :base(message, inner)
        {

        }

        //protected ComparableException(SerializationInfo info, StreamingContext context)
        //{
        //    info.AddValue("Com")
        //}
    }

    public abstract class SFAction<ActionType, ParamType> 
    {
        public int FrameID;
        public ActionType ActionID;
        public ParamType Param;


        //public int CompareTo(object obj)
        //{
        //    SFAction<ActionType, ParamType> target = obj as SFAction<ActionType, ParamType>;
        //    if (obj == null || target.ActionID.CompareTo(ActionID) != 0)
        //    {
        //        throw new ComparableException();
        //    }
        //    return target.Param.CompareTo(ActionID)
        //}

        abstract public ParamType SelectParam();
    }

    public class SFDelegateAction<ActionType, ParamType> : SFAction<ActionType, ParamType> 
    {
        private Func<ParamType> paramSelector;

        public SFDelegateAction(ActionType action, Func<ParamType> paramSelector)
        {
            this.ActionID = action;
            this.paramSelector = paramSelector;
        }

        public override ParamType SelectParam()
        {
            return paramSelector();
        }
    }

}

