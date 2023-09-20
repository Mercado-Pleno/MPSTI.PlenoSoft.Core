using System;
using System.Linq;
using System.Reflection;

namespace MPSTI.PlenoSoft.Core.Office.OpenXml.Planilhas.Integracao
{
    public enum XlType
    {
        General,
        String,
        Int,
        Long,
        Double,
        Decimal,
        DateTime,
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ExcelColumnAttribute : Attribute
    {
        private PropertyInfo PropertyInfo { get; set; }

        public string Title { get; set; }
        public int Order { get; set; }
        public string Format { get; set; }
        public double Width { get; set; }
        public XlType ExcelType { get; set; }

        public string PropertyName => PropertyInfo.DeclaringType.Name + "." + PropertyInfo.Name;
        public string DateFormat
        {
            get => Format ??= "{0:yyyy/MM/dd}";
            set => Format = value;
        }

        public ExcelColumnAttribute(string title, int order, XlType xlType = XlType.General, string format = null)
        {
            Width = 20.00;
            Title = title;
            Order = order;
            ExcelType = xlType;
            Format = format;
        }

        private ExcelColumnAttribute(PropertyInfo propertyInfo, int posicao)
            : this(propertyInfo.Name, posicao) => Set(propertyInfo);

        private ExcelColumnAttribute Set(PropertyInfo propertyInfo)
        {
            PropertyInfo = propertyInfo;
            return this;
        }

        public static ExcelColumnAttribute[] GetAttributes<T>() => GetAttributes(typeof(T));

        public static ExcelColumnAttribute[] GetAttributes(Type type)
        {
            var properties = type.GetProperties();
            var attributes = properties.Select(p => p.GetCustomAttribute<ExcelColumnAttribute>(true)?.Set(p));

            if (!attributes.Any(x => x != null))
                attributes = properties.Select((p, i) => new ExcelColumnAttribute(p, i));
            
            return attributes.Where(x => x != null).OrderBy(x => x.Order).ToArray();
        }

        public object GetValue<TClass>(TClass dto)
        {
            try
            {
                return PropertyInfo.GetValue(dto, null);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}