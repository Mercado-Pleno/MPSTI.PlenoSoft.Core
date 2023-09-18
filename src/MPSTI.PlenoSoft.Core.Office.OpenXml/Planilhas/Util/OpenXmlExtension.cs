using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MPSTI.PlenoSoft.Core.Office.OpenXml.Planilhas.Util
{
	public static class OpenXmlExtension
	{
		public static SheetData GetSheetDataBySheet(this WorkbookPart workbookPart, Sheet sheet)
		{
			if ((workbookPart == null) || (sheet == null)) return null;
			var worksheetPart = workbookPart.GetPartById(sheet.Id) as WorksheetPart;
			return worksheetPart?.Worksheet.Elements<SheetData>().FirstOrDefault();
		}

		public static IEnumerable<Row> GetRows(this SheetData sheetData, Func<Row, Int32, Boolean> filtro = null)
		{
			var rows = sheetData.ChildElements.OfType<Row>();
			return (filtro != null) ? rows.Where(filtro) : rows;
		}

		public static Row GetRow(this SheetData sheetData, Int32 linha)
		{
			var row = sheetData.GetRows((r, i) => r.RowIndex == linha).FirstOrDefault();
			if (row == null)
			{
				row = new Row { RowIndex = (UInt32)linha };
                sheetData.Append(row);
			}
			return row;
		}
	}
}