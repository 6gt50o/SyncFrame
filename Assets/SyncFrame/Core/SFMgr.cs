using SyncFrame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SyncFrame
{

	public partial class SFMgr<T, ActionType, ParamType> : MonoBehaviour  where T : SFMgr<T, ActionType, ParamType> where ActionType : IComparable
    {
        /// <summary>
        /// class helper
        /// </summary>
        public class SFActionsAgent : SFActionsAgent<T, ActionType, ParamType>
        {
            public SFActionsAgent(T mgr) : base(mgr)
            {
            }
        }

        public class SFDelegateAction : SFDelegateAction<ActionType, ParamType>
        {
            public SFDelegateAction(ActionType action, Func<ParamType> paramSelector) : base(action, paramSelector)
            {
            }
        }

        public abstract class SFAction : SFAction<ActionType, ParamType>
        {
        }

        public Action<int> OnSyncFrameEnd;
		public event Action<int> OnSyncFrame;
        public Action OnStop;
        public Action OnUpdate;

        private int curFrameID = -1;
        private int targetFrameID = 0;
        private bool bStart = false;

		/// <summary>
		/// Gets the current frame I.
		/// </summary>
		/// <value>The current frame I.</value>
        public int CurFrameID
        {
            get
            {
                return this.curFrameID;
            }
        }

		/// The machines.
        private List<ISFActionSyncMachine<ActionType, ParamType>> machines
            = new List<ISFActionSyncMachine<ActionType, ParamType>>();

		/// The machines flag.
        private Dictionary<ISFActionSyncMachine<ActionType, ParamType>, bool> machinesFlag 
            = new Dictionary<ISFActionSyncMachine<ActionType, ParamType>, bool>();

        //save temp actions for next frame to handle
        private List<SFAction<ActionType, ParamType>> tempActions = new List<SFAction<ActionType, ParamType>>();

        //save current actions for current frame
        private List<SFAction<ActionType, ParamType>> currentFrameActions = new List<SFAction<ActionType, ParamType>>();

        // Use this for initialization
        public void Start()
        {
            //add default syncmachine
            AddActionMachine(new SFLocalActionSyncMachine<T, ActionType, ParamType>(this as T));
        }

		/// <summary>
		/// Fixeds the update.
		/// </summary>
        private void FixedUpdate()
        {
            if (targetFrameID != this.curFrameID && bStart)
            {
                this.curFrameID = targetFrameID;
                if (OnSyncFrame != null)
                    OnSyncFrame(curFrameID);

                HandleCurrentFrameActions();
            }
        }

        //IEnumerator AsyncUpdate()
        //{
        //    while (true)
        //    {
        //        yield return new WaitForEndOfFrame();

        //    }
        //    yield return null;
        //}

        protected void Update()
        {
            if (OnUpdate != null && bStart)
            {
                OnUpdate();
            }
        }

        /// <summary>
        /// 开始进行同步
        /// </summary>
        public void StartSync()
        {
            //this.curFrameID = this.targetFrameID = -1;
            bStart = true;
        }

        /// <summary>
        /// TODO 应该是在下一帧才进行Stop
        /// </summary>
        public void StopSync()
        {
            bStart = false;
            if (OnStop != null)
                OnStop();
        }

		/// <summary>
		/// Pauses the sync.
		/// </summary>
		public void PauseSync()
		{
			bStart = false;
		}

		/// <summary>
		/// Resumes the sync.
		/// </summary>
		public void ResumeSync()
		{
			bStart = true;

		}

		/// <summary>
		/// Gets the current frame actions.
		/// </summary>
		/// <returns>The current frame actions.</returns>
		public List<SFAction<ActionType, ParamType>> GetCurrentFrameActions()
		{
			return currentFrameActions;
		}

        /// <summary>
        /// 追加Action，会在下一帧进行同步处理
        /// </summary>
        /// <param name="action"></param>
		public SFAction<ActionType, ParamType> PostAction(SFAction<ActionType, ParamType> action)
        {
            action.FrameID = curFrameID + 1;
            tempActions.Add(action);
			return action;
        }

        /// <summary>
        /// 追加一个Action代理
        /// </summary>
        /// <param name="action"></param>
        /// <param name="paramSelector"></param>
        /// <param name="onPost"></param>
		public SFAction<ActionType, ParamType> PostDelegateAction(ActionType action, Func<ParamType> paramSelector, Action<ActionType, ParamType> onPost = null)
        {
            return PostAction(new SFDelegateAction<ActionType, ParamType>(action, paramSelector));
        }

		/// <summary>
		/// Starts the sync A frame.
		/// </summary>
        public void StartSyncAFrame()
        {

        }
        
        /// <summary>
        /// 添加一个动作处理引擎
        /// </summary>
        /// <param name="m"></param>
        public void AddActionMachine(ISFActionSyncMachine<ActionType, ParamType> m)
        {
            machines.Add(m);
            machinesFlag.Add(m, false);
        }

		/// <summary>
		/// Removes the action machine.
		/// </summary>
		/// <param name="m">M.</param>
        public void RemoveActionMachine(ISFActionSyncMachine<ActionType, ParamType> m)
        {
            machines.Remove(m);
            machinesFlag.Remove(m);
        }

		private void DisposeActionMachines()
		{
			foreach (var m in machines) {
				m.Dispose ();
			}
		}

        /// <summary>
        /// 返回一个动作处理引擎
        /// </summary>
        /// <typeparam name="FindType"></typeparam>
        /// <returns></returns>
        public FindType FindMachine<FindType>() where FindType : ISFActionSyncMachine<ActionType, ParamType>
        {
            var tmp = machines.Find(
                    (a) => {
                        //Debug.Log("type is " + a.GetType().ToString());
                        return a.GetType() == typeof(FindType);
                    }
                );

            if (tmp == null)
            {
                return default(FindType);
            }

            return (FindType)tmp;
        }

		/// <summary>
		/// Gets the action.
		/// </summary>
		/// <returns>The action.</returns>
		/// <param name="act">Act.</param>
        public SFAction<ActionType, ParamType> GetAction(ActionType act)
        {
            return currentFrameActions.Find((a)=> a.ActionID.CompareTo(act) == 0);
        }

		/// <summary>
		/// Handles the current frame actions.
		/// </summary>
        private void HandleCurrentFrameActions()
        {
            if (tempActions.Count <= 0)
            {
                //currentFrameActions.Clear();
                prepareNextFrame();
                return;
            }

            //get the selector
            foreach (var a in tempActions)
            {
                a.Param =  a.SelectParam();
            }
           
            foreach (var m in machines)
            {
                //clear flag
                //machinesFlag[m] = false;

                m.PostActions(tempActions);
                m.DispatchActions( ()=> checkSyncEnd(m));
            }

            //Debug.Log("tempActions " + tempActions.Count);
            //Debug.Log("nextFrameActions " + nextFrameActions.Count);
        }

        /// <summary>
        /// 是否同步完成
        /// </summary>
        /// <returns></returns>
        private bool IsReady()
        {
            bool bEnd = true;
            foreach (bool b in machinesFlag.Values)
            {
                if (!b)
                {
                    bEnd = false;
                    break;
                }
            }
            return bEnd;
        }

        private void checkSyncEnd(ISFActionSyncMachine<ActionType, ParamType> m)
        {
            machinesFlag[m] = true;

            if (IsReady())
            {
                prepareNextFrame();
            }
        }

        /// <summary>
        /// 为下一帧运行准备
        /// </summary>
        private void prepareNextFrame()
        {
            if (targetFrameID != curFrameID)
                return;

            targetFrameID = this.curFrameID + 1;
            if (OnSyncFrameEnd != null)
                OnSyncFrameEnd(curFrameID);
            
            currentFrameActions.Clear();
            currentFrameActions.AddRange(tempActions);
            tempActions.Clear();

            //clear flag
            foreach (var m in machines)
            {
                //clear flag
                machinesFlag[m] = false;
            }
        }

		private void OnApplicationQuit()
		{
			DisposeActionMachines ();
		}
    }

}
