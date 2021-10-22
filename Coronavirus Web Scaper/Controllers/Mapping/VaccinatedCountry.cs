namespace CoronavirusWebScaper.Controllers.Mapping
{
	public class VaccinatedCountry
	{
		public int Total { get; set; }

		public int Last { get; set; }

		public LastByTypeOfVaccine LastByType { get; set; }

		public int TotalCompleted { get; set; }
	}
}
