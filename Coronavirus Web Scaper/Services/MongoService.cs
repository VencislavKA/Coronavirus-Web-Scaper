using CoronavirusWebScaper.Controllers.Mapping;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;

namespace CoronavirusWebScaper.Services
{
	public class MongoService : IMongoService
	{
		private readonly IMongoCollection<RootObject> mongoCollection;

		public MongoService()
		{
			var config = new ConfigurationBuilder()
				.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
				.AddJsonFile("appsettings.Development.json").Build();
			var section = config.GetSection("ConnectionStrings");
			var client = new MongoClient(section.Value);
			mongoCollection = client.GetDatabase("Corona").GetCollection<RootObject>("Corona");
		}
		
		public IMongoCollection<RootObject> GetCollection() => mongoCollection;

		public RootObject GetRootobject() => mongoCollection.AsQueryable().OrderByDescending(x => x.DateScraped).First();
	}
}
