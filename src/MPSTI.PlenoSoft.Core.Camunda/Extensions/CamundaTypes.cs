using System;
using System.Collections.Generic;

namespace MPSTI.PlenoSoft.Core.Camunda.Extensions
{
	public static class CamundaTypes
	{
		public const string DateTimeFormatWithTimeZone = "yyyy-MM-dd'T'HH:mm:sszzz";
		public const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

		public const string Int16 = "int";
		public const string Int32 = "int";
		public const string Int64 = "long";
		public const string Number = "double";
		public const string Boolean = "Boolean";
		public const string Json = "Json";
		public const string Object = "Object";
		public const string String = "String";
		public const string cGuid = "String";
		public const string DateTime = "String";

		private static readonly Dictionary<Type, string> _dicTypes = new()
		{
			{ typeof(short),   Int16 },
			{ typeof(short?),  Int16 },
			{ typeof(ushort),  Int16 },
			{ typeof(ushort?), Int16 },

			{ typeof(int),   Int32 },
			{ typeof(int?),  Int32 },
			{ typeof(uint),  Int32 },
			{ typeof(uint?), Int32 },

			{ typeof(long),   Int64 },
			{ typeof(long?),  Int64 },
			{ typeof(ulong),  Int64 },
			{ typeof(ulong?), Int64 },

			{ typeof(decimal),   Number },
			{ typeof(decimal?), Number },
			{ typeof(double),   Number },
			{ typeof(double?),  Number },
			{ typeof(float),    Number },
			{ typeof(float?),   Number },

			{ typeof(bool),   Boolean },
			{ typeof(bool?),  Boolean },

			{ typeof(char),   String },
			{ typeof(char?),  String },
			{ typeof(string), String },

			{ typeof(Guid),   cGuid },
			{ typeof(Guid?),  cGuid },

			{ typeof(DateTime),  DateTime },
			{ typeof(DateTime?), DateTime },
			{ typeof(DateOnly),  DateTime },
			{ typeof(DateOnly?), DateTime },

			{ typeof(DateTimeOffset),  DateTime },
			{ typeof(DateTimeOffset?), DateTime },

			{ typeof(object), Object },
		};

		public static string GetCamundaType<T>(T value)
		{
			if (IsJson(value as string))
				return Json;

			var type = GetType(value);

			if (type.IsEnum)
				return String;

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

			return value.StartsWith("{") && value.EndsWith("}")
				|| value.StartsWith("[") && value.EndsWith("]");
		}

		public static string DateTimeToStringTZ(DateTime dateTime)
		{
			return dateTime.ToString(DateTimeFormatWithTimeZone);
		}
	}
}