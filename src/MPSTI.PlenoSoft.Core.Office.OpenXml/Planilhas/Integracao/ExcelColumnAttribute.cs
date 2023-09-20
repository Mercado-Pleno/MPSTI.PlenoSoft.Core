using System;
using System.Linq;
using System.Reflection;

namespace MPSTI.PlenoSoft.Core.Office.OpenXml.Planilhas.Integracao
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ExcelColumnAttribute : Attribute
    {
        private PropertyInfo PropertyInfo { get; set; }

        public string Title { get; set; }
        public int Order { get; set; }
        public double? Width { get; set; }
        public string PropertyName => PropertyInfo?.DeclaringType.Name + "." + PropertyInfo?.Name;

        public ExcelColumnAttribute(string title, int order, double width = 0)
        {
            Title = title;
            Order = order;
            if (width > 0)
                Width = width;
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

        public object GetValue<TClass>(TClass dto) => PropertyInfo.GetValue(dto, null);
    }
}