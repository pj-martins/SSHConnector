using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SSHConnector
{
    public partial class ucWorkspace : UserControl
    {
        public event EventHandler TerminalSettingsUpdated;
        public event EventHandler NameChanged;
        public event EventHandler<TerminalAddedEventArgs> TerminalAdded;
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
            chkLaunchSSH.Checked = Terminal.TunnelSSH;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtKey.Text = dlg.FileNames[0];
            }
        }

        private void saveSettings()
        {
            Terminal.Name = txtName.Text;
            Terminal.Host = txtHost.Text;
            Terminal.Port = (int)numPort.Value;
            Terminal.Key = txtKey.Text;
            Terminal.TunnelPort = (int)numTunnelPort.Value;
            Terminal.TunnelDestination = txtTunnelDestination.Text;
            Terminal.TunnelSSH = chkLaunchSSH.Checked;
            TerminalSettingsUpdated.Invoke(this, new EventArgs());
            NameChanged.Invoke(this, new EventArgs());
        }

        private void runSSH(Terminal terminal, bool captureError = false)
        {
            //string arguments = $"-ssh {Terminal.Host} -P {Terminal.Port}";
            //if (!string.IsNullOrEmpty(Terminal.Key)) arguments += $" -i {Terminal.Key}";
            //if (!string.IsNullOrEmpty(Terminal.TunnelDestination)) arguments += $" -L {Terminal.TunnelPort}:{Terminal.TunnelDestination}";
            //Process.Start("C:\\Program Files\\PuTTY\\putty.exe", arguments);

            // string arguments = "/K ssh.exe";
            string arguments = "";

            arguments += $" {terminal.Host} -p {terminal.Port}";
            if (!string.IsNullOrEmpty(terminal.Key)) arguments += $" -i {terminal.Key}";
            if (!string.IsNullOrEmpty(terminal.TunnelDestination))
            {
                arguments += $" -L {terminal.TunnelPort}:{terminal.TunnelDestination}";
            }
            // var startInf = new ProcessStartInfo("cmd.exe", arguments);
            var startInf = new ProcessStartInfo("ssh.exe", arguments.Trim());
            if (terminal.TunnelSSH || captureError)
            {
                startInf.UseShellExecute = false;
                startInf.RedirectStandardOutput = true;
                startInf.RedirectStandardError = true;
                startInf.CreateNoWindow = true;
            }
            var proc = Process.Start(startInf);
            if (captureError)
            {
                var error = proc.StandardError.ReadToEnd();
                this.Invoke(new Action(() => MessageBox.Show("ERROR:\r\n" + error)));
            }
            if (terminal.TunnelSSH)
            {
                DateTime current = DateTime.Now;
                proc.OutputDataReceived += delegate (object sender, DataReceivedEventArgs e)
                {
                    current = DateTime.Now;
                };
                proc.BeginOutputReadLine();
                while ((DateTime.Now - current).TotalSeconds < 0) continue;
                new Thread(new ThreadStart(() =>
                {
                    var parts = terminal.Host.Split('@');
                    var tunnel = new Terminal()
                    {
                        Host = $"{(parts.Length < 2 ? "" : $"{parts[0]}@")}127.0.0.1",
                        Key = terminal.Key,
                        Port = terminal.TunnelPort
                    };
                    runSSH(tunnel);
                })).Start();
            }
            proc.WaitForExit();
            if (!captureError && (proc.ExitTime - proc.StartTime).TotalSeconds < 2)
            {
                runSSH(terminal, true);
            }
            else
            {
                this.Invoke(new Action(() =>
                {
                    btnOpenShell.Text = "Connect";
                }));
            }
        }

        private void btnOpenShell_Click(object sender, EventArgs e)
        {
            btnOpenShell.Enabled = false;
            saveSettings();
            if (!File.Exists("ssh.exe")) File.WriteAllBytes("ssh.exe", Properties.Resources.ssh);
            new Thread(new ThreadStart(() =>
            {
                runSSH(Terminal);
            })).Start();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            var copy = new Terminal()
            {
                Host = Terminal.Host,
                Key = Terminal.Key,
                Name = $"{Terminal.Name} Copy",
                Port = Terminal.Port,
                TunnelDestination = Terminal.TunnelDestination,
                TunnelPort = Terminal.TunnelPort,
                TunnelSSH = Terminal.TunnelSSH
            };
            TerminalAdded?.Invoke(this, new TerminalAddedEventArgs(copy));
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            saveSettings();
            ucExplore1.Enabled = true;
            ucExplore1.Connect(Terminal);
        }

        private void Frm_SettingsChanged(object sender, EventArgs e)
        {
            TerminalSettingsUpdated?.Invoke(sender, e);
        }
    }
}
