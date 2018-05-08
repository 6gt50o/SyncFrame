using System;

using SyncFrame;


public class TestCarAgent : SFUDPSyncAgent<TestRemoteCarControl, TransformActionMapper<TestAction>, float>
{
	public TestCarAgent (TestRemoteCarControl mgr) : base(mgr)
	{
		
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


