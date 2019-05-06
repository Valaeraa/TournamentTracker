using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrackerLibrary;
using TrackerLibrary.Models;

namespace TrackerMVCUI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            List<TournamentModel> tournaments = GlobalConfig.Connection.GetTournament_All();

            return View(tournaments);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}