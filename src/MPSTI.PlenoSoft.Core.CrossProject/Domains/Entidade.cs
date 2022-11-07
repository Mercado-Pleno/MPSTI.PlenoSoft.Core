using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MPSTI.PlenoSoft.Core.CrossProject.Domains
{
	public abstract class Entidade : IEntidade
	{
		[Key, Required]
		public long Id { get; set; }
		public Guid Guid { get; set; }

		public Entidade()
		{
			Guid = Guid.NewGuid();
		}

		protected abstract IEnumerable<string> ObterValidacoes();

		public DomainValidation Validar(bool throwsException)
		{
			var validacoes = ObterValidacoes();
			var domainValidation = new DomainValidation(validacoes);
			return domainValidation.Validar(throwsException);
		}
	}
}