using Coronavirus_Web_Scaper.Controllers.Mapping;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coronavirus_Web_Scaper.Controllers
{
    [Route("api/")]
	[ApiController]
	public class ApiController : ControllerBase
	{
        private IMongoCollection<Rootobject> dbcollection = Mongo.GetCollection();

        [HttpGet("data/all")]
        public ActionResult<Rootobject> GetAll()
        {
            return Mongo.GetRootobject(this.dbcollection);
        }

        [HttpGet("data/regions")]
        public ActionResult<IEnumerable<Region>> GetRegions()
        {
            return Mongo.GetRootobject(this.dbcollection).Regions;
        }
        
    }
}
