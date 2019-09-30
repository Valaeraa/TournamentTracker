using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TrackerMVCUI.Models
{
    public class TournamentMVCCreateModel
    {
        [Display(Name = "Tournament Name")]
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string TournamentName { get; set; }

        [Display(Name = "Entry Fee")]
        [Required]
        [DataType(DataType.Currency)]
        public decimal EntryFee { get; set; }

        [Display(Name = "Entered Teams")]
        public List<SelectListItem> EnteredTeams { get; set; } = new List<SelectListItem>();

        public List<string> SelectedEnteredTeams { get; set; } = new List<string>();

        [Display(Name = "Prizes")]
        public List<SelectListItem> Prizes { get; set; } = new List<SelectListItem>();

        public List<string> SelectedPrizes { get; set; } = new List<string>();
    }
}