using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SyncFrame
{

	public interface ISFActionSyncMachine<ActionType, ParamType>  : IDisposable
    {
        void PostAction(SFAction<ActionType, ParamType> action);

        void PostActions(List<SFAction<ActionType, ParamType>> actions);

        void DispatchActions(Action onEnd);
    }
}
