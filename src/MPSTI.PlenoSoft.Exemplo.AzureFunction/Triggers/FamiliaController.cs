using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using MPSTI.PlenoSoft.Exemplo.AzureFunction.Repository;
using MPSTI.PlenoSoft.Exemplo.AzureFunction.Contracts;
using Microsoft.Azure.Cosmos;

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
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req,ILogger log)
        {
            try
            {
                var familia = await _familiaRepository.GetByIdOnly("fa872c5a-665d-4f71-bff4-56f26fa31fb0");

			    await _familiaRepository.UpdateItem(new Familia { Id = Guid.Parse("fa872c5a-665d-4f71-bff4-56f26fa31fb0"), FirstName = "Luani", LastName = "Fernandes", Cpf = "123" });
			    await _familiaRepository.UpdateItem(new Familia { Id = Guid.Parse("6fdf1933-7628-40de-aa04-c7a546606c4c"), FirstName = "Bruno", LastName = "Fernandes", Cpf = "456" });
			    await _familiaRepository.UpdateItem(new Familia { Id = Guid.Parse("849325fc-5656-4aad-983b-d5802ecd4d14"), FirstName = "Filho", LastName = "Fernandes", Cpf = "789" });

			    await _familiaRepository.UpdateItem(new Familia { Id = Guid.Parse("1f00b4bf-0000-0400-0000-63799e3e0000"), FirstName = "Rita", LastName = "Nogueira", Cpf = "000" });

                familia = await _familiaRepository.GetByItem(familia);
				await _familiaRepository.UpdateItem(familia);

                var familiares = await _familiaRepository.GetAllByPK("Fernandes");
                var nogueira = await _familiaRepository.GetByPKOnly("Nogueira");

                familiares = await _familiaRepository.GetAll();


				return new OkObjectResult(familiares);
            }
            catch (CosmosException exception)
            {
                return new BadRequestObjectResult(new { exception.ResponseBody, exception.Message });
			}
		}
    }
}