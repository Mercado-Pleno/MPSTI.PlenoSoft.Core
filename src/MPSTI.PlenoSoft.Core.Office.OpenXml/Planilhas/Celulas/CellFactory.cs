using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using System;

namespace MPSTI.PlenoSoft.Core.Office.OpenXml.Planilhas.Celulas
{
    public static class CellFactory
    {
        public static Cell Create(int linha, int coluna, object obj, uint? style)
        {
            var celula = new Celula(coluna, linha);

            var cell = obj switch
            {
                null => new Cell { CellReference = celula.Referencia },
                bool => LogicalCell.Create(celula, Convert.ToBoolean(obj)),
                DateTime => new DateCell(celula, Convert.ToDateTime(obj)),
                decimal => new NumberCell(celula, Convert.ToDecimal(obj)),
                double => new NumberCell(celula, Convert.ToDouble(obj)),
                float => new NumberCell(celula, Convert.ToSingle(obj)),
                long => new NumberCell(celula, Convert.ToInt64(obj)),
                int => new NumberCell(celula, Convert.ToInt32(obj)),
                short => new NumberCell(celula, Convert.ToInt16(obj)),
                byte => new NumberCell(celula, Convert.ToByte(obj)),
                ulong => new NumberCell(celula, Convert.ToUInt64(obj)),
                uint => new NumberCell(celula, Convert.ToUInt32(obj)),
                ushort => new NumberCell(celula, Convert.ToUInt16(obj)),
                sbyte => new NumberCell(celula, Convert.ToSByte(obj)),
                char => new TextCell(celula, Convert.ToChar(obj)),
                string => CreateCell(celula, Convert.ToString(obj)),
                _ => new TextCell(celula, Convert.ToString(obj)),
            };

            cell.StyleIndex = (UInt32Value)style ?? cell.StyleIndex;

            return cell;
        }

        private static Cell CreateCell(Celula celula, string value) =>
            value.StartsWith('=') ? new FormulaCell(celula, value) : new TextCell(celula, value);
    }
}