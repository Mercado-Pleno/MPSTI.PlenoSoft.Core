using Newtonsoft.Json;
using System;

namespace MPSTI.PlenoSoft.Core.CrossProject.Utils
{
	public class EnumConverter<TEnum> : JsonConverter where TEnum : Enum
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			var enumerado = (TEnum)value;
			writer.WriteValue(enumerado.GetString());
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			var enumString = (string)reader.Value;

			return Enum.Parse(typeof(TEnum), enumString, true);
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(string);
		}
	}
}
