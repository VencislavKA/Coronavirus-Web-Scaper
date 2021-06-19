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
		public object Id { get; set; }

		public string Date { get; set; }

		public string Date_scraped { get; set; }

		public string Country { get; set; }

		public Overall Overall { get; set; }

		public List<Region> Regions { get; set; }

		public Stats Stats { get; set; }
	}
}
