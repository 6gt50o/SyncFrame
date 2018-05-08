using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ProtoBuf;

namespace SyncFrame
{
	/// <summary>
	/// Transform action.
	/// </summary>
	public enum TransformAction
	{
		PosX = 600,
		PosY,
		PosZ,
		RotX,
		RotY,
		RotZ
	}

	/// <summary>
	/// I transfromable.
	/// </summary>
	public interface ITransfromable
	{

	}

	/// <summary>
	/// SF transform delegate action.
	/// </summary>
	public class SFTransformDelegateAction<ParamType>
	{
		private Func<Transform> transSelector;

		public Func<Transform> TransSelector
		{
			get {
				return transSelector;
			}
		}

		public SFTransformDelegateAction(Func<Transform> transSelector)
		{

			this.transSelector = transSelector;
		}

	}

	/// <summary>
	/// SF postion delegate action.
	/// </summary>
	public class SFPostionDelegateAction<ParamType>
	{
		private Func< Vector3> posSelector;
		private Func< Quaternion> rotSelector;

		public Func<Quaternion> RotSelector
		{
			get {
				return rotSelector;
			}
		}

		public Func<Vector3> PosSelector
		{
			get {
				return posSelector;
			}
		}

		public SFPostionDelegateAction(Func< Vector3> posSelector, Func< Quaternion> rotSelector)
		{

			this.posSelector = posSelector;
			this.rotSelector = rotSelector;
		}

	}

	/// <summary>
	/// Transform action mapper.
	/// </summary>
	[ProtoContract]
	public class TransformActionMapper<T>: IComparable, ITransfromable, IConvertible  where T : IConvertible, IComparable
	{
		[ProtoMember(1)]
		public int data;

		private bool bTransAction = false;

		public TransformActionMapper() 
		{
			this.data = -1;
		}

		public TransformActionMapper(T data) 
		{
			this.data = Convert.ToInt32(data);
		}

		public TransformActionMapper(TransformAction data) 
		{
			this.data = Convert.ToInt32(data);
			bTransAction = true;
		}

		public int CompareTo(object obj)
		{
			//if ()
			return data.CompareTo(((TransformActionMapper<T>)obj).data);
			//return _innerCompareTo(obj);
		}
		//protected abstract int _innerCompareTo (object obj);

		public static implicit operator TransformActionMapper<T>(T data)
		{
			//TODO use the object pool
			return new TransformActionMapper<T> (data);
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
//		public static implicit operator List<TransformActionMapper<T>>(List<T> datas)
//		{
//			//TODO use the object pool
//			var rets = new List<TransformActionMapper<T> >();
//
//			foreach (var d in datas) {
//				rets.Add (d);
//			}
//			return rets;
//		}

		public static implicit operator TransformActionMapper<T>(TransformAction data)
		{
			//TODO use the object pool
			return new TransformActionMapper<T> (data);
		}
	}


	/// <summary>
	/// SF mgr.
	/// </summary>
	public partial class SFMgr<T, ActionType, ParamType> : MonoBehaviour where T : SFMgr<T, ActionType, ParamType> where ActionType : IComparable
	{
//		public class SFTransformActionsAgent<SubAction> : SFTransformActionsAgent<T, SubAction>
//		{
//			
//		}

	}


	//	[ProtoContract]
	//	public class TransformActionDetailMapper<T> : TransformActionMapper<T> where T : IComparable 
	//	{
	//		[ProtoMember(2)]
	//		public T data;
	//
	//		public TransformActionDetailMapper() 
	//		{
	//			this.data = default(T);
	//		}
	//
	//		public TransformActionDetailMapper(T data) 
	//		{
	//			this.data = data;
	//		}
	//
	////		public static explicit operator TransformActionMapper<T>(T data)
	////		{
	////			return new TransformActionMapper<T> (data);
	////		}
	//
	//
	//
	//		protected override int _innerCompareTo(object obj)
	//		{
	//			//if ()
	//			return data.CompareTo(((TransformActionDetailMapper<T>)obj).data);
	//		}
	//	}

}

