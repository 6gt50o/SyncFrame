using System;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;
using SyncFrame;
using UniRx;

public class TestRemoteCarControl : SFMgr<TestRemoteCarControl, TransformActionMapper<TestAction>, float>
{
	// the car controller we want to use
	public GameObject AgentCarTemplate;

	// Use this for initialization
	new void Start()
	{
		base.Start();
	
		//Observable.NextFrame ().Subscribe (_ => {

			//test for UDP
			InitUDP (4001, 2);

			var ma = GetUDPSyncMachine ();
			ma.OnAgentEnter += ag => {
				AddAgentCar (ag as TestCarAgent);
			};

			ma.AgentCreator = x => {
				return new TestCarAgent (this);
			};
		//});

		StartSync ();

	}

	private void AddAgentCar(TestCarAgent agent)
	{
		Observable.NextFrame().Subscribe(_=> {
			//Debug.Log("AddAgentCar");
			var car = GameObject.Instantiate (AgentCarTemplate);
			if (car != null) {
				//Debug.Log("AddAgentCar obj");
				TestAgentCar carAgent = car.gameObject.GetComponent<TestAgentCar> ();
				carAgent.SetAgent (agent);
			}
		});
		

	}
}


