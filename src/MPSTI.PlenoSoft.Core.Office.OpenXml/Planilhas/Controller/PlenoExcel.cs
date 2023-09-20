using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using MPSTI.PlenoSoft.Core.Office.OpenXml.Planilhas.Integracao;
using MPSTI.PlenoSoft.Core.Office.OpenXml.Planilhas.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MPSTI.PlenoSoft.Core.Office.OpenXml.Planilhas.Controller
{
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
                _spreadsheetDocument = SpreadsheetDocument.Create(arquivoExcel.FullName, SpreadsheetDocumentType.Workbook, autoSave);

                var workbookPart = _spreadsheetDocument.AddWorkbookPart();
                workbookPart.Workbook = new Workbook(fileVersion);
                workbookPart.Workbook.AppendChild(new Sheets());
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




    public enum Style : uint
    {
        Geral = 0,
        Long = 1,
        Double = 4,
        Date = 14,
        Header = 999
    }

    public class StyleFactory
    {
        private static readonly IDictionary<Style, UInt32Value> _styles = new Dictionary<Style, UInt32Value>();

        private StyleFactory() { }

        internal static Stylesheet Create(Stylesheet stylesheet = null)
        {
            var styleFactory = new StyleFactory();
            if (stylesheet == null)
            {
                stylesheet = new Stylesheet();
                styleFactory.CreateChildren(stylesheet);
                styleFactory.CreateDefaults(stylesheet);
            }
            styleFactory.CreateStyles(stylesheet, _styles);
            return stylesheet;
        }

        internal static uint? Get(Style? style) => style.HasValue && _styles.TryGetValue(style.Value, out var value) ? value : null;

        private void CreateChildren(Stylesheet stylesheet)
        {
            stylesheet.Borders = new Borders { Count = 0 };
            stylesheet.CellFormats = new CellFormats { Count = 0 };
            stylesheet.CellStyleFormats = new CellStyleFormats { Count = 0 };
            stylesheet.CellStyles = new CellStyles { Count = 0 };
            stylesheet.Colors = new Colors();
            stylesheet.DifferentialFormats = new DifferentialFormats { Count = 0 };
            stylesheet.Fills = new Fills { Count = 0 };
            stylesheet.Fonts = new Fonts { Count = 0 };
            stylesheet.NumberingFormats = new NumberingFormats { Count = 0 };
            stylesheet.StylesheetExtensionList = new StylesheetExtensionList();
            stylesheet.TableStyles = new TableStyles { Count = 0 };
        }

        private void CreateDefaults(Stylesheet stylesheet)
        {
            New(stylesheet, CreateBorder(true, true, true, true, true));
            New(stylesheet, CreateCellStyleFormat(0, 0, 0));
            New(stylesheet, CreateCellStyle("Normal", 0, 0));
            New(stylesheet, CreateFill(PatternValues.None));
            New(stylesheet, CreateFont("Calibri", 11D, null, null, null, null));
        }

        private void CreateStyles(Stylesheet stylesheet, IDictionary<Style, UInt32Value> styles)
        {
            var fontId = New(stylesheet, CreateFont("Calibri", 11, true, null, null, null));
            styles[Style.Geral] = New(stylesheet, CreateCellFormat((uint)Style.Geral, 0, 0));
            styles[Style.Long] = New(stylesheet, CreateCellFormat((uint)Style.Long, 0, 0));
            styles[Style.Double] = New(stylesheet, CreateCellFormat((uint)Style.Double, 0, 0));
            styles[Style.Date] = New(stylesheet, CreateCellFormat((uint)Style.Date, 0, 0));
            styles[Style.Header] = New(stylesheet, CreateCellFormat((uint)Style.Header, fontId, 0));
        }

        private Border CreateBorder(Boolean? top, Boolean? bottom, Boolean? left, Boolean? right, Boolean? diagonal)
        {
            var border = new Border();

            if (left.HasValue && left.Value)
                border.Append(new LeftBorder());

            if (right.HasValue && right.Value)
                border.Append(new RightBorder());

            if (top.HasValue && top.Value)
                border.Append(new TopBorder());

            if (bottom.HasValue && bottom.Value)
                border.Append(new BottomBorder());

            if (diagonal.HasValue && diagonal.Value)
                border.Append(new DiagonalBorder());

            return border;
        }

        private CellFormat CreateCellFormat(UInt32? numberFormatId, UInt32Value fontId, UInt32? fillId)
        {
            var cellFormat = new CellFormat();

            if (fontId != null)
                cellFormat.FontId = fontId;

            if (fillId.HasValue)
                cellFormat.FillId = fillId.Value;

            if (numberFormatId.HasValue)
            {
                cellFormat.NumberFormatId = numberFormatId.Value;
                cellFormat.ApplyNumberFormat = BooleanValue.FromBoolean(true);
            }

            return cellFormat;
        }

        private CellStyleFormat CreateCellStyleFormat(UInt32? numberFormatId, UInt32Value fontId, UInt32? fillId)
        {
            var cellStyleFormat = new CellStyleFormat();

            if (fontId != null)
                cellStyleFormat.FontId = fontId;

            if (fillId.HasValue)
                cellStyleFormat.FillId = fillId.Value;

            if (numberFormatId.HasValue)
            {
                cellStyleFormat.NumberFormatId = numberFormatId.Value;
                cellStyleFormat.ApplyNumberFormat = BooleanValue.FromBoolean(true);
            }

            return cellStyleFormat;
        }

        private CellStyle CreateCellStyle(String name, UInt32? formatId, UInt32? builtinId)
        {
            return new CellStyle
            {
                Name = name,
                FormatId = formatId,
                BuiltinId = builtinId
            };
        }

        private Fill CreateFill(PatternValues? patternValues)
        {
            var fill = new Fill();

            if (patternValues.HasValue)
                fill.PatternFill = new PatternFill { PatternType = patternValues.Value };

            return fill;
        }

        private Font CreateFont(String fontName, Double? fontSize, Boolean? isBold, UInt32? color, Int32? fontFamilyNumbering, FontSchemeValues? fontSchemeValues)
        {
            var font = new Font();

            if (!String.IsNullOrWhiteSpace(fontName))
                font.Append(new FontName() { Val = fontName });

            if (fontSize.HasValue)
                font.Append(new FontSize() { Val = fontSize.Value });

            if (isBold.HasValue && isBold.Value)
                font.Append(new Bold());

            if (color.HasValue)
                font.Append(new Color { Theme = color.Value });

            if (fontFamilyNumbering.HasValue)
                font.Append(new FontFamilyNumbering { Val = fontFamilyNumbering.Value });

            if (fontSchemeValues.HasValue)
                font.Append(new FontScheme { Val = fontSchemeValues.Value });

            return font;
        }

        #region // "News"
        private UInt32Value New(Stylesheet stylesheet, Border border)
        {
            stylesheet.Borders.Append(border);
            return stylesheet.Borders.Count++;
        }
        private UInt32Value New(Stylesheet stylesheet, CellFormat cellFormat)
        {
            stylesheet.CellFormats.Append(cellFormat);
            return stylesheet.CellFormats.Count++;
        }
        private UInt32Value New(Stylesheet stylesheet, CellStyleFormat cellStyleFormat)
        {
            stylesheet.CellStyleFormats.Append(cellStyleFormat);
            return stylesheet.CellStyleFormats.Count++;
        }
        private UInt32Value New(Stylesheet stylesheet, CellStyle cellStyle)
        {
            stylesheet.CellStyles.Append(cellStyle);
            return stylesheet.CellStyles.Count++;
        }
        private UInt32Value New(Stylesheet stylesheet, Color color)
        {
            stylesheet.Colors.Append(color);
            return new UInt32Value((uint)stylesheet.Colors.ChildElements.Count);
        }
        private UInt32Value New(Stylesheet stylesheet, DifferentialFormat differentialFormat)
        {
            stylesheet.DifferentialFormats.Append(differentialFormat);
            return stylesheet.DifferentialFormats.Count++;
        }
        private UInt32Value New(Stylesheet stylesheet, Fill fill)
        {
            stylesheet.Fills.Append(fill);
            return stylesheet.Fills.Count++;
        }
        private UInt32Value New(Stylesheet stylesheet, Font font)
        {
            stylesheet.Fonts.Append(font);
            return stylesheet.Fonts.Count++;
        }
        private UInt32Value New(Stylesheet stylesheet, NumberingFormat numberingFormat)
        {
            stylesheet.NumberingFormats.Append(numberingFormat);
            return stylesheet.NumberingFormats.Count++;
        }
        private UInt32Value New(Stylesheet stylesheet, StylesheetExtension stylesheetExtension)
        {
            stylesheet.StylesheetExtensionList.Append(stylesheetExtension);
            return new UInt32Value((uint)stylesheet.StylesheetExtensionList.ChildElements.Count);
        }
        private UInt32Value New(Stylesheet stylesheet, TableStyle tableStyle)
        {
            stylesheet.TableStyles.Append(tableStyle);
            return stylesheet.TableStyles.Count++;
        }
        #endregion // "News"

        private sealed class CellStyleFormat : CellFormat { }
    }
}