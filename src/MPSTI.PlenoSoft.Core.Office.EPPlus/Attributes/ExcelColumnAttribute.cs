using System;
using System.Reflection;

namespace MPSTI.PlenoSoft.Core.Office.EPPlus.Attributes
{
    public enum TipoEnum
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
        public TipoEnum Tipo { get; set; }

        public string NumberFormat { get; set; }

        public string DataFormat
        {
            get { return Formato ?? (Formato = "{0:yyyy/MM/dd}"); }
            set { Formato = value; }
        }

        public ExcelColumnAttribute(string titulo, int posicao) : this(null, titulo, posicao, null, null) { }

        public ExcelColumnAttribute(string propertyName, string titulo, int posicao, TipoEnum? tipo = null, string formato = null)
        {
            PropertyName = propertyName;
            Largura = 20.00;
            Titulo = titulo;
            Posicao = posicao;
            Tipo = tipo ?? TipoEnum.String;
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
            TipoEnum.String => typeof(string),
            TipoEnum.Int => typeof(int),
            TipoEnum.Long => typeof(long),
            TipoEnum.Double => typeof(double),
            TipoEnum.Decimal => typeof(decimal),
            TipoEnum.DateTime => typeof(DateTime),
            TipoEnum.General => typeof(object),
            _ => typeof(object),
        };
    }
}