using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrackerLibrary.Models;
using TrackerLibrary;
using TrackerWebUI.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace TrackerWebUI.Controllers
{
    public class TeamsController : Controller
    {
        private readonly ILogger<TeamsController> _logger;

        public TeamsController(ILogger<TeamsController> logger)
        {
            _logger = logger;
        }

        // GET: TeamsController
        public ActionResult Index()
        {
            List<TeamModel> allTeams = GlobalConfig.Connection.GetTeam_All();

            return View(allTeams);
        }

        // GET: TeamsController/Create
        public ActionResult Create()
        {
            List<PersonModel> people = GlobalConfig.Connection.GetPerson_All();

            TeamMVCModel input = new TeamMVCModel
            {
                TeamMembers = people.Select(x => new SelectListItem { Text = x.FullName, Value = x.Id.ToString() }).ToList()
            };

            return View(input);
        }

        // POST: TeamsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TeamMVCModel model)
        {
            try
            {
                if (ModelState.IsValid && model.SelectedTeamMembers.Count > 0)
                {
                    var t = new TeamModel()
                    {
                        TeamName = model.TeamName,
                        TeamMembers = model.SelectedTeamMembers.Select(x => new PersonModel { Id = int.Parse(x) }).ToList()
                    };

                    GlobalConfig.Connection.CreateTeam(t);

                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Create");
                }
            }
            catch
            {
                return View();
            }
        }
    }
}
