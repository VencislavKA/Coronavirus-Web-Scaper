namespace Coronavirus_Web_Scaper.Controllers.Mapping
{
	public class ConfirmedCountry
	{
		public int Total { get; set; }

		public TotalByTypeOfTest Total_by_type { get; set; }

		public int Last { get; set; }

		public LastByTypeOfTest Last_by_type { get; set; }

		public Medical Medical { get; set; }
	}
}
