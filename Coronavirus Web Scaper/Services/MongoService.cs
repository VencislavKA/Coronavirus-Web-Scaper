using Coronavirus_Web_Scaper.Controllers.Mapping;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using MongoDB.Driver.Linq;

namespace Coronavirus_Web_Scaper.Services
{
	public class MongoService : IMongoService
	{
		private readonly IMongoCollection<RootObject> mongoCollection;

		public MongoService()
		{
			var config = new ConfigurationBuilder()
				.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
				.AddJsonFile("appsettings.json").Build();
			var section = config.GetSection("ConnectionStrings");
			var client = new MongoClient(section.Value);
			mongoCollection = client.GetDatabase("Corona").GetCollection<RootObject>("Corona");
		}

		public IMongoCollection<RootObject> GetCollection() => mongoCollection;

		public RootObject GetRootobject() => mongoCollection.AsQueryable().OrderByDescending(x => x.DateScraped).First();
		
    }
}
