using Coronavirus_Web_Scaper.Controllers.Mapping;
using Coronavirus_Web_Scaper.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Diagnostics;
using System.Linq;

namespace Coronavirus_Web_Scaper.Controllers
{
	public class HomeController : Controller
	{
		IMongoService Mongo { get; }

		public HomeController(IMongoService mongo)
		{
			Mongo = mongo;
		}

		/// <summary>
		/// This is a method for my index page in the site.
		/// Here I get data from the database and put it in the view.
		/// </summary>
		/// <returns>I return the View that refers to tha name of the method</returns>
		public IActionResult Index()
		{
			var data = Mongo.GetRootobject();
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
