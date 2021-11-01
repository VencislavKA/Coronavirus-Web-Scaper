namespace CoronavirusWebScaper.Controllers.Mapping
{
	public class ConfirmedCasesStats
	{
		public double TotalCasesPerTestedPrc { get; set; }

		public double LastCasesPerTestedPrc { get; set; }

		public TotalCasesByTypeOfTestStats TotalByTypeOfTestStatsPrc { get; set; }

		public LastCasesByTypeOfTestStats LastByTypeOfTestStatsPrc { get; set; }

		public double CasesInMedicinePrc { get; set; }
	}
}
