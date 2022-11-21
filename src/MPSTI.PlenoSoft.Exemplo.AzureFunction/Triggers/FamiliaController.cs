using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using MPSTI.PlenoSoft.Exemplo.AzureFunction.Contracts;
using MPSTI.PlenoSoft.Exemplo.AzureFunction.Repository;
using System;
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

		[FunctionName("FamiliaControllerCreate")]
		public async Task<IActionResult> Create([HttpTrigger(AuthorizationLevel.Function, "get", Route = "Create")] HttpRequest req, ILogger log)
		{
			try
			{
				var luani = new Familia { Id = Guid.Parse("fa872c5a-665d-4f71-bff4-56f26fa31fb0"), FirstName = "Luani", LastName = "Fernandes", Doc = "123" };
				var bruno = new Familia { Id = Guid.Parse("6fdf1933-7628-40de-aa04-c7a546606c4c"), FirstName = "Bruno", LastName = "Fernandes", Doc = "456" };
				var filho = new Familia { Id = Guid.Parse("849325fc-5656-4aad-983b-d5802ecd4d14"), FirstName = "Filho", LastName = "Fernandes", Doc = "789" };
				var maria = new Familia { Id = Guid.Parse("1f00b4bf-0000-0400-0000-63799e3e0000"), FirstName = "Maria", LastName = "Nogueira", Doc = "852" };

				await _familiaRepository.UpdateItem(luani);
				await _familiaRepository.UpdateItem(bruno);
				await _familiaRepository.UpdateItem(filho);
				await _familiaRepository.UpdateItem(maria);

				var familiaFernandes = await _familiaRepository.GetAllByPK("Fernandes");
				var familiaNogueira = (await _familiaRepository.GetAllByPK("Nogueira")).SingleOrDefault();

				var familiaAll = await _familiaRepository.GetAll();

				return new OkObjectResult(familiaAll);
			}
			catch (CosmosException exception)
			{
				return new BadRequestObjectResult(new { exception.ResponseBody, exception.Message });
			}
		}

		[FunctionName("FamiliaController")]
		public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req, ILogger log)
		{
			try
			{
				var luani = await _familiaRepository.GetByIdOnly("fa872c5a-665d-4f71-bff4-56f26fa31fb0");
				var lista = await _familiaRepository.GetAll();
				lista.ToList().ForEach(item => item.Doc = string.Join("", item.Doc.Reverse()));
				await _familiaRepository.UpdateItem(luani);

				var result = await _familiaRepository.ExecuteBatch(lista, (partitionKeyValue, items, batch) =>
				{
					foreach (var item in items)
						batch.UpsertItem<Familia>(item);
				});

				var result2 = await _familiaRepository.ExecuteBatch("Fernandes", (partitionKeyValue, batch) =>
				{
					batch.UpsertItem<Familia>(luani);
					foreach (var item in lista.Where(x => x.LastName == partitionKeyValue))
						batch.UpsertItem<Familia>(item);
				});

				return new OkObjectResult(lista);
			}
			catch (CosmosException exception)
			{
				return new BadRequestObjectResult(new { exception.ResponseBody, exception.Message });
			}
		}
	}
}