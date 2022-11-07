using System;

namespace MPSTI.PlenoSoft.Core.WatiN.Net4.Controle
{
	public class InputText : GetSet
	{
		public override void Set(String idOuNome, Object valor, Boolean forceChange = false)
		{
			navegador.SetText(idOuNome, Convert.ToString(valor), forceChange);
		}

		public override Object Get(String idOuNome, Boolean isValue)
		{
			return navegador.GetTextValue(idOuNome, isValue);
		}
	}
}