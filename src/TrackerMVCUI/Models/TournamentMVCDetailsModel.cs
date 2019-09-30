using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TrackerMVCUI.Models
{
    public class TournamentMVCDetailsModel
    {
        [Display(Name = "Tournament Name")]
        public string TournamentName { get; set; }

        public List<RoundMVCModel> Rounds { get; set; } = new List<RoundMVCModel>();

        public List<MatchupMVCModel> Matchups { get; set; }
    }
}