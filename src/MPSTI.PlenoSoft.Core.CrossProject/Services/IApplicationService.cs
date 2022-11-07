using System.Collections.Generic;

namespace MPSTI.PlenoSoft.Core.CrossProject.Services
{
	public interface IApplicationService<TViewModel>
	{
		IEnumerable<TViewModel> ObterTodos();
		TViewModel ObterPorId(long id);
		void Incluir(TViewModel viewModel);
		void Alterar(TViewModel viewModel);
		void Excluir(TViewModel viewModel);
	}
}