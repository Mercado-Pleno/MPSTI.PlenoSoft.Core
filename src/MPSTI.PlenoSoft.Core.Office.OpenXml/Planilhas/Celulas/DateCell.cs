using DocumentFormat.OpenXml.Spreadsheet;
using System;

namespace MPSTI.PlenoSoft.Core.Office.OpenXml.Planilhas.Celulas
{
    public class DateCell : Cell
	{
		public DateCell(Celula celula, DateTime? dateTime)
		{
			DataType = CellValues.Date;
			CellReference = celula.Referencia;
			StyleIndex = 3;
			CellValue = new CellValue
			{
				Text = (dateTime.HasValue && (dateTime.Value != default))
					? dateTime.Value.ToString("yyyy-MM-dd")
					: String.Empty
			};
		}
	}
}