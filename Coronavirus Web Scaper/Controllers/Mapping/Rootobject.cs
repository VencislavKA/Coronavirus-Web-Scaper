using System;
using System.Collections.Generic;

namespace CoronavirusWebScaper.Controllers.Mapping
{
	public class RootObject
	{
		public RootObject()
		{
			this.Id = Guid.NewGuid().ToString();
		}

		public string Id { get; set; }

		public DateTime Date { get; set; }

		public DateTime DateScraped { get; set; }

		public string Country { get; set; }

		public Overall Overall { get; set; }

		public List<Region> Regions { get; set; }

		public Stats Stats { get; set; }
	}
}
