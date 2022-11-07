using System;

namespace MPSTI.PlenoSoft.Core.WatiN.Net4.Controle
{
	public class InputLink : GetSet
	{
		public override void Set(String idOuNome, Object valor, Boolean forceChange = false)
		{
			navegador.ClickLink(idOuNome, Convert.ToString(valor).ToUpper().Contains("CLICK") || forceChange);
		}

		public override Object Get(String idOuNome, Boolean isValue)
		{
			return navegador.GetLinkValue(idOuNome, isValue);
		}
	}
}