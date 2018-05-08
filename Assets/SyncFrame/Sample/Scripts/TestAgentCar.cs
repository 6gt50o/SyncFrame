using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SyncFrame;
using UnityEngine.UI;

using UnityStandardAssets.Vehicles.Car;
public class TestAgentCar : MonoBehaviour {

	private CarController m_Car; // th
	//public TestCarControl CarController;

	//private SFUDPSyncAgent<TestCarControl, TransformActionMapper<TestAction>, float> agent;
	private TestCarAgent agent;

	private void Awake()
	{
		// get the car controller
		m_Car = GetComponent<CarController>(); 


	}

	public void SetAgent(TestCarAgent agent)
	{
		this.agent = agent;

		agent.mgr.OnSyncFrame += _=>
		{
			if (agent != null)
			{
				float h = agent.GetAxis(TestAction.A0);
				float v = agent.GetAxis(TestAction.A1);

				m_Car.Move(h, v, v, 0);

				//Debug.Log("Frame UDPSync id is " + _.ToString() + " h  " + h.ToString() + " data is " + v.ToString());

				if (agent.GetButtonDown(TestAction.A2))
				{
					Debug.Log("GetButtonDown From UDPSync");
					m_Car.GetComponent<Rigidbody>().AddForce(Vector3.up * 400000);
				}

				if (agent.GetButtonUp(TestAction.A2))
				{
					Debug.Log("GetButton Up  From UDPSync");
					//m_Car.GetComponent<Rigidbody>().AddForce(Vector3.up * 400000);
				}


				if (agent.GetButton(TestAction.A2))
				{
					Debug.Log("GetButton  From UDPSync");
				}

				Vector3 pos;
				Quaternion q;
				//				if (transAgent.TyeGetActionPostion(out pos, out q))
				//				{
				//					//Debug.Log("Frame id is " + _.ToString() + "  TyeGetActionPostion " + pos.ToString() + " and rotation " + q.ToString());
				//					transform.position = pos;
				//					transform.rotation = q;
				//				}
			}
		};
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
