using Coronavirus_Web_Scaper.Controllers.Mapping;
using Coronavirus_Web_Scaper.Models;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Timers;

namespace Coronavirus_Web_Scaper.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		private IMongoCollection<Rootobject> dbcollection = Mongo.GetCollection();

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		/// <summary>
		/// This is a method for my index page in the site.
		/// Here I get data from the database and put it in the view.
		/// </summary>
		/// <returns>I return the View that refers to tha name of the method</returns>
		public IActionResult Index()
		{
			var data = Mongo.GetRootobject(dbcollection);
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
		public IActionResult RegionData(int id)
		{
			var rootobject = Mongo.GetRootobject(dbcollection);
			var region = rootobject.regions.Where(x => x.id == id).FirstOrDefault();
			return this.View(region);
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
