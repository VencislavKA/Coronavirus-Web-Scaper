﻿namespace Coronavirus_Web_Scaper.Models
{
	public class Confirmed
        {
            public int total { get; set; }
            public Total_By_Type_Of_Test total_by_type { get; set; }
            public int last { get; set; }
            public Last_By_Type_Of_Test last_by_type { get; set; }
            public Medical medical { get; set; }
        }
}
