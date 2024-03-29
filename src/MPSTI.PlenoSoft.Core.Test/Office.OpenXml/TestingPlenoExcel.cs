using MPSTI.PlenoSoft.Core.Office.OpenXml.Planilhas.Controller;
using MPSTI.PlenoSoft.Core.Office.OpenXml.Planilhas.Integracao;
using System.Linq;

namespace MPSTI.PlenoSoft.Core.Test.Office.OpenXml
{
    public class TestingPlenoExcel
    {
        [FactDebuggerOnly]
        public void WhenCenario1_StatusTrue()
        {
            var directory = new DirectoryInfo("D:/Excel");
            directory.GetFiles("*.xlsx").Select(x => { x.Delete(); return true; }).ToArray();

            var fileInfo = new FileInfo(Path.Combine(directory.FullName, $"A-{DateTime.Now:HH.mm.ss}.xlsx"));
            var plenoExcel = new PlenoExcel(fileInfo, Mode.Padrao | Mode.ApagarSeExistir | Mode.FazerBackupAntes);

            var pessoas = PessoaFactory.Create(10000);
            plenoExcel.Export(pessoas);

            plenoExcel.Close();

            pessoas.Should().NotBeNull();
        }



        public static class PessoaFactory
        {
            private static long _id = 0;
            private static long NewId() => ++_id;


            public static IEnumerable<Pessoa> Create(int total)
                => Enumerable.Range(0, total).Select(CreateOne).ToArray();

            public static Pessoa CreateOne(int id)
            {
                var nascimento = new DateTime(1982, 11, 30, 0, 0, 0, DateTimeKind.Local);
                return new Pessoa
                {
                    Id = NewId(),
                    Ativo = id % 10 != 5,
                    Classe = Convert.ToChar(65 + id % 26),
                    Genero = (Genero)(id % 3),
                    LimiteCredito = Convert.ToDecimal(id % 99) / 100m + id,
                    Nascimento = nascimento.AddDays(id),
                    Nome = $"Pessoa {id:000}",
                };
            }

        }


        public enum Genero { Indefinido = 0, Feminino = 1, Masculino = 2, }

        public class Pessoa
        {
            [ExcelColumn("Id", 1, 7)]
            public long Id { get; set; }

            [ExcelColumn("Nome", 2, 30)]
            public string Nome { get; set; }

            [ExcelColumn("G�nero", 3, 12)]
            public Genero Genero { get; set; }

            [ExcelColumn("Nascimento", 4, 12)]
            public DateTime Nascimento { get; set; }

            [ExcelColumn("LimiteCredito", 5, 14)]
            public decimal LimiteCredito { get; set; }

            [ExcelColumn("Ativo", 6, 13)]
            public bool Ativo { get; set; }

            [ExcelColumn("Classe", 7, 6)]
            public char Classe { get; set; }
        }
    }
}