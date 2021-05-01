using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TrackerLibrary;
using TrackerLibrary.Models;

namespace TrackerUI
{
    public partial class TournamentDashboardForm : Form
    {
        List<TournamentModel> tournaments = GlobalConfig.Connection.GetTournament_All();
        private readonly ILogger<TournamentDashboardForm> _logger;
        private readonly IServiceProvider _service;

        public TournamentDashboardForm(ILogger<TournamentDashboardForm> logger, IServiceProvider service)
        {
            InitializeComponent();

            WireUpLists();

            _logger = logger;
            _service = service;
        }

        private void WireUpLists()
        {
            loadExistingTournamentDropDown.DataSource = null;
            loadExistingTournamentDropDown.DataSource = tournaments;
            loadExistingTournamentDropDown.DisplayMember = "TournamentName";
        }

        private void createTournamentButton_Click(object sender, EventArgs e)
        {
            var form = _service.GetService<CreateTournamentForm>();
            form.Show();
        }

        private void loadTournamentButton_Click(object sender, EventArgs e)
        {
            TournamentModel tm = (TournamentModel)loadExistingTournamentDropDown.SelectedItem;
            var form = _service.GetService<TournamentViewerForm>();
            form.InitializeViewer(tm);
            form.Show();
        }
    }
}
