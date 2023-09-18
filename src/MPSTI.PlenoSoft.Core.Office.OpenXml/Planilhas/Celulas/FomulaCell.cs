using DocumentFormat.OpenXml.Spreadsheet;
using MPSTI.PlenoSoft.Core.Office.OpenXml.Planilhas.Controller;
using System;

namespace MPSTI.PlenoSoft.Core.Office.OpenXml.Planilhas.Celulas
{
	public class FormulaCell : Cell
	{
		public FormulaCell(Celula celula, String formula)
		{
			DataType = CellValues.Number;
			CellFormula = new CellFormula { CalculateCell = true, Text = formula.Substring(1) };
			CellReference = celula.Referencia;
			StyleIndex = 2;
		}
	}
}