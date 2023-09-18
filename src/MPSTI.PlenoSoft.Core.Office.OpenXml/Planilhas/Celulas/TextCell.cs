using DocumentFormat.OpenXml.Spreadsheet;
using MPSTI.PlenoSoft.Core.Office.OpenXml.Planilhas.Controller;
using System;

namespace MPSTI.PlenoSoft.Core.Office.OpenXml.Planilhas.Celulas
{
	public class TextCell : Cell
	{
		public TextCell(Celula celula, String texto)
		{
			DataType = CellValues.InlineString;
			CellReference = celula.Referencia;
			InlineString = new InlineString { Text = new Text { Text = texto ?? String.Empty } };
		}

		public TextCell(Celula celula, Char chr) : this(celula, chr.ToString()) { }
	}
}