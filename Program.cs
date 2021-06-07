using Coronavirus_Web_Scaper.Controllers.Mapping;
using Coronavirus_Web_Scaper.Models;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace Coronavirus_Web_Scaper
{
    public class Program
    {
        private static IMongoCollection<Rootobject> dbcollection = Mongo.GetCollection();
        /// <summary>
        /// Creting timer to know wnen to update the information in the database.
        /// </summary>
        //static Timer timer = new Timer(86400000);
        static Timer timer = new Timer(1000);
        /// <summary>
        /// In the main method I check if there is a record in the database. If there is not I create it. 
        /// After that I start the timer with atached event on it to update the information in the database.
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
			if (Mongo.GetRootobject(dbcollection) == null)
			{
                dbcollection.InsertOne(GetValidatedDataFromSite());
            }
			timer.Elapsed += Timer_Elapsed1;
            timer.AutoReset = true;
            timer.Start();
            CreateHostBuilder(args).Build().Run();
        }

		private static void Timer_Elapsed1(object sender, ElapsedEventArgs e)
		{
            timer.Stop();
            var filter = Builders<Rootobject>.Filter.Eq("country", "BG");
            dbcollection.DeleteOne(filter);
            dbcollection.InsertOne(GetValidatedDataFromSite());
            timer.Start();
        }

        private static Rootobject GetValidatedDataFromSite()
        {
            HtmlWeb web = new HtmlWeb();
            var doc = web.Load("https://coronavirus.bg");
            var doc2 = web.Load("https://coronavirus.bg/bg/statistika");
            Rootobject dataModel = new Rootobject()
            {
                date = DateTime.Now.ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffffffzzz"),
                date_scraped = DateTime.Now.ToString("u"),
                country = "BG",
                overall = new Overall()
                {
                    tested = new Tested()
                    {
                        total = ValidateNumber(doc, "/html/body/main/div[1]/div/div[5]/div[1]/p[1]"),
                        last = ValidateNumber(doc, "/html/body/main/div[1]/div/div[5]/div[1]/p[3]"),
                        total_by_type = new Total_By_Type_Of_Test()
                        {
                            pcr = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[1]/table[2]/tbody/tr[2]/td[2]"),
                            antigen = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[1]/table[2]/tbody/tr[1]/td[2]")
                        },
                        last_by_type = new Last_By_Type_Of_Test()
                        {
                            pcr = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[1]/table[2]/tbody/tr[1]/td[3]"),
                            antigen = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[1]/table[2]/tbody/tr[2]/td[3]")
                        }
                    },
                    confirmed = new Confirmed()
                    {
                        total = ValidateNumber(doc, "/html/body/main/div[1]/div/div[5]/div[2]/p[1]"),
                        last = ValidateNumber(doc, "/html/body/main/div[1]/div/div[4]/div[1]/h1"),
                        medical = new Medical()
                        {
                            total = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[3]/table/tbody/tr[6]/td[2]"),
                            total_by_type = new Total_By_Type_Of_Possition()
                            {
                                doctor = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[3]/table/tbody/tr[1]/td[2]"),
                                nurces = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[3]/table/tbody/tr[2]/td[2]"),
                                paramedics_1 = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[3]/table/tbody/tr[3]/td[2]"),
                                paramedics_2 = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[3]/table/tbody/tr[4]/td[2]"),
                                other = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[3]/table/tbody/tr[5]/td[2]"),
                            },

                        },
                        total_by_type = new Total_By_Type_Of_Test()
                        {
                            pcr = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[1]/table[3]/tbody/tr[1]/td[2]"),
                            antigen = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[1]/table[3]/tbody/tr[2]/td[2]")
                        },
                        last_by_type = new Last_By_Type_Of_Test()
                        {
                            pcr = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[1]/table[3]/tbody/tr[1]/td[3]"),
                            antigen = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[1]/table[3]/tbody/tr[2]/td[3]")
                        }
                    },
                    active = new Active()
                    {
                        current = ValidateNumber(doc, "/html/body/main/div[1]/div/div[5]/div[2]/p[3]"),
                        current_by_type = new Current_By_Type()
                        {
                            hospitalized = ValidateNumber(doc, "/html/body/main/div[1]/div/div[5]/div[4]/p[1]"),
                            icu = ValidateNumber(doc, "/html/body/main/div[1]/div/div[5]/div[4]/p[3]")
                        }
                    },
                    recovered = new Recovered()
                    {
                        total = ValidateNumber(doc, "/html/body/main/div[1]/div/div[5]/div[3]/p[1]"),
                        last = ValidateNumber(doc, "/html/body/main/div[1]/div/div[5]/div[3]/p[3]")
                    },
                    deceased = new Deceased()
                    {
                        total = ValidateNumber(doc, "/html/body/main/div[1]/div/div[5]/div[5]/p[1]"),
                        last = ValidateNumber(doc, "/html/body/main/div[1]/div/div[5]/div[5]/p[3]")
                    },
                    vaccinated = new Vaccinated()
                    {
                        total = ValidateNumber(doc, "/html/body/main/div[1]/div/div[5]/div[6]/p[1]"),
                        total_completed = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[4]/table/tbody/tr[29]/td[7]"),
                        last = ValidateNumber(doc, "/html/body/main/div[1]/div/div[5]/div[6]/p[3]"),
                        last_by_type = new Last_By_Type_Of_Vaccine()
                        {
                            astrazeneca = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[4]/table/tbody/tr[29]/td[5]"),
                            janssen = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[4]/table/tbody/tr[29]/td[6]"),
                            moderna = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[4]/table/tbody/tr[29]/td[4]"),
                            pfeizer = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[4]/table/tbody/tr[29]/td[3]")
                        }
                    },
                },
            };
            dataModel.regions = new List<Region>();
            string[] nameByEKATTE = Enum.GetNames(typeof(NameByЕКАТТЕ));
            for (int i = 1; i <= 28; i++)
            {
                dataModel.regions.Add(new Region()
                {
                    id = i,
                    Name = doc2.DocumentNode.SelectNodes("/html/body/main/div[3]/div/div[1]/div[2]/table/tbody/tr[" + i + "]/td[1]").FirstOrDefault().InnerText,
                    NameByЕКАТТЕ = nameByEKATTE[i - 1],
                    regisonData = new RegionData()
                    {
                        confirmed = new ConfirmedRegion()
                        {
                            total = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[2]/table/tbody/tr[" + i + "]/td[2]"),
                            last = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[2]/table/tbody/tr[" + i + "]/td[3]")
                        },
                        vaccinated = new VaccinatedRegion()
                        {
                            total = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[4]/table/tbody/tr[" + i + "]/td[2]"),
                            total_completed = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[4]/table/tbody/tr[" + i + "]/td[7]"),
                            last_by_type = new Last_By_Type_Of_Vaccine()
                            {
                                pfeizer = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[4]/table/tbody/tr[" + i + "]/td[3]"),
                                moderna = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[4]/table/tbody/tr[" + i + "]/td[4]"),
                                astrazeneca = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[4]/table/tbody/tr[" + i + "]/td[5]"),
                                janssen = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[4]/table/tbody/tr[" + i + "]/td[6]")
                            },
                            last = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[4]/table/tbody/tr[" + i + "]/td[3]")
                            + ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[4]/table/tbody/tr[" + i + "]/td[4]")
                            + ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[4]/table/tbody/tr[" + i + "]/td[5]")
                            + ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[4]/table/tbody/tr[" + i + "]/td[6]")
                        }
                    }
                });
            }
            dataModel.stats = new Stats()
            {
                tested = new TestedStats()
                {
                    total_by_type_prc = new Total_By_Type_Of_Test_Stats()
                    {
                        pcr = Math.Round((double)dataModel.overall.tested.total_by_type.pcr / dataModel.overall.tested.total * 100, 2),
                        antigen = Math.Round((double)dataModel.overall.tested.total_by_type.antigen / dataModel.overall.tested.total * 100, 2)
                    },
                    last_by_type_prc = new Last_By_Type_Of_Test_Stats()
                    {
                        pcr = Math.Round((double)dataModel.overall.tested.last_by_type.pcr / dataModel.overall.tested.last * 100, 2),
                        antigen = Math.Round((double)dataModel.overall.tested.last_by_type.antigen / dataModel.overall.tested.last * 100, 2)
                    }
                },
                confirmed = new ConfirmedStats()
                {
                    total_per_tested_prc = Math.Round((double)dataModel.overall.confirmed.total / dataModel.overall.tested.total * 100, 2),
                    last_per_tested_prc = Math.Round((double)dataModel.overall.confirmed.last / dataModel.overall.tested.last * 100, 2),
                    total_by_type_prc = new Total_By_Type_Of_Test_Stats()
                    {
                        pcr = Math.Round((double)dataModel.overall.confirmed.total / dataModel.overall.tested.total_by_type.pcr * 100, 2),
                        antigen = Math.Round((double)dataModel.overall.confirmed.total / dataModel.overall.tested.total_by_type.antigen * 100, 2)
                    },
                    last_by_type_prc = new Last_By_Type_Of_Test_Stats()
                    {
                        pcr = Math.Round((double)dataModel.overall.confirmed.last / dataModel.overall.tested.last_by_type.pcr * 100, 2),
                        antigen = Math.Round((double)dataModel.overall.confirmed.last / dataModel.overall.tested.last_by_type.antigen * 100, 2)
                    },
                    medical_prc = Math.Round((double)dataModel.overall.confirmed.medical.total / dataModel.overall.confirmed.total * 100, 2)
                },
                active = new ActiveStats()
                {
                    hospitalized_per_active = Math.Round((double)dataModel.overall.active.current_by_type.hospitalized / dataModel.overall.active.current * 100, 2),
                    icu_per_hospitalized = Math.Round((double)dataModel.overall.active.current_by_type.icu / dataModel.overall.active.current * 100, 2)
                }
            };

            if (ValidateModelData(dataModel) == false)
            {
                return null;
            }
            return dataModel;
        }

        private static int ValidateNumber(HtmlDocument doc, string pathern)
        {
            int parsed = 0;
            int.TryParse(doc.DocumentNode.SelectNodes(pathern).FirstOrDefault().InnerText.Replace(" ", string.Empty), out parsed);
            return parsed;
        }

        private static bool ValidateModelData(Rootobject rootobject)
        {
            var testedLast = rootobject.overall.tested.last;
            var lastTypesSum = rootobject.overall.tested.last_by_type.pcr + rootobject.overall.tested.last_by_type.antigen;
            if (testedLast != lastTypesSum)
            {
                return false;
            }
            var totatlConfirmedLast = rootobject.overall.confirmed.last;
            var lastConfirmedTypesSum = rootobject.overall.confirmed.last_by_type.antigen + rootobject.overall.confirmed.last_by_type.pcr;
            if (totatlConfirmedLast != lastConfirmedTypesSum)
            {
                return false;
            }
            var RegionsLastSum = 0;
            foreach (var item in rootobject.regions)
            {
                RegionsLastSum += item.regisonData.confirmed.last;
            }
            if (totatlConfirmedLast != RegionsLastSum)
            {
                return false;
            }
            var vacinatedLast = rootobject.overall.vaccinated.last;
            var lastVacinatedByTypesSum = rootobject.overall.vaccinated.last_by_type.astrazeneca
                + rootobject.overall.vaccinated.last_by_type.janssen
                + rootobject.overall.vaccinated.last_by_type.moderna
                + rootobject.overall.vaccinated.last_by_type.pfeizer;
            if (vacinatedLast != lastVacinatedByTypesSum)
            {
                return false;
            }
            return true;
        }


        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
