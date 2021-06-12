namespace Coronavirus_Web_Scaper.Controllers.Mapping
{
	public class VaccinatedCountry
	{
		public int Total { get; set; }

		public int Last { get; set; }

		public LastByTypeOfVaccine Last_by_type { get; set; }

		public int Total_completed { get; set; }
	}
}
