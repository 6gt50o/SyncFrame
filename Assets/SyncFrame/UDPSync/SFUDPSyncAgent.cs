using System;

namespace SyncFrame
{
	public class SFUDPSyncAgent<T, ActionType, ParamType> : SFActionsAgent<T, ActionType, ParamType> 
		where T : SFMgr<T, ActionType, ParamType> where ActionType : IComparable
	{

		private SFFrameJson<ActionType, ParamType> frameData;

		public int AgentID = -1;

		public SFUDPSyncAgent(T mgr) : base(mgr)
		{
			mgr.OnSyncFrameEnd += OnPreSyncFrame;
		}

		public void UpdateFrame(SFFrameJson<ActionType, ParamType> frame)
		{
			this.frameData = frame;
		}

		public new ParamType GetAction(ActionType act)
		{
			if (frameData == null)
				return default(ParamType);
			return frameData.FindAction(act);
		}

		public new bool TryGetAction(ActionType act, out ParamType param)
		{
			if (frameData == null) {
				
				param  = default(ParamType);
				return false;
			}
			
			return frameData.TryFindAction(act, out param);
		}

		private void OnPreSyncFrame(int frameID)
		{
			
		}

	}
}

