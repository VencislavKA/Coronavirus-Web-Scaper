namespace CoronavirusWebScaper.Controllers.Mapping
{
	public class TestedCountry
	{
		public int TotalTested { get; set; }

		public TotalCasesByTypeOfTest TotalByType { get; set; }

		public int LastTested { get; set; }

		public LastCasesByTypeOfTest LastByType { get; set; }
	}
}
