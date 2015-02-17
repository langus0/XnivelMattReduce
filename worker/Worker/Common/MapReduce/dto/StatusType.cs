using System;
using System.Reflection;
namespace Common
{
	public enum StatusType
	{ 	
		[Description("Idle")]
		IDLE,
		[Description("Prepared")]
		PREPARED,
		[Description("Mapper")]
		MAPPER,
		[Description("Waiting for reduce")]
		WAITING_FOR_REDUCE,
		[Description("Reduce")]
		REDUCE
	}

	public class DescriptionAttribute : System.Attribute
	{

		private string _value;

		public DescriptionAttribute(string value)
		{
			_value = value;
		}

		public string Value
		{
			get { return _value; }
		}

	}

	public static class StringEnum
	{
			public static string GetDescription (this Enum value)
		{            
			FieldInfo field = value.GetType ().GetField (value.ToString ());

			DescriptionAttribute attribute
				= Attribute.GetCustomAttribute (field, typeof(DescriptionAttribute))
				as DescriptionAttribute;

			return attribute == null ? value.ToString () : attribute.Value;
		}
	}
}

