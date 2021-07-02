namespace Coronavirus_Web_Scaper.Controllers.Mapping
{
	public class TestedCountry
	{
		public int Total { get; set; }

		public TotalByTypeOfTest TotalByType { get; set; }

		public int Last { get; set; }

		public LastByTypeOfTest LastByType { get; set; }
	}
}
