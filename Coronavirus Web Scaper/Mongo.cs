using Coronavirus_Web_Scaper.Controllers.Mapping;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;

namespace Coronavirus_Web_Scaper
{
	public static class Mongo
	{
		public static IMongoCollection<Rootobject> GetCollection()
		{
			var config = new ConfigurationBuilder()
				.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
				.AddJsonFile("appsettings.json").Build();
			var section = config.GetSection("ConnectionStrings");
			var client = new MongoClient(section.Value);
			return client.GetDatabase("Corona").GetCollection<Rootobject>("Corona");
		}

		public static Rootobject GetRootobject(IMongoCollection<Rootobject> collection)
		{
			var filter = Builders<Rootobject>.Filter.Eq("Country", "BG");
			return collection.Find(filter).FirstOrDefault();
		}
    }
}
