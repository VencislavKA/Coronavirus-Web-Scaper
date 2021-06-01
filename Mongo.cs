using Coronavirus_Web_Scaper.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Coronavirus_Web_Scaper
{
	public static class Mongo
	{
		/// <summary>
		/// I Create client that connects to my mongoDB server and returns MongoCollection
		/// </summary>
		/// <returns>MongoCollecrion</returns>
		public static IMongoCollection<Rootobject> GetCollection()
		{
			var client = new MongoClient("mongodb+srv://admin:admin@cluster0.ci13y.mongodb.net/myFirstDatabase?retryWrites=true&w=majority");
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
			return collection.Find(filter).FirstOrDefault();
		}
    }
}
