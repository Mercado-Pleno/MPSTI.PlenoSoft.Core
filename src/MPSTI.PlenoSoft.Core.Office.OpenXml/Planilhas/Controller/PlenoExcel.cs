using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using MPSTI.PlenoSoft.Core.Office.OpenXml.Planilhas.Celulas;
using MPSTI.PlenoSoft.Core.Office.OpenXml.Planilhas.Integracao;
using MPSTI.PlenoSoft.Core.Office.OpenXml.Planilhas.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MPSTI.PlenoSoft.Core.Office.OpenXml.Planilhas.Controller
{
    /// <summary>
    /// https://learn.microsoft.com/pt-br/office/open-xml/spreadsheets
    /// </summary>
    public class PlenoExcel
    {
        private readonly Mode _mode;
        private readonly string _defaultFirstSheetName = $"Plan{DateTime.UtcNow.Ticks}";
        private readonly SpreadsheetDocument _spreadsheetDocument;

        protected virtual WorkbookPart WorkbookPart => _spreadsheetDocument.WorkbookPart;
        protected virtual Workbook Workbook => WorkbookPart.Workbook;
        protected virtual Sheets Sheets => Workbook.Sheets;

        public PlenoExcel(FileInfo arquivoExcel, Mode modo = Mode.Padrao)
        {
            var editavel = modo.Is(Mode.Editavel);
            var autoSave = modo.Is(Mode.SalvarAutomaticamente);
            var backup = modo.Is(Mode.FazerBackupAntes);
            var apagar = modo.Is(Mode.ApagarSeExistir);
            var criar = modo.Is(Mode.CriarSeNaoExistir);
            _mode = modo;

            if (!arquivoExcel.Directory.Exists())
                arquivoExcel.Directory.Create();

            if (backup)
                arquivoExcel.Backup();

            if (apagar && arquivoExcel.Exists())
                arquivoExcel.Delete();

            if (arquivoExcel.Exists())
                _spreadsheetDocument = SpreadsheetDocument.Open(arquivoExcel.FullName, editavel, new OpenSettings { AutoSave = autoSave });
            else if (!criar)
                throw new PlenoExcelException("Configurações de [Modo] inválidas.");
            else
            {
                var fileVersion = new FileVersion { ApplicationName = "PlenoExcel", BuildVersion = Assembly.GetExecutingAssembly().ObterVersaoDoAssembly().ToString() };
                var calculationProperties = new CalculationProperties { ForceFullCalculation = true, FullCalculationOnLoad = true, CalculationOnSave = true };
                _spreadsheetDocument = SpreadsheetDocument.Create(arquivoExcel.FullName, SpreadsheetDocumentType.Workbook, autoSave);

                var workbookPart = _spreadsheetDocument.AddWorkbookPart();
                workbookPart.Workbook = new Workbook(fileVersion, new Sheets()) { CalculationProperties = calculationProperties };
                workbookPart.AddNewPart<WorkbookStylesPart>().Stylesheet = StyleFactory.Create();

                CreateSheet(_defaultFirstSheetName);
            }
        }

        public Sheet GetOrCreateSheet(string name) => GetSheet(name) ?? CreateSheet(name);

        public Sheet GetSheet(string name) => Sheets.OfType<Sheet>().FirstOrDefault(s => name.Equals(s.Name, StringComparison.CurrentCultureIgnoreCase));

        public Sheet RenameSheet(string name, string newName)
        {
            var sheet = GetSheet(name);
            if ((sheet != null) && !string.IsNullOrWhiteSpace(newName))
                sheet.Name = newName;
            return sheet;
        }

        public Sheet CreateSheet(string name)
        {
            var worksheetPart = WorkbookPart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet(new Columns(), new SheetData());

            var sheet = new Sheet()
            {
                Id = WorkbookPart.GetIdOfPart(worksheetPart),
                SheetId = Convert.ToUInt32(Sheets.Count() + 1),
                Name = name
            };

            Sheets.Append(sheet);
            return sheet;
        }

        public void Export<T>(IEnumerable<T> list, string sheetName = null)
        {
            var type = list.GetTypeOfItems();
            var name = sheetName ?? type.Name;
            var sheet = RenameSheet(_defaultFirstSheetName, name) ?? GetOrCreateSheet(name);
            var sheetData = WorkbookPart.GetSheetDataBySheet(sheet);
            var attributes = ExcelColumnAttribute.GetAttributes(type);
            sheetData.WriteAll(list, attributes);
            Save(true);
        }

        public void Save(bool allowSave = true)
        {
            if (_mode.Is(Mode.Escrita) && allowSave)
                _spreadsheetDocument.Save();
        }

        public void Close()
        {
            Save(_mode.Is(Mode.SalvarAutomaticamente));
            _spreadsheetDocument.Dispose();
        }
    }
}