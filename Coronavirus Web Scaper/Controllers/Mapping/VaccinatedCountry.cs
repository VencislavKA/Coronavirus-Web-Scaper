namespace CoronavirusWebScaper.Controllers.Mapping
{
	public class VaccinatedCountry
	{
		public int TotalVaccinated { get; set; }

		public int LastVaccinated { get; set; }

		public LastVacinatedByTypeOfVaccine LastVaccinatedByType { get; set; }

		public int TotalVaccinesCompleted { get; set; }
	}
}
