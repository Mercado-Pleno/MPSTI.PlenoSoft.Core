using System;
using System.Collections.Generic;
using System.Linq;

namespace MPSTI.PlenoSoft.Core.CrossProject.Domains
{
	public class DomainValidation
	{
		private readonly string[] _validacoes;
		public bool EhValido => !_validacoes.Any();

		public IEnumerable<string> Validacoes => _validacoes;

		private string InvalidMessage => "Operação não realizada:\r\n - " + string.Join(";\r\n - ", _validacoes) + ";\r\n\r\nVerifique as validações acima e tente novamente!";
		private string ValidMessage => "Operação válida";
		public string Mensagem => EhValido ? ValidMessage : InvalidMessage;

		public DomainValidation(IEnumerable<string> validacoes)
		{
			_validacoes = validacoes.ToArray();
		}

		public DomainValidation Validar(bool throwsException = true)
		{
			if (throwsException && !EhValido)
				throw new ApplicationException(InvalidMessage);

			return this;
		}
	}
}