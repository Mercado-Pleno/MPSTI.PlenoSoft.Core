using System;
using System.ComponentModel.DataAnnotations;

namespace MPSTI.PlenoSoft.Core.Extensions.Abstracts
{
	public abstract class Entity<TId> : IEntity where TId : struct, IComparable, IFormattable, IConvertible, IComparable<TId>, IEquatable<TId>
	{
		[Display(Name = "Id")]
		public TId Id { get; set; }

		object IEntity.Id { get => Id; set => Id = (TId)value; }
	}

	public abstract class Entity : Entity<long> { }
}