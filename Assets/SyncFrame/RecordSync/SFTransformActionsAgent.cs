using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SyncFrame
{
	/// <summary>
	/// SF transform actions agent.
	/// </summary>
	// the paramtype must be the float type
	public class SFTransformActionsAgent<T, SubAction>
		: SFActionsAgent<T, TransformActionMapper<SubAction>, float>
		where T : SFMgr<T, TransformActionMapper<SubAction>, float> where SubAction : IConvertible, IComparable
	{

		/// <summary>
		/// Initializes a new instance of the <see cref="SyncFrame.SFTransformActionsAgent`2"/> class.
		/// </summary>
		/// <param name="mgr">Mgr.</param>
		public SFTransformActionsAgent(T mgr) : base(mgr)
		{
			//mgr.OnSyncFrameEnd +=;
		}

		/// <summary>
		/// Sets the cached mask.
		/// </summary>
		/// <param name="list">List.</param>
		public void SetCachedMask(List<SubAction> list)
		{
			//TODO use the object pool
			var rets = new List<TransformActionMapper<SubAction> >();

			foreach (var d in list) {
				rets.Add (d);
			}
			SetCachedMask( rets);
		}


		/// <summary>
		/// 追加Action，会在下一帧进行同步处理
		/// </summary>
		/// <param name="action"></param>
		public void PostTransformAction(SFTransformDelegateAction<float> action) 
		{
			//create the six actions
			mgr.PostDelegateAction((TransformAction.PosX), ()=> {
				Transform trans =  action.TransSelector();
				return (float)(Math.Round((trans.position.x), 4));
			});

			mgr.PostDelegateAction((TransformAction.PosY), ()=> {
				Transform trans =  action.TransSelector();
				return (float)(Math.Round((trans.position.y), 4));
			});

			mgr.PostDelegateAction((TransformAction.PosZ), ()=> {
				Transform trans =  action.TransSelector();
				return (float)(Math.Round((trans.position.z), 4));
			});

			mgr.PostDelegateAction((TransformAction.RotX), ()=> {
				Transform trans =  action.TransSelector();
				return (float)(Math.Round((trans.rotation.eulerAngles.x), 4));
			});

			mgr.PostDelegateAction((TransformAction.RotY), ()=> {
				Transform trans =  action.TransSelector();
				return (float)(Math.Round((trans.rotation.eulerAngles.y), 4));
			});

			mgr.PostDelegateAction((TransformAction.RotZ), ()=> {
				Transform trans =  action.TransSelector();
				return (float)(Math.Round((trans.rotation.eulerAngles.z), 4));
			});
			//action.FrameID = curFrameID + 1;
			//tempActions.Add(action);
		}

		/// <summary>
		/// Posts the transform action.
		/// </summary>
		/// <param name="transSelector">Trans selector.</param>
		public void PostTransformAction(Func<Transform> transSelector)
		{
			SFTransformDelegateAction<float> action = new SFTransformDelegateAction<float> (transSelector);
			PostTransformAction (action);
		}

		/// <summary>
		/// Posts the transform action.
		/// </summary>
		/// <param name="action">Action.</param>
		public void PostTransformAction(SFPostionDelegateAction<float> action) 
		{
			//create the six actions
			mgr.PostDelegateAction((TransformAction.PosX), ()=> {
				Vector3 vec =  action.PosSelector();
				return (vec.x);
			});

			mgr.PostDelegateAction((TransformAction.PosY), ()=> {
				Vector3 vec =  action.PosSelector();
				return (vec.y);
			});

			mgr.PostDelegateAction((TransformAction.PosZ), ()=> {
				Vector3 vec =  action.PosSelector();
				return (vec.z);
			});

			mgr.PostDelegateAction((TransformAction.RotX), ()=> {
				Quaternion rotation =  action.RotSelector();
				return (rotation.eulerAngles.x);
			});

			mgr.PostDelegateAction((TransformAction.RotY), ()=> {
				Quaternion rotation =  action.RotSelector();
				return (rotation.eulerAngles.y);
			});

			mgr.PostDelegateAction((TransformAction.RotZ), ()=> {
				Quaternion rotation =  action.RotSelector();
				return (rotation.eulerAngles.z);
			});
			//action.FrameID = curFrameID + 1;
			//tempActions.Add(action);
		}

		/// <summary>
		/// Posts the transform action.
		/// </summary>
		/// <param name="posSelector">Position selector.</param>
		/// <param name="rotSelector">Rot selector.</param>
		public void PostTransformAction(Func< Vector3> posSelector, Func< Quaternion> rotSelector)
		{
			SFPostionDelegateAction<float> action = new SFPostionDelegateAction<float> (posSelector, rotSelector);
			PostTransformAction (action);
		}

		/// <summary>
		/// Tyes the get action postion.
		/// </summary>
		/// <returns><c>true</c>, if get action postion was tyed, <c>false</c> otherwise.</returns>
		/// <param name="postion">Postion.</param>
		/// <param name="rotation">Rotation.</param>
		public bool TyeGetActionPostion(out Vector3 postion, out Quaternion rotation)
		{
			float x, y, z, rx, ry, rz;
			if (TryGetAction (TransformAction.PosX, out x) &&
				TryGetAction (TransformAction.PosY, out y) &&
				TryGetAction (TransformAction.PosZ, out z) &&
				TryGetAction (TransformAction.RotX, out rx) &&
				TryGetAction (TransformAction.RotY, out ry) &&
				TryGetAction (TransformAction.RotZ, out rz)) {
				postion = new Vector3 (x, y, z);
				rotation = Quaternion.Euler (rx, ry, rz);
				return true;
			} else {
				postion = Vector3.zero;
				rotation = Quaternion.identity;
				return false;
			}
		}

	}



//	public static class TransformRecorderAgentSupports
//	{
//		public static void PostTransformAction(this SFActionsAgent<T, ActionType, float> mgr, SFTransformDelegateAction<float> action)
//		{
//			
//		}
//	}
}

