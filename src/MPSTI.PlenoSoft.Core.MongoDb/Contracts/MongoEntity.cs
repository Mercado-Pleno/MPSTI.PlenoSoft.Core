﻿using MPSTI.PlenoSoft.Core.MongoDb.Interfaces;
using Newtonsoft.Json;

namespace MPSTI.PlenoSoft.Core.MongoDb.Contracts
{
	public abstract class MongoEntity<TId> : IMongoEntity<TId>
	{
		[JsonProperty("id")]
		public virtual TId Id { get; set; }
	}
}