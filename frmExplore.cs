using PaJaMa.Common;
using PaJaMa.WinControls;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SSHConnector
{
	public partial class frmExplore : Form
	{
		public Terminal Terminal { get; set; }
		public event EventHandler SettingsChanged;
		private SftpClient _client;
		private List<string> _lastSSHd = new List<string>();
		private bool _refreshing = false;
		private TreeNode _toSelect = null;
		public frmExplore()
		{
			InitializeComponent();
		}

		private void frmExplore_Load(object sender, EventArgs e)
		{
			string pwd = string.IsNullOrEmpty(Terminal.PasswordEncrypted) ? string.Empty : PaJaMa.Common.EncrypterDecrypter.Decrypt(Terminal.PasswordEncrypted, Terminal.PWD);
			_client = new Renci.SshNet.SftpClient(Terminal.Host, Terminal.Port, Terminal.User, pwd);
			try
			{
				_client.Connect();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				this.Close();
				return;
			}
			this.Text = Terminal.Host;
			doRefresh();
		}

		private void doRefresh()
		{
			_refreshing = true;
			if (!string.IsNullOrEmpty(Terminal.LastSSHd))
			{
				_lastSSHd = Terminal.LastSSHd.Split('/').Where(x => !string.IsNullOrEmpty(x)).ToList();
			}
			refreshTree(null);
			_refreshing = false;
			System.Threading.Thread.Sleep(100);
			treeMain.SelectedNode = _toSelect;
			_toSelect = null;
		}

		private void frmExplore_FormClosing(object sender, FormClosingEventArgs e)
		{
			_client.Disconnect();
		}

		private void refreshTree(TreeNode parentNode)
		{
			if (parentNode != null) parentNode.Nodes.Clear();
			else treeMain.Nodes.Clear();
			var parent = parentNode == null ? null : parentNode.Tag as SftpFile;
			var files = _client.ListDirectory(parent == null ? "/" : parent.FullName);
			lblFullPath.Text = parent == null ? "/" : parent.FullName;
			foreach (var file in files.OrderBy(f => f.Name))
			{
				if (file.Name == "." || file.Name == "..") continue;
				var tv = new TreeNode();
				tv.Text = file.Name;
				tv.Tag = file;
				if (file.IsDirectory)
				{
					var tvchild = new TreeNode();
					tvchild.Text = "TEMP";
					tv.Nodes.Add(tvchild);
				}
				if (parentNode != null) parentNode.Nodes.Add(tv);
				else treeMain.Nodes.Add(tv);
				if (_lastSSHd.Any() && tv.Text == _lastSSHd[0])
				{
					_lastSSHd.RemoveAt(0);
					if (_lastSSHd.Any())
					{
						tv.Expand();
					}
					_toSelect = tv;
				}
			}
		}

		private void treeMain_BeforeExpand(object sender, TreeViewCancelEventArgs e)
		{
			if (e.Node == null) return;
			refreshTree(e.Node);
			if (!_refreshing)
			{
				Terminal.LastSSHd = (e.Node.Tag as SftpFile).FullName;
				SettingsChanged?.Invoke(this, new EventArgs());
			}
		}

		private void treeMain_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			if (e.Node == null) return;
			lblFullPath.Text = (e.Node.Tag as SftpFile).FullName;
			Terminal.LastSSHd = (e.Node.Tag as SftpFile).FullName;
			SettingsChanged?.Invoke(this, new EventArgs());
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
					recursivelyDownloadFiles(treeMain.SelectedNodes.Select(n => n.Tag as SftpFile), folder.SelectedPath, worker, PromptResult.Yes);
				};
				PaJaMa.WinControls.WinProgressBox.ShowProgress(worker, "", this, true);

			}
		}

		private void recursivelyDownloadFiles(IEnumerable<SftpFile> files, string parentDirectory, BackgroundWorker worker, PromptResult lastResult)
		{
			foreach (var file in files)
			{
				if (worker.CancellationPending) return;
				worker.ReportProgress(0, $"Processing {file.FullName}");
				if (file.Name == "." || file.Name == "..") continue;
				if (file.IsDirectory)
				{
					recursivelyDownloadFile(file, parentDirectory, worker, lastResult);
				}
				else
				{
					var path = Path.Combine(parentDirectory, file.Name);
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
					var stream = new FileStream(Path.Combine(parentDirectory, file.Name), FileMode.Create);
					var res = _client.BeginDownloadFile(file.FullName, stream);
					while (!res.IsCompleted)
					{
						Thread.Sleep(100);
					}
					_client.EndDownloadFile(res);
				}
			}
		}

		private void recursivelyDownloadFile(SftpFile parent, string parentDirectory, BackgroundWorker worker, PromptResult lastResult)
		{
			var parts = parent.FullName.Split('/');
			var fullDir = parentDirectory;
			fullDir = Path.Combine(fullDir, parts.Last());
			if (!Directory.Exists(fullDir)) Directory.CreateDirectory(fullDir);
			var childFiles = _client.ListDirectory(parent.FullName);
			recursivelyDownloadFiles(childFiles, fullDir, worker, lastResult);
		}

		private void btnUpload_Click(object sender, EventArgs e)
		{
			var dlg = new OpenFileDialog();
			dlg.Multiselect = true;
			if (!string.IsNullOrEmpty(Terminal.LastUploaded)) dlg.InitialDirectory = Terminal.LastUploaded;
			if (dlg.ShowDialog() == DialogResult.OK)
			{
				BackgroundWorker worker = new BackgroundWorker();
				worker.WorkerSupportsCancellation = true;
				worker.DoWork += (object bwsender, DoWorkEventArgs bwe) =>
				{
					foreach (var file in dlg.FileNames)
					{
						if (worker.CancellationPending) return;
						worker.ReportProgress(0, $"Processing {file}");
						var res = _client.BeginUploadFile(new FileStream(file, FileMode.Open), $"{lblFullPath.Text}/{Path.GetFileName(file)}");
						while (!res.IsCompleted)
						{
							Thread.Sleep(100);
						}
						_client.EndUploadFile(res);
					}
				};
				PaJaMa.WinControls.WinProgressBox.ShowProgress(worker, "", this, true);
			}
			doRefresh();
		}

		private void btnDelete_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show("Are you sure?", "Delete", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
			var selected = treeMain.SelectedNodes.Select(n => n.Tag as SftpFile);
			foreach (var file in selected)
			{
				if (file.IsDirectory) _client.DeleteDirectory(file.FullName);
				else _client.DeleteFile(file.FullName);
			}
			doRefresh();
		}
	}
}
