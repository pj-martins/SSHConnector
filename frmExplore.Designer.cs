
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
			this.treeMain = new PaJaMa.WinControls.MultiSelectTreeView();
			this.panel1 = new System.Windows.Forms.Panel();
			this.lblFullPath = new System.Windows.Forms.Label();
			this.btnDownload = new System.Windows.Forms.Button();
			this.btnUpload = new System.Windows.Forms.Button();
			this.btnDelete = new System.Windows.Forms.Button();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// treeMain
			// 
			this.treeMain.AllowDragNodes = false;
			this.treeMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeMain.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
			this.treeMain.Location = new System.Drawing.Point(0, 0);
			this.treeMain.Name = "treeMain";
			this.treeMain.Size = new System.Drawing.Size(566, 416);
			this.treeMain.TabIndex = 0;
			this.treeMain.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeMain_BeforeExpand);
			this.treeMain.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeMain_NodeMouseClick);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.btnDelete);
			this.panel1.Controls.Add(this.btnUpload);
			this.panel1.Controls.Add(this.btnDownload);
			this.panel1.Controls.Add(this.lblFullPath);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 416);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(566, 34);
			this.panel1.TabIndex = 1;
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
			// btnDownload
			// 
			this.btnDownload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnDownload.Location = new System.Drawing.Point(488, 6);
			this.btnDownload.Name = "btnDownload";
			this.btnDownload.Size = new System.Drawing.Size(75, 23);
			this.btnDownload.TabIndex = 1;
			this.btnDownload.Text = "Download";
			this.btnDownload.UseVisualStyleBackColor = true;
			this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
			// 
			// btnUpload
			// 
			this.btnUpload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnUpload.Location = new System.Drawing.Point(407, 6);
			this.btnUpload.Name = "btnUpload";
			this.btnUpload.Size = new System.Drawing.Size(75, 23);
			this.btnUpload.TabIndex = 2;
			this.btnUpload.Text = "Upload";
			this.btnUpload.UseVisualStyleBackColor = true;
			this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
			// 
			// btnDelete
			// 
			this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnDelete.Location = new System.Drawing.Point(326, 6);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new System.Drawing.Size(75, 23);
			this.btnDelete.TabIndex = 3;
			this.btnDelete.Text = "Delete";
			this.btnDelete.UseVisualStyleBackColor = true;
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// frmExplore
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(566, 450);
			this.Controls.Add(this.treeMain);
			this.Controls.Add(this.panel1);
			this.Name = "frmExplore";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Explore";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmExplore_FormClosing);
			this.Load += new System.EventHandler(this.frmExplore_Load);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private PaJaMa.WinControls.MultiSelectTreeView treeMain;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button btnUpload;
		private System.Windows.Forms.Button btnDownload;
		private System.Windows.Forms.Label lblFullPath;
		private System.Windows.Forms.Button btnDelete;
	}
}