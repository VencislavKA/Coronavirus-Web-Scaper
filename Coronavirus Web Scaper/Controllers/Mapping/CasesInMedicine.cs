namespace CoronavirusWebScaper.Controllers.Mapping
{
	public class CasesInMedicine
	{
		public int Total { get; set; }

		public TotalCasesByTypeOfPossition TotalByTypeOfPossition { get; set; }

		public int Last { get; set; }

		public LastCasesByTypeOfPossition LastByTypeOfPossition { get; set; }
	}
}
