using MPSTI.PlenoSoft.Core.Office.EPPlus.Attributes;
using OfficeOpenXml;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MPSTI.PlenoSoft.Core.Office.EPPlus.Controller
{
	public class ExcelReport
	{
		public async Task<byte[]> GerarAsync<T>(IEnumerable<T> list)
		{
			if (!list.Any()) return new byte[0];

			using var stream = new MemoryStream();
			using var package = new ExcelPackage(stream);

			package.Workbook.Properties.Author = "";
			package.Workbook.Properties.Title = "";
			var worksheet = package.Workbook.Worksheets.Add("Plan1");

			var dataTable = GetDataTable(list);
			worksheet.Cells.LoadFromDataTable(dataTable, true, OfficeOpenXml.Table.TableStyles.Light8);

			package.Save();

			stream.Position = 0;

			return await Task.FromResult(stream.ToArray());
		}

		private DataTable GetDataTable<T>(IEnumerable<T> list)
		{
			var dataTable = new DataTable();

			var properties = list.FirstOrDefault()?.GetType().GetProperties();
			var dataColumns = GetDataColumns(properties);
			dataTable.Columns.AddRange(dataColumns.ToArray());

			foreach (var item in list)
			{
				var row = dataTable.NewRow();
				row.ItemArray = properties.Select(x => x.GetValue(item)).ToArray();
				dataTable.Rows.Add(row);
			}

			return dataTable;
		}

		private IEnumerable<DataColumn> GetDataColumns(PropertyInfo[] properties)
		{
			foreach (var property in properties)
			{
				foreach (var attribute in property.GetCustomAttributes(true))
				{
					if (attribute is ExcelColumnAttribute excelColumn)
						yield return new DataColumn(excelColumn.Titulo);
				}
			}
		}
	}
}