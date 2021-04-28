using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SSHConnector
{
    public partial class ucWorkspace : UserControl
    {
        public event EventHandler TerminalSettingsUpdated;
        public event EventHandler NameChanged;
        public Terminal Terminal { get; set; }
        public ucWorkspace()
        {
            InitializeComponent();
        }

        private void frmCreateUpdate_Load(object sender, EventArgs e)
        {
            txtName.Text = Terminal.Name;
            txtHost.Text = Terminal.Host;
            numPort.Value = Terminal.Port;
            txtKey.Text = Terminal.Key;
            numTunnelPort.Value = Terminal.TunnelPort;
            txtTunnelDestination.Text = Terminal.TunnelDestination;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtKey.Text = dlg.FileNames[0];
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Terminal.Name = txtName.Text;
            Terminal.Host = txtHost.Text;
            Terminal.Port = (int)numPort.Value;
            Terminal.Key = txtKey.Text;
            Terminal.TunnelPort = (int)numTunnelPort.Value;
            Terminal.TunnelDestination = txtTunnelDestination.Text;
            TerminalSettingsUpdated.Invoke(this, new EventArgs());
            NameChanged.Invoke(this, new EventArgs());

            string arguments = $"-ssh {Terminal.Host} -P {Terminal.Port}";
            if (!string.IsNullOrEmpty(Terminal.Key)) arguments += $" -i {Terminal.Key}";
            if (!string.IsNullOrEmpty(Terminal.TunnelDestination)) arguments += $" -L {Terminal.TunnelPort}:{Terminal.TunnelDestination}";
            Process.Start("C:\\Program Files\\PuTTY\\putty.exe", arguments);
        }
    }
}
