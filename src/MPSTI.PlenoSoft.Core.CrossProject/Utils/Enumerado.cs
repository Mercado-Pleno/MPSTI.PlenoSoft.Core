using System;
using System.Linq;

namespace MPSTI.PlenoSoft.Core.CrossProject.Utils
{
	public class Enumerado
	{
		public Enum Enum { get; set; }
		public string Name { get; set; }
		public int Value { get; set; }
		public string GroupName { get; set; }
		public string Description { get; set; }

		public Enumerado(Enum enumerado, Type type)
		{
			Enum = enumerado;
			Value = Convert.ToInt32(enumerado);
			Name = enumerado.ToString("G");
			var attributes = type.GetAttributes(Name);
			Description = attributes.GetDescription() ?? Name;
			GroupName = attributes.GetGroupName();
		}

		public static TEnum[] GetEnums<TEnum>() => Enum.GetValues(typeof(TEnum)).OfType<TEnum>().ToArray();

		public static Enumerado[] GetAll<TEnum>() where TEnum : Enum => GetEnums<TEnum>().Select(e => new Enumerado(e, typeof(TEnum))).ToArray();
	}
}