using Coronavirus_Web_Scaper.Controllers.Mapping;
using Coronavirus_Web_Scaper.Models;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace Coronavirus_Web_Scaper
{
    public class Program
    {
        private static IMongoCollection<Rootobject> dbcollection = Mongo.GetCollection();
        /// <summary>
        /// Creting timer to know wnen to update the information in the database.
        /// </summary>
        //static Timer timer = new Timer(86400000);
        static Timer timer = new Timer(10000);
        /// <summary>
        /// In the main method I check if there is a record in the database. If there is not I create it. 
        /// After that I start the timer with atached event on it to update the information in the database.
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
