using System;

namespace MPSTI.PlenoSoft.Core.Office.OpenXml.Planilhas.Celulas
{
    public class Celula
    {
        private readonly int _linha;
        private readonly int _coluna;

        public string NomeColuna => ObterNomePor(_coluna);
        public string Referencia => NomeColuna + _linha.ToString();

        public Celula(int coluna, int linha)
        {
            _linha = linha;
            _coluna = coluna;
        }

        public static string ObterNomePor(int coluna)
        {
            var col = Convert.ToByte(coluna);
            if (col < 26)
                return string.Format("{0}", (char)(64 + col));

            return string.Format("{0}{1}", (char)(64 + col / 26), (char)(64 + col % 26));
        }
    }
}