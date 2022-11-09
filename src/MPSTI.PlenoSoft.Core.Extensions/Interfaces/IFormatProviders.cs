using System;

namespace MPSTI.PlenoSoft.Core.Extensions.Interfaces
{
    public interface IFormatProviders : IFormatProvider
    {
        IFormatProvider PT_BR { get; }
        IFormatProvider EN_US { get; }
        IFormatProvider Default { get; }
    }
}