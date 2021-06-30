using Coronavirus_Web_Scaper.Controllers.Mapping;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Diagnostics;
using System.Linq;

namespace Coronavirus_Web_Scaper.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> logger;

		private readonly IMongoCollection<Rootobject> dbcollection;

		public HomeController(ILogger<HomeController> logger)
		{
			dbcollection = Mongo.GetCollection();
			this.logger = logger;
		}

		/// <summary>
		/// This is a method for my index page in the site.
		/// Here I get data from the database and put it in the view.
		/// </summary>
		/// <returns>I return the View that refers to tha name of the method</returns>
		public IActionResult Index()
		{
			var col = Mongo.GetCollection();
			var data = Mongo.GetRootobject(col);
			if (data == null)
			{
				return this.Error();
			}
			return this.View(data);
		}

		/// <summary>
		/// In this method I get the region from the database by its id and send it to the view
		/// </summary>
		/// <param name="id">This id refers to the id of every region in my database.</param>
		/// <returns>Returns a view with that data for it in the format of class Region</returns>
		public IActionResult RegionData(string name)
		{
			var rootobject = Mongo.GetRootobject(dbcollection);
			var region = rootobject.Regions.Where(x => x.NameByЕКАТТЕ == name).FirstOrDefault();
			return this.View(region);
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
