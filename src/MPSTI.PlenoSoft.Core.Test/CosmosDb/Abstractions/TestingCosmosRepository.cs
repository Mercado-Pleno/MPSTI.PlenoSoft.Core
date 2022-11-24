using Microsoft.Azure.Cosmos;
using Moq;
using MPSTI.PlenoSoft.Core.Azure.CosmosDb.Abstractions;
using MPSTI.PlenoSoft.Core.Azure.CosmosDb.Contracts;
using MPSTI.PlenoSoft.Core.Azure.CosmosDb.Extensions;
using MPSTI.PlenoSoft.Core.Azure.CosmosDb.Interfaces;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace MPSTI.PlenoSoft.Core.Test.CosmosDb.Abstractions
{
	public class TestingCosmosRepository
	{
		private readonly MyCosmosRepository _cosmosRepository;
		private readonly Mock<Container> _containerMock;
		private readonly Mock<FeedIterator<ICosmosEntity>> _feedIteratorMock;
		private readonly Mock<TransactionalBatch> _transactionalBatchMock;
		private readonly ICosmosEntity _cosmosEntity;

		public TestingCosmosRepository()
		{
			_cosmosEntity = new MyEntity { Id = "123" };

			_transactionalBatchMock = new Mock<TransactionalBatch>();
			_transactionalBatchMock.Setup(x => x.ExecuteAsync(default))
				.ReturnsAsync(new Mock<TransactionalBatchResponse>().Object);


			var itemResponseMock = new Mock<ItemResponse<ICosmosEntity>>();
			itemResponseMock.Setup(x => x.Resource).Returns(_cosmosEntity);


			var feedResponseMock = new Mock<FeedResponse<ICosmosEntity>>();
			feedResponseMock.Setup(x => x.Resource).Returns(new[] { _cosmosEntity });


			_feedIteratorMock = new Mock<FeedIterator<ICosmosEntity>>();
			_feedIteratorMock.SetupSequence(x => x.HasMoreResults).Returns(true).Returns(false);
			_feedIteratorMock.Setup(x => x.ReadNextAsync(It.IsAny<CancellationToken>()))
				.ReturnsAsync(feedResponseMock.Object);


			_containerMock = new Mock<Container>();
			_containerMock.Setup(x => x.GetItemQueryIterator<ICosmosEntity>(It.IsAny<QueryDefinition>(), It.IsAny<string>(), It.IsAny<QueryRequestOptions>()))
				.Returns(_feedIteratorMock.Object);
			_containerMock.Setup(x => x.CreateItemAsync(_cosmosEntity, It.IsAny<PartitionKey>(), It.IsAny<ItemRequestOptions>(), default))
				.ReturnsAsync(itemResponseMock.Object);
			_containerMock.Setup(x => x.UpsertItemAsync(_cosmosEntity, It.IsAny<PartitionKey>(), It.IsAny<ItemRequestOptions>(), default))
				.ReturnsAsync(itemResponseMock.Object);
			_containerMock.Setup(x => x.DeleteItemAsync<ICosmosEntity>(_cosmosEntity.Id, It.IsAny<PartitionKey>(), It.IsAny<ItemRequestOptions>(), default))
				.ReturnsAsync(itemResponseMock.Object);
			_containerMock.Setup(x => x.PatchItemAsync<ICosmosEntity>(_cosmosEntity.Id, It.IsAny<PartitionKey>(), It.IsAny<IReadOnlyList<PatchOperation>>(), It.IsAny<PatchItemRequestOptions>(), default))
				.ReturnsAsync(itemResponseMock.Object);
			_containerMock.Setup(x => x.ReadItemAsync<ICosmosEntity>(It.IsAny<string>(), It.IsAny<PartitionKey>(), null, default))
				.ReturnsAsync(itemResponseMock.Object);
			_containerMock.Setup(x => x.CreateTransactionalBatch(It.IsAny<PartitionKey>()))
				.Returns(_transactionalBatchMock.Object);


			var containerResponseMock = new Mock<ContainerResponse>();
			containerResponseMock.Setup(x => x.Container).Returns(_containerMock.Object);


			var databaseMock = new Mock<Database>();
			databaseMock.Setup(x => x.CreateContainerIfNotExistsAsync("B", "/id", It.IsAny<int?>(), It.IsAny<RequestOptions>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(containerResponseMock.Object);


			var databaseResponseMock = new Mock<DatabaseResponse>();
			databaseResponseMock.Setup(x => x.Database).Returns(databaseMock.Object);


			var cosmosClientMock = new Mock<CosmosClient>();
			cosmosClientMock.Setup(x => x.GetContainer("A", "B")).Returns(_containerMock.Object);
			cosmosClientMock.Setup(x => x.CreateDatabaseIfNotExistsAsync("A", It.IsAny<int?>(), It.IsAny<RequestOptions>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(databaseResponseMock.Object);

			_cosmosRepository = new MyCosmosRepository(cosmosClientMock.Object);
		}

		[Fact]
		public void AoInscricaoRepository_DeveRetornarDatabaseNameCorretamente() => _cosmosRepository.DatabaseName.Should().Be("A");

		[Fact]
		public void AoInscricaoRepository_DeveRetornarContainerNameCorretamente() => _cosmosRepository.ContainerName.Should().Be("B");

		[Fact]
		public void AoInstanciarSemUmCosmosClient_DeveDispararExcecao() => Assert.Throws<ArgumentNullException>("cosmosClient", () => new MyCosmosRepository(null));

		[Fact]
		public async Task AoChamarInsertAsync_DeveAcionarCreateItemAsync()
		{
			_ = await _cosmosRepository.InsertAsync(_cosmosEntity);

			_containerMock.Verify(x => x.CreateItemAsync(_cosmosEntity, It.IsAny<PartitionKey>(), It.IsAny<ItemRequestOptions>(), default), Times.Once);
		}

		[Fact]
		public async Task AoChamarInsertAsyncComParametroNulo_DeveAcionarCreateItemAsync()
		{
			_ = await _cosmosRepository.InsertAsync(null);

			_containerMock.Verify(x => x.CreateItemAsync(_cosmosEntity, It.IsAny<PartitionKey>(), It.IsAny<ItemRequestOptions>(), default), Times.Never);
		}

		[Fact]
		public async Task AoChamarUpdateAsync_DeveAcionarUpsertItemAsync()
		{
			_ = await _cosmosRepository.UpdateAsync(_cosmosEntity);

			_containerMock.Verify(x => x.UpsertItemAsync(_cosmosEntity, It.IsAny<PartitionKey>(), It.IsAny<ItemRequestOptions>(), default), Times.Once);
		}

		[Fact]
		public async Task AoChamarUpdateAsyncComParametroNulo_DeveNaoAcionarUpsertItemAsync()
		{
			_ = await _cosmosRepository.UpdateAsync(null);

			_containerMock.Verify(x => x.UpsertItemAsync(_cosmosEntity, It.IsAny<PartitionKey>(), It.IsAny<ItemRequestOptions>(), default), Times.Never);
		}

		[Fact]
		public async Task AoChamarUpdateAsyncComETag_DeveAcionarUpsertItemAsync()
		{
			(_cosmosEntity as MyEntity).ETag = "321";

			_ = await _cosmosRepository.UpdateAsync(_cosmosEntity);

			_containerMock.Verify(x => x.UpsertItemAsync(_cosmosEntity, It.IsAny<PartitionKey>(), It.IsAny<ItemRequestOptions>(), default), Times.Once);
		}

		[Fact]
		public async Task AoChamarDeleteAsync_DeveAcionarDeleteItemAsync()
		{
			_ = await _cosmosRepository.DeleteAsync(_cosmosEntity);

			_containerMock.Verify(x => x.DeleteItemAsync<ICosmosEntity>(_cosmosEntity.Id, It.IsAny<PartitionKey>(), It.IsAny<ItemRequestOptions>(), default), Times.Once);
		}

		[Fact]
		public async Task AoChamarDeleteAsyncComParametroNulo_DeveAcionarDeleteItemAsync()
		{
			_ = await _cosmosRepository.DeleteAsync(null);

			_containerMock.Verify(x => x.DeleteItemAsync<ICosmosEntity>(_cosmosEntity.Id, It.IsAny<PartitionKey>(), It.IsAny<ItemRequestOptions>(), default), Times.Never);
		}

		[Fact]
		public async Task AoChamarPatchAsync_DeveAcionarPatchItemAsync()
		{
			_ = await _cosmosRepository.PatchAsync(_cosmosEntity, "/", _cosmosEntity.Id, _cosmosEntity.PartitionKeyValue);

			_containerMock.Verify(x => x.PatchItemAsync<ICosmosEntity>(It.IsAny<string>(), It.IsAny<PartitionKey>(), It.IsAny<IReadOnlyList<PatchOperation>>(), It.IsAny<PatchItemRequestOptions>(), default), Times.Once);
		}

		[Fact]
		public async Task AoChamarPatchAsyncComParametroNulo_DeveAcionarPatchItemAsync()
		{
			_ = await _cosmosRepository.PatchAsync<ICosmosEntity>(null, "/", _cosmosEntity.Id, _cosmosEntity.PartitionKeyValue);

			_containerMock.Verify(x => x.PatchItemAsync<ICosmosEntity>(It.IsAny<string>(), It.IsAny<PartitionKey>(), It.IsAny<IReadOnlyList<PatchOperation>>(), It.IsAny<PatchItemRequestOptions>(), default), Times.Never);
		}

		[Theory]
		[InlineData(" ", " ", " "), InlineData("", "", ""), InlineData(null, null, null)]
		[InlineData("/", "id", ""), InlineData("/", "", "pk"), InlineData("", "id", "pk")]
		[InlineData("/", "id", " "), InlineData("/", " ", "pk"), InlineData(" ", "id", "pk")]
		[InlineData("/", "id", null), InlineData("/", null, "pk"), InlineData(null, "id", "pk")]
		public async Task AoChamarPatchAsyncComAlgumParametroNulo_DeveAcionarPatchItemAsync(string path, string id, string partitionKeyValue)
		{
			_ = await _cosmosRepository.PatchAsync(_cosmosEntity, path, id, partitionKeyValue);

			_containerMock.Verify(x => x.PatchItemAsync<ICosmosEntity>(It.IsAny<string>(), It.IsAny<PartitionKey>(), It.IsAny<IReadOnlyList<PatchOperation>>(), It.IsAny<PatchItemRequestOptions>(), default), Times.Never);
		}

		[Fact]
		public async Task AoChamarGetByPKAsIdAsyncComPKOk_DeveRetornarValorDefault()
		{
			var entity = await _cosmosRepository.GetByPKAsIdAsync(_cosmosEntity.PartitionKeyValue);

			entity.Should().NotBeNull();
			_containerMock.Verify(x => x.ReadItemAsync<ICosmosEntity>(It.IsAny<string>(), It.IsAny<PartitionKey>(), null, default), Times.Once);
		}

		[Fact]
		public async Task AoChamarGetByPKAsIdAsyncComPKNulo_DeveRetornarValorDefault()
		{
			var entity = await _cosmosRepository.GetByPKAsIdAsync(null);

			entity.Should().BeNull();
			_containerMock.Verify(x => x.ReadItemAsync<ICosmosEntity>(It.IsAny<string>(), It.IsAny<PartitionKey>(), null, default), Times.Never);
		}

		[Fact]
		public async Task AoChamarGetAsyncComEntityNulo_DeveRetornarValorDefault()
		{
			var entity = await _cosmosRepository.GetAsync(cosmosEntity: null);

			entity.Should().BeNull();
			_containerMock.Verify(x => x.ReadItemAsync<ICosmosEntity>(It.IsAny<string>(), It.IsAny<PartitionKey>(), null, default), Times.Never);
		}

		[Fact]
		public async Task AoChamarGetAsyncComEntityInstanciadoInexistente_DeveRetornarValorDefault()
		{
			_containerMock.Setup(x => x.ReadItemAsync<ICosmosEntity>(It.IsAny<string>(), It.IsAny<PartitionKey>(), null, default))
				.Throws(new CosmosException("", HttpStatusCode.NotFound, 1, "", 0));

			var entity = await _cosmosRepository.GetAsync(_cosmosEntity);

			entity.Should().BeNull();
			_containerMock.Verify(x => x.ReadItemAsync<ICosmosEntity>(It.IsAny<string>(), It.IsAny<PartitionKey>(), null, default), Times.Once);
		}

		[Fact]
		public async Task AoChamarGetAsyncComEntityInstanciadoExistente_DeveRetornarUmItemInstanciado()
		{
			var entity = await _cosmosRepository.GetAsync(_cosmosEntity);

			entity.Should().NotBeNull();
			_containerMock.Verify(x => x.ReadItemAsync<ICosmosEntity>(It.IsAny<string>(), It.IsAny<PartitionKey>(), null, default), Times.Once);
		}

		[Fact]
		public async Task AoChamarGetAsyncComIdVazio_DeveRetornarDefault()
		{
			var entity = await _cosmosRepository.GetAsync(id: null, "partitionKeyValue");

			entity.Should().BeNull();
			_containerMock.Verify(x => x.ReadItemAsync<ICosmosEntity>(It.IsAny<string>(), It.IsAny<PartitionKey>(), null, default), Times.Never);
		}

		[Fact]
		public async Task AoChamarGetAsyncComPKVazio_DeveRetornarDefault()
		{
			var entity = await _cosmosRepository.GetAsync(id: "id", null);

			entity.Should().BeNull();
			_containerMock.Verify(x => x.ReadItemAsync<ICosmosEntity>(It.IsAny<string>(), It.IsAny<PartitionKey>(), null, default), Times.Never);
		}

		[Fact]
		public async Task AoChamarGetAsyncComIdVazioPKVazio_DeveRetornarDefault()
		{
			var entity = await _cosmosRepository.GetAsync(id: string.Empty, string.Empty);

			entity.Should().BeNull();
			_containerMock.Verify(x => x.ReadItemAsync<ICosmosEntity>(It.IsAny<string>(), It.IsAny<PartitionKey>(), null, default), Times.Never);
		}

		[Fact]
		public async Task AoChamarGetAsyncComIdOkPKOk_DeveRetornarInstanciado()
		{
			var entity = await _cosmosRepository.GetAsync(id: _cosmosEntity.Id, _cosmosEntity.PartitionKeyValue);

			entity.Should().NotBeNull();
			_containerMock.Verify(x => x.ReadItemAsync<ICosmosEntity>(It.IsAny<string>(), It.IsAny<PartitionKey>(), null, default), Times.Once);
		}

		[Fact]
		public async Task AoChamarGetAsyncComIdNulo_DeveRetornarDefault()
		{
			var entity = await _cosmosRepository.GetAsync(id: null);

			entity.Should().BeNull();
			_containerMock.Verify(x => x.GetItemQueryIterator<ICosmosEntity>(It.IsAny<QueryDefinition>(), It.IsAny<string>(), It.IsAny<QueryRequestOptions>()), Times.Never);
		}

		[Fact]
		public async Task AoChamarGetAsyncComIdOk_DeveRetornarInstanciado()
		{
			var entity = await _cosmosRepository.GetAsync(id: _cosmosEntity.Id);

			entity.Should().NotBeNull();
			_containerMock.Verify(x => x.GetItemQueryIterator<ICosmosEntity>(It.IsAny<QueryDefinition>(), It.IsAny<string>(), It.IsAny<QueryRequestOptions>()), Times.Once);
		}

		[Fact]
		public async Task AoChamarGetAsyncComIdsVazio_DeveRetornarListaVazia()
		{
			var list = await _cosmosRepository.GetAsync(ids: Array.Empty<string>());

			list.Should().NotBeNull();
			list.Should().BeEmpty();

			_containerMock.Verify(x => x.GetItemQueryIterator<ICosmosEntity>(It.IsAny<QueryDefinition>(), It.IsAny<string>(), It.IsAny<QueryRequestOptions>()), Times.Never);
		}

		[Fact]
		public async Task AoChamarGetAsyncComIdsNulos_DeveRetornarListaVazia()
		{
			var list = await _cosmosRepository.GetAsync(ids: null);

			list.Should().NotBeNull();
			list.Should().BeEmpty();

			_containerMock.Verify(x => x.GetItemQueryIterator<ICosmosEntity>(It.IsAny<QueryDefinition>(), It.IsAny<string>(), It.IsAny<QueryRequestOptions>()), Times.Never);
		}

		[Fact]
		public async Task AoChamarGetAsyncComIdsInexistentes_DeveRetornarListaVazia()
		{
			_feedIteratorMock.Setup(x => x.HasMoreResults).Returns(false);

			var list = await _cosmosRepository.GetAsync(ids: new[] { "inexistente" });

			list.Should().NotBeNull();
			list.Should().BeEmpty();

			_containerMock.Verify(x => x.GetItemQueryIterator<ICosmosEntity>(It.IsAny<QueryDefinition>(), It.IsAny<string>(), It.IsAny<QueryRequestOptions>()), Times.Once);
		}

		[Fact]
		public async Task AoChamarGetAsyncComIdsExistentes_DeveRetornarListaPreenchida()
		{
			var list = await _cosmosRepository.GetAsync(ids: new[] { _cosmosEntity.Id });

			list.Should().NotBeNull();
			list.Should().NotBeEmpty();

			_containerMock.Verify(x => x.GetItemQueryIterator<ICosmosEntity>(It.IsAny<QueryDefinition>(), It.IsAny<string>(), It.IsAny<QueryRequestOptions>()), Times.Once);
		}

		[Fact]
		public async Task AoChamarGetAllAsyncSemParametro_DeveRetornarListaPreenchida()
		{
			var list = await _cosmosRepository.GetAllAsync();

			list.Should().NotBeNull();
			list.Should().NotBeEmpty();

			_containerMock.Verify(x => x.GetItemQueryIterator<ICosmosEntity>(It.IsAny<QueryDefinition>(), It.IsAny<string>(), It.IsAny<QueryRequestOptions>()), Times.Once);
		}

		[Fact]
		public async Task AoChamarGetAllAsyncComPartitionKeyValue_DeveRetornarListaPreenchida()
		{
			var list = await _cosmosRepository.GetAllAsync("partitionKeyValue");

			list.Should().NotBeNull();
			list.Should().NotBeEmpty();

			_containerMock.Verify(x => x.GetItemQueryIterator<ICosmosEntity>(It.IsAny<QueryDefinition>(), It.IsAny<string>(), It.IsAny<QueryRequestOptions>()), Times.Once);
		}

		[Fact]
		public async Task AoChamarGetAllAsyncComPartitionKeyValueNulo_DeveRetornarListaPreenchida()
		{
			var list = await _cosmosRepository.GetAllAsync(partitionKeyValue: null);

			list.Should().NotBeNull();
			list.Should().BeEmpty();

			_containerMock.Verify(x => x.GetItemQueryIterator<ICosmosEntity>(It.IsAny<QueryDefinition>(), It.IsAny<string>(), It.IsAny<QueryRequestOptions>()), Times.Never);
		}

		[Fact]
		public async Task AoChamarGetAllAsyncComQueryRequestOptions_DeveRetornarListaPreenchida()
		{
			var queryRequestOptions = new QueryRequestOptions();
			var list = await _cosmosRepository.GetAllAsync(queryRequestOptions);

			list.Should().NotBeNull();
			list.Should().NotBeEmpty();

			_containerMock.Verify(x => x.GetItemQueryIterator<ICosmosEntity>(It.IsAny<QueryDefinition>(), It.IsAny<string>(), queryRequestOptions), Times.Once);
		}

		[Fact]
		public async Task AoChamarQueryAsyncComQuery_DeveRetornarListaPreenchida()
		{
			var query = "Select * From c Where c.Id = @id";
			var list = await _cosmosRepository.QueryAsync(query);

			list.Should().NotBeNull();
			list.Should().NotBeEmpty();

			_containerMock.Verify(x => x.GetItemQueryIterator<ICosmosEntity>(It.Is<QueryDefinition>(x => x.QueryText == query), It.IsAny<string>(), It.IsAny<QueryRequestOptions>()), Times.Once);
		}

		[Fact]
		public async Task AoChamarQueryAsyncComQueryNula_DeveRetornarListaPreenchida()
		{
			var list = await _cosmosRepository.QueryAsync(query: null);

			list.Should().NotBeNull();
			list.Should().NotBeEmpty();

			_containerMock.Verify(x => x.GetItemQueryIterator<ICosmosEntity>(It.Is<QueryDefinition>(x => x.QueryText == CosmosRepository<ICosmosEntity>.DefaultQuery), It.IsAny<string>(), It.IsAny<QueryRequestOptions>()), Times.Once);
		}

		[Fact]
		public async Task AoChamarQueryAsyncENaoExistir_DeveRetornarListaVazia()
		{
			_feedIteratorMock.Setup(x => x.HasMoreResults).Returns(false);
			var query = "Select * From c Where c.Id = @id";

			var list = await _cosmosRepository.QueryAsync(query);

			list.Should().NotBeNull();
			list.Should().BeEmpty();

			_containerMock.Verify(x => x.GetItemQueryIterator<ICosmosEntity>(It.Is<QueryDefinition>(x => x.QueryText == query), It.IsAny<string>(), It.IsAny<QueryRequestOptions>()), Times.Once);
		}

		[Fact]
		public async Task AoChamarQueryAsyncComParametroENaoExistir_DeveRetornarListaVazia()
		{
			_feedIteratorMock.Setup(x => x.HasMoreResults).Returns(false);
			var query = "Select * From c Where c.Id = @id";
			var parameters = new Dictionary<string, object> { { "@id", "nao-existe" } };
			var queryDefinition = new QueryDefinition(query).WithParameters(parameters);
			var queryRequestOptions = new QueryRequestOptions().WithMaxItemCount(1, true);

			var lista = await _cosmosRepository.QueryAsync(queryDefinition, queryRequestOptions);

			lista.Should().NotBeNull();
			lista.Should().HaveCount(0);

			_containerMock.Verify(x => x.GetItemQueryIterator<ICosmosEntity>(It.IsAny<QueryDefinition>(), It.IsAny<string>(), It.IsAny<QueryRequestOptions>()), Times.Once);
		}

		[Fact]
		public async Task AoChamarExecuteBatchComUmaLista_DeveExecutarEmLotes()
		{
			var list = new List<ICosmosEntity> { _cosmosEntity };

			var result = await _cosmosRepository.ExecuteBatch(list, (partitionKeyValue, items, batch) =>
			{
				foreach (var item in items)
					batch.UpsertItem(item);
			});


			result.Should().NotBeNull();
			_transactionalBatchMock.Verify(x => x.ExecuteAsync(default), Times.Once);
		}

		[Fact]
		public async Task AoChamarExecuteBatchComUmaPK_DeveExecutarEmLotes()
		{
			var list = new List<ICosmosEntity> { _cosmosEntity };

			var result = await _cosmosRepository.ExecuteBatch("123", (partitionKeyValue, batch) =>
			{
				batch.UpsertItem(_cosmosEntity);
				foreach (var item in list.Where(x => x.PartitionKeyValue == partitionKeyValue))
					batch.UpsertItem(item);
			});

			result.Should().NotBeNull();
			_transactionalBatchMock.Verify(x => x.ExecuteAsync(default), Times.Once);
		}
	}



	public class MyEntity : CosmosEntityConcurrent<string>
	{
		protected override string PartitionKeyValue => Id;
	}

	public class MyCosmosRepository : CosmosRepository<ICosmosEntity>
	{
		public override string DatabaseName => "A";
		public override string ContainerName => "B";
		public override string PartitionKeyPath => "/id";

		public MyCosmosRepository(CosmosClient cosmosClient) : base(cosmosClient)
		{
			CreateDatabaseAndContainer(cosmosClient).GetAwaiter().GetResult();
		}
	}
}
