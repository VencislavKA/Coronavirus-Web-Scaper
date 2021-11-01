using CoronavirusWebScaper.Controllers.Mapping;
using CoronavirusWebScaper.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CoronavirusWebScaper.Controllers
{
	[Route("api/")]
	[ApiController]
	public class ApiController : ControllerBase
	{
		private IMongoService MongoService { get; }

		public ApiController(IMongoService mongoService)
		{
			MongoService = mongoService;
		}

		[HttpGet("data/all")]
		public ActionResult<CoronaData> GetAll()
		{
			return MongoService.GetFirstRecordFromMongoCollection();
		}

		[HttpGet("data/regions")]
		public ActionResult<IEnumerable<CasesByRegions>> GetRegions()
		{
			return MongoService.GetFirstRecordFromMongoCollection().Regions;
		}
	}
}
