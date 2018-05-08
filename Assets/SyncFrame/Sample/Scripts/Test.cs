

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using ProtoBuf;
using UnityStandardAssets.Vehicles.Car;
using System;

namespace SyncFrame
{

    public enum TestAction
    {
		Default = 0,			//一定要定义Defaul为0，有时需要判断Action的值是否有效，或者直接使用Try
        A0 = 10, 
        A1,
        A2
    }

	public enum TestAction2
	{
		A0, 
		A1,
		A2
	}



    /// <summary>
    /// 自定义一个Agent
    /// </summary>
	public class TestAgent : SFActionsAgent<Test, TransformActionMapper<TestAction>, float>
    {
        public TestAgent(Test mgr) : base(mgr)
        {
        }
    }



	public class Test : SFMgr<Test, TransformActionMapper<TestAction>, float>
    {

        private CarController m_Car; // the car controller we want to use

        private void Awake()
        {
            // get the car controller
            m_Car = GetComponent<CarController>();
        }

        // Use this for initialization
        new void Start()
        {
            base.Start();

            //EnableRecord();

            //var agent = new SFActionsAgent(this);

            //test for read Recorder
            var recorder = new SFRecorderAgent(this);

            recorder.LoadActionsJson("test.json");
            

            base.OnSyncFrame += (f) =>
            {
                //this.PostAction(new SFDelegateAction(TestAction.A0, () => { return f; }));

                //float p =  agent.GetAction(TestAction.A0);

                //for test recorder json
                //float p = recorder.GetAction(TestAction.A0);
                float h = recorder.GetAction(TestAction.A0);
                float v = recorder.GetAction(TestAction.A1);

                m_Car.Move(h, v, v, 0);
                Debug.Log("Frame id is " + h.ToString() + " data is " + v.ToString());
            };

            base.OnSyncFrameEnd += (f) =>
            {
                //Debug.Log("OnSyncFrameEnd id is " + f.ToString() + Time.time);
            };

            base.OnStop += () =>
            {
                //SaveRecord("test.json");
            };

        }



        // Update is called once per frame
        new
        void Update()
        {

        }
    }

}
