using ProtoBuf;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDPZ;
using System.IO;
using ProtoBuf.Meta;

public class TestRoom : MonoBehaviour {

    [ProtoContract]
    //[ProtoInclude(2, typeof(PersionSub))]
    public class Persion
    {
        [ProtoMember(1)]
        public int ID;

    }


    //[ProtoContract]
    public class PersionSub : Persion
    {
        static PersionSub()
        {
            RuntimeTypeModel a = RuntimeTypeModel.Default;
            a[typeof(Persion)].AddSubType(2, typeof(PersionSub));// 这一句也可以写在Animal的static定义里
            a[typeof(PersionSub)].Add(1, "SuID");
        }

       // [ProtoMember(2)]
        public int SuID;
    }


    [ProtoContract]
    [ProtoInclude(3, typeof(Persion))]
    public class PersionNode : Persion
    {
        [ProtoMember(1)]

        public Persion per;
    }


    [ProtoContract]
    public class TestData
    {
        [ProtoMember(1)]
        public int Data = 101;

        [ProtoMember(2)]
        public int Data1 = 102;
    }

	//
	public enum TestMsg
	{
		Data = RoomMsg.DataStart,
		Data1
	}

	private UDPRoom<TestMsg> room = new UDPRoom<TestMsg>();

    // Use this for initialization
    void Start () {
        using (MemoryStream memory = new MemoryStream())//初始化MemoryStream类
        {
            var package = new PersionSub();
            package.ID = 100;
            package.SuID = 101;

            Serializer.Serialize(memory, package);


            byte[] arrayNew = memory.ToArray();
            Debug.Log("data " + arrayNew.ToString());
        }

        room.Init(4001);
        //room.Connect("192.168.43.46", 3001);
		room.Connect("127.0.0.1", 3001);
		room.OnConnected += () => Debug.Log ("OnConnected");
		room.OnDisConnected += () => Debug.Log ("OnDisConnected");

        room.OnUserEnter += x => Debug.Log("Enter " + x);
        room.OnUserLeave += x => Debug.Log("Leave " + x);
        room.OnData += (x, reader) =>
        {
            //Debug.Log("data " + x);
            TestData data = reader.Get<TestData>();
            Debug.Log("data " + data.Data);
        };
        room.Run();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnApplicationQuit()
    {
        room.Dispose();
    }
}
