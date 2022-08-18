using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSHConnector
{
    public class Terminal
    {
        public static string PWD = "S5H_PASSWORD";
        public string Name { get; set; }
        public string Host { get; set; }
        public string User { get; set; }
        public string PasswordEncrypted { get; set; }
        public int Port { get; set; }
        public string Key { get; set; }
        public int TunnelPort { get; set; }
        public string TunnelDestination { get; set; }
        public bool TunnelSSH { get; set; }
        public string LastDownloaded { get; set; }
        public string LastUploaded { get; set; }
        public string LastSSHd { get; set; }

        public Terminal()
        {
            Port = 22;
        }
    }

    public class TerminalAddedEventArgs : EventArgs
    {
        public Terminal Terminal { get; private set; }
        public TerminalAddedEventArgs(Terminal terminal)
        {
            Terminal = terminal;
        }
    }
}
