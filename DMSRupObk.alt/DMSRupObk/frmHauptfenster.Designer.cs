namespace DMSRupObk
{
    partial class frmHauptfenster
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmHauptfenster));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.dokumenteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importAusVerzeichnisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dokumentHinzufügenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.einstellungenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.iNIDateiBearbeitenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.verzeichnisstrukturDokumenteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dokumenteToolStripMenuItem,
            this.einstellungenToolStripMenuItem});
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.Name = "menuStrip1";
            // 
            // dokumenteToolStripMenuItem
            // 
            this.dokumenteToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importAusVerzeichnisToolStripMenuItem,
            this.dokumentHinzufügenToolStripMenuItem});
            this.dokumenteToolStripMenuItem.Name = "dokumenteToolStripMenuItem";
            resources.ApplyResources(this.dokumenteToolStripMenuItem, "dokumenteToolStripMenuItem");
            // 
            // importAusVerzeichnisToolStripMenuItem
            // 
            this.importAusVerzeichnisToolStripMenuItem.Name = "importAusVerzeichnisToolStripMenuItem";
            resources.ApplyResources(this.importAusVerzeichnisToolStripMenuItem, "importAusVerzeichnisToolStripMenuItem");
            // 
            // dokumentHinzufügenToolStripMenuItem
            // 
            this.dokumentHinzufügenToolStripMenuItem.Name = "dokumentHinzufügenToolStripMenuItem";
            resources.ApplyResources(this.dokumentHinzufügenToolStripMenuItem, "dokumentHinzufügenToolStripMenuItem");
            // 
            // einstellungenToolStripMenuItem
            // 
            this.einstellungenToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.iNIDateiBearbeitenToolStripMenuItem,
            this.verzeichnisstrukturDokumenteToolStripMenuItem});
            this.einstellungenToolStripMenuItem.Name = "einstellungenToolStripMenuItem";
            resources.ApplyResources(this.einstellungenToolStripMenuItem, "einstellungenToolStripMenuItem");
            // 
            // iNIDateiBearbeitenToolStripMenuItem
            // 
            this.iNIDateiBearbeitenToolStripMenuItem.Name = "iNIDateiBearbeitenToolStripMenuItem";
            resources.ApplyResources(this.iNIDateiBearbeitenToolStripMenuItem, "iNIDateiBearbeitenToolStripMenuItem");
            // 
            // verzeichnisstrukturDokumenteToolStripMenuItem
            // 
            this.verzeichnisstrukturDokumenteToolStripMenuItem.Name = "verzeichnisstrukturDokumenteToolStripMenuItem";
            resources.ApplyResources(this.verzeichnisstrukturDokumenteToolStripMenuItem, "verzeichnisstrukturDokumenteToolStripMenuItem");
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // frmHauptfenster
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmHauptfenster";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem dokumenteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importAusVerzeichnisToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dokumentHinzufügenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem einstellungenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem iNIDateiBearbeitenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem verzeichnisstrukturDokumenteToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}

