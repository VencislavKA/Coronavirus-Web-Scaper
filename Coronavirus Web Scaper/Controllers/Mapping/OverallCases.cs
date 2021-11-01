namespace CoronavirusWebScaper.Controllers.Mapping
{
	public class OverallCases
	{
		public TestedCountry Tested { get; set; }

		public ConfirmedCasesInCountry Confirmed { get; set; }

		public ActiveCasesInCountry ActiveCases { get; set; }

		public RecoveredPatientCountry Recovered { get; set; }

		public DeceasedCasesInCountry Deceased { get; set; }

		public VaccinatedCountry Vaccinated { get; set; }
	}
}
