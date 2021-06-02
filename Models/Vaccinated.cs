namespace Coronavirus_Web_Scaper.Models
{
	public class Vaccinated
        {
            public int total { get; set; }
            public int last { get; set; }
            public Last_By_Type_Of_Vaccine last_by_type { get; set; }
            public int total_completed { get; set; }
        }
}
