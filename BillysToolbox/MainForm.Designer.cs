namespace BillysToolbox
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            fileToolStripMenuItem = new ToolStripMenuItem();
            newToolStripMenuItem = new ToolStripMenuItem();
            u8ArchiveToolStripMenuItem = new ToolStripMenuItem();
            postEffectToolStripMenuItem = new ToolStripMenuItem();
            bLIGHTToolStripMenuItem1 = new ToolStripMenuItem();
            bDOFToolStripMenuItem1 = new ToolStripMenuItem();
            bBLMToolStripMenuItem = new ToolStripMenuItem();
            bFGToolStripMenuItem = new ToolStripMenuItem();
            bLMAPToolStripMenuItem = new ToolStripMenuItem();
            kMPToolStripMenuItem = new ToolStripMenuItem();
            kCLToolStripMenuItem = new ToolStripMenuItem();
            bMMToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            saveAsToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            settingsToolStripMenuItem = new ToolStripMenuItem();
            menuStrip = new MenuStrip();
            windowToolStripMenuItem = new ToolStripMenuItem();
            toolsToolStripMenuItem = new ToolStripMenuItem();
            nImageScalerToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem1 = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            toolStrip1 = new ToolStrip();
            saveToolStripButton = new ToolStripButton();
            toolStripButton2 = new ToolStripButton();
            toolStripSeparator4 = new ToolStripSeparator();
            openToolStripButton = new ToolStripButton();
            toolStripSeparator2 = new ToolStripSeparator();
            settingsToolStripButton = new ToolStripButton();
            tPLToolStripMenuItem = new ToolStripMenuItem();
            menuStrip.SuspendLayout();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { newToolStripMenuItem, openToolStripMenuItem, saveToolStripMenuItem, saveAsToolStripMenuItem, toolStripSeparator1, settingsToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            newToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { u8ArchiveToolStripMenuItem, postEffectToolStripMenuItem, kMPToolStripMenuItem, kCLToolStripMenuItem, bMMToolStripMenuItem, tPLToolStripMenuItem });
            newToolStripMenuItem.Image = Properties.Resources.page_white_add;
            newToolStripMenuItem.Name = "newToolStripMenuItem";
            newToolStripMenuItem.Size = new Size(184, 26);
            newToolStripMenuItem.Text = "New";
            // 
            // u8ArchiveToolStripMenuItem
            // 
            u8ArchiveToolStripMenuItem.Image = Properties.Resources.szs;
            u8ArchiveToolStripMenuItem.Name = "u8ArchiveToolStripMenuItem";
            u8ArchiveToolStripMenuItem.Size = new Size(184, 26);
            u8ArchiveToolStripMenuItem.Text = "U8 Archive";
            u8ArchiveToolStripMenuItem.Click += u8ArchiveToolStripMenuItem_Click;
            // 
            // postEffectToolStripMenuItem
            // 
            postEffectToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { bLIGHTToolStripMenuItem1, bDOFToolStripMenuItem1, bBLMToolStripMenuItem, bFGToolStripMenuItem, bLMAPToolStripMenuItem });
            postEffectToolStripMenuItem.Image = Properties.Resources.camera;
            postEffectToolStripMenuItem.Name = "postEffectToolStripMenuItem";
            postEffectToolStripMenuItem.Size = new Size(184, 26);
            postEffectToolStripMenuItem.Text = "Post-Effect";
            // 
            // bLIGHTToolStripMenuItem1
            // 
            bLIGHTToolStripMenuItem1.Image = Properties.Resources.help;
            bLIGHTToolStripMenuItem1.Name = "bLIGHTToolStripMenuItem1";
            bLIGHTToolStripMenuItem1.Size = new Size(113, 22);
            bLIGHTToolStripMenuItem1.Text = "BLIGHT";
            bLIGHTToolStripMenuItem1.Click += bLIGHTToolStripMenuItem1_Click;
            // 
            // bDOFToolStripMenuItem1
            // 
            bDOFToolStripMenuItem1.Image = Properties.Resources.bdof;
            bDOFToolStripMenuItem1.Name = "bDOFToolStripMenuItem1";
            bDOFToolStripMenuItem1.Size = new Size(113, 22);
            bDOFToolStripMenuItem1.Text = "BDOF";
            bDOFToolStripMenuItem1.Click += bDOFToolStripMenuItem1_Click;
            // 
            // bBLMToolStripMenuItem
            // 
            bBLMToolStripMenuItem.Image = Properties.Resources.bblm;
            bBLMToolStripMenuItem.Name = "bBLMToolStripMenuItem";
            bBLMToolStripMenuItem.Size = new Size(113, 22);
            bBLMToolStripMenuItem.Text = "BBLM";
            // 
            // bFGToolStripMenuItem
            // 
            bFGToolStripMenuItem.Image = Properties.Resources.bfg;
            bFGToolStripMenuItem.Name = "bFGToolStripMenuItem";
            bFGToolStripMenuItem.Size = new Size(113, 22);
            bFGToolStripMenuItem.Text = "BFG";
            // 
            // bLMAPToolStripMenuItem
            // 
            bLMAPToolStripMenuItem.Image = Properties.Resources.blmap;
            bLMAPToolStripMenuItem.Name = "bLMAPToolStripMenuItem";
            bLMAPToolStripMenuItem.Size = new Size(113, 22);
            bLMAPToolStripMenuItem.Text = "BLMAP";
            // 
            // kMPToolStripMenuItem
            // 
            kMPToolStripMenuItem.Enabled = false;
            kMPToolStripMenuItem.Image = Properties.Resources.kmp;
            kMPToolStripMenuItem.Name = "kMPToolStripMenuItem";
            kMPToolStripMenuItem.Size = new Size(184, 26);
            kMPToolStripMenuItem.Text = "KMP";
            kMPToolStripMenuItem.Click += kMPToolStripMenuItem_Click;
            // 
            // kCLToolStripMenuItem
            // 
            kCLToolStripMenuItem.Image = Properties.Resources.brres;
            kCLToolStripMenuItem.Name = "kCLToolStripMenuItem";
            kCLToolStripMenuItem.Size = new Size(184, 26);
            kCLToolStripMenuItem.Text = "KCL";
            kCLToolStripMenuItem.Click += kCLToolStripMenuItem_Click;
            // 
            // bMMToolStripMenuItem
            // 
            bMMToolStripMenuItem.Image = Properties.Resources.mii;
            bMMToolStripMenuItem.Name = "bMMToolStripMenuItem";
            bMMToolStripMenuItem.Size = new Size(184, 26);
            bMMToolStripMenuItem.Text = "BMM";
            bMMToolStripMenuItem.Click += bMMToolStripMenuItem_Click;
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Image = Properties.Resources.folder;
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(184, 26);
            openToolStripMenuItem.Text = "Open...";
            openToolStripMenuItem.Click += openToolStripMenuItem_Click;
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Image = Properties.Resources.disk;
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new Size(184, 26);
            saveToolStripMenuItem.Text = "Save";
            saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
            // 
            // saveAsToolStripMenuItem
            // 
            saveAsToolStripMenuItem.Image = Properties.Resources.save_as;
            saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            saveAsToolStripMenuItem.Size = new Size(184, 26);
            saveAsToolStripMenuItem.Text = "Save as...";
            saveAsToolStripMenuItem.Click += saveAsToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(181, 6);
            // 
            // settingsToolStripMenuItem
            // 
            settingsToolStripMenuItem.Image = Properties.Resources.tool;
            settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            settingsToolStripMenuItem.Size = new Size(184, 26);
            settingsToolStripMenuItem.Text = "Settings";
            // 
            // menuStrip
            // 
            menuStrip.ImageScalingSize = new Size(20, 20);
            menuStrip.ImeMode = ImeMode.NoControl;
            menuStrip.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, windowToolStripMenuItem, toolsToolStripMenuItem, aboutToolStripMenuItem });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new Size(1237, 24);
            menuStrip.TabIndex = 1;
            menuStrip.Text = "menuStrip";
            // 
            // windowToolStripMenuItem
            // 
            windowToolStripMenuItem.Name = "windowToolStripMenuItem";
            windowToolStripMenuItem.Size = new Size(63, 20);
            windowToolStripMenuItem.Text = "Window";
            // 
            // toolsToolStripMenuItem
            // 
            toolsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { nImageScalerToolStripMenuItem });
            toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            toolsToolStripMenuItem.Size = new Size(46, 20);
            toolsToolStripMenuItem.Text = "Tools";
            // 
            // nImageScalerToolStripMenuItem
            // 
            nImageScalerToolStripMenuItem.Image = Properties.Resources.images;
            nImageScalerToolStripMenuItem.Name = "nImageScalerToolStripMenuItem";
            nImageScalerToolStripMenuItem.Size = new Size(167, 22);
            nImageScalerToolStripMenuItem.Text = "2^N Image Scaler";
            nImageScalerToolStripMenuItem.Click += nImageScalerToolStripMenuItem_Click;
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { aboutToolStripMenuItem1, helpToolStripMenuItem });
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new Size(52, 20);
            aboutToolStripMenuItem.Text = "About";
            // 
            // aboutToolStripMenuItem1
            // 
            aboutToolStripMenuItem1.Image = Properties.Resources.help;
            aboutToolStripMenuItem1.Name = "aboutToolStripMenuItem1";
            aboutToolStripMenuItem1.Size = new Size(107, 22);
            aboutToolStripMenuItem1.Text = "About";
            aboutToolStripMenuItem1.Click += aboutToolStripMenuItem1_Click;
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.Image = Properties.Resources.kmp;
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(107, 22);
            helpToolStripMenuItem.Text = "Help";
            helpToolStripMenuItem.Click += helpToolStripMenuItem_Click;
            // 
            // toolStrip1
            // 
            toolStrip1.ImageScalingSize = new Size(20, 20);
            toolStrip1.Items.AddRange(new ToolStripItem[] { saveToolStripButton, toolStripButton2, toolStripSeparator4, openToolStripButton, toolStripSeparator2, settingsToolStripButton });
            toolStrip1.Location = new Point(0, 24);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(1237, 27);
            toolStrip1.TabIndex = 3;
            toolStrip1.Text = "toolStrip1";
            // 
            // saveToolStripButton
            // 
            saveToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            saveToolStripButton.Image = Properties.Resources.disk;
            saveToolStripButton.ImageTransparentColor = Color.Magenta;
            saveToolStripButton.Name = "saveToolStripButton";
            saveToolStripButton.Size = new Size(24, 24);
            saveToolStripButton.Text = "Save";
            saveToolStripButton.Click += saveToolStripButton_Click;
            // 
            // toolStripButton2
            // 
            toolStripButton2.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButton2.Image = Properties.Resources.save_as;
            toolStripButton2.ImageTransparentColor = Color.Magenta;
            toolStripButton2.Name = "toolStripButton2";
            toolStripButton2.Size = new Size(24, 24);
            toolStripButton2.Text = "Save as...";
            toolStripButton2.Click += toolStripButton2_Click;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new Size(6, 27);
            // 
            // openToolStripButton
            // 
            openToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            openToolStripButton.Image = Properties.Resources.folder;
            openToolStripButton.ImageTransparentColor = Color.Magenta;
            openToolStripButton.Name = "openToolStripButton";
            openToolStripButton.Size = new Size(24, 24);
            openToolStripButton.Text = "Open...";
            openToolStripButton.Click += openToolStripButton_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(6, 27);
            // 
            // settingsToolStripButton
            // 
            settingsToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            settingsToolStripButton.Image = Properties.Resources.tool;
            settingsToolStripButton.ImageTransparentColor = Color.Magenta;
            settingsToolStripButton.Name = "settingsToolStripButton";
            settingsToolStripButton.Size = new Size(24, 24);
            settingsToolStripButton.Text = "Settings";
            // 
            // tPLToolStripMenuItem
            // 
            tPLToolStripMenuItem.Image = Properties.Resources.Decal;
            tPLToolStripMenuItem.Name = "tPLToolStripMenuItem";
            tPLToolStripMenuItem.Size = new Size(184, 26);
            tPLToolStripMenuItem.Text = "TPL";
            tPLToolStripMenuItem.Click += tPLToolStripMenuItem_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1237, 687);
            Controls.Add(toolStrip1);
            Controls.Add(menuStrip);
            Icon = (Icon)resources.GetObject("$this.Icon");
            IsMdiContainer = true;
            MainMenuStrip = menuStrip;
            Name = "MainForm";
            Text = "MKW Toolbox";
            Load += MainForm_Load;
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem newToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private MenuStrip menuStrip;
        private ToolStrip toolStrip1;
        private ToolStripButton saveToolStripButton;
        private ToolStripButton openToolStripButton;
        private ToolStripMenuItem windowToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem1;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripButton settingsToolStripButton;
        private ToolStripSplitButton toolStripSplitButton1;
        private ToolStripButton toolStripButton2;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripMenuItem u8ArchiveToolStripMenuItem;
        private ToolStripMenuItem kCLToolStripMenuItem;
        private ToolStripMenuItem postEffectToolStripMenuItem;
        private ToolStripMenuItem bLIGHTToolStripMenuItem1;
        private ToolStripMenuItem bDOFToolStripMenuItem1;
        private ToolStripMenuItem bBLMToolStripMenuItem;
        private ToolStripMenuItem bFGToolStripMenuItem;
        private ToolStripMenuItem bLMAPToolStripMenuItem;
        private ToolStripMenuItem bMMToolStripMenuItem;
        private ToolStripMenuItem kMPToolStripMenuItem;
        private ToolStripMenuItem toolsToolStripMenuItem;
        private ToolStripMenuItem nImageScalerToolStripMenuItem;
        private ToolStripMenuItem tPLToolStripMenuItem;
    }
}