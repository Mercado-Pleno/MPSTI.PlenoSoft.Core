using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using MPSTI.PlenoSoft.Exemplo.AzureFunction.Contracts;
using MPSTI.PlenoSoft.Exemplo.AzureFunction.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MPSTI.PlenoSoft.Exemplo.AzureFunction.Triggers
{
	public class FamiliaController
	{
		private readonly IFamiliaRepository _familiaRepository;

		public FamiliaController(IFamiliaRepository familiaRepository)
		{
			_familiaRepository = familiaRepository;
		}

		[FunctionName("FamiliaController")]
		public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req, ILogger log)
		{
			try
			{
				var luani = new Familia { Id = Guid.Parse("fa872c5a-665d-4f71-bff4-56f26fa31fb0"), FirstName = "Luani", LastName = "Fernandes", Doc = "123" };
				var bruno = new Familia { Id = Guid.Parse("6fdf1933-7628-40de-aa04-c7a546606c4c"), FirstName = "Bruno", LastName = "Fernandes", Doc = "456" };
				var filho = new Familia { Id = Guid.Parse("849325fc-5656-4aad-983b-d5802ecd4d14"), FirstName = "Filho", LastName = "Fernandes", Doc = "789" };
				var maria = new Familia { Id = Guid.Parse("1f00b4bf-0000-0400-0000-63799e3e0000"), FirstName = "Maria", LastName = "Nogueira", Doc = "852" };
				var lista = new List<Familia> { luani, bruno, filho, maria };

				var luAntes = await _familiaRepository.GetByIdOnly("fa872c5a-665d-4f71-bff4-56f26fa31fb0");

				await _familiaRepository.UpdateItem(luani);
				await _familiaRepository.UpdateItem(bruno);
				await _familiaRepository.UpdateItem(filho);
				await _familiaRepository.UpdateItem(maria);

				var luDepois = await _familiaRepository.GetByItem(luAntes);
				await _familiaRepository.UpdateItem(luDepois);

				var familiaFernandes = await _familiaRepository.GetAllByPK("Fernandes");
				var familiaNogueira = await _familiaRepository.GetByPKOnly("Nogueira");

				lista.ForEach(f => f.Doc = string.Join("", f.Doc.Reverse()));
				var result = await _familiaRepository.ExecuteBatch("Fernandes", batch => batch
					.UpsertItem<Familia>(luani)
					.UpsertItem<Familia>(bruno)
					.UpsertItem<Familia>(filho)
				);

				var familiaAll = await _familiaRepository.GetAll();
				return new OkObjectResult(familiaAll);
			}
			catch (CosmosException exception)
			{
				return new BadRequestObjectResult(new { exception.ResponseBody, exception.Message });
			}
		}
	}
}