using Xunit;

namespace MPSTI.PlenoSoft.Core.xUnit.Attributes
{
    public class FactDebuggerOnlyAttribute : FactAttribute
    {
        private const string SkipReason = "Teste desativado com [FactDebuggerOnly]. Caso necessário, rode localmente com o Debug Anexado (Debugger Attached). Right Click -> Debug Tests.";
        private readonly bool _active;
        public override string Skip { get => _active ? null : base.Skip; set => base.Skip = $"{value} {SkipReason}"; }

        public FactDebuggerOnlyAttribute(string reason = "")
        {
            Skip = reason;
#if DEBUG
			_active = System.Diagnostics.Debugger.IsAttached;
#else
			_active = false;
#endif
        }
    }
}