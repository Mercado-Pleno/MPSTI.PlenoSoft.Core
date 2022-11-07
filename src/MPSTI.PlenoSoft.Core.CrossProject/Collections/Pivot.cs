using System;
using System.Collections.Generic;
using System.Linq;

namespace MPSTI.PlenoSoft.Core.CrossProject.Collections
{
	public class Pivot<T>
	{
		private readonly Func<T, object> _row;
		private readonly Func<T, object> _col;
		private readonly Func<IEnumerable<T>, object> _inf;

		public Pivot(Func<T, object> rowField, Func<T, object> colField, Func<IEnumerable<T>, object> calcField)
		{
			_row = rowField;
			_col = colField;
			_inf = calcField;
		}

		public object[,] Execute(IEnumerable<T> dataSource)
		{
			var linhas = dataSource.OrderBy(_row).Select(_row).Distinct().ToList();
			var colunas = dataSource.OrderBy(_col).Select(_col).Distinct().ToList();
			var matriz = new object[linhas.Count + 1, colunas.Count + 1];

			foreach (var linha in linhas)
			{
				var indexRow = linhas.IndexOf(linha) + 1;
				matriz[indexRow, 0] = linha;

				foreach (var coluna in colunas)
				{
					var indexCol = colunas.IndexOf(coluna) + 1;
					matriz[0, indexCol] = coluna;

					var dados = dataSource.Where(ds => _row(ds).Equals(linha) && _col(ds).Equals(coluna));
					var informacao = _inf?.Invoke(dados.ToArray());

					matriz[indexRow, indexCol] = informacao;
				}
			}
			return matriz;
		}

		public IEnumerable<string> Print(object[,] matriz, string split = "\t")
		{
			var linhas = matriz.GetUpperBound(0);
			var colunas = matriz.GetUpperBound(1);

			for (var linha = 0; linha <= linhas; linha++)
			{
				var result = "";
				for (var coluna = 0; coluna <= colunas; coluna++)
				{
					var dados = matriz[linha, coluna] ?? "";
					result += split + dados.ToString();
				}
				yield return result.Substring(split.Length);
			}
		}
	}

	public class PivotBuilder<T>
	{
		private Func<T, object> _row;
		private Func<T, object> _col;
		private Func<IEnumerable<T>, object> _inf;

		public PivotBuilder<T> DefinirLinha(Func<T, object> row)
		{
			_row = row;
			return this;
		}

		public PivotBuilder<T> DefinirColuna(Func<T, object> col)
		{
			_col = col;
			return this;
		}

		public PivotBuilder<T> DefinirInformacao(Func<IEnumerable<T>, object> inf)
		{
			_inf = inf;
			return this;
		}

		public Pivot<T> Build()
		{
			return new Pivot<T>(_row, _col, _inf);
		}
	}
}