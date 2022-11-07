using System;
using System.ComponentModel.DataAnnotations;

namespace MPSTI.PlenoSoft.Core.Extensions.Abstracao
{
	public interface IUniqueId
	{
		long UId { get; set; }
	}

	public interface IEntidade<TId> where TId : IComparable, IComparable<TId>, IConvertible, IEquatable<TId>, IFormattable
	{
		TId Id { get; set; }
	}


	public abstract class ClasseBase<TId> where TId : struct, IComparable, IFormattable, IConvertible, IComparable<TId>, IEquatable<TId>
	{
		protected static long _controle = 0;

		[Display(Name = "Id")]
		public TId Id { get; set; }

		public virtual void EhValido() { }
	}

	public abstract class Tipo : ClasseBase<short>
	{
		public string Sigla { get; set; }
		public string Descricao { get; set; }
		public short OrdemApresentacao { get; set; }
	}

	public abstract class Cadastro : ClasseBase<int>
	{
		public DateTime Inclusao { get; set; }
		public DateTime? Alteracao { get; set; }
		public string LoginInclusao { get; set; }
		public string LoginAlteracao { get; set; }
	}

	public abstract class Entidade : ClasseBase<long>, IUniqueId, IEntidade<long>
	{
		long IUniqueId.UId { get => Id; set => Id = value; }
		public DateTime Inclusao { get; set; }
		public DateTime? Alteracao { get; set; }
		public string LoginInclusao { get; set; }
		public string LoginAlteracao { get; set; }
	}
}