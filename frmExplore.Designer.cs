
namespace SSHConnector
{
	partial class frmExplore
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.treeMain = new PaJaMa.WinControls.MultiSelectTreeView();
            this.mnuTree = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewContentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.downloadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnUpload = new System.Windows.Forms.Button();
            this.lblFullPath = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lstSearchResults = new System.Windows.Forms.ListBox();
            this.mnuSearchResults = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.viewContentsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.downloadToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.findInFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTree.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.mnuSearchResults.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeMain
            // 
            this.treeMain.AllowDragNodes = false;
            this.treeMain.ContextMenuStrip = this.mnuTree;
            this.treeMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeMain.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            this.treeMain.Location = new System.Drawing.Point(0, 0);
            this.treeMain.Name = "treeMain";
            this.treeMain.Size = new System.Drawing.Size(997, 582);
            this.treeMain.TabIndex = 0;
            this.treeMain.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeMain_BeforeExpand);
            this.treeMain.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeMain_NodeMouseClick);
            // 
            // mnuTree
            // 
            this.mnuTree.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshToolStripMenuItem,
            this.viewContentsToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.downloadToolStripMenuItem,
            this.findToolStripMenuItem,
            this.findInFilesToolStripMenuItem});
            this.mnuTree.Name = "mnuTree";
            this.mnuTree.Size = new System.Drawing.Size(181, 158);
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.refreshToolStripMenuItem.Text = "&Refresh";
            this.refreshToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
            // 
            // viewContentsToolStripMenuItem
            // 
            this.viewContentsToolStripMenuItem.Name = "viewContentsToolStripMenuItem";
            this.viewContentsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.viewContentsToolStripMenuItem.Text = "&View Contents";
            this.viewContentsToolStripMenuItem.Click += new System.EventHandler(this.viewContentsToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.deleteToolStripMenuItem.Text = "&Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // downloadToolStripMenuItem
            // 
            this.downloadToolStripMenuItem.Name = "downloadToolStripMenuItem";
            this.downloadToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.downloadToolStripMenuItem.Text = "D&ownload";
            this.downloadToolStripMenuItem.Click += new System.EventHandler(this.downloadToolStripMenuItem_Click);
            // 
            // findToolStripMenuItem
            // 
            this.findToolStripMenuItem.Name = "findToolStripMenuItem";
            this.findToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.findToolStripMenuItem.Text = "&Find";
            this.findToolStripMenuItem.Click += new System.EventHandler(this.findToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnUpload);
            this.panel1.Controls.Add(this.lblFullPath);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 582);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(997, 34);
            this.panel1.TabIndex = 1;
            // 
            // btnUpload
            // 
            this.btnUpload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpload.Location = new System.Drawing.Point(919, 6);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(75, 23);
            this.btnUpload.TabIndex = 2;
            this.btnUpload.Text = "Upload";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // lblFullPath
            // 
            this.lblFullPath.AutoSize = true;
            this.lblFullPath.Location = new System.Drawing.Point(12, 11);
            this.lblFullPath.Name = "lblFullPath";
            this.lblFullPath.Size = new System.Drawing.Size(12, 13);
            this.lblFullPath.TabIndex = 0;
            this.lblFullPath.Text = "/";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeMain);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lstSearchResults);
            this.splitContainer1.Panel2Collapsed = true;
            this.splitContainer1.Size = new System.Drawing.Size(997, 582);
            this.splitContainer1.SplitterDistance = 685;
            this.splitContainer1.TabIndex = 2;
            // 
            // lstSearchResults
            // 
            this.lstSearchResults.ContextMenuStrip = this.mnuSearchResults;
            this.lstSearchResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstSearchResults.FormattingEnabled = true;
            this.lstSearchResults.Location = new System.Drawing.Point(0, 0);
            this.lstSearchResults.Name = "lstSearchResults";
            this.lstSearchResults.Size = new System.Drawing.Size(96, 100);
            this.lstSearchResults.TabIndex = 0;
            // 
            // mnuSearchResults
            // 
            this.mnuSearchResults.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewContentsToolStripMenuItem1,
            this.downloadToolStripMenuItem1});
            this.mnuSearchResults.Name = "mnuSearchResults";
            this.mnuSearchResults.Size = new System.Drawing.Size(151, 48);
            // 
            // viewContentsToolStripMenuItem1
            // 
            this.viewContentsToolStripMenuItem1.Name = "viewContentsToolStripMenuItem1";
            this.viewContentsToolStripMenuItem1.Size = new System.Drawing.Size(150, 22);
            this.viewContentsToolStripMenuItem1.Text = "&View Contents";
            this.viewContentsToolStripMenuItem1.Click += new System.EventHandler(this.viewContentsToolStripMenuItem1_Click);
            // 
            // downloadToolStripMenuItem1
            // 
            this.downloadToolStripMenuItem1.Name = "downloadToolStripMenuItem1";
            this.downloadToolStripMenuItem1.Size = new System.Drawing.Size(150, 22);
            this.downloadToolStripMenuItem1.Text = "D&ownload";
            this.downloadToolStripMenuItem1.Click += new System.EventHandler(this.downloadToolStripMenuItem1_Click);
            // 
            // findInFilesToolStripMenuItem
            // 
            this.findInFilesToolStripMenuItem.Name = "findInFilesToolStripMenuItem";
            this.findInFilesToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.findInFilesToolStripMenuItem.Text = "F&ind In Files";
            this.findInFilesToolStripMenuItem.Click += new System.EventHandler(this.findInFilesToolStripMenuItem_Click);
            // 
            // frmExplore
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(997, 616);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel1);
            this.Name = "frmExplore";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Explore";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmExplore_FormClosing);
            this.Load += new System.EventHandler(this.frmExplore_Load);
            this.mnuTree.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.mnuSearchResults.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private PaJaMa.WinControls.MultiSelectTreeView treeMain;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button btnUpload;
		private System.Windows.Forms.Label lblFullPath;
        private System.Windows.Forms.ContextMenuStrip mnuTree;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewContentsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem downloadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem findToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListBox lstSearchResults;
        private System.Windows.Forms.ContextMenuStrip mnuSearchResults;
        private System.Windows.Forms.ToolStripMenuItem viewContentsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem downloadToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem findInFilesToolStripMenuItem;
    }
}