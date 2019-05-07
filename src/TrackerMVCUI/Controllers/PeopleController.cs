using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrackerLibrary;
using TrackerLibrary.Models;

namespace TrackerMVCUI.Controllers
{
    public class PeopleController : Controller
    {
        // GET: People
        public ActionResult Index()
        {
            List<PersonModel> availiblePeople = GlobalConfig.Connection.GetPerson_All();

            return View(availiblePeople);
        }

        // GET: People/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        // GET: People/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: People/Create
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Create(PersonModel p)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    GlobalConfig.Connection.CreatePerson(p);

                    return RedirectToAction("Index"); 
                }
                else
                {
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }
    }
}
