using CoronavirusWebScaper.Controllers.Mapping;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoronavirusWebScaper.Services
{
	public class MongoCollection
	{
		public readonly IMongoCollection<CoronaData> mongoCollection;

		public MongoCollection()
		{
			var config = new ConfigurationBuilder()
				.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
				.AddJsonFile("appsettings.Development.json").Build();
			var section = config.GetSection("ConnectionStrings");
			var client = new MongoClient(section.Value);
			mongoCollection = client.GetDatabase("Corona").GetCollection<CoronaData>("Corona");
		}
	}
}
