using PaJaMa.Common;
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
        private SSHHelper _sshHelper;
        public event EventHandler SettingsChanged;
        private object _lock = new object();
        public frmExplore()
        {
            InitializeComponent();
        }

        private void frmExplore_Load(object sender, EventArgs e)
        {
            _sshHelper = new SSHHelper(Terminal);
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

        private void refreshSSHFiles(SSHFileDirectory fileDir, TreeNodeCollection nodes)
        {
            var curr = lblFullPath.Text;
            lblFullPath.Text = "Loading...";
            List<SSHFileDirectory> filesDirs;
            if (fileDir == null)
            {
                filesDirs = _sshHelper.GetFilesDirectories(null);
            }
            else
            {
                _sshHelper.PopulateChildren(fileDir);
                filesDirs = fileDir.Children;
            }
            foreach (var fd in filesDirs)
            {
                TreeNode childNode = nodes.Add(fd.ShortPath);
                if (fd.IsDirectory)
                {
                    childNode.Nodes.Add("__");
                }

                childNode.Tag = fd;
            }
            lblFullPath.Text = curr;
        }



        private void treeMain_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Nodes.Count == 1 && e.Node.Nodes[0].Text == "__")
            {
                e.Node.Nodes.Clear();
                refreshSSHFiles(e.Node.Tag as SSHFileDirectory, e.Node.Nodes);
            }
        }

        private void treeMain_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node == null) return;
            lblFullPath.Text = e.Node.Tag.ToString();
        }

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
            refreshSSHFiles(treeMain.SelectedNode.Tag as SSHFileDirectory, treeMain.SelectedNode.Nodes);
        }

        private void viewContents(string path)
        {
            var content = _sshHelper.RunCommand($"cat {path}");
            var tmpDir = Path.Combine(Path.GetTempPath(), "GitStudio");
            if (!Directory.Exists(tmpDir)) Directory.CreateDirectory(tmpDir);
            var tmpFile = Path.Combine(tmpDir, $"ssh_{Guid.NewGuid()}.tmp");
            File.WriteAllLines(tmpFile, content);
            Process.Start($"C:\\Program Files\\Notepad++\\notepad++.exe", tmpFile);
        }

        private void viewContentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeMain.SelectedNode == null) return;
            var file = (treeMain.SelectedNode.Tag as SSHFileDirectory).Path;
            viewContents(file);
        }

        private void viewContentsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var item = lstSearchResults.SelectedItem;
            var file = item is SSHFileContentsSearchResults csr ? csr.Path : item.ToString();
            viewContents(file);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure?", "Delete", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
            foreach (var node in treeMain.SelectedNodes.ToList())
            {
                var file = (node.Tag as SSHFileDirectory).Path;
                _sshHelper.RunCommand($"rm {file}");
                treeMain.Nodes.Remove(node);
            }
        }

        private void recursivelyDownloadFile(SSHFileDirectory sshParent, string parentDirectory, List<SSHFileDirectory> symLinks, BackgroundWorker worker, PromptResult lastResult)
        {
            _sshHelper.PopulateChildren(sshParent);
            var fullDir = parentDirectory;
            fullDir = Path.Combine(fullDir, sshParent.ShortPath);
            if (!Directory.Exists(fullDir)) Directory.CreateDirectory(fullDir);
            recursivelyDownloadFiles(sshParent.Children, fullDir, symLinks, worker, lastResult);
        }

        private void recursivelyDownloadFiles(IEnumerable<SSHFileDirectory> files, string parentDirectory, List<SSHFileDirectory> symLinks, BackgroundWorker worker, PromptResult lastResult)
        {
            foreach (var file in files)
            {
                if (worker.CancellationPending) return;
                worker.ReportProgress(0, $"Processing {file.Path}");
                if (!string.IsNullOrEmpty(file.SymLink))
                {
                    symLinks.Add(file);
                }
                else if (file.IsDirectory)
                {
                    recursivelyDownloadFile(file, parentDirectory, symLinks, worker, lastResult);
                }
                else
                {
                    var path = Path.Combine(parentDirectory, file.ShortPath);
                    if (File.Exists(path))
                    {
                        if (lastResult == PromptResult.No || lastResult == PromptResult.Yes)
                        {
                            lastResult = ScrollableMessageBox.ShowDialog($"{path} exists, overwrite?", "File Exists",
                                ScrollableMessageBoxButtons.Yes,
                                ScrollableMessageBoxButtons.YesToAll,
                                ScrollableMessageBoxButtons.No,
                                ScrollableMessageBoxButtons.NoToAll
                                );
                        }
                    }
                    if (lastResult == PromptResult.No || lastResult == PromptResult.NoToAll)
                    {
                        continue;
                    }

                    var content = _sshHelper.RunCommand($"cat {file.Path}");
                    File.WriteAllLines(Path.Combine(parentDirectory, file.ShortPath), content);
                }
            }
        }

        private void downloadToolStripMenuItem_Click(object sender, EventArgs e)
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
                List<SSHFileDirectory> symLinks = new List<SSHFileDirectory>();
                worker.DoWork += (object bwsender, DoWorkEventArgs bwe) =>
                {
                    recursivelyDownloadFiles(treeMain.SelectedNodes.Select(n => n.Tag as SSHFileDirectory), folder.SelectedPath, symLinks, worker, PromptResult.Yes);
                };
                WinProgressBox.ShowProgress(worker, "", this, true);
                if (symLinks.Any())
                {
                    MessageBox.Show($"Following are symlinks:\n{string.Join("\n", symLinks.Select(x => $"{x.Path} -> {x.SymLink}"))}");
                }
            }
        }

        private void downloadToolStripMenuItem1_Click(object sender, EventArgs e)
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
                var item = lstSearchResults.SelectedItem;
                var file = item is SSHFileContentsSearchResults csr ? csr.Path : item.ToString();
                var content = _sshHelper.RunCommand($"cat {file}");
                File.WriteAllLines(Path.Combine(folder.SelectedPath, file.Split('/').Last()), content);
            }
        }

        private void find(bool inFiles)
        {
            var input = inFiles ?
                InputBox.Show("Enter search text", "Search text") :
                InputBox.Show("Enter file or directory name", "File/Directory");
            if (input.Result == DialogResult.OK)
            {
                var nodes = treeMain.SelectedNodes.ToList();
                BackgroundWorker worker = new BackgroundWorker();
                worker.WorkerSupportsCancellation = true;
                List<SSHFileDirectory> symLinks = new List<SSHFileDirectory>();
                List<object> results = new List<object>();
                worker.DoWork += (object bwsender, DoWorkEventArgs bwe) =>
                {
                    foreach (var node in nodes)
                    {
                        var tag = node.Tag as SSHFileDirectory;
                        if (tag.IsDirectory)
                        {
                            if (worker.CancellationPending) return;
                            results.AddRange(
                                _sshHelper.RunCommand($"cd {tag.Path} && sudo {(inFiles ? "grep -rnw './' -e" : "find ./ -name")} '{input.Text}'")
                                .Select(x => inFiles ? (object)new SSHFileContentsSearchResults() { Path = $"{tag.Path}{x.Split(':')[0].Substring(1)}", Containing = String.Join(":", x.Split(':').Skip(1)) }
                                : $"{tag.Path}{x.Substring(1)}")
                                );
                        }
                    }
                };
                WinProgressBox.ShowProgress(worker, "", this, true, ProgressBarStyle.Marquee);
                lstSearchResults.Items.Clear();
                results.ForEach(x => lstSearchResults.Items.Add(x));
                splitContainer1.Panel2Collapsed = false;
            }
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            find(false);
        }

        private void findInFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            find(true);
        }
    }
}
