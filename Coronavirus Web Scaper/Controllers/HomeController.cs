using CoronavirusWebScaper.Controllers.Mapping;
using CoronavirusWebScaper.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Diagnostics;
using System.Linq;

namespace CoronavirusWebScaper.Controllers
{
	public class HomeController : Controller
	{
		IMongoService Mongo { get; }

		public HomeController(IMongoService mongo)
		{
			Mongo = mongo;
		}
		
		public IActionResult Index()
		{
			var data = Mongo.GetRootobject();
			if (data == null)
			{
				return this.Error();
			}
			return this.View(data);
		}
		
		public IActionResult RegionData(string name)
		{
			var rootobject = Mongo.GetRootobject();
			var region = rootobject.Regions.Where(x => x.NameByЕkatte == name).FirstOrDefault();
			return this.View(region);
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
