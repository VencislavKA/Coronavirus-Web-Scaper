namespace Coronavirus_Web_Scaper.Controllers.Mapping
{
	public class VaccinatedRegion
	{
		public int total { get; set; }

		public object last { get; set; }

		public Last_By_Type_Of_Vaccine last_by_type { get; set; }

		public int total_completed { get; set; }
	}
}
