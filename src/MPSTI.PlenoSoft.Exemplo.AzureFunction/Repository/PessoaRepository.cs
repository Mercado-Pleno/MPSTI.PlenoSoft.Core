using MongoDB.Bson;
using MongoDB.Driver;
using MPSTI.PlenoSoft.Core.MongoDb.Abstractions;
using MPSTI.PlenoSoft.Core.MongoDb.Interfaces;
using MPSTI.PlenoSoft.Exemplo.AzureFunction.Contracts;
using System;
using System.Linq.Expressions;

namespace MPSTI.PlenoSoft.Exemplo.AzureFunction.Repository
{
	public interface IPessoaRepository : IMongoRepository<Pessoa, ObjectId> { }

	public class PessoaRepository : MongoRepository<Pessoa, ObjectId>, IPessoaRepository
	{
		public override string DatabaseName => "MercadoPleno";
		public override string CollectionName => "Pessoa";
		public PessoaRepository(IMongoClient mongoClient) : base(mongoClient) { }

		protected override bool IsDefaultValue(ObjectId id) => id == ObjectId.Empty;

		protected override Expression<Func<Pessoa, bool>> GetFilterById(ObjectId id) => e => e.Id == id;
	}
}