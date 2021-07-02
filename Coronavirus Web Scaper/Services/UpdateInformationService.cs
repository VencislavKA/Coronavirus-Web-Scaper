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

        private IMongoService MongoService { get; }

		public UpdateInformationService(IMongoService mongoService)
		{
			MongoService = mongoService;
		}

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
            MongoService.GetCollection().InsertOne(GetValidatedDataFromSite());
        }

        private static RootObject GetValidatedDataFromSite()
        {
            HtmlWeb web = new();
            var doc = web.Load("https://coronavirus.bg");
            var doc2 = web.Load("https://coronavirus.bg/bg/statistika");
            RootObject dataModel = new()
            {
                Date = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffffffzzz")),
                DateScraped = DateTime.Parse(DateTime.Now.ToString("u")),
                Country = "BG",
                Overall = new Overall()
                {
                    Tested = new TestedCountry()
                    {
                        Total = ValidateNumber(doc, "/html/body/main/div[1]/div/div[5]/div[1]/p[1]"),
                        Last = ValidateNumber(doc, "/html/body/main/div[1]/div/div[5]/div[1]/p[3]"),
                        TotalByType = new TotalByTypeOfTest()
                        {
                            Pcr = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[1]/table[2]/tbody/tr[2]/td[2]"),
                            Antigen = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[1]/table[2]/tbody/tr[1]/td[2]")
                        },
                        LastByType = new LastByTypeOfTest()
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
                            TotalByType = new TotalByTypeOfPossition()
                            {
                                Doctor = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[3]/table/tbody/tr[1]/td[2]"),
                                Nurces = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[3]/table/tbody/tr[2]/td[2]"),
                                Paramedics1 = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[3]/table/tbody/tr[3]/td[2]"),
                                Paramedics2 = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[3]/table/tbody/tr[4]/td[2]"),
                                Other = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[3]/table/tbody/tr[5]/td[2]"),
                            },

                        },
                        TotalByType = new TotalByTypeOfTest()
                        {
                            Pcr = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[1]/table[3]/tbody/tr[1]/td[2]"),
                            Antigen = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[1]/table[3]/tbody/tr[2]/td[2]")
                        },
                        LastByType = new LastByTypeOfTest()
                        {
                            Pcr = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[1]/table[3]/tbody/tr[1]/td[3]"),
                            Antigen = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[1]/table[3]/tbody/tr[2]/td[3]")
                        }
                    },
                    Active = new ActiveCountry()
                    {
                        Current = ValidateNumber(doc, "/html/body/main/div[1]/div/div[5]/div[2]/p[3]"),
                        CurrentByType = new CurrentByType()
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
                        TotalCompleted = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[4]/table/tbody/tr[29]/td[7]"),
                        Last = ValidateNumber(doc, "/html/body/main/div[1]/div/div[5]/div[6]/p[3]"),
                        LastByType = new LastByTypeOfVaccine()
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
                    NameByЕkatte = nameByEKATTE[i - 1],
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
                            TotalCompleted = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[4]/table/tbody/tr[" + i + "]/td[7]"),
                            LastByType = new LastByTypeOfVaccine()
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
                    TotalByTypePrc = new TotalByTypeOfTestStats()
                    {
                        Pcr = Math.Round((double)dataModel.Overall.Tested.TotalByType.Pcr / dataModel.Overall.Tested.Total * 100, 2),
                        Antigen = Math.Round((double)dataModel.Overall.Tested.TotalByType.Antigen / dataModel.Overall.Tested.Total * 100, 2)
                    },
                    LastByTypePrc = new LastByTypeOfTestStats()
                    {
                        Pcr = Math.Round((double)dataModel.Overall.Tested.LastByType.Pcr / dataModel.Overall.Tested.Last * 100, 2),
                        Antigen = Math.Round((double)dataModel.Overall.Tested.LastByType.Antigen / dataModel.Overall.Tested.Last * 100, 2)
                    }
                },
                Confirmed = new ConfirmedStats()
                {
                    TotalPerTestedPrc = Math.Round((double)dataModel.Overall.Confirmed.Total / dataModel.Overall.Tested.Total * 100, 2),
                    LastPerTestedPrc = Math.Round((double)dataModel.Overall.Confirmed.Last / dataModel.Overall.Tested.Last * 100, 2),
                    TotalByTypePrc = new TotalByTypeOfTestStats()
                    {
                        Pcr = Math.Round((double)dataModel.Overall.Confirmed.Total / dataModel.Overall.Tested.TotalByType.Pcr * 100, 2),
                        Antigen = Math.Round((double)dataModel.Overall.Confirmed.Total / dataModel.Overall.Tested.TotalByType.Antigen * 100, 2)
                    },
                    LastByTypePrc = new LastByTypeOfTestStats()
                    {
                        Pcr = Math.Round((double)dataModel.Overall.Confirmed.Last / dataModel.Overall.Tested.LastByType.Pcr * 100, 2),
                        Antigen = Math.Round((double)dataModel.Overall.Confirmed.Last / dataModel.Overall.Tested.LastByType.Antigen * 100, 2)
                    },
                    MedicalPrc = Math.Round((double)dataModel.Overall.Confirmed.Medical.Total / dataModel.Overall.Confirmed.Total * 100, 2)
                },
                Active = new ActiveStats()
                {
                    HospitalizedPerActive = Math.Round((double)dataModel.Overall.Active.CurrentByType.Hospitalized / dataModel.Overall.Active.Current * 100, 2),
                    IcuPerHospitalized = Math.Round((double)dataModel.Overall.Active.CurrentByType.Icu / dataModel.Overall.Active.Current * 100, 2)
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
			_ = int.TryParse(doc.DocumentNode.SelectNodes(pathern).FirstOrDefault().InnerText.Replace(" ", string.Empty), out int parsed);
			return parsed;
        }

        private static bool ValidateModelData(RootObject rootobject)
        {
            var testedLast = rootobject.Overall.Tested.Last;
            var lastTypesSum = rootobject.Overall.Tested.LastByType.Pcr + rootobject.Overall.Tested.LastByType.Antigen;
            if (testedLast != lastTypesSum)
            {
                return false;
            }
            var totatlConfirmedLast = rootobject.Overall.Confirmed.Last;
            var lastConfirmedTypesSum = rootobject.Overall.Confirmed.LastByType.Antigen + rootobject.Overall.Confirmed.LastByType.Pcr;
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
            var lastVacinatedByTypesSum = rootobject.Overall.Vaccinated.LastByType.Astrazeneca
                + rootobject.Overall.Vaccinated.LastByType.Janssen
                + rootobject.Overall.Vaccinated.LastByType.Moderna
                + rootobject.Overall.Vaccinated.LastByType.Pfeizer;
            if (vacinatedLast != lastVacinatedByTypesSum)
            {
                return false;
            }
            return true;
        }
    }
}
