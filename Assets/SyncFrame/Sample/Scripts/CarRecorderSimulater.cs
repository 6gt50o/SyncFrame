using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SyncFrame
{
    public class CarRecorderSimulater : MonoBehaviour
    {
        TestCarControl CarFrame;

        //TestCarControl.SFRecorderAgent recorder;
		SFRecorderAgentWithTransformActionsSupport<TestCarControl, TestAction> recorder;

        // Use this for initialization
        void Start()
        {
            CarFrame = GetComponent<TestCarControl>();
            //recorder = new TestCarControl.SFRecorderAgent(CarFrame);
			recorder = new SFRecorderAgentWithTransformActionsSupport<TestCarControl, TestAction>(CarFrame);

            recorder.LoadActionsJson("test.json");
        }

        // Update is called once per frame
        void Update()
        {

        }

        public float GetAxis(TestAction action)
        {
            float param = 0;
            if (recorder.TryGetAction(action, out param))
            {

            }

            return param;
        }


        public bool GetButtonDown(TestAction action)
        {
            float param = 0;
            if (recorder.TryGetAction(action, out param))
            {

            }

            return param != 0;
        }

		public bool GetButtonUp(TestAction action)
		{
			float param = 1;
			if (recorder.TryGetAction(action, out param))
			{
				return param == 0;
			}

			return false;
		}

		public bool TryGetActionPostion(out Vector3 postion, out Quaternion rotation)
		{
			return recorder.TryGetActionPostion (out postion, out rotation);
		}
    }

}
