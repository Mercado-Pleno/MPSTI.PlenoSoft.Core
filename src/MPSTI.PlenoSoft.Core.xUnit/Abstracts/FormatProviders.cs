using System;
using System.Globalization;

namespace MPSTI.PlenoSoft.Core.xUnit.Abstracts
{
	public interface IFormatProviders : IFormatProvider
	{
		IFormatProvider PT_BR { get; }
		IFormatProvider EN_US { get; }
		IFormatProvider Default { get; }
	}

	public class FormatProviders : IFormatProviders
	{
		public static readonly CultureInfo ptBR = new("pt-BR");
		public static readonly CultureInfo enUS = new("en-US");

		public IFormatProvider PT_BR => ptBR;
		public IFormatProvider EN_US => enUS;
		public IFormatProvider Default => enUS;

		public object GetFormat(Type formatType) => Default.GetFormat(formatType);
	}
}