namespace Coronavirus_Web_Scaper.Controllers.Mapping
{
	public class VaccinatedRegion
	{
		public int Total { get; set; }

		public object Last { get; set; }

		public LastByTypeOfVaccine Last_by_type { get; set; }

		public int Total_completed { get; set; }
	}
}
