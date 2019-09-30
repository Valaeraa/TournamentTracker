using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrackerLibrary;
using TrackerLibrary.Models;
using TrackerMVCUI.Models;

namespace TrackerMVCUI.Controllers
{
    public class TournamentsController : Controller
    {
        // GET: Tournaments
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Details(int id, int roundId = 0)
        {
            List<TournamentModel> tournaments = GlobalConfig.Connection.GetTournament_All();

            try
            {
                var input = new TournamentMVCDetailsModel();
                TournamentModel t = tournaments.Where(x => x.Id == id).First();

                input.TournamentName = t.TournamentName;
                var orderedRounds = t.Rounds.OrderBy(x => x.First().MatchupRound).ToList();
                var activeFound = false;

                for (int i = 0; i < orderedRounds.Count; i++)
                {
                    RoundStatus status = RoundStatus.Locked;

                    if (!activeFound)
                    {
                        if (orderedRounds[i].TrueForAll(x => x.Winner != null))
                        {
                            status = RoundStatus.Complete;
                        }
                        else
                        {
                            status = RoundStatus.Active;
                            activeFound = true;
                        } 
                    }

                    input.Rounds.Add(
                        new RoundMVCModel
                        {
                            RoundName = "Round " + (i + 1),
                            Status = status,
                            RoundNumber = i + 1
                        });
                }

                return View(input);
            }
            catch
            {

                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Tournaments/Create
        public ActionResult Create()
        {
            var input = new TournamentMVCCreateModel();

            List<TeamModel> allTeams = GlobalConfig.Connection.GetTeam_All();
            List<PrizeModel> allPrizes = GlobalConfig.Connection.GetPrizes_All();

            input.EnteredTeams = allTeams.Select(x => new SelectListItem { Text = x.TeamName, Value = x.Id.ToString() }).ToList();
            input.Prizes = allPrizes.Select(x => new SelectListItem { Text = x.PlaceName, Value = x.Id.ToString() }).ToList();

            return View(input);
        }

        // POST: Tournaments/Create
        [ValidateAntiForgeryToken()]
        [HttpPost]
        public ActionResult Create(TournamentMVCCreateModel model)
        {
            try
            {
                if (ModelState.IsValid && model.SelectedEnteredTeams.Count > 0)
                {
                    List<PrizeModel> allPrizes = GlobalConfig.Connection.GetPrizes_All();
                    List<TeamModel> allTeams = GlobalConfig.Connection.GetTeam_All();

                    TournamentModel t = new TournamentModel();
                    t.TournamentName = model.TournamentName;
                    t.EntryFee = model.EntryFee;
                    //t.EnteredTeams = model.SelectedEnteredTeams.Select(x => allTeams.Where(y => y.Id == int.Parse(x)).First()).ToList();
                    //t.Prizes = model.SelectedPrizes.Select(x => allPrizes.Where(y => y.Id == int.Parse(x)).First()).ToList();
                    t.EnteredTeams = model.SelectedEnteredTeams.Select(x => new TeamModel { Id = int.Parse(x) }).ToList();
                    t.Prizes = model.SelectedPrizes.Select(x => new PrizeModel { Id = int.Parse(x) }).ToList();

                    // Wire our matchups
                    TournamentLogic.CreateRounds(t);

                    GlobalConfig.Connection.CreateTournament(t);

                    t.AlertUsersToNewRound();

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("Create");
                }
            }
            catch (Exception)
            {
                return View();
            }
        }
    }
}