using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PaJaMa.WinControls;
using PaJaMa.Common;
using System.Threading;

namespace SSHConnector
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            FormSettings.LoadSettings(this);
            var terminals = SettingsHelper.GetUserSettings<List<Terminal>>();
            if (terminals != null)
            {
                foreach (var terminal in terminals)
                {
                    addWorkspace(terminal, null);
                }
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormSettings.SaveSettings(this);
        }

        private ucWorkspace addWorkspace(Terminal terminal, PaJaMa.WinControls.TabControl.TabPage tabPage)
        {
            var uc = new ucWorkspace();
            bool add = false;
            if (tabPage == null)
            {
                tabPage = new PaJaMa.WinControls.TabControl.TabPage();
                add = true;
            }
            tabPage.Text = terminal.Name;
            uc.Dock = DockStyle.Fill;
            uc.Terminal = terminal;
            uc.TerminalSettingsUpdated += Uc_TerminalSettingsUpdated;
            uc.NameChanged += Uc_NameChanged;
            tabPage.Controls.Add(uc);
            if (add)
            {
                tabWorkspaces.TabPages.Add(tabPage);
                tabWorkspaces.SelectedTab = tabPage;
            }
            return uc;
        }

        private void Uc_NameChanged(object sender, EventArgs e)
        {
            var uc = sender as ucWorkspace;
            tabWorkspaces.SelectedTab.Text = uc.Terminal.Name;
        }

        private void Uc_TerminalSettingsUpdated(object sender, EventArgs e)
        {
            saveSettings();
        }

        private void tabWorkspaces_TabAdding(object sender, PaJaMa.WinControls.TabControl.TabEventArgs e)
        {
            addWorkspace(new Terminal() { Name = "Workspace " + tabWorkspaces.TabPages.Count + 1 }, e.TabPage);
        }

        private void saveSettings()
        {
            SettingsHelper.SaveUserSettings<List<Terminal>>(tabWorkspaces.TabPages.Select(p => (p.Controls[0] as ucWorkspace).Terminal).ToList());
        }

        private void tabWorkspaces_TabClosing(object sender, PaJaMa.WinControls.TabControl.TabEventArgs e)
        {
            saveSettings();
        }
    }
}
