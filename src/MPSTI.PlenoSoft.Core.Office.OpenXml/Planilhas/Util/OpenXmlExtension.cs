using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using MPSTI.PlenoSoft.Core.Office.OpenXml.Planilhas.Celulas;
using MPSTI.PlenoSoft.Core.Office.OpenXml.Planilhas.Controller;
using MPSTI.PlenoSoft.Core.Office.OpenXml.Planilhas.Integracao;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MPSTI.PlenoSoft.Core.Office.OpenXml.Planilhas.Util
{
    public static class OpenXmlExtension
	{

        public static Type GetTypeOfItems<T>(this IEnumerable<T> _) => typeof(T);

        public static void WriteAll(this SheetData sheetdata, IEnumerable lista, IEnumerable<ExcelColumnAttribute> attributes)
        {
            WriteHeader(sheetdata, attributes);
            WriteLines(sheetdata, lista, attributes);
        }

        public static void WriteHeader(this SheetData sheetData, IEnumerable<ExcelColumnAttribute> attributes)
        {
            WriteLine(sheetData, attributes, Style.Header, attribute => attribute.Title);
            ResizeColumns(sheetData, attributes);
        }

        public static void ResizeColumns(this SheetData sheetData, IEnumerable<ExcelColumnAttribute> attributes)
        {
            var columns = sheetData.Parent.GetFirstChild<Columns>();
            columns.Resize(attributes.Select(x => x.Width));
        }

        public static void WriteLines(this SheetData sheetdata, IEnumerable lista, IEnumerable<ExcelColumnAttribute> attributes)
        {
            foreach (var item in lista)
                WriteLine(sheetdata, attributes, style: null, attribute => attribute.GetValue(item));
        }

        public static void WriteLine(this SheetData sheetdata, IEnumerable<ExcelColumnAttribute> attributes, Style? style, Func<ExcelColumnAttribute, object> getValue)
        {
            if (attributes.Any())
            {
                var line = sheetdata.ChildElements.Count + 1;
                var column = 0;
                var newRow = sheetdata.GetRow(line);

                foreach (var attribute in attributes)
                {
                    var value = getValue.Invoke(attribute);
                    var cell = CellFactory.Create(line, ++column, value, StyleFactory.Get(style));
                    newRow.AppendChild(cell);
                }
            }
        }

        public static SheetData GetSheetDataBySheet(this WorkbookPart workbookPart, Sheet sheet)
		{
			if ((workbookPart == null) || (sheet == null)) return null;
			var worksheetPart = workbookPart.GetPartById(sheet.Id) as WorksheetPart;
			return worksheetPart?.Worksheet.GetFirstChild<SheetData>();
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

        public static Columns Resize(this Columns columns, IEnumerable<double> widths)
        {
            columns.RemoveAllChildren();

            var index = 0;
            foreach (var width in widths)
            {
                var column = CreateColumn(++index, width);
                columns.Append(column);
            }
            return columns;
        }

        public static Column CreateColumn(int index, double? width, bool bestFit = true)
        {
            return new Column
            {
                BestFit = bestFit,
                Min = (UInt32)index,
                Max = (UInt32)index,
                Width = width,
                CustomWidth = width.HasValue,
            };
        }

    }
}