namespace Coronavirus_Web_Scaper.Models
{
	public class Overall
        {
            public Tested tested { get; set; }
            public Confirmed confirmed { get; set; }
            public Active active { get; set; }
            public Recovered recovered { get; set; }
            public Deceased deceased { get; set; }
            public Vaccinated vaccinated { get; set; }
        }
}
