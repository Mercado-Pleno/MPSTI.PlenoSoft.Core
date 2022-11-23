using System;
using System.Collections.Generic;

namespace MPSTI.PlenoSoft.Core.Camunda.Extensions
{
	/// <summary>
	/// https://docs.camunda.io/docs/components/concepts/variables/
	/// https://docs.camunda.org/manual/7.17/reference/rest/overview/variables/
	/// https://docs.camunda.org/manual/7.17/user-guide/process-engine/variables/#supported-variable-values
	/// </summary>
	public static class CamundaTypes
	{
		public const string DateTimeFormatWithTimeZone = "yyyy-MM-dd'T'HH:mm:sszzz";
		public const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

		public const string Boolean = "Boolean";
		public const string Bytes = "Bytes";
		public const string Short = "Short";
		public const string Integer = "Integer";
		public const string Long = "Long";
		public const string Number = "Number";
		public const string Double = "Double";
		public const string Date = "Date";
		public const string String = "String";
		public const string Null = "Null";
		public const string File = "File";
		public const string Object = "Object";
		public const string Json = "Json";
		public const string Xml = "Xml";
		public const string Array = "Array";

		public const string cInt16 = Short;
		public const string cInt32 = Integer;
		public const string cInt64 = Long;
		public const string cDecimal = Double;
		public const string cDouble = Double;
		public const string cFloat = Double;
		public const string cBoolean = Boolean;
		public const string cString = String;
		public const string cGuid = String;
		public const string cEnum = String;
		public const string cTime = String;
		public const string cDateTime = String;
		public const string cDate = Date;
		public const string cObject = Object;

		private static readonly Dictionary<Type, string> _dicTypes = new()
		{
			{ typeof(short),           cInt16 },
			{ typeof(short?),          cInt16 },
			{ typeof(ushort),          cInt16 },
			{ typeof(ushort?),         cInt16 },
			{ typeof(int),             cInt32 },
			{ typeof(int?),            cInt32 },
			{ typeof(uint),            cInt32 },
			{ typeof(uint?),           cInt32 },
			{ typeof(long),            cInt64 },
			{ typeof(long?),           cInt64 },
			{ typeof(ulong),           cInt64 },
			{ typeof(ulong?),          cInt64 },
			{ typeof(decimal),         cDecimal },
			{ typeof(decimal?),        cDecimal },
			{ typeof(double),          cDouble },
			{ typeof(double?),         cDouble },
			{ typeof(float),           cFloat },
			{ typeof(float?),          cFloat },
			{ typeof(bool),            cBoolean },
			{ typeof(bool?),           cBoolean },
			{ typeof(char),            cString },
			{ typeof(char?),           cString },
			{ typeof(string),          cString },
			{ typeof(Guid),            cGuid },
			{ typeof(Guid?),           cGuid },
			{ typeof(Enum),            cEnum },
			{ typeof(DateTime),        cDateTime },
			{ typeof(DateTime?),       cDateTime },
			{ typeof(DateOnly),        cDate },
			{ typeof(DateOnly?),       cDate },
			{ typeof(DateTimeOffset),  cTime },
			{ typeof(DateTimeOffset?), cTime },
			{ typeof(object),          cObject },
		};

		public static string GetCamundaType<T>(T value)
		{
			if (IsJson(value as string))
				return Json;

			var type = GetType(value);

			if (type.IsEnum)
				return cEnum;

			if (_dicTypes.TryGetValue(type, out string typeString))
				return typeString;

			return type.Name;
		}

		public static T ChangeType<T>(object value, T defaultValue)
		{
			var type = GetType(defaultValue);
			if (value is DateTimeOffset dateTimeOffsetValue)
				return (T)Convert.ChangeType(dateTimeOffsetValue.DateTime, type);

			if (defaultValue is Guid && Guid.TryParse(value as string, out var guid))
				return (T)Convert.ChangeType(guid, type);

			if (!type.IsEnum)
				return (T)Convert.ChangeType(value, type) ?? defaultValue;

			if (Enum.TryParse(type, value?.ToString(), true, out var enumerado))
				return (T)enumerado;

			return defaultValue;
		}

		public static object ConvertToCamundaValue<T>(T value)
		{
			if (value is DateTime dateTime)
				return dateTime.ToString(DateTimeFormat);

			if (value is Guid guid)
				return guid.ToString();

			var type = GetType(value);
			if (type.IsEnum)
				return value.ToString();

			return value;
		}

		private static Type GetType<T>(T value)
		{
			var type = value?.GetType() ?? typeof(T);
			return Nullable.GetUnderlyingType(type) ?? type;
		}

		private static bool IsJson(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
				return false;

			value = value.Trim();

			return (value.StartsWith("{") && value.EndsWith("}"))
				|| (value.StartsWith("[") && value.EndsWith("]"));
		}

		public static string DateTimeToStringTZ(DateTime dateTime)
		{
			return dateTime.ToString(DateTimeFormatWithTimeZone);
		}
	}
}