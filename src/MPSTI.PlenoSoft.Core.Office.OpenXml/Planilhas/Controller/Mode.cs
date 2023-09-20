using System;

namespace MPSTI.PlenoSoft.Core.Office.OpenXml.Planilhas.Controller
{
    [Flags]
    public enum Mode
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
}