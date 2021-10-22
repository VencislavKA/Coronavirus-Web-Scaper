namespace CoronavirusWebScaper.Controllers.Mapping
{
	public class Stats
	{
		public TestedStats Tested { get; set; }

		public ConfirmedStats Confirmed { get; set; }

		public ActiveStats Active { get; set; }
	}
}
