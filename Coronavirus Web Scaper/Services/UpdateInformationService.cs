using Coronavirus_Web_Scaper.Controllers.Mapping;
using Coronavirus_Web_Scaper.Models;
using HtmlAgilityPack;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Coronavirus_Web_Scaper.Services
{
	public class UpdateInformationService : IHostedService
	{

        private Timer timer;

        private IMongoCollection<Rootobject> dbcollection = Mongo.GetCollection();

        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(
            UpdateInformation,
            null,
            TimeSpan.Zero,
            TimeSpan.FromHours(24)
            );

			
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        private void UpdateInformation(object state)
		{
            var filter = Builders<Rootobject>.Filter.Eq("Country", "BG");
            dbcollection.ReplaceOne(filter, GetValidatedDataFromSite());
        }

        private static Rootobject GetValidatedDataFromSite()
        {
            HtmlWeb web = new HtmlWeb();
            var doc = web.Load("https://coronavirus.bg");
            var doc2 = web.Load("https://coronavirus.bg/bg/statistika");
            Rootobject dataModel = new Rootobject()
            {
                Date = DateTime.Now.ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffffffzzz"),
                Date_scraped = DateTime.Now.ToString("u"),
                Country = "BG",
                Overall = new Overall()
                {
                    Tested = new TestedCountry()
                    {
                        Total = ValidateNumber(doc, "/html/body/main/div[1]/div/div[5]/div[1]/p[1]"),
                        Last = ValidateNumber(doc, "/html/body/main/div[1]/div/div[5]/div[1]/p[3]"),
                        Total_by_type = new TotalByTypeOfTest()
                        {
                            Pcr = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[1]/table[2]/tbody/tr[2]/td[2]"),
                            Antigen = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[1]/table[2]/tbody/tr[1]/td[2]")
                        },
                        Last_by_type = new LastByTypeOfTest()
                        {
                            Pcr = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[1]/table[2]/tbody/tr[1]/td[3]"),
                            Antigen = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[1]/table[2]/tbody/tr[2]/td[3]")
                        }
                    },
                    Confirmed = new ConfirmedCountry()
                    {
                        Total = ValidateNumber(doc, "/html/body/main/div[1]/div/div[5]/div[2]/p[1]"),
                        Last = ValidateNumber(doc, "/html/body/main/div[1]/div/div[4]/div[1]/h1"),
                        Medical = new Medical()
                        {
                            Total = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[3]/table/tbody/tr[6]/td[2]"),
                            Total_by_type = new TotalByTypeOfPossition()
                            {
                                Doctor = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[3]/table/tbody/tr[1]/td[2]"),
                                Nurces = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[3]/table/tbody/tr[2]/td[2]"),
                                Paramedics_1 = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[3]/table/tbody/tr[3]/td[2]"),
                                Paramedics_2 = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[3]/table/tbody/tr[4]/td[2]"),
                                Other = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[3]/table/tbody/tr[5]/td[2]"),
                            },

                        },
                        Total_by_type = new TotalByTypeOfTest()
                        {
                            Pcr = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[1]/table[3]/tbody/tr[1]/td[2]"),
                            Antigen = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[1]/table[3]/tbody/tr[2]/td[2]")
                        },
                        Last_by_type = new LastByTypeOfTest()
                        {
                            Pcr = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[1]/table[3]/tbody/tr[1]/td[3]"),
                            Antigen = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[1]/table[3]/tbody/tr[2]/td[3]")
                        }
                    },
                    Active = new ActiveCountry()
                    {
                        Current = ValidateNumber(doc, "/html/body/main/div[1]/div/div[5]/div[2]/p[3]"),
                        Current_by_type = new CurrentByType()
                        {
                            Hospitalized = ValidateNumber(doc, "/html/body/main/div[1]/div/div[5]/div[4]/p[1]"),
                            Icu = ValidateNumber(doc, "/html/body/main/div[1]/div/div[5]/div[4]/p[3]")
                        }
                    },
                    Recovered = new RecoveredCountry()
                    {
                        Total = ValidateNumber(doc, "/html/body/main/div[1]/div/div[5]/div[3]/p[1]"),
                        Last = ValidateNumber(doc, "/html/body/main/div[1]/div/div[5]/div[3]/p[3]")
                    },
                    Deceased = new DeceasedCountry()
                    {
                        Total = ValidateNumber(doc, "/html/body/main/div[1]/div/div[5]/div[5]/p[1]"),
                        Last = ValidateNumber(doc, "/html/body/main/div[1]/div/div[5]/div[5]/p[3]")
                    },
                    Vaccinated = new VaccinatedCountry()
                    {
                        Total = ValidateNumber(doc, "/html/body/main/div[1]/div/div[5]/div[6]/p[1]"),
                        Total_completed = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[4]/table/tbody/tr[29]/td[7]"),
                        Last = ValidateNumber(doc, "/html/body/main/div[1]/div/div[5]/div[6]/p[3]"),
                        Last_by_type = new LastByTypeOfVaccine()
                        {
                            Astrazeneca = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[4]/table/tbody/tr[29]/td[5]"),
                            Janssen = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[4]/table/tbody/tr[29]/td[6]"),
                            Moderna = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[4]/table/tbody/tr[29]/td[4]"),
                            Pfeizer = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[4]/table/tbody/tr[29]/td[3]")
                        }
                    },
                },
            };
            dataModel.Regions = new List<Region>();
            string[] nameByEKATTE = Enum.GetNames(typeof(NameByEkatte));
            for (int i = 1; i <= 28; i++)
            {
                dataModel.Regions.Add(new Region()
                {
                    Name = doc2.DocumentNode.SelectNodes("/html/body/main/div[3]/div/div[1]/div[2]/table/tbody/tr[" + i + "]/td[1]").FirstOrDefault().InnerText,
                    NameByЕКАТТЕ = nameByEKATTE[i - 1],
                    RegisonData = new RegionData()
                    {
                        Confirmed = new ConfirmedRegion()
                        {
                            Total = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[2]/table/tbody/tr[" + i + "]/td[2]"),
                            Last = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[2]/table/tbody/tr[" + i + "]/td[3]")
                        },
                        Vaccinated = new VaccinatedRegion()
                        {
                            Total = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[4]/table/tbody/tr[" + i + "]/td[2]"),
                            Total_completed = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[4]/table/tbody/tr[" + i + "]/td[7]"),
                            Last_by_type = new LastByTypeOfVaccine()
                            {
                                Pfeizer = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[4]/table/tbody/tr[" + i + "]/td[3]"),
                                Moderna = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[4]/table/tbody/tr[" + i + "]/td[4]"),
                                Astrazeneca = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[4]/table/tbody/tr[" + i + "]/td[5]"),
                                Janssen = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[4]/table/tbody/tr[" + i + "]/td[6]")
                            },
                            Last = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[4]/table/tbody/tr[" + i + "]/td[3]")
                            + ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[4]/table/tbody/tr[" + i + "]/td[4]")
                            + ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[4]/table/tbody/tr[" + i + "]/td[5]")
                            + ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[4]/table/tbody/tr[" + i + "]/td[6]")
                        }
                    }
                });
            }
            dataModel.Stats = new Stats()
            {
                Tested = new TestedStats()
                {
                    Total_by_type_prc = new TotalByTypeOfTestStats()
                    {
                        Pcr = Math.Round((double)dataModel.Overall.Tested.Total_by_type.Pcr / dataModel.Overall.Tested.Total * 100, 2),
                        Antigen = Math.Round((double)dataModel.Overall.Tested.Total_by_type.Antigen / dataModel.Overall.Tested.Total * 100, 2)
                    },
                    Last_by_type_prc = new LastByTypeOfTestStats()
                    {
                        Pcr = Math.Round((double)dataModel.Overall.Tested.Last_by_type.Pcr / dataModel.Overall.Tested.Last * 100, 2),
                        Antigen = Math.Round((double)dataModel.Overall.Tested.Last_by_type.Antigen / dataModel.Overall.Tested.Last * 100, 2)
                    }
                },
                Confirmed = new ConfirmedStats()
                {
                    Total_per_tested_prc = Math.Round((double)dataModel.Overall.Confirmed.Total / dataModel.Overall.Tested.Total * 100, 2),
                    Last_per_tested_prc = Math.Round((double)dataModel.Overall.Confirmed.Last / dataModel.Overall.Tested.Last * 100, 2),
                    Total_by_type_prc = new TotalByTypeOfTestStats()
                    {
                        Pcr = Math.Round((double)dataModel.Overall.Confirmed.Total / dataModel.Overall.Tested.Total_by_type.Pcr * 100, 2),
                        Antigen = Math.Round((double)dataModel.Overall.Confirmed.Total / dataModel.Overall.Tested.Total_by_type.Antigen * 100, 2)
                    },
                    Last_by_type_prc = new LastByTypeOfTestStats()
                    {
                        Pcr = Math.Round((double)dataModel.Overall.Confirmed.Last / dataModel.Overall.Tested.Last_by_type.Pcr * 100, 2),
                        Antigen = Math.Round((double)dataModel.Overall.Confirmed.Last / dataModel.Overall.Tested.Last_by_type.Antigen * 100, 2)
                    },
                    Medical_prc = Math.Round((double)dataModel.Overall.Confirmed.Medical.Total / dataModel.Overall.Confirmed.Total * 100, 2)
                },
                Active = new ActiveStats()
                {
                    Hospitalized_per_active = Math.Round((double)dataModel.Overall.Active.Current_by_type.Hospitalized / dataModel.Overall.Active.Current * 100, 2),
                    Icu_per_hospitalized = Math.Round((double)dataModel.Overall.Active.Current_by_type.Icu / dataModel.Overall.Active.Current * 100, 2)
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
            var testedLast = rootobject.Overall.Tested.Last;
            var lastTypesSum = rootobject.Overall.Tested.Last_by_type.Pcr + rootobject.Overall.Tested.Last_by_type.Antigen;
            if (testedLast != lastTypesSum)
            {
                return false;
            }
            var totatlConfirmedLast = rootobject.Overall.Confirmed.Last;
            var lastConfirmedTypesSum = rootobject.Overall.Confirmed.Last_by_type.Antigen + rootobject.Overall.Confirmed.Last_by_type.Pcr;
            if (totatlConfirmedLast != lastConfirmedTypesSum)
            {
                return false;
            }
            var RegionsLastSum = 0;
            foreach (var item in rootobject.Regions)
            {
                RegionsLastSum += item.RegisonData.Confirmed.Last;
            }
            if (totatlConfirmedLast != RegionsLastSum)
            {
                return false;
            }
            var vacinatedLast = rootobject.Overall.Vaccinated.Last;
            var lastVacinatedByTypesSum = rootobject.Overall.Vaccinated.Last_by_type.Astrazeneca
                + rootobject.Overall.Vaccinated.Last_by_type.Janssen
                + rootobject.Overall.Vaccinated.Last_by_type.Moderna
                + rootobject.Overall.Vaccinated.Last_by_type.Pfeizer;
            if (vacinatedLast != lastVacinatedByTypesSum)
            {
                return false;
            }
            return true;
        }
    }
}
