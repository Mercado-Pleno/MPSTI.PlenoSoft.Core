using System;

namespace MPSTI.PlenoSoft.Core.WatiN.Net4.Controle
{
	public class InputSelect : GetSet
	{
		public override void Set(String idOuNome, Object valor, Boolean forceChange = false)
		{
			navegador.SetCombo(idOuNome, Convert.ToString(valor), forceChange);
		}

		public override Object Get(String idOuNome, Boolean isValue)
		{
			return navegador.GetComboValue(idOuNome, isValue);
		}
	}
}