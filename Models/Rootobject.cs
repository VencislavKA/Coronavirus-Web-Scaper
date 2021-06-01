using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Coronavirus_Web_Scaper.Models
{
        /// <summary>
        /// Here is my Rootobject that help to map the data betwean the database and my clases that i can work with.
        /// </summary>
        public class Rootobject
        {
		    public int Id { get; set; }
		    public string date { get; set; }
            public string date_scraped { get; set; }
            public string country { get; set; }
            public Overall overall { get; set; }
            public List<Region> regions { get; set; }
            public Stats stats { get; set; }
        }
        
        public class Overall
        {
            public Tested tested { get; set; }
            public Confirmed confirmed { get; set; }
            public Active active { get; set; }
            public Recovered recovered { get; set; }
            public Deceased deceased { get; set; }
            public Vaccinated vaccinated { get; set; }
        }

        public class Tested
        {
            public int total { get; set; }
            public Total_By_Type_Of_Test total_by_type { get; set; }
            public int last { get; set; }
            public Last_By_Type_Of_Test last_by_type { get; set; }
        }

        public class Total_By_Type_Of_Test
        {
            public int pcr { get; set; }
            public int antigen { get; set; }
        }
        public class Total_By_Type_Of_Test_Stats
        {
            public double pcr { get; set; }
            public double antigen { get; set; }
        }

        public class Last_By_Type_Of_Test
        {
            public int pcr { get; set; }
            public int antigen { get; set; }
        }
        public class Last_By_Type_Of_Test_Stats
        {
            public double pcr { get; set; }
            public double antigen { get; set; }
        }

        public class Confirmed
        {
            public int total { get; set; }
            public Total_By_Type_Of_Test total_by_type { get; set; }
            public int last { get; set; }
            public Last_By_Type_Of_Test last_by_type { get; set; }
            public Medical medical { get; set; }
        }
        public class Medical
        {
            public int total { get; set; }
            public Total_By_Type_Of_Possition total_by_type { get; set; }
            public int last { get; set; }
            public Last_By_Type_Of_Possition last_by_type { get; set; }
        }

        public class Total_By_Type_Of_Possition
        {
            public int doctor { get; set; }
            public int nurces { get; set; }
            public int paramedics_1 { get; set; }
            public int paramedics_2 { get; set; }
            public int other { get; set; }
        }

        public class Last_By_Type_Of_Possition
        {
            public int doctor { get; set; }
            public int nurces { get; set; }
            public int paramedics_1 { get; set; }
            public int paramedics_2 { get; set; }
            public int other { get; set; }
        }

        public class Active
        {
            public int current { get; set; }
            public Current_By_Type current_by_type { get; set; }
        }

        public class Current_By_Type
        {
            public int hospitalized { get; set; }
            public int icu { get; set; }
        }

        public class Recovered
        {
            public int total { get; set; }
            public int last { get; set; }
        }

        public class Deceased
        {
            public int total { get; set; }
            public int last { get; set; }
        }

        public class Vaccinated
        {
            public int total { get; set; }
            public int last { get; set; }
            public Last_By_Type_Of_Vaccine last_by_type { get; set; }
            public int total_completed { get; set; }
        }

        public class Last_By_Type_Of_Vaccine
        {
            public int pfeizer { get; set; }
            public int astrazeneca { get; set; }
            public int moderna { get; set; }
            public int janssen { get; set; }
        }

        public class Region
        {
		    public int id { get; set; }

		    public string NameByЕКАТТЕ { get; set; }

		    public string Name { get; set; }

            public RegionData regisonData { get; set; }
        }

        public class RegionData
        {
            public ConfirmedRegion confirmed { get; set; }
            public VaccinatedRegion vaccinated { get; set; }
        }

        public class ConfirmedRegion
        {
            public int total { get; set; }
            public int last { get; set; }
        }

        public class VaccinatedRegion
        {
            public int total { get; set; }
            public object last { get; set; }
            public Last_By_Type_Of_Vaccine last_by_type { get; set; }
            public int total_completed { get; set; }
        }
        public class Stats
        {
            public TestedStats tested { get; set; }
            public ConfirmedStats confirmed { get; set; }
            public ActiveStats active { get; set; }
        }

        public class TestedStats
        {
            public Total_By_Type_Of_Test_Stats total_by_type_prc { get; set; }
            public Last_By_Type_Of_Test_Stats last_by_type_prc { get; set; }
        }
        public class ConfirmedStats
        {
            public double total_per_tested_prc { get; set; }
            public double last_per_tested_prc { get; set; }
            public Total_By_Type_Of_Test_Stats total_by_type_prc { get; set; }
            public Last_By_Type_Of_Test_Stats last_by_type_prc { get; set; }
            public double medical_prc { get; set; }
        }
        public class ActiveStats
        {
            public double hospitalized_per_active { get; set; }
            public double icu_per_hospitalized { get; set; }
        }
}
