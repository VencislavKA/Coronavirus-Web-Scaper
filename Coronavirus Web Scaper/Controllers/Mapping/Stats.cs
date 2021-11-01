namespace CoronavirusWebScaper.Controllers.Mapping
{
	public class Stats
	{
		public TestedStats Tested { get; set; }

		public ConfirmedCasesStats Confirmed { get; set; }

		public ActiveCasesStats Active { get; set; }
	}
}
