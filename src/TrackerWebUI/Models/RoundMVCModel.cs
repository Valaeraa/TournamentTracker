using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrackerWebUI.Helpers;

namespace TrackerWebUI.Models
{
    public class RoundMVCModel
    {
        public int RoundNumber { get; set; }
        public string RoundName { get; set; }
        public RoundStatus Status { get; set; }
    }
}