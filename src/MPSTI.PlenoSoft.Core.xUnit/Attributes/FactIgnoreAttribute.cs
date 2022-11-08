using Xunit;

namespace MPSTI.PlenoSoft.Core.xUnit.Attributes
{
	public class FactIgnoreAttribute : FactAttribute
    {
        private const string SkipReason = "Teste desativado com [FactIgnore].";
        public override string Skip { get => base.Skip; set => base.Skip = $"{value} {SkipReason}"; }

        public FactIgnoreAttribute(string reason = "") { Skip = reason; }
    }
}