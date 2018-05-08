using ProtoBuf;
using SyncFrame;
using System;
using System.Collections;
using System.Collections.Generic;
using UDPZ;
using UniRx;
using UnityEngine;

public class TestUnnor : MonoBehaviour {

    //同时用于发送与接收
    [ProtoContract]
    public class TestData
    {
        [ProtoMember(1)]
        public int Data = 101;

        [ProtoMember(2)]
        public int Data1 = 102;
    }

    public enum TestCodeEnum
    {
        Down = 1,
        Up,
    }
    private UDPComConnor<TestCodeEnum> udpConnor = new UDPComConnor<TestCodeEnum>();
    IDisposable runObj;
    // Use this for initialization
    void Start () {
        udpConnor.Init(4001);
        udpConnor.Connect("192.168.0.142", 3001);

        // ObserveOnMainThread return to mainthread
        runObj = Observable.Start(() =>
        {
            Run();
        }).Subscribe();

    }
    private void Run()
    {
        udpConnor.Send<TestData>(new TestData(), TestCodeEnum.Down);
        while (true)
        {
            
            TestData o;
            CommonHeader<TestCodeEnum> header;
            if (udpConnor.Recv<TestData>(out o, out header) > 0)
            {
                Debug.Log("Recv " + o.Data.ToString() + " header " + header.Code);
            }
            else
            {
                Debug.Log("Timeout");
            }
            //break;
        }
    }
    // Update is called once per frame
    void Update () {
		
	}

    private void OnApplicationQuit()
    {
        runObj.Dispose();
    }
}
