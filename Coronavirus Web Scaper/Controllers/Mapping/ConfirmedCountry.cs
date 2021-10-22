﻿namespace CoronavirusWebScaper.Controllers.Mapping
{
	public class ConfirmedCountry
	{
		public int Total { get; set; }

		public TotalByTypeOfTest TotalByType { get; set; }

		public int Last { get; set; }

		public LastByTypeOfTest LastByType { get; set; }

		public Medical Medical { get; set; }
	}
}
