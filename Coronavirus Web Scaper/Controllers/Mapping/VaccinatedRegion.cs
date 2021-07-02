namespace Coronavirus_Web_Scaper.Controllers.Mapping
{
	public class VaccinatedRegion
	{
		public int Total { get; set; }

		public object Last { get; set; }

		public LastByTypeOfVaccine LastByType { get; set; }

		public int TotalCompleted { get; set; }
	}
}
