using BugTracker_1._1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker_1._1.Controllers
{
    public class ChartsController : Controller
    {
        // GET: Charts
        private ApplicationDbContext db = new ApplicationDbContext();
        public JsonResult PopulateChart()
        {
            var chartData = new ChartData();

            foreach (var project in db.Projects.ToList())
            {
                chartData.Labels.Add(project.Name);
                chartData.Data.Add(db.Tickets.Where(t => t.ProjectId == project.Id).Count());
            }            


            return Json(chartData);
        }
    }
}