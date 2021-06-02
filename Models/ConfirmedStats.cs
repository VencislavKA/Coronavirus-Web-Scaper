namespace Coronavirus_Web_Scaper.Models
{
	public class ConfirmedStats
        {
            public double total_per_tested_prc { get; set; }
            public double last_per_tested_prc { get; set; }
            public Total_By_Type_Of_Test_Stats total_by_type_prc { get; set; }
            public Last_By_Type_Of_Test_Stats last_by_type_prc { get; set; }
            public double medical_prc { get; set; }
        }
}
