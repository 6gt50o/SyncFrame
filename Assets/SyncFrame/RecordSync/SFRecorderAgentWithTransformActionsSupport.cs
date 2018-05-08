using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SyncFrame
{
	public class SFRecorderAgentWithTransformActionsSupport<T, SubAction> : SFRecorderAgent<T, TransformActionMapper<SubAction>, float> 
		where T : SFMgr<T, TransformActionMapper<SubAction>, float>
		where SubAction : IConvertible, IComparable
	{

		public SFRecorderAgentWithTransformActionsSupport(T mgr) : base(mgr)
		{
			//mgr.OnSyncFrameEnd += OnPreSyncFrame;
		}

		/// <summary>
		/// Tyes the get action postion.
		/// </summary>
		/// <returns><c>true</c>, if get action postion was tyed, <c>false</c> otherwise.</returns>
		/// <param name="postion">Postion.</param>
		/// <param name="rotation">Rotation.</param>
		public bool TryGetActionPostion(out Vector3 postion, out Quaternion rotation)
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
}

