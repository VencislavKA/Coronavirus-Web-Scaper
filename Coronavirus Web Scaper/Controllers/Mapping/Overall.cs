namespace CoronavirusWebScaper.Controllers.Mapping
{
	public class Overall
	{
		public TestedCountry Tested { get; set; }

		public ConfirmedCountry Confirmed { get; set; }

		public ActiveCountry Active { get; set; }

		public RecoveredCountry Recovered { get; set; }

		public DeceasedCountry Deceased { get; set; }

		public VaccinatedCountry Vaccinated { get; set; }
	}
}
