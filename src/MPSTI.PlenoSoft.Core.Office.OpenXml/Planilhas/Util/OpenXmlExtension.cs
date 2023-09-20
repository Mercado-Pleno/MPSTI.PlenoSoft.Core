using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using MPSTI.PlenoSoft.Core.Office.OpenXml.Planilhas.Celulas;
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

        public static void WriteAll(this SheetData sheetData, IEnumerable lista, IEnumerable<ExcelColumnAttribute> attributes)
        {
            if (attributes.Any())
            {
                WriteHeader(sheetData, attributes);
                WriteLines(sheetData, lista, attributes);
            }
        }

        public static void WriteHeader(this SheetData sheetData, IEnumerable<ExcelColumnAttribute> attributes)
        {
            var style = StyleFactory.Get(Style.Header);
            var line = sheetData.ChildElements.Count + 1;
            var newRow = sheetData.NewRow(line);
            var column = 0;
            foreach (var attribute in attributes)
            {
                var cell = CellFactory.Create(line, ++column, attribute.Title, style);
                newRow.AppendChild(cell);
            }
            ResizeColumns(sheetData, attributes);
        }

        public static void ResizeColumns(this SheetData sheetData, IEnumerable<ExcelColumnAttribute> attributes)
        {
            var columns = sheetData.Parent.GetFirstChild<Columns>();
            columns.Resize(attributes.Select(x => x.Width));
        }

        public static void WriteLines(this SheetData sheetData, IEnumerable lista, IEnumerable<ExcelColumnAttribute> attributes)
        {
            foreach (var item in lista)
                WriteLine(sheetData, item, attributes);
        }

        public static void WriteLine(this SheetData sheetData, object item, IEnumerable<ExcelColumnAttribute> attributes)
        {
            var line = sheetData.ChildElements.Count + 1;
            var newRow = sheetData.NewRow(line);
            var column = 0;
            foreach (var attribute in attributes)
            {
                var value = attribute.GetValue(item);
                var cell = CellFactory.Create(line, ++column, value, null);
                newRow.AppendChild(cell);
            }
        }

        public static SheetData GetSheetDataBySheet(this WorkbookPart workbookPart, Sheet sheet)
        {
            if ((workbookPart == null) || (sheet == null)) return new SheetData();
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
                row = sheetData.NewRow(linha);
            return row;
        }

        private static Row NewRow(this SheetData sheetData, int linha)
        {
            var row = new Row { RowIndex = (UInt32)linha };
            sheetData.Append(row);
            return row;
        }

        public static Columns Resize(this Columns columns, IEnumerable<double?> widths)
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