using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;

namespace MPSTI.PlenoSoft.Core.Office.OpenXml.Planilhas.Celulas
{
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
        private static readonly IDictionary<Style, uint?> _styles = new Dictionary<Style, uint?>();

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

        private void CreateStyles(Stylesheet stylesheet, IDictionary<Style, uint?> styles)
        {
            var fontId = New(stylesheet, CreateFont("Calibri", 11, true, null, null, null));
            styles[Style.Geral] = New(stylesheet, CreateCellFormat((uint)Style.Geral, 0, 0));
            styles[Style.Long] = New(stylesheet, CreateCellFormat((uint)Style.Long, 0, 0));
            styles[Style.Double] = New(stylesheet, CreateCellFormat((uint)Style.Double, 0, 0));
            styles[Style.Date] = New(stylesheet, CreateCellFormat((uint)Style.Date, 0, 0));
            styles[Style.Header] = New(stylesheet, CreateCellFormat((uint)Style.Header, fontId, 0));
        }

        private Border CreateBorder(bool? top, bool? bottom, bool? left, bool? right, bool? diagonal)
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

        private CellFormat CreateCellFormat(uint? numberFormatId, UInt32Value fontId, uint? fillId)
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

        private CellStyleFormat CreateCellStyleFormat(uint? numberFormatId, UInt32Value fontId, uint? fillId)
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

        private CellStyle CreateCellStyle(string name, uint? formatId, uint? builtinId)
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

        private Font CreateFont(string fontName, double? fontSize, bool? isBold, uint? color, int? fontFamilyNumbering, FontSchemeValues? fontSchemeValues)
        {
            var font = new Font();

            if (!string.IsNullOrWhiteSpace(fontName))
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
        public UInt32Value New(Stylesheet stylesheet, Border border)
        {
            stylesheet.Borders.Append(border);
            return stylesheet.Borders.Count++;
        }
        public UInt32Value New(Stylesheet stylesheet, CellFormat cellFormat)
        {
            stylesheet.CellFormats.Append(cellFormat);
            return stylesheet.CellFormats.Count++;
        }
        public UInt32Value New(Stylesheet stylesheet, CellStyleFormat cellStyleFormat)
        {
            stylesheet.CellStyleFormats.Append(cellStyleFormat);
            return stylesheet.CellStyleFormats.Count++;
        }
        public UInt32Value New(Stylesheet stylesheet, CellStyle cellStyle)
        {
            stylesheet.CellStyles.Append(cellStyle);
            return stylesheet.CellStyles.Count++;
        }
        public UInt32Value New(Stylesheet stylesheet, Color color)
        {
            stylesheet.Colors.Append(color);
            return new UInt32Value((uint)stylesheet.Colors.ChildElements.Count);
        }
        public UInt32Value New(Stylesheet stylesheet, DifferentialFormat differentialFormat)
        {
            stylesheet.DifferentialFormats.Append(differentialFormat);
            return stylesheet.DifferentialFormats.Count++;
        }
        public UInt32Value New(Stylesheet stylesheet, Fill fill)
        {
            stylesheet.Fills.Append(fill);
            return stylesheet.Fills.Count++;
        }
        public UInt32Value New(Stylesheet stylesheet, Font font)
        {
            stylesheet.Fonts.Append(font);
            return stylesheet.Fonts.Count++;
        }
        public UInt32Value New(Stylesheet stylesheet, NumberingFormat numberingFormat)
        {
            stylesheet.NumberingFormats.Append(numberingFormat);
            return stylesheet.NumberingFormats.Count++;
        }
        public UInt32Value New(Stylesheet stylesheet, StylesheetExtension stylesheetExtension)
        {
            stylesheet.StylesheetExtensionList.Append(stylesheetExtension);
            return new UInt32Value((uint)stylesheet.StylesheetExtensionList.ChildElements.Count);
        }
        public UInt32Value New(Stylesheet stylesheet, TableStyle tableStyle)
        {
            stylesheet.TableStyles.Append(tableStyle);
            return stylesheet.TableStyles.Count++;
        }
        #endregion // "News"

        public sealed class CellStyleFormat : CellFormat { internal CellStyleFormat() { } }
    }
}