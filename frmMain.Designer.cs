namespace SSHConnector
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.tabWorkspaces = new PaJaMa.WinControls.TabControl.TabControl();
            this.SuspendLayout();
            // 
            // tabWorkspaces
            // 
            this.tabWorkspaces.AllowAdd = true;
            this.tabWorkspaces.AllowRemove = true;
            this.tabWorkspaces.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabWorkspaces.Location = new System.Drawing.Point(0, 0);
            this.tabWorkspaces.Name = "tabWorkspaces";
            this.tabWorkspaces.SelectedTab = null;
            this.tabWorkspaces.Size = new System.Drawing.Size(800, 450);
            this.tabWorkspaces.TabIndex = 0;
            this.tabWorkspaces.TabClosing += new PaJaMa.WinControls.TabControl.TabEventHandler(this.tabWorkspaces_TabClosing);
            this.tabWorkspaces.TabAdding += new PaJaMa.WinControls.TabControl.TabEventHandler(this.tabWorkspaces_TabAdding);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tabWorkspaces);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMain";
            this.Text = "SSH Connector";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private PaJaMa.WinControls.TabControl.TabControl tabWorkspaces;
    }
}

