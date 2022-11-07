using System.ComponentModel.DataAnnotations;

namespace MPSTI.PlenoSoft.Core.CrossProject.Domains
{
	public interface IEntidade
	{
		[Key, Required]
		long Id { get; set; }
	}
}