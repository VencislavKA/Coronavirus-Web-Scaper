namespace CoronavirusWebScaper.Controllers.Mapping
{
	public class ConfirmedCasesInCountry
	{
		public int TotalCases { get; set; }

		public TotalCasesByTypeOfTest TotalCasesByType { get; set; }

		public int LastCases { get; set; }

		public LastCasesByTypeOfTestStats LastCasesByType { get; set; }

		public CasesInMedicine CasesInMedicine { get; set; }
	}
}
