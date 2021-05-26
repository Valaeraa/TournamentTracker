using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;
using TrackerLibrary.DataAccess.TextHelpers;
using Microsoft.Extensions.Logging;

namespace TrackerLibrary.DataAccess
{
    // TODO - Rename TextConnection to match with file name
    public class TextConnection : IDataConnection
    {
        //private readonly ILogger<TextConnection> _logger;

        public TextConnection(/*ILogger<TextConnection> logger*/)
        {
            //_logger = logger;
        }

        public void CreatePerson(PersonModel model)
        {
            //_logger.LogInformation("Creating person: {person}.", model);

            List<PersonModel> people = GlobalConfig.PeopleFile.FullFilePath().LoadFile().ConvertToPersonModels();

            int currentID = 1;

            if (people.Count > 0)
            {
                currentID = people.OrderByDescending(x => x.Id).First().Id + 1;
            }

            model.Id = currentID;

            people.Add(model);

            people.SaveToPeopleFile();
        }
        
        public void CreatePrize(PrizeModel model)
        {
            //_logger.LogInformation("Creating prize: {prize}.", model);

            // Load the text file and convert the text to a List<PrizeModel>
            List<PrizeModel> prizes = GlobalConfig.PrizesFile.FullFilePath().LoadFile().ConvertToPrizeModels();

            // Find the max ID
            int currentID = 1;

            if (prizes.Count > 0)
            {
                currentID = prizes.OrderByDescending(x => x.Id).First().Id + 1;
            }

            model.Id = currentID;

            // Add the new record with the new ID (max + 1)
            prizes.Add(model);

            // Convert the prizes to a list<string>
            // Save the list<string> to the text file
            prizes.SaveToPrizeFile();
        }

        public List<PersonModel> GetPerson_All()
        {
            //_logger.LogInformation("Getting all people.");

            return GlobalConfig.PeopleFile.FullFilePath().LoadFile().ConvertToPersonModels();
        }

        public void CreateTeam(TeamModel model)
        {
            //_logger.LogInformation("Creating team: {team}.", model);

            List<TeamModel> teams = GlobalConfig.TeamFile.FullFilePath().LoadFile().ConvertToTeamModels();

            // Find the max ID
            int currentID = 1;

            if (teams.Count > 0)
            {
                currentID = teams.OrderByDescending(x => x.Id).First().Id + 1;
            }

            model.Id = currentID;

            teams.Add(model);

            teams.SaveToTeamFile();
        }

        public List<TeamModel> GetTeam_All()
        {
            //_logger.LogInformation("Getting all teams.");

            return GlobalConfig.TeamFile.FullFilePath().LoadFile().ConvertToTeamModels();
        }

        public List<PrizeModel> GetPrizes_All()
        {
            //_logger.LogInformation("Getting all prizes.");

            return GlobalConfig.PrizesFile.FullFilePath().LoadFile().ConvertToPrizeModels();
        }

        public void CreateTournament(TournamentModel model)
        {
            //_logger.LogInformation("Creating tournament: {tournament}.", model);

            List<TournamentModel> tournaments = GlobalConfig.TournamentFile
                .FullFilePath()
                .LoadFile()
                .ConvertToTournamentModels();

            int currentID = 1;

            if (tournaments.Count > 0)
            {
                currentID = tournaments.OrderByDescending(x => x.Id).First().Id + 1;
            }

            model.Id = currentID;

            model.SaveRoundsToFile();

            tournaments.Add(model);

            tournaments.SaveToTournamentFile();

            TournamentLogic.UpdateTournamentResults(model);
        }

        public List<TournamentModel> GetTournament_All()
        {
            //_logger.LogInformation("Getting all tournaments.");

            return GlobalConfig.TournamentFile.FullFilePath().LoadFile().ConvertToTournamentModels();
        }

        public void UpdateMatchup(MatchupModel model)
        {
            //_logger.LogInformation("Updating matchup: {matchup}.", model);

            model.UpdateMatchupToFile();
        }

        public void CompleteTournament(TournamentModel model)
        {
            //_logger.LogInformation("Completing tournament: {tournament}.", model);

            List<TournamentModel> tournaments = GlobalConfig.TournamentFile
                .FullFilePath()
                .LoadFile()
                .ConvertToTournamentModels();
            
            tournaments.Remove(model);

            tournaments.SaveToTournamentFile();

            TournamentLogic.UpdateTournamentResults(model);
        }
    }
}
