using SyncFrame;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

//
//public class TestKeyAgent : SFActionsAgent<TestCarControl, TransformActionMapper<TestAction>, float>

public class TestKeyAgent : SFTransformActionsAgent<TestCarControl, TestAction>
{
    CarRecorderSimulater recorder;
    public bool bReplay = false;

    public TestKeyAgent(TestCarControl mgr, bool bReplay) : base(mgr)
    {
        this.bReplay = bReplay;
        mgr.OnSyncFrame += OnPreNextSyncFrame;
        mgr.OnUpdate += OnUpdate;
        recorder = mgr.GetComponent<CarRecorderSimulater>();

		//new List<TransformActionMapper<TestAction>>()

		SetCachedMask (new List<TestAction>{ TestAction.A0, TestAction.A1, TestAction.A2 });
    }

    private void OnUpdate()
    {
        //需要判断Input类型判断是否需要把GetKeyDown放到Update中
        if (Input.GetKeyDown(KeyCode.H))
        {
            mgr.PostDelegateAction(TestAction.A2, () => 1);
        }

		if (Input.GetKeyUp(KeyCode.H))
		{
			mgr.PostDelegateAction(TestAction.A2, () => 0);
		}
    }

    private void OnPreNextSyncFrame(int frameID)
    {
        if (!bReplay)
        {
			float h = (float)(Math.Round( CrossPlatformInputManager.GetAxis("Horizontal"), 4));
			float v = (float)(Math.Round(  CrossPlatformInputManager.GetAxis("Vertical"), 4));

//			if (v > 0)
//				Debug.Log("get input h is " + h + " v is " + v );

            mgr.PostDelegateAction(TestAction.A0, () => h);
            mgr.PostDelegateAction(TestAction.A1, () => v);



			//post the transform
			if (frameID % 10 == 0)
			{
				PostTransformAction(new SFTransformDelegateAction<float>(()=> mgr.transform));
			}

			//mgr.PostDelegateAction (TransformAction.PosX, () => 100);
        }
        else
        {
            float h = recorder.GetAxis(TestAction.A0);
            float v = recorder.GetAxis(TestAction.A1);

            if (recorder.GetButtonDown(TestAction.A2))
            {
                mgr.PostDelegateAction(TestAction.A2, () => 1);
			} 

			if (recorder.GetButtonUp(TestAction.A2))
			{
				mgr.PostDelegateAction(TestAction.A2, () => 0);
			}


			Vector3 pos;
			Quaternion rot;
			if (recorder.TryGetActionPostion (out pos, out rot)) {
				PostTransformAction(()=> pos, ()=> rot);
			}

            mgr.PostDelegateAction(TestAction.A0, () => h);
            mgr.PostDelegateAction(TestAction.A1, () => v);
        }

    }

    public float GetAxis(TestAction action)
    {
        float param = 0;
        if (TryGetAction(action, out param))
        {
            
        }

        return param;
    }

    public bool GetButtonDown(TestAction action)
    {
        float param = 0;
        if (TryGetAction(action, out param))
        {

        }

        return param != 0;
    }

	public bool GetButtonUp(TestAction action)
	{
		float param = 1;
		if (TryGetAction(action, out param))
		{
			return param == 0;
		}

		return false;
	}

	public bool GetButton(TestAction action)
	{
		float param = 0;
		if (TryGetCachedAction(action, out param))
		{

		}

		return param != 0;
	}

}
