using ProtoBuf;
using ProtoBuf.Meta;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace UDPZ
{
    [ProtoContract]
    [System.Serializable]
    public class NullMsg
    {
//		[ProtoMember(1)]
//		public int  Data = 3;
    }

	/// <summary>
	/// Common header.
	/// </summary>
    [Serializable]
   // [ProtoContract]
    //[ProtoInclude(2, typeof(UIDHeader<RoomMsg>))]
    public class CommonHeader<CodeType> where CodeType : IConvertible
    {
       // [ProtoMember(1)]
        public int Code = 1;

        public CommonHeader(CodeType type)
        {
            Code = Convert.ToInt32(type);
        }

        public CommonHeader()
        {
            Code = Convert.ToInt32(default(CodeType));
        }

        public static implicit operator CommonHeader<CodeType>(CodeType type)
        {
            //TODO use the object pool
            return new CommonHeader<CodeType>(type);
        }

        //public CodeType CCode
        //{
        //    get
        //    {
        //        return Code;
        //    }
        //}
    }

	/// <summary>
	/// UID header.
	/// </summary>
   	[ProtoContract]
    public class UIDHeader<CodeType> : CommonHeader<CodeType> where CodeType : IConvertible
    {
        [ProtoMember(2)]
        public int ID = 100;

        static UIDHeader()
        {
			Debug.Log ("UIDHeader ");
            RuntimeTypeModel a = RuntimeTypeModel.Default;
//            a[typeof(CommonHeader<CodeType>)].AddSubType(2, typeof(UIDHeader<CodeType>));// 这一句也可以写在Animal的static定义里
//            a[typeof(UIDHeader<CodeType>)].Add(1, "ID");
			a[typeof(UIDHeader<CodeType>)].Add(1, "Code");
        }

        public UIDHeader(CodeType type) : base(type)
        {

        }
	
		public UIDHeader()
		{

		}

        public static implicit operator UIDHeader<CodeType>(CodeType type)
        {
            //TODO use the object pool
            return new UIDHeader<CodeType>(type);
        }

    }

	/// <summary>
	/// UDP protocal.
	/// </summary>
    [Serializable]
    [ProtoContract]
    public class UDPProtocal<H, D>
    {
        [ProtoMember(1)]
        public H Header;

        [ProtoMember(2)]
        public D Data;
    }

	/// <summary>
	/// UDP protocal ret. for inner use to return the data
	/// </summary>
    [Serializable]
    [ProtoContract]
    public class UDPProtocalRet<H>
    {
        [ProtoMember(1)]
        public H Header;
    }

	/// <summary>
	/// Proto data reader. for read the data by type
	/// </summary>
    public class ProtoDataReader<Header>
    {
        public byte[]  RawData;

        public T Get<T>()
        {
            T ret = default(T);
            using (MemoryStream memory = new MemoryStream(RawData))
            {
                //var ret = Serializer.Deserialize<UDPProtocalRet<Header>>(memory);
                var retPack = Serializer.Deserialize<UDPProtocal<Header, T> >(memory);
                if (retPack != null)
                {
                    ret = retPack.Data;
                }
            }

            return ret;
        }
	

		public Header GetHeader()
		{
			var ret = default(Header);
			using (MemoryStream memory = new MemoryStream(RawData))
			{
				var retHeader = Serializer.Deserialize<UDPProtocalRet<Header>>(memory);
				//var retPack = Serializer.Deserialize<UDPProtocal<Header, T> >(memory);
				if (retHeader != null)
					ret = retHeader.Header;
			}

			return ret;
		}
    }

    public class UDPComConnor<SubEnum> : UDPConnor<CommonHeader<SubEnum>> where SubEnum: IConvertible { }
}
