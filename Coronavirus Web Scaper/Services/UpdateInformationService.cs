using CoronavirusWebScaper.Controllers.Mapping;
using CoronavirusWebScaper.Models;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CoronavirusWebScaper.Services
{
	public class UpdateInformationService : IHostedService
	{
		private Timer timer;

		private readonly IConfiguration configuration;

		private IMongoService MongoService { get; }

		public UpdateInformationService(IMongoService mongoService, IConfiguration config)
		{
			configuration = config;
			MongoService = mongoService;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			int hoursToUpdateNext = int.Parse(configuration.GetSection("UpdateInformationSettings").GetSection("UpdateInformationTimer").Value);

			timer = new Timer(
			UpdateInformation,
			null,
			TimeSpan.Zero,
			TimeSpan.FromHours(hoursToUpdateNext)
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
			MongoService.GetAllRecords().InsertOne(GetValidatedDataFromSite());
		}

		private static CoronaData GetValidatedDataFromSite()
		{
			HtmlWeb web = new();
			var doc = web.Load("https://coronavirus.bg");
			var doc2 = web.Load("https://coronavirus.bg/bg/statistika");
			CoronaData dataModel = new()
			{
				Date = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffffffzzz")),
				DateScraped = DateTime.Parse(DateTime.Now.ToString("u")),
				Country = "BG",
				Overall = new OverallCases()
				{
					Tested = new TestedCountry()
					{
						TotalTested = ValidateNumber(doc, "/html/body/main/div[1]/div/div[5]/div[1]/p[1]"),
						LastTested = ValidateNumber(doc, "/html/body/main/div[1]/div/div[5]/div[1]/p[3]"),
						TotalByType = new TotalCasesByTypeOfTest()
						{
							Pcr = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[1]/table[2]/tbody/tr[1]/td[2]"),
							Antigen = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[1]/table[2]/tbody/tr[2]/td[2]")
						},
						LastByType = new()
						{
							CasesConfirmedByPcr = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[1]/table[2]/tbody/tr[1]/td[3]"),
							CasesConfirmedByAntigen = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[1]/table[2]/tbody/tr[2]/td[3]")
						}
					},
					Confirmed = new ConfirmedCasesInCountry()
					{
						TotalCases = ValidateNumber(doc, "/html/body/main/div[1]/div/div[5]/div[2]/p[1]"),
						LastCases = ValidateNumber(doc, "/html/body/main/div[1]/div/div[4]/div[1]/h1"),
						CasesInMedicine = new()
						{
							Total = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[3]/table/tbody/tr[6]/td[2]"),
							TotalByTypeOfPossition = new()
							{
								Doctor = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[3]/table/tbody/tr[1]/td[2]"),
								Nurces = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[3]/table/tbody/tr[2]/td[2]"),
								Paramedics1 = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[3]/table/tbody/tr[3]/td[2]"),
								Paramedics2 = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[3]/table/tbody/tr[4]/td[2]"),
								Other = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[3]/table/tbody/tr[5]/td[2]"),
							},

						},
						TotalCasesByType = new()
						{
							Pcr = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[1]/table[3]/tbody/tr[1]/td[2]"),
							Antigen = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[1]/table[3]/tbody/tr[2]/td[2]")
						},
						LastCasesByType = new()
						{
							Pcr = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[1]/table[3]/tbody/tr[1]/td[3]"),
							Antigen = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[1]/table[3]/tbody/tr[2]/td[3]")
						}
					},
					ActiveCases = new()
					{
						CurrentCases = ValidateNumber(doc, "/html/body/main/div[1]/div/div[5]/div[2]/p[3]"),
						CurrentCasesByType = new CurrentCasesByType()
						{
							HospitalizedCases = ValidateNumber(doc, "/html/body/main/div[1]/div/div[5]/div[4]/p[1]"),
							IntensiceCareCases = ValidateNumber(doc, "/html/body/main/div[1]/div/div[5]/div[4]/p[3]")
						}
					},
					Recovered = new RecoveredPatientCountry()
					{
						TotalRecovered = ValidateNumber(doc, "/html/body/main/div[1]/div/div[5]/div[3]/p[1]"),
						LastRecovered = ValidateNumber(doc, "/html/body/main/div[1]/div/div[5]/div[3]/p[3]")
					},
					Deceased = new DeceasedCasesInCountry()
					{
						TotalCases = ValidateNumber(doc, "/html/body/main/div[1]/div/div[5]/div[5]/p[1]"),
						LastCases = ValidateNumber(doc, "/html/body/main/div[1]/div/div[5]/div[5]/p[3]")
					},
					Vaccinated = new VaccinatedCountry()
					{
						TotalVaccinated = ValidateNumber(doc, "/html/body/main/div[1]/div/div[5]/div[6]/p[1]"),
						TotalVaccinesCompleted= ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[4]/table/tbody/tr[29]/td[7]"),
						LastVaccinated = ValidateNumber(doc, "/html/body/main/div[1]/div/div[5]/div[6]/p[3]"),
						LastVaccinatedByType = new LastVacinatedByTypeOfVaccine()
						{
							Astrazeneca = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[4]/table/tbody/tr[29]/td[5]"),
							Janssen = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[4]/table/tbody/tr[29]/td[6]"),
							Moderna = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[4]/table/tbody/tr[29]/td[4]"),
							Pfeizer = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[4]/table/tbody/tr[29]/td[3]")
						}
					},
				},
			};
			dataModel.Regions = new List<CasesByRegions>();
			string[] nameByEKATTE = Enum.GetNames(typeof(NameByEkatte));
			for (int i = 1; i <= 28; i++)
			{
				dataModel.Regions.Add(new CasesByRegions()
				{
					Name = doc2.DocumentNode.SelectNodes("/html/body/main/div[3]/div/div[1]/div[2]/table/tbody/tr[" + i + "]/td[1]").FirstOrDefault().InnerText,
					NameByЕkatte = nameByEKATTE[i - 1],
					RegisonData = new RegionCasesData()
					{
						Confirmed = new()
						{
							TotalCasesInRegion = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[2]/table/tbody/tr[" + i + "]/td[2]"),
							LastCasesInRegion = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[2]/table/tbody/tr[" + i + "]/td[3]")
						},
						Vaccinated = new VaccinatedRegion()
						{
							Total = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[4]/table/tbody/tr[" + i + "]/td[2]"),
							TotalCompleted = ValidateNumber(doc2, "/html/body/main/div[3]/div/div[1]/div[4]/table/tbody/tr[" + i + "]/td[7]"),
							LastByType = new LastVacinatedByTypeOfVaccine()
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
				Tested = new()
				{
					TotalCasesByTypeOfTestStats = new()
					{
						Pcr = Math.Round((double)dataModel.Overall.Tested.TotalByType.Pcr / dataModel.Overall.Tested.TotalTested * 100, 2),
						Antigen = Math.Round((double)dataModel.Overall.Tested.TotalByType.Antigen / dataModel.Overall.Tested.TotalTested * 100, 2)
					},
					LastCasesByTypeOfTestStats = new()
					{
						Pcr = Math.Round((double)dataModel.Overall.Tested.LastByType.CasesConfirmedByPcr / dataModel.Overall.Tested.TotalTested * 100, 2),
						Antigen = Math.Round((double)dataModel.Overall.Tested.LastByType.CasesConfirmedByAntigen / dataModel.Overall.Tested.LastTested * 100, 2)
					}
				},
				Confirmed = new ConfirmedCasesStats()
				{
					TotalCasesPerTestedPrc = Math.Round((double)dataModel.Overall.Confirmed.TotalCases / dataModel.Overall.Tested.TotalTested * 100, 2),
					LastCasesPerTestedPrc = Math.Round((double)dataModel.Overall.Confirmed.LastCases / dataModel.Overall.Tested.LastTested * 100, 2),
					TotalByTypeOfTestStatsPrc = new TotalCasesByTypeOfTestStats()
					{
						Pcr = Math.Round((double)dataModel.Overall.Confirmed.TotalCases / dataModel.Overall.Tested.TotalByType.Pcr * 100, 2),
						Antigen = Math.Round((double)dataModel.Overall.Confirmed.TotalCases / dataModel.Overall.Tested.TotalByType.Antigen * 100, 2)
					},
					LastByTypeOfTestStatsPrc = new LastCasesByTypeOfTestStats()
					{
						Pcr = Math.Round((double)dataModel.Overall.Confirmed.LastCases / dataModel.Overall.Tested.LastByType.CasesConfirmedByPcr * 100, 2),
						Antigen = Math.Round((double)dataModel.Overall.Confirmed.LastCases / dataModel.Overall.Tested.LastByType.CasesConfirmedByAntigen * 100, 2)
					},
					CasesInMedicinePrc = Math.Round((double)dataModel.Overall.Confirmed.CasesInMedicine.Total / dataModel.Overall.Confirmed.TotalCases * 100, 2)
				},
				Active = new ActiveCasesStats() { 
					HospitalizedPerActive = Math.Round((double)dataModel.Overall.ActiveCases.CurrentCasesByType.HospitalizedCases / dataModel.Overall.ActiveCases.CurrentCases * 100, 2), 
					IcuPerHospitalized = Math.Round((double)dataModel.Overall.ActiveCases.CurrentCasesByType.IntensiceCareCases / dataModel.Overall.ActiveCases.CurrentCases * 100, 2) 
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

		private static bool ValidateModelData(CoronaData coronaData)
		{
			var testedLast = coronaData.Overall.Tested.LastTested;
			var lastTypesSum = coronaData.Overall.Tested.LastByType.CasesConfirmedByPcr + coronaData.Overall.Tested.LastByType.CasesConfirmedByAntigen;
			if (testedLast != lastTypesSum)
			{
				return false;
			}
			var totatlConfirmedLast = coronaData.Overall.Confirmed.LastCases;
			var lastConfirmedTypesSum = coronaData.Overall.Confirmed.LastCasesByType.Antigen + coronaData.Overall.Confirmed.LastCasesByType.Pcr;
			if (totatlConfirmedLast != lastConfirmedTypesSum)
			{
				return false;
			}
			var RegionsLastSum = 0;
			foreach (var item in coronaData.Regions)
			{
				RegionsLastSum += item.RegisonData.Confirmed.LastCasesInRegion;
			}
			if (totatlConfirmedLast != RegionsLastSum)
			{
				return false;
			}
			var vacinatedLast = coronaData.Overall.Vaccinated.LastVaccinated;
			var lastVacinatedByTypesSum = coronaData.Overall.Vaccinated.LastVaccinatedByType.Astrazeneca
				+ coronaData.Overall.Vaccinated.LastVaccinatedByType.Janssen
				+ coronaData.Overall.Vaccinated.LastVaccinatedByType.Moderna
				+ coronaData.Overall.Vaccinated.LastVaccinatedByType.Pfeizer;
			if (vacinatedLast != lastVacinatedByTypesSum)
			{
				return false;
			}
			return true;
		}
	}
}
