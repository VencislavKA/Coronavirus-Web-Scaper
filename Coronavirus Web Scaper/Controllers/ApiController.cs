using Coronavirus_Web_Scaper.Controllers.Mapping;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Coronavirus_Web_Scaper.Services;

namespace Coronavirus_Web_Scaper.Controllers
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
        public ActionResult<RootObject> GetAll()
        {
            return MongoService.GetRootobject();
        }

        [HttpGet("data/regions")]
        public ActionResult<IEnumerable<Region>> GetRegions()
        {
            return MongoService.GetRootobject().Regions;
        }
    }
}
