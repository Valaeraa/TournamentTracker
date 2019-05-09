using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TrackerMVCUI.Models
{
    public class TournamentMVCModel
    {
        [Display(Name = "Tournament Name")]
        [StringLength(50, MinimumLength = 2)]
        [Required]
        public string TournamentName { get; set; }

        [Display(Name = "Entry Fee")]
        [DataType(DataType.Currency)]
        [Required]
        public decimal EntryFee { get; set; }

        [Display(Name = "Entered Teams")]
        public List<SelectListItem> EnteredTeams { get; set; } = new List<SelectListItem>();

        public List<string> SelectedEnteredTeams { get; set; } = new List<string>();

        [Display(Name = "Prizes")]
        [StringLength(50, MinimumLength = 2)]
        [Required]
        public List<SelectListItem> Prizes { get; set; } = new List<SelectListItem>();

        public List<string> SelectedPrizes { get; set; } = new List<string>();
    }
}