namespace CoronavirusWebScaper.Controllers.Mapping
{
	public class Medical
	{
		public int Total { get; set; }

		public TotalByTypeOfPossition TotalByType { get; set; }

		public int Last { get; set; }

		public LastByTypeOfPossition LastByType { get; set; }
	}
}
