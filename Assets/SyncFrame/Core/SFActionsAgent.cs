using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SyncFrame
{
	public class SFActionsAgent<T, ActionType, ParamType> where T : SFMgr<T, ActionType, ParamType> where ActionType : IComparable
    {
        public T mgr;

        private List<SFAction<ActionType, ParamType>> cachedActions = new List<SFAction<ActionType, ParamType>>();

		private int cachedMask;

		/// <summary>
		/// Initializes a new instance of the <see cref="SyncFrame.SFActionsAgent`3"/> class.
		/// </summary>
		/// <param name="mgr">Mgr.</param>
        public SFActionsAgent(T mgr)
        {
            this.mgr = mgr;


			//Cache current frame actions
			mgr.OnSyncFrameEnd += _ => {

				if (cachedMask == 0)
					return;

				//ClearCached ();
				
				var acts = mgr.GetCurrentFrameActions();

				foreach (var a in acts)
				{
					if (CheckCachedMask(a.ActionID))
					{
						CacheAction(a);
					}
				}
			};
        }



		/// <summary>
		/// Gets the action.
		/// </summary>
		/// <returns>The action.</returns>
		/// <param name="act">Act.</param>
        public ParamType GetAction(ActionType act)
        {
            var action = mgr.GetAction(act);
            if (action != null)
                return action.Param;

            return default(ParamType);
        }

		/// <summary>
		/// Tries the get action.
		/// </summary>
		/// <returns><c>true</c>, if get action was tryed, <c>false</c> otherwise.</returns>
		/// <param name="act">Act.</param>
		/// <param name="param">Parameter.</param>
        public bool TryGetAction(ActionType act, out ParamType param)
        {
            var action = mgr.GetAction(act);
            if (action != null)
            {
                param = action.Param;
                return true;
            }

            param = default(ParamType);
            return false;
        }

        /// <summary>
        /// 缓存Action
        /// </summary>
        /// <param name="act"></param>
        public void CacheAction(ActionType act)
        {
            var action = mgr.GetAction(act);
            if (action != null)
            {
                //cachedActions.Add(action);
				CacheAction(action);
            }
        }

        /// <summary>
        /// 缓存Action
        /// </summary>
        /// <param name="action"></param>
        public void CacheAction(SFAction<ActionType, ParamType> action)
        {
			SFAction<ActionType, ParamType> curAction = null;
			if (TryGetCachedAction (action, out curAction)) {
				cachedActions.Remove (curAction);
			}

			cachedActions.Add (action);
        }


        /// <summary>
        /// 获取缓存的Action
        /// </summary>
        /// <param name="act"></param>
        /// <returns></returns>
        public ParamType GetCachedAction(ActionType act)
        {
            var action = cachedActions.Find((a) => a.ActionID.CompareTo(act) == 0);
            if (action != null)
                return action.Param;

            return default(ParamType);
        }

        /// <summary>
        /// 获取缓存的Action
        /// </summary>
        /// <param name="act"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public bool TryGetCachedAction(ActionType act, out ParamType param)
        {
            var action = cachedActions.Find((a) => a.ActionID.CompareTo(act) == 0);
            if (action != null)
            {
                param = action.Param;
                return true;
            }

            param = default(ParamType);
            return false;
        }

		/// <summary>
		/// Tries the get cached action.
		/// </summary>
		/// <returns><c>true</c>, if get cached action was tryed, <c>false</c> otherwise.</returns>
		/// <param name="action">Action.</param>
		/// <param name="retAction">Ret action.</param>
		public bool TryGetCachedAction(SFAction<ActionType, ParamType> action, out SFAction<ActionType, ParamType> retAction)
		{
			var actionFind = cachedActions.Find((a) => a.ActionID.CompareTo(action.ActionID) == 0);
			if (action != null)
			{
				retAction = actionFind;
				return true;
			}

			retAction = null;
			return false;
		}


		/// <summary>
		/// Sets the cached mask.
		/// </summary>
		/// <param name="list">List.</param>
		public void SetCachedMask(List<ActionType> list)
		{
			foreach (var a in list) {
				cachedMask |= 1 << Convert.ToInt32 (a) ;
			}
		}


		/// <summary>
		/// Checks the cached mask.
		/// </summary>
		/// <returns><c>true</c>, if cached mask was checked, <c>false</c> otherwise.</returns>
		/// <param name="action">Action.</param>
		public bool CheckCachedMask(ActionType action)
		{
			return (cachedMask & 1 << Convert.ToInt32 (action)) != 0;
		}

		/// <summary>
		/// Clears the cached.
		/// </summary>
		public void ClearCached()
		{
			cachedActions.Clear ();
		}

    }
}

