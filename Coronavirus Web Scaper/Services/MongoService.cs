using CoronavirusWebScaper.Controllers.Mapping;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;

namespace CoronavirusWebScaper.Services
{
	public class MongoService : IMongoService
	{
		private readonly MongoCollection mongo = new();
		
		public IMongoCollection<CoronaData> GetAllRecords() => mongo.mongoCollection;

		public CoronaData GetFirstRecordFromMongoCollection() => mongo.mongoCollection.AsQueryable().OrderByDescending(x => x.DateScraped).First();
	}
}
