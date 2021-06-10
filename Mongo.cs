using Coronavirus_Web_Scaper.Controllers.Mapping;
using Coronavirus_Web_Scaper.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Configuration;

namespace Coronavirus_Web_Scaper
{
	public static class Mongo
	{
		private static Rootobject CacheRootobject { get; set; }
		/// <summary>
		/// I Create client that connects to my mongoDB server and returns MongoCollection
		/// </summary>
		/// <returns>MongoCollecrion</returns>
		public static IMongoCollection<Rootobject> GetCollection()
		{
			var config = new ConfigurationBuilder()
				.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
				.AddJsonFile("appsettings.json").Build();
			var section = config.GetSection("ConnectionStrings");
			var client = new MongoClient(section.Value);
			return client.GetDatabase("Corona").GetCollection<Rootobject>("Corona");
		}

		/// <summary>
		/// In this method I use the MongoCollection and find the first and only record the database
		/// </summary>
		/// <param name="collection">This is a MongoCollection that I use to read from the MongoDb database</param>
		/// <returns>Returns the record that I use to store all of needed data for the site</returns>
		public static Rootobject GetRootobject(IMongoCollection<Rootobject> collection)
		{
			var filter = Builders<Rootobject>.Filter.Eq("country", "BG");
			if (collection.Find(filter).FirstOrDefault() != CacheRootobject)
			{
				CacheRootobject = collection.Find(filter).FirstOrDefault();
			}
			return CacheRootobject;
		}
    }
}
