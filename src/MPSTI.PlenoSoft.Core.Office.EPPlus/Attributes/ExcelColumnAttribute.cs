using System;
using System.Reflection;

namespace MPSTI.PlenoSoft.Core.Office.EPPlus.Attributes
{
    public enum DataType
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
        public PropertyInfo PropertyInfo { get; private set; }
        public double Largura { get; set; }
        public string PropertyName { get; set; }
        public string Titulo { get; set; }
        public int Posicao { get; set; }
        public string Formato { get; set; }
        public DataType Tipo { get; set; }

        public string NumberFormat { get; set; }

        public string DataFormat
        {
            get { return Formato ??= "{0:yyyy/MM/dd}"; }
            set { Formato = value; }
        }

        public ExcelColumnAttribute(string titulo, int posicao) : this(null, titulo, posicao, null, null) { }

        public ExcelColumnAttribute(string propertyName, string titulo, int posicao, DataType? tipo = null, string formato = null)
        {
            PropertyName = propertyName;
            Largura = 20.00;
            Titulo = titulo;
            Posicao = posicao;
            Tipo = tipo ?? Attributes.DataType.String;
            if (formato != null) Formato = formato;
        }

        public ExcelColumnAttribute Mapear(PropertyInfo propertyInfo)
        {
            PropertyName = propertyInfo.DeclaringType.Name + "." + propertyInfo.Name;
            PropertyInfo = propertyInfo;
            return this;
        }

        public Type DataType => Tipo switch
        {
            Attributes.DataType.String => typeof(string),
            Attributes.DataType.Int => typeof(int),
            Attributes.DataType.Long => typeof(long),
            Attributes.DataType.Double => typeof(double),
            Attributes.DataType.Decimal => typeof(decimal),
            Attributes.DataType.DateTime => typeof(DateTime),
            Attributes.DataType.General => typeof(object),
            _ => typeof(object),
        };
    }
}