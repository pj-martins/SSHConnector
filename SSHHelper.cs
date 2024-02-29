using SSHConnector.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SSHConnector
{
    public class SSHHelper
    {
        private Terminal _terminal;
        public SSHHelper(Terminal terminal)
        {
            _terminal = terminal;
        }

        public void PopulateChildren(SSHFileDirectory fileDirectory)
        {
            if (fileDirectory.Children == null && fileDirectory.IsDirectory)
            {
                fileDirectory.Children = GetFilesDirectories(fileDirectory.Path);
            }
        }

        public List<SSHFileDirectory> GetFilesDirectories(string parentPath)
        {
            var lst = new List<SSHFileDirectory>();
            var lines = RunCommand($"cd {(string.IsNullOrEmpty(parentPath) ? "/" : parentPath)} && ls -l -a -F");
            foreach (var line in lines)
            {
                var parts = line.Split(' ').Where(x => !string.IsNullOrEmpty(x.Trim())).ToList();
                if (parts.Count >= 9)
                {
                    SSHFileDirectory fileDir = new SSHFileDirectory();
                    var lastParts = parts.Skip(8).ToList();
                    if (lastParts[0] == "./" || lastParts[0] == "../") continue;
                    var ind = lastParts.IndexOf("->");
                    if (ind > 0)
                    {
                        fileDir.SymLink = string.Join(" ", lastParts.Skip(ind + 1)).Trim();
                        lastParts = lastParts.Take(ind).ToList();
                        lastParts.Add("/");
                    }

                    var dt = parts.Skip(parts.Count - 4).Take(3);
                    var sub = string.Join(" ", lastParts).Trim();
                    if (sub.EndsWith("/"))
                    {
                        sub = sub.Substring(0, sub.Length - 1).Trim();
                        fileDir.IsDirectory = true;
                    }
                    else
                    {
                        if (sub.EndsWith("*")) sub = sub.Substring(0, sub.Length - 1).Trim();
                    }

                    fileDir.ShortPath = sub;
                    fileDir.Path = $"{parentPath}/{sub}";
                    fileDir.ModifiedDate = string.Join(" ", dt);
                    lst.Add(fileDir);
                }
            }
            return lst;
        }

        public List<string> RunCommand(string command)
        {
            var args = $"{_terminal.Host} -t \"{command}\"";
            var res = new ProcessHelper().Run("ssh.exe", Resources.ssh, args);
            if (res.Item2.Any())
            {
                System.Windows.Forms.MessageBox.Show(String.Join("\n", res.Item2));
            }
            return res.Item1;
        }
    }

    public class SSHFileDirectory
    {
        public string Path { get; set; }
        public string ShortPath { get; set; }
        public bool IsDirectory { get; set; }
        public string SymLink { get; set; }
        public string ModifiedDate { get; set; }
        public List<SSHFileDirectory> Children { get; set; }

        public override string ToString()
        {
            return $"{Path}     {ModifiedDate}";
        }
    }

    public class SSHFileContentsSearchResults
    {
        public string Path { get; set; }
        public string Containing { get; set; }
        public override string ToString()
        {
            return $"{Path}:{Containing}";
        }
    }
}
