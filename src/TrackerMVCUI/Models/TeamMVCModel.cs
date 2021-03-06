﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TrackerMVCUI.Models
{
    public class TeamMVCModel
    {
        [Display(Name = "Team Name")]
        [StringLength(50, MinimumLength = 2)]
        [Required]
        public string TeamName { get; set; }

        [Display(Name = "Team Members")]
        public List<SelectListItem> TeamMembers { get; set; } = new List<SelectListItem>();

        public List<string> SelectedTeamMembers { get; set; } = new List<string>();
    }
}