using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPSTI.PlenoSoft.Core.WatiN.Net4;
using MPSTI.PlenoSoft.Core.WatiN.Net4.Util;
using WatiN.Core;

namespace MPSTI.PlenoSoft.Core.Test.Net4
{
	[TestClass]
	public class Testando_WatiN
	{
		[TestMethod]
		public void Exemplo_De_Como_Automatizar_Pesquisa_No_Google()
		{
			var browser = WatiNExtension.ObterNavegador<IE>();
			browser.GoTo("https://www.google.com/search?q=WatiN");
			var html = browser.Html;
			Assert.IsNotNull(html);
		}

		[TestMethod]
		public void Exemplo_De_Como_Automatizar_Pesquisa_De_CEP_Nos_Correios()
		{
			var navegador = Navegador.New(TipoNavegador.InternetExplorer);
			navegador.IrPara("http://www.buscacep.correios.com.br/servicos/dnec/menuAction.do?Metodo=menuCep", TimeSpan.FromSeconds(10), "CEP");
			navegador.SetText("relaxation", "20090000", false);
			navegador.ClickButton("Buscar", true, true);
			var html = navegador.GetHtml();
			navegador.Fechar();
			Assert.IsNotNull(html);
		}
	}
}