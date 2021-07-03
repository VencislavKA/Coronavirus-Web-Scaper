using Coronavirus_Web_Scaper.Controllers.Mapping;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;

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

		/// <summary>
		/// Gets the colection for the database
		/// </summary>
		/// <returns>Returns the mongoCollectin</returns>
		public IMongoCollection<RootObject> GetCollection() => mongoCollection;

		/// <summary>
		/// Gets the last record in the database
		/// </summary>
		/// <returns>Returns the newest record</returns>
		public RootObject GetRootobject() => mongoCollection.AsQueryable().OrderByDescending(x => x.DateScraped).First();

	}
}
