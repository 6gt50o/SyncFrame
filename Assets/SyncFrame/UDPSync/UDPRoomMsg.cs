using System;

namespace UDPZ
{
	/// <summary>
	/// Room message. your custom Msg must set the Enter and Leave (from 1)
	/// </summary>
	public enum RoomMsg
	{
		//request and response
		Connect = 1,
		Disconnect = 2,

		//event
		UserEnter = 3,
		UserLeave = 4,

		//for sub msg type base
		DataStart = 100,		//所有数据类型子类型的起始位置 

	}

	public class RoomMsgMapper<T> : IConvertible, IComparable
	{
		public int data;

		public RoomMsgMapper(T data) 
		{
			this.data = Convert.ToInt32(data);
		}

		public RoomMsgMapper(RoomMsg data) 
		{
			this.data = Convert.ToInt32(data);
			//bTransAction = true;
		}


		public int CompareTo(object obj)
		{
			//if ()
			return data.CompareTo(((RoomMsgMapper<T>)obj).data);
			//return _innerCompareTo(obj);
		}

		public TypeCode GetTypeCode()  
		{  
			return TypeCode.Object;  
		}  

		public bool ToBoolean(IFormatProvider provider)  
		{  
			throw new Exception("The method or operation is not implemented.");  
		}  

		public byte ToByte(IFormatProvider provider)  
		{  
			throw new Exception("The method or operation is not implemented.");  
		}  

		public char ToChar(IFormatProvider provider)  
		{  
			throw new Exception("The method or operation is not implemented.");  
		}  

		public DateTime ToDateTime(IFormatProvider provider)  
		{  
			throw new Exception("The method or operation is not implemented."); 
		}  

		public decimal ToDecimal(IFormatProvider provider)  
		{  
			throw new Exception("The method or operation is not implemented."); 
		}  

		public double ToDouble(IFormatProvider provider)  
		{  
			throw new Exception("The method or operation is not implemented.");  
		}  

		public short ToInt16(IFormatProvider provider)  
		{  
			throw new Exception("The method or operation is not implemented."); 
		}  

		public int ToInt32(IFormatProvider provider)  
		{  
			return this.data;//Convert.ToInt32(this.userData);  
		}  

		public long ToInt64(IFormatProvider provider)  
		{  
			throw new Exception("The method or operation is not implemented.");  
		}  

		public sbyte ToSByte(IFormatProvider provider)  
		{  
			throw new Exception("The method or operation is not implemented.");  
		}  

		public float ToSingle(IFormatProvider provider)  
		{  
			throw new Exception("The method or operation is not implemented.");  
		}  

		public string ToString(IFormatProvider provider)  
		{  
			throw new Exception("The method or operation is not implemented."); 
		}  

		public object ToType(Type conversionType, IFormatProvider provider)  
		{  
			switch (Type.GetTypeCode(conversionType))  
			{  
			case TypeCode.Object:  

				throw new InvalidCastException(String.Format("Conversion to a {0} is not supported.", conversionType.Name));  
			case TypeCode.Int32:  
				return ToInt32(null);  
			case TypeCode.Decimal:  
				return ToDecimal(null);  
			case TypeCode.DateTime:  
				return ToDateTime(null);  
			case TypeCode.String:  
				return ToString(null);  
			default:  
				throw new InvalidCastException(String.Format("Conversion to {0} is not supported.", conversionType.Name));  
			}  
		}  

		public ushort ToUInt16(IFormatProvider provider)  
		{  
			throw new Exception("The method or operation is not implemented.");  
		}  

		public uint ToUInt32(IFormatProvider provider)  
		{  
			throw new Exception("The method or operation is not implemented.");  
		}  

		public ulong ToUInt64(IFormatProvider provider)  
		{  
			throw new Exception("The method or operation is not implemented.");  
		}

		public static implicit operator RoomMsgMapper<T>(RoomMsg data)
		{
			//TODO use the object pool
			return new RoomMsgMapper<T> (data);
		}

		public static implicit operator RoomMsgMapper<T>(T data)
		{
			//TODO use the object pool
			return new RoomMsgMapper<T> (data);
		}
	}
}

