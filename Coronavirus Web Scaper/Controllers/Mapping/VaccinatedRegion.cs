namespace CoronavirusWebScaper.Controllers.Mapping
{
	public class VaccinatedRegion
	{
		public int Total { get; set; }

		public object Last { get; set; }

		public LastVacinatedByTypeOfVaccine LastByType { get; set; }

		public int TotalCompleted { get; set; }
	}
}
