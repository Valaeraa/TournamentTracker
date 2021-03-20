﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrackerLibrary.Models;
using TrackerLibrary;

namespace TrackerWebUI.Controllers
{
    public class PrizesController : Controller
    {
        // GET: PrizesController
        public ActionResult Index()
        {
            List<PrizeModel> allPrizes = GlobalConfig.Connection.GetPrizes_All();

            return View(allPrizes);
        }

        // GET: PrizesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PrizesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PrizeModel prize)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    GlobalConfig.Connection.CreatePrize(prize);

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