namespace CoronavirusWebScaper.Controllers.Mapping
{
	public class ConfirmedStats
	{
		public double TotalPerTestedPrc { get; set; }

		public double LastPerTestedPrc { get; set; }

		public TotalByTypeOfTestStats TotalByTypePrc { get; set; }

		public LastByTypeOfTestStats LastByTypePrc { get; set; }

		public double MedicalPrc { get; set; }
	}
}
