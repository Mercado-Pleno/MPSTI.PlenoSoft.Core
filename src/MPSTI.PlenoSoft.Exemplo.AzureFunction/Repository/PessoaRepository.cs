﻿using MongoDB.Driver;
using MPSTI.PlenoSoft.Core.MongoDb.Abstractions;
using MPSTI.PlenoSoft.Core.MongoDb.Interfaces;
using MPSTI.PlenoSoft.Exemplo.AzureFunction.Contracts;

namespace MPSTI.PlenoSoft.Exemplo.AzureFunction.Repository
{
	public interface IPessoaRepository : IMongoRepository<Pessoa> { }

	public class PessoaRepository : MongoRepository<Pessoa>, IPessoaRepository
	{
		public override string DatabaseName => "MercadoPleno";
		public override string CollectionName => "Pessoa";
		public PessoaRepository(IMongoClient mongoClient) : base(mongoClient) { }
	}
}