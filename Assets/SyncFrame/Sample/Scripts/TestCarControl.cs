using SyncFrame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;
using System.IO;
using ProtoBuf;
using UniRx;

public class TestCarControl : SFMgr<TestCarControl, TransformActionMapper<TestAction>, float>
{

    private CarController m_Car; 

    public bool bEnableRecord = false;
    public bool bReplay = false;
    public bool bAutoStart = false;
    private void Awake()
    {
        // get the car controller
        m_Car = GetComponent<CarController>(); 
        
    }

	new void Start()
	{

		base.Start();
		Observable.NextFrame ().Subscribe (_ => {
			InitStart();
		});
	}
    // Use this for initialization
    
    void InitStart()
    {

        if (bAutoStart)
            StartSync();

        if (bEnableRecord)
            EnableRecord();

        //test for UDP
        //InitUDP(4001, 1);

		//var ma = GetUDPSyncMachine ();

        TestKeyAgent agent = new TestKeyAgent(this, bReplay);

		SFTransformActionsAgent<TestCarControl, TestAction> transAgent = new SFTransformActionsAgent<TestCarControl, TestAction> (this);

        base.OnSyncFrame += _=>
        {
            float h = agent.GetAxis(TestAction.A0);
            float v = agent.GetAxis(TestAction.A1);

            m_Car.Move(h, v, v, 0);

            //Debug.Log("Frame id is " + _.ToString() + " h  " + h.ToString() + " data is " + v.ToString());

            if (agent.GetButtonDown(TestAction.A2))
            {
                Debug.Log("GetButtonDown ");
                m_Car.GetComponent<Rigidbody>().AddForce(Vector3.up * 400000);
            }

			if (agent.GetButtonUp(TestAction.A2))
			{
				Debug.Log("GetButton Up ");
				//m_Car.GetComponent<Rigidbody>().AddForce(Vector3.up * 400000);
			}


			if (agent.GetButton(TestAction.A2))
			{
				Debug.Log("GetButton ");
			}

			Vector3 pos;
			Quaternion q;
			if (transAgent.TyeGetActionPostion(out pos, out q))
			{
				//Debug.Log("Frame id is " + _.ToString() + "  TyeGetActionPostion " + pos.ToString() + " and rotation " + q.ToString());
				transform.position = pos;
				transform.rotation = q;
			}

        };

        base.OnStop += () =>
        {
            if (bEnableRecord)
                SaveRecord("test.json");
        };
    }





    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }
}
