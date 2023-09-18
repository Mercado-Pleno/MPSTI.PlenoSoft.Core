using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using MPSTI.PlenoSoft.Core.Office.OpenXml.Planilhas.Integracao;
using MPSTI.PlenoSoft.Core.Office.OpenXml.Planilhas.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MPSTI.PlenoSoft.Core.Office.OpenXml.Planilhas.Controller
{
    [Flags]
    public enum Modo
    {
        Leitura = 1,
        Escrita = 2,
        CriarSeNaoExistir = 4,
        SalvarAutomaticamente = 8,
        ApagarSeExistir = 16,
        FazerBackupAntes = 32,
        Padrao = Leitura | Escrita | CriarSeNaoExistir | SalvarAutomaticamente,
        Seguro = Padrao | FazerBackupAntes,
        SempreCriaNovo = Leitura | Escrita | ApagarSeExistir | CriarSeNaoExistir | SalvarAutomaticamente,
        Editavel = Escrita | CriarSeNaoExistir,
    }

    public class PlenoExcel
    {
        private readonly Modo _modo;
        private readonly SpreadsheetDocument _spreadsheetDocument;

        protected virtual WorkbookPart WorkbookPart => _spreadsheetDocument.WorkbookPart;
        protected virtual Workbook Workbook => WorkbookPart.Workbook;
        protected virtual IEnumerable<WorksheetPart> WorksheetParts => WorkbookPart.WorksheetParts;
        protected virtual WorksheetPart WorksheetPart => WorksheetParts.FirstOrDefault();
        protected virtual Worksheet Worksheet => WorksheetPart.Worksheet;
        protected virtual IEnumerable<SheetData> SheetDatas => Worksheet.OfType<SheetData>();
        protected virtual SheetData SheetData => SheetDatas.FirstOrDefault();
        protected virtual Sheets Sheets => Workbook.Sheets;


        public PlenoExcel(FileInfo arquivoExcel, Modo modo = Modo.Padrao)
        {
            var editavel = modo.Is(Modo.Editavel);
            var autoSave = modo.Is(Modo.SalvarAutomaticamente);
            var backup = modo.Is(Modo.FazerBackupAntes);
            var apagar = modo.Is(Modo.ApagarSeExistir);
            var criar = modo.Is(Modo.CriarSeNaoExistir);
            _modo = modo;

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
                _spreadsheetDocument = SpreadsheetDocument.Create(arquivoExcel.FullName, SpreadsheetDocumentType.Workbook, autoSave);

                var workbookPart = _spreadsheetDocument.AddWorkbookPart();
                workbookPart.Workbook = new Workbook(fileVersion);

                var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet(new SheetData());

                workbookPart.Workbook.AppendChild(new Sheets());

                CreateSheet("Plan1");
            }
        }

        public Sheet GetOrCreateSheet(string name) => GetSheet(name) ?? CreateSheet(name);

        public Sheet GetSheet(string name) => Sheets.OfType<Sheet>().FirstOrDefault(s => name.Equals(s.Name, StringComparison.CurrentCultureIgnoreCase));

        public Sheet CreateSheet(string name)
        {
            var sheet = new Sheet()
            {
                Id = WorkbookPart.GetIdOfPart(WorksheetPart),
                SheetId = Convert.ToUInt32(Sheets.Count() + 1),
                Name = name
            };

            Sheets.Append(sheet);
            return sheet;
        }

        public void Save() => Save(true);

        public void Close()
        {
            Save(_modo.Is(Modo.SalvarAutomaticamente));
            _spreadsheetDocument.Dispose();
        }

        private void Save(Boolean deveSalvar)
        {
            if (_modo.Is(Modo.Escrita) && deveSalvar)
                _spreadsheetDocument.Save();
        }

        public void Export(IEnumerable enumerable, string name = null)
        {
            ExportCore(enumerable, name);
            Save();
        }

        private void ExportCore(IEnumerable enumerable, string name)
        {
            var type = enumerable.GetTypeOfItems(out var lista);
            var sheet = GetSheet("Plan1") ?? GetOrCreateSheet(name ?? type.Name);
            sheet.Name = name ?? type.Name;
            var sheetData = WorkbookPart.GetSheetDataBySheet(sheet);

            if (lista.OfType<Object>().Any())
                sheetData.AdicionarDados(lista, ExcelColumnAttribute.GetAttributes(type));
        }
    }

    public static class SheetDataExtensions
    {
        public static Type GetTypeOfItems<TCollection>(this TCollection collection, out IEnumerable iEnumerable)
        {
            iEnumerable = (collection as IEnumerable) ?? new[] { collection };
            return iEnumerable.GetType().GetGenericArguments()?.FirstOrDefault()
                ?? iEnumerable.OfType<Object>().FirstOrDefault()?.GetType()
                ?? typeof(TCollection);
        }

        public static void AdicionarDados(this SheetData sheetdata, IEnumerable lista, IEnumerable<ExcelColumnAttribute> attributes)
        {
            AddHeader(sheetdata, attributes);
            AddLines(sheetdata, lista, attributes);
        }

        public static void AddHeader(this SheetData sheetData, IEnumerable<ExcelColumnAttribute> attributes)
        {
            AddLine(sheetData, attributes, style: null, attribute => attribute.Title);
        }

        public static void AddLines(this SheetData sheetdata, IEnumerable lista, IEnumerable<ExcelColumnAttribute> attributes)
        {
            foreach (var item in lista)
                AddLine(sheetdata, attributes, style: null, attribute => attribute.GetValue(item));
        }

        public static void AddLine(this SheetData sheetdata, IEnumerable<ExcelColumnAttribute> attributes, uint? style, Func<ExcelColumnAttribute, object> getValue)
        {
            if (attributes.Any())
            {
                var linha = sheetdata.ChildElements.Count + 1;
                var newRow = sheetdata.GetRow(linha);

                foreach (var attribute in attributes)
                {
                    var value = getValue.Invoke(attribute);
                    var cell = Celula.Criar(attribute.Order, linha, value, style);
                    newRow.AppendChild(cell);
                }
            }
        }
    }
}