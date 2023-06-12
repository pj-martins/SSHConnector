using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSHConnector
{
    public class ProcessHelper
    {
        private object _lock = new object();
        private object _lockError = new object();
        public Tuple<List<string>, List<string>> Run(string executable, byte[] resource, string args)
        {
            var lines = new List<string>();
            var errorLines = new List<string>();
            if (!File.Exists(executable))
            {
                File.WriteAllBytes(executable, resource);
            }
            var inf = new ProcessStartInfo(executable, args);
            inf.UseShellExecute = false;
            inf.RedirectStandardOutput = true;
            inf.RedirectStandardError = true;
            inf.StandardOutputEncoding = Encoding.ASCII;
            inf.StandardErrorEncoding = Encoding.ASCII;
            inf.WindowStyle = ProcessWindowStyle.Hidden;
            inf.CreateNoWindow = true;
            var p = new Process();
            p.StartInfo = inf;
            p.OutputDataReceived += new DataReceivedEventHandler((sender, e) =>
            {
                if (e.Data != null && !e.Data.Contains("Connection ") && !e.Data.Contains("No such file or directory"))
                {
                    lock (_lock)
                    {
                        lines.Add(e.Data);
                    }
                }
            });
            p.ErrorDataReceived += new DataReceivedEventHandler((sender, e) =>
            {
                if (string.IsNullOrEmpty(e.Data)) return;
                lock (_lockError)
                {
                    errorLines.Add(e.Data);
                }
            });
            p.Start();
            p.BeginOutputReadLine();
            p.BeginErrorReadLine();
            p.WaitForExit();
            return new Tuple<List<string>, List<string>>(lines, errorLines);
        }   
    }
}
