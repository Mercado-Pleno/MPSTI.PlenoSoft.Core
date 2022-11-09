using System;
using System.Globalization;
using MPSTI.PlenoSoft.Core.Extensions.Interfaces;

namespace MPSTI.PlenoSoft.Core.Extensions.Providers
{

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