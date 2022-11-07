using Microsoft.AspNetCore.Mvc;
using MPSTI.PlenoSoft.Core.CrossProject.Connections;
using MPSTI.PlenoSoft.Core.CrossProject.Services;
using System;

namespace MPSTI.PlenoSoft.Core.CrossProject.Web.MVC
{
	public abstract class BaseControllerMVC<TViewModel> : BaseController where TViewModel : class, new()
	{
		public BaseControllerMVC(IConnectionManagerFactory connectionManagerFactory)
			: base(connectionManagerFactory) { }

		protected abstract IApplicationService<TViewModel> Service { get; }

		public virtual ActionResult Index()
		{
			var listaViewModel = Service.ObterTodos();
			return View(listaViewModel);
		}

		public virtual ActionResult Create()
		{
			var viewModel = new TViewModel();
			return View(viewModel);
		}

		public virtual ActionResult Details(long id)
		{
			var viewModel = Service.ObterPorId(id);
			return View(viewModel);
		}

		public virtual ActionResult Edit(int id)
		{
			return Details(id);
		}

		public virtual ActionResult Delete(int id)
		{
			return Details(id);
		}


		[HttpPost, ValidateAntiForgeryToken]
		public virtual ActionResult Create(TViewModel viewModel)
		{
			try
			{
				Service.Incluir(viewModel);
				return RedirectToAction(nameof(Index));
			}
			catch (Exception exception)
			{
				ViewBag.Error = exception.Message;
				return View();
			}
		}

		[HttpPost, ValidateAntiForgeryToken]
		public virtual ActionResult Edit(int id, TViewModel viewModel)
		{
			try
			{
				Service.Alterar(viewModel);
				return RedirectToAction(nameof(Index));
			}
			catch (Exception exception)
			{
				ViewBag.Error = exception.Message;
				return View();
			}
		}

		[HttpPost, ValidateAntiForgeryToken]
		public virtual ActionResult Delete(int id, TViewModel viewModel)
		{
			try
			{
				Service.Excluir(viewModel);
				return RedirectToAction(nameof(Index));
			}
			catch (Exception exception)
			{
				ViewBag.Error = exception.Message;
				return View();
			}
		}
	}
}