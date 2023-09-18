using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using MPSTI.PlenoSoft.Core.Office.OpenXml.Planilhas.Celulas;
using System;

namespace MPSTI.PlenoSoft.Core.Office.OpenXml.Planilhas.Controller
{
    public readonly struct Celula
	{
		public readonly String Coluna;
		public readonly Int32 Linha;
		public readonly Int32 NumeroColuna;
		public readonly String Referencia;

		public Celula(String coluna, Int32 linha)
		{
			Coluna = coluna;
			Linha = linha;
			NumeroColuna = Util.Coluna.ObterIndicePor(Coluna);
			Referencia = Coluna + Linha.ToString();
		}

		public static Celula From(Int32 coluna, Int32 linha)
		{
			return new Celula(Util.Coluna.ObterNomePor(coluna), linha);
		}

		public static Cell Criar(Int32 coluna, Int32 linha, Object obj, UInt32? style)
		{
			return Criar(From(coluna, linha), obj, style);
		}

		public static Cell Criar(Celula celula, Object obj, UInt32? style)
		{
			Cell c;
			if (obj == null)
				c = new Cell { CellReference = celula.Referencia };

			else if (obj is Boolean)
				c = LogicalCell.Create(celula, Convert.ToBoolean(obj));

			else if (obj is DateTime)
				c = new DateCell(celula, Convert.ToDateTime(obj));

			else if (obj is Decimal)
				c = new NumberCell(celula, Convert.ToDecimal(obj));

			else if (obj is Double)
				c = new NumberCell(celula, Convert.ToDouble(obj));

			else if (obj is Single)
				c = new NumberCell(celula, Convert.ToSingle(obj));

			else if (obj is Int64)
				c = new NumberCell(celula, Convert.ToInt64(obj));

			else if (obj is Int32)
				c = new NumberCell(celula, Convert.ToInt32(obj));

			else if (obj is Int16)
				c = new NumberCell(celula, Convert.ToInt16(obj));

			else if (obj is Byte)
				c = new NumberCell(celula, Convert.ToByte(obj));

			else if (obj is UInt64)
				c = new NumberCell(celula, Convert.ToUInt64(obj));

			else if (obj is UInt32)
				c = new NumberCell(celula, Convert.ToUInt32(obj));

			else if (obj is UInt16)
				c = new NumberCell(celula, Convert.ToUInt16(obj));

			else if (obj is SByte)
				c = new NumberCell(celula, Convert.ToSByte(obj));

			else if (obj is Char)
				c = new TextCell(celula, Convert.ToChar(obj));

			else if (obj is String)
				c = Convert.ToString(obj).StartsWith('=') ? new FormulaCell(celula, Convert.ToString(obj)) : new TextCell(celula, Convert.ToString(obj)) as Cell;

			else
				c = new TextCell(celula, Convert.ToString(obj));

			c.StyleIndex = (UInt32Value)style ?? c.StyleIndex;

			return c;
		}
	}
}