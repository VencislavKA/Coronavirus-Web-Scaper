namespace Coronavirus_Web_Scaper.Controllers.Mapping
{
	public class ConfirmedStats
	{
		public double Total_per_tested_prc { get; set; }

		public double Last_per_tested_prc { get; set; }

		public TotalByTypeOfTestStats Total_by_type_prc { get; set; }

		public LastByTypeOfTestStats Last_by_type_prc { get; set; }

		public double Medical_prc { get; set; }
	}
}
