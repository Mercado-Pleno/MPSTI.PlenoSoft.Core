using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace MPSTI.PlenoSoft.Core.CrossProject.Utils
{
	public static class EnumUtil
	{
		public static string GetString<TEnum>(this TEnum enumerado) where TEnum : Enum
		{
			var @enum = enumerado.ToString("G");
			var @type = enumerado.GetType();
			var attrib = @type.GetAttributes(@enum);
			return attrib.GetDescription() ?? @enum;
		}

		public static object[] GetAttributes(this Type type, string descricao)
		{
			var fieldInfo = type.GetField(descricao);
			return fieldInfo?.GetCustomAttributes(true) ?? new object[0];
		}

		public static string GetGroupName(this object[] atributos)
		{
			var atributo = atributos?.FirstOrDefault(a => a is DisplayAttribute);
			return atributo is DisplayAttribute display ? display.GroupName : null;
		}

		public static string GetDescription(this object[] atributos)
		{
			var atributo = atributos?.FirstOrDefault(a => a is DisplayAttribute)
				?? atributos?.FirstOrDefault(a => a is DisplayNameAttribute)
				?? atributos?.FirstOrDefault(a => a is DescriptionAttribute)
				?? atributos?.FirstOrDefault(a => a is XmlEnumAttribute)
				?? atributos?.FirstOrDefault(a => a is EnumMemberAttribute)
				?? atributos?.FirstOrDefault(a => a is JsonPropertyAttribute)
				;

			if (atributo is DisplayAttribute display)
				return display.Name ?? display.Description ?? display.GroupName;

			if (atributo is DisplayNameAttribute displayName)
				return displayName.DisplayName;

			if (atributo is DescriptionAttribute description)
				return description.Description;

			if (atributo is XmlEnumAttribute xmlEnum)
				return xmlEnum.Name;

			if (atributo is EnumMemberAttribute enumMember)
				return enumMember.Value;

			if (atributo is JsonPropertyAttribute jsonProperty)
				return jsonProperty.PropertyName;

			return null;
		}
	}
}