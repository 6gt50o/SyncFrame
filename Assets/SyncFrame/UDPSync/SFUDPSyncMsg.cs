using System;
using UDPZ;
using ProtoBuf;
using System.Collections;
using System.Collections.Generic;

namespace SyncFrame
{
	public enum SFUDPSyncMsg
	{
		FrameData = RoomMsg.DataStart,			//broadcast to all user
		SyncEnd,					//sync complete
		FrameDataRemote				// from remote user's sync frame data
	}


	[ProtoContract]
	public class SFUDPSyncFrameData<ActionType, ParamType> where ActionType : IComparable
	{
		[ProtoMember(1)]
		public int UID;

		[ProtoMember(2)]
		public SFFrameJson<ActionType, ParamType> FrameData;

	}

	[ProtoContract]
	public class SFUDPSyncFrameDataResponse
	{
		[ProtoMember(1)]
		public int FrameID;

		[ProtoMember(2)]
		public int Status;
	}

}

