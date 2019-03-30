using System;
using System.ComponentModel.DataAnnotations;

namespace MPSC.PlenoSoft.Core.Utils.Abstracao
{
	public interface IUniqueId
	{
		Int64 UId { get; set; }
	}

	public interface IEntidade<TId> where TId : IComparable, IComparable<TId>, IConvertible, IEquatable<TId>, IFormattable
	{
		TId Id { get; set; }
	}


	public abstract class ClasseBase<TId> where TId : struct, IComparable, IFormattable, IConvertible, IComparable<TId>, IEquatable<TId>
	{
		protected static Int64 _controle = 0;

		[Display(Name = "Id")]
		public TId Id { get; set; }

		public virtual void EhValido() { }
	}

	public abstract class Tipo : ClasseBase<Int16>
	{
		public String Sigla { get; set; }
		public String Descricao { get; set; }
		public Int16 OrdemApresentacao { get; set; }
	}

	public abstract class Cadastro : ClasseBase<Int32>
	{
		public DateTime Inclusao { get; set; }
		public DateTime? Alteracao { get; set; }
		public String LoginInclusao { get; set; }
		public String LoginAlteracao { get; set; }
	}

	public abstract class Entidade : ClasseBase<Int64>, IUniqueId, IEntidade<Int64>
	{
		Int64 IUniqueId.UId { get => Id; set => Id = value; }
		public DateTime Inclusao { get; set; }
		public DateTime? Alteracao { get; set; }
		public String LoginInclusao { get; set; }
		public String LoginAlteracao { get; set; }
	}
}