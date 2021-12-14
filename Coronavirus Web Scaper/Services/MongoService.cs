using CoronavirusWebScaper.Controllers.Mapping;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;

namespace CoronavirusWebScaper.Services
{
	public class MongoService : IMongoService
	{
		private MongoCollection mongo;

		public MongoService()
		{
			mongo = new();
		}

		public IMongoCollection<CoronaData> GetAllRecords() 
		{
			try{
				return mongo.mongoCollection;
			}
			catch
			{
				mongo = new();
				return mongo.mongoCollection;
			}
		}

		public CoronaData GetFirstRecordFromMongoCollection()
		{
			try 
			{
				return mongo.mongoCollection.AsQueryable().OrderByDescending(x => x.DateScraped).First();
			}
			catch
			{
				mongo = new();
				return mongo.mongoCollection.AsQueryable().OrderByDescending(x => x.DateScraped).First();
			}
		}
	}
}
