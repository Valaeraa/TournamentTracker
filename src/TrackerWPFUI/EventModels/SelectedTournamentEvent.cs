using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;

namespace TrackerWPFUI.EventModels
{
    public class SelectedTournamentEvent
    {
        public TournamentModel SelectedTournament { get; set; }

        public SelectedTournamentEvent(TournamentModel tournament)
        {
            SelectedTournament = tournament;
        }
    }
}
