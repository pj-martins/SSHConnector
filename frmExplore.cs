using PaJaMa.WinControls;
using SSHConnector.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SSHConnector
{
    public partial class frmExplore : Form
    {
        public Terminal Terminal { get; set; }
        public event EventHandler SettingsChanged;
        private object _lock = new object();
        public frmExplore()
        {
            InitializeComponent();
        }

        private void frmExplore_Load(object sender, EventArgs e)
        {
            FormSettings.LoadSettings(this);
            doRefresh();
        }

        private void frmExplore_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormSettings.SaveSettings(this);
        }

        private void doRefresh()
        {
            refreshSSHFiles(null, treeMain.Nodes);
        }

        private void refreshSSHFiles(string parentPath, TreeNodeCollection nodes)
        {
            var curr = lblFullPath.Text;
            lblFullPath.Text = "Loading...";
            var lines = runCommand($"cd {(string.IsNullOrEmpty(parentPath) ? "/" : parentPath)} && ls -l -a -F");
            foreach (var line in lines)
            {
                var parts = line.Split(' ').Where(x => !string.IsNullOrEmpty(x.Trim())).ToList();
                if (parts.Count >= 9)
                {
                    var lastParts = parts.Skip(8).ToList();
                    if (lastParts[0] == "./" || lastParts[0] == "../") continue;
                    var ind = lastParts.IndexOf("->");
                    if (ind > 0)
                    {
                        lastParts = lastParts.Take(ind).ToList();
                        lastParts.Add("/");
                    }

                    var dt = parts.Skip(parts.Count - 4).Take(3);
                    var sub = string.Join(" ", lastParts).Trim();
                    TreeNode childNode;
                    if (sub.EndsWith("/"))
                    {
                        if (sub.EndsWith("/")) sub = sub.Substring(0, sub.Length - 1).Trim();
                        childNode = nodes.Add(sub);
                        childNode.Nodes.Add("__");
                    }
                    else
                    {
                        if (sub.EndsWith("*")) sub = sub.Substring(0, sub.Length - 1).Trim();
                        childNode = nodes.Add(sub);
                        
                    }

                    childNode.Tag = new SSHFileDirectory() { Path = $"{parentPath}/{sub}", ModifiedDate = string.Join(" ", dt) };
                }
            }
            lblFullPath.Text = curr;
        }

        private List<string> runCommand(string command)
        {
            var lines = new List<string>();
            if (!File.Exists("ssh.exe"))
            {
                File.WriteAllBytes("ssh.exe", Resources.ssh);
            }
            var args = $"{Terminal.Host} -t \"{command}\"";
            var inf = new ProcessStartInfo("ssh", args);
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
                if (e.Data != null && !e.Data.Contains("Connection "))
                {
                    lock (_lock)
                    {
                        lines.Add(e.Data);
                    }
                }
            });
            p.Start();
            p.BeginOutputReadLine();
            p.BeginErrorReadLine();
            p.WaitForExit();
            return lines;
        }

        private void treeMain_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Nodes.Count == 1 && e.Node.Nodes[0].Text == "__")
            {
                e.Node.Nodes.Clear();
                refreshSSHFiles((e.Node.Tag as SSHFileDirectory).Path, e.Node.Nodes);
            }
        }

        private void treeMain_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node == null) return;
            lblFullPath.Text = e.Node.Tag.ToString();
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            var folder = new FolderBrowserDialog();
            if (!string.IsNullOrEmpty(Terminal.LastDownloaded))
            {
                folder.SelectedPath = Terminal.LastDownloaded;
            }
            if (folder.ShowDialog() == DialogResult.OK)
            {
                Terminal.LastDownloaded = folder.SelectedPath;
                SettingsChanged?.Invoke(this, new EventArgs());
                BackgroundWorker worker = new BackgroundWorker();
                worker.WorkerSupportsCancellation = true;
                worker.DoWork += (object bwsender, DoWorkEventArgs bwe) =>
                {
                    // recursivelyDownloadFiles(treeMain.SelectedNodes.Select(n => n.Tag as SftpFile), folder.SelectedPath, worker, PromptResult.Yes);
                };
                PaJaMa.WinControls.WinProgressBox.ShowProgress(worker, "", this, true);

            }
        }

        //private void recursivelyDownloadFiles(IEnumerable<SftpFile> files, string parentDirectory, BackgroundWorker worker, PromptResult lastResult)
        //{
        //    foreach (var file in files)
        //    {
        //        if (worker.CancellationPending) return;
        //        worker.ReportProgress(0, $"Processing {file.FullName}");
        //        if (file.Name == "." || file.Name == "..") continue;
        //        if (file.IsDirectory)
        //        {
        //            recursivelyDownloadFile(file, parentDirectory, worker, lastResult);
        //        }
        //        else
        //        {
        //            var path = Path.Combine(parentDirectory, file.Name);
        //            if (File.Exists(path))
        //            {
        //                if (lastResult == PromptResult.No || lastResult == PromptResult.Yes)
        //                {
        //                    lastResult = ScrollableMessageBox.ShowDialog($"{path} exists, overwrite?", "File Exists",
        //                        ScrollableMessageBoxButtons.Yes,
        //                        ScrollableMessageBoxButtons.YesToAll,
        //                        ScrollableMessageBoxButtons.No,
        //                        ScrollableMessageBoxButtons.NoToAll
        //                        );
        //                }
        //            }
        //            if (lastResult == PromptResult.No || lastResult == PromptResult.NoToAll)
        //            {
        //                continue;
        //            }
        //            var stream = new FileStream(Path.Combine(parentDirectory, file.Name), FileMode.Create);
        //            var res = _client.BeginDownloadFile(file.FullName, stream);
        //            while (!res.IsCompleted)
        //            {
        //                Thread.Sleep(100);
        //            }
        //            _client.EndDownloadFile(res);
        //        }
        //    }
        //}

        //private void recursivelyDownloadFile(SftpFile parent, string parentDirectory, BackgroundWorker worker, PromptResult lastResult)
        //{
        //    var parts = parent.FullName.Split('/');
        //    var fullDir = parentDirectory;
        //    fullDir = Path.Combine(fullDir, parts.Last());
        //    if (!Directory.Exists(fullDir)) Directory.CreateDirectory(fullDir);
        //    var childFiles = _client.ListDirectory(parent.FullName);
        //    recursivelyDownloadFiles(childFiles, fullDir, worker, lastResult);
        //}

        private void btnUpload_Click(object sender, EventArgs e)
        {
            //var dlg = new OpenFileDialog();
            //dlg.Multiselect = true;
            //if (!string.IsNullOrEmpty(Terminal.LastUploaded)) dlg.InitialDirectory = Terminal.LastUploaded;
            //if (dlg.ShowDialog() == DialogResult.OK)
            //{
            //    BackgroundWorker worker = new BackgroundWorker();
            //    worker.WorkerSupportsCancellation = true;
            //    worker.DoWork += (object bwsender, DoWorkEventArgs bwe) =>
            //    {
            //        foreach (var file in dlg.FileNames)
            //        {
            //            if (worker.CancellationPending) return;
            //            worker.ReportProgress(0, $"Processing {file}");
            //            var res = _client.BeginUploadFile(new FileStream(file, FileMode.Open), $"{lblFullPath.Text}/{Path.GetFileName(file)}");
            //            while (!res.IsCompleted)
            //            {
            //                Thread.Sleep(100);
            //            }
            //            _client.EndUploadFile(res);
            //        }
            //    };
            //    PaJaMa.WinControls.WinProgressBox.ShowProgress(worker, "", this, true);
            //}
            //doRefresh();
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeMain.SelectedNode == null) return;
            treeMain.SelectedNode.Nodes.Clear();
            refreshSSHFiles((treeMain.SelectedNode.Tag as SSHFileDirectory).Path, treeMain.SelectedNode.Nodes);
        }

        private void viewContentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeMain.SelectedNode == null) return;
            var file = (treeMain.SelectedNode.Tag as SSHFileDirectory).Path;
            var content = runCommand($"cat {file}");
            var tmpDir = Path.Combine(Path.GetTempPath(), "GitStudio");
            if (!Directory.Exists(tmpDir)) Directory.CreateDirectory(tmpDir);
            var tmpFile = Path.Combine(tmpDir, Guid.NewGuid() + ".tmp");
            File.WriteAllLines(tmpFile, content);
            Process.Start($"C:\\Program Files\\Notepad++\\notepad++.exe", tmpFile);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure?", "Delete", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
            foreach (var node in treeMain.SelectedNodes.ToList())
            {
                var file = (node.Tag as SSHFileDirectory).Path;
                runCommand($"rm {file}");
                treeMain.Nodes.Remove(node);
            }
        }
    }
}
