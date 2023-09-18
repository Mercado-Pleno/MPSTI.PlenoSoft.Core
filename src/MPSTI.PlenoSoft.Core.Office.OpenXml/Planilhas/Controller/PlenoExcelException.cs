using System;
using System.Runtime.Serialization;

namespace MPSTI.PlenoSoft.Core.Office.OpenXml.Planilhas.Controller
{
    [Serializable]
    public class PlenoExcelException : Exception
    {
        public PlenoExcelException() { }

        public PlenoExcelException(string message) : base(message) { }

        public PlenoExcelException(string message, Exception innerException) : base(message, innerException) { }

        protected PlenoExcelException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}