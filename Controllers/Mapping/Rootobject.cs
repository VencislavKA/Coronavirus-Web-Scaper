using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Coronavirus_Web_Scaper.Controllers.Mapping
{
	/// <summary>
	/// Here is my Rootobject that help to map the data betwean the database and my clases that i can work with.
	/// </summary>
	public class Rootobject
	{
		public int Id { get; set; }

		public string date { get; set; }

		public string date_scraped { get; set; }

		public string country { get; set; }

		public Overall overall { get; set; }

		public List<Region> regions { get; set; }

		public Stats stats { get; set; }
	}
}
