namespace BillysToolbox.Editors
{
    partial class KMPEditorForm
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
            TreeNode treeNode1 = new TreeNode("Start Position(s)");
            TreeNode treeNode2 = new TreeNode("Enemy Routes");
            TreeNode treeNode3 = new TreeNode("Item Routes");
            TreeNode treeNode4 = new TreeNode("Checkpoints");
            TreeNode treeNode5 = new TreeNode("Objects");
            TreeNode treeNode6 = new TreeNode("Routes");
            TreeNode treeNode7 = new TreeNode("Areas");
            TreeNode treeNode8 = new TreeNode("Cameras");
            TreeNode treeNode9 = new TreeNode("Respawns");
            TreeNode treeNode10 = new TreeNode("Cannons");
            TreeNode treeNode11 = new TreeNode("Battle Endpoints");
            TreeNode treeNode12 = new TreeNode("Stage Info");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KMPEditorForm));
            menuStrip = new MenuStrip();
            editMenuItem = new ToolStripMenuItem();
            importBackgroundModelMenuItem = new ToolStripMenuItem();
            calculateYValuesMenuItem = new ToolStripMenuItem();
            linkRespawnsMenuItem = new ToolStripMenuItem();
            sectionTree = new TreeView();
            entryListBox = new ListBox();
            entryPropertyGrid = new PropertyGrid();
            addButton = new Button();
            removeButton = new Button();
            entryGroupBox = new GroupBox();
            propertyGroupBox = new GroupBox();
            viewport = new Viewport2D();
            sectionGroupBox = new GroupBox();
            splitContainer1 = new SplitContainer();
            splitContainer2 = new SplitContainer();
            menuStrip.SuspendLayout();
            entryGroupBox.SuspendLayout();
            propertyGroupBox.SuspendLayout();
            sectionGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip
            // 
            menuStrip.Items.AddRange(new ToolStripItem[] { editMenuItem });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new Size(1113, 24);
            menuStrip.TabIndex = 11;
            menuStrip.Text = "menuStrip";
            // 
            // editMenuItem
            // 
            editMenuItem.DropDownItems.AddRange(new ToolStripItem[] { importBackgroundModelMenuItem, calculateYValuesMenuItem, linkRespawnsMenuItem });
            editMenuItem.MergeAction = MergeAction.Insert;
            editMenuItem.MergeIndex = 1;
            editMenuItem.Name = "editMenuItem";
            editMenuItem.Size = new Size(39, 20);
            editMenuItem.Text = "Edit";
            // 
            // importBackgroundModelMenuItem
            // 
            importBackgroundModelMenuItem.Image = Properties.Resources.brres;
            importBackgroundModelMenuItem.Name = "importBackgroundModelMenuItem";
            importBackgroundModelMenuItem.Size = new Size(169, 22);
            importBackgroundModelMenuItem.Text = "Import KCL...";
            importBackgroundModelMenuItem.Click += importBackgroundModelMenuItem_Click;
            // 
            // calculateYValuesMenuItem
            // 
            calculateYValuesMenuItem.Image = Properties.Resources.ModuleScript;
            calculateYValuesMenuItem.Name = "calculateYValuesMenuItem";
            calculateYValuesMenuItem.Size = new Size(169, 22);
            calculateYValuesMenuItem.Text = "Calculate Y values";
            calculateYValuesMenuItem.Click += calculateYValuesMenuItem_Click;
            // 
            // linkRespawnsMenuItem
            // 
            linkRespawnsMenuItem.Image = Properties.Resources.ModuleScript;
            linkRespawnsMenuItem.Name = "linkRespawnsMenuItem";
            linkRespawnsMenuItem.Size = new Size(169, 22);
            linkRespawnsMenuItem.Text = "Link respawns";
            linkRespawnsMenuItem.Click += linkRespawnsMenuItem_Click;
            // 
            // sectionTree
            // 
            sectionTree.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            sectionTree.Enabled = false;
            sectionTree.HideSelection = false;
            sectionTree.Location = new Point(6, 22);
            sectionTree.Name = "sectionTree";
            treeNode1.Name = "ktptNode";
            treeNode1.Text = "Start Position(s)";
            treeNode2.Name = "enptNode";
            treeNode2.Text = "Enemy Routes";
            treeNode3.Name = "itptNode";
            treeNode3.Text = "Item Routes";
            treeNode4.Name = "ckptNode";
            treeNode4.Text = "Checkpoints";
            treeNode5.Name = "gobjNode";
            treeNode5.Text = "Objects";
            treeNode6.Name = "potiNode";
            treeNode6.Text = "Routes";
            treeNode7.Name = "areaNode";
            treeNode7.Text = "Areas";
            treeNode8.Name = "cameNode";
            treeNode8.Text = "Cameras";
            treeNode9.Name = "jgpt";
            treeNode9.Text = "Respawns";
            treeNode10.Name = "cnptNode";
            treeNode10.Text = "Cannons";
            treeNode11.Name = "msptNode";
            treeNode11.Text = "Battle Endpoints";
            treeNode12.Name = "stgiNode";
            treeNode12.Text = "Stage Info";
            sectionTree.Nodes.AddRange(new TreeNode[] { treeNode1, treeNode2, treeNode3, treeNode4, treeNode5, treeNode6, treeNode7, treeNode8, treeNode9, treeNode10, treeNode11, treeNode12 });
            sectionTree.Size = new Size(159, 582);
            sectionTree.TabIndex = 1;
            sectionTree.AfterSelect += sectionTree_AfterSelect;
            // 
            // entryListBox
            // 
            entryListBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            entryListBox.FormattingEnabled = true;
            entryListBox.ItemHeight = 15;
            entryListBox.Location = new Point(6, 22);
            entryListBox.Name = "entryListBox";
            entryListBox.Size = new Size(220, 244);
            entryListBox.TabIndex = 2;
            entryListBox.SelectedIndexChanged += entryListBox_SelectedIndexChanged;
            // 
            // entryPropertyGrid
            // 
            entryPropertyGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            entryPropertyGrid.HelpVisible = false;
            entryPropertyGrid.Location = new Point(6, 22);
            entryPropertyGrid.Name = "entryPropertyGrid";
            entryPropertyGrid.PropertySort = PropertySort.NoSort;
            entryPropertyGrid.Size = new Size(220, 272);
            entryPropertyGrid.TabIndex = 3;
            entryPropertyGrid.ToolbarVisible = false;
            // 
            // addButton
            // 
            addButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            addButton.Location = new Point(6, 275);
            addButton.Name = "addButton";
            addButton.Size = new Size(97, 23);
            addButton.TabIndex = 4;
            addButton.Text = "Add";
            addButton.UseVisualStyleBackColor = true;
            addButton.Click += addButton_Click;
            // 
            // removeButton
            // 
            removeButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            removeButton.Location = new Point(109, 275);
            removeButton.Name = "removeButton";
            removeButton.Size = new Size(119, 23);
            removeButton.TabIndex = 5;
            removeButton.Text = "Remove";
            removeButton.UseVisualStyleBackColor = true;
            removeButton.Click += removeButton_Click;
            // 
            // entryGroupBox
            // 
            entryGroupBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            entryGroupBox.Controls.Add(addButton);
            entryGroupBox.Controls.Add(removeButton);
            entryGroupBox.Controls.Add(entryListBox);
            entryGroupBox.Enabled = false;
            entryGroupBox.Location = new Point(3, 3);
            entryGroupBox.Name = "entryGroupBox";
            entryGroupBox.Size = new Size(234, 304);
            entryGroupBox.TabIndex = 6;
            entryGroupBox.TabStop = false;
            entryGroupBox.Text = "Entries";
            // 
            // propertyGroupBox
            // 
            propertyGroupBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            propertyGroupBox.Controls.Add(entryPropertyGrid);
            propertyGroupBox.Enabled = false;
            propertyGroupBox.Location = new Point(3, 313);
            propertyGroupBox.Name = "propertyGroupBox";
            propertyGroupBox.Size = new Size(234, 300);
            propertyGroupBox.TabIndex = 7;
            propertyGroupBox.TabStop = false;
            propertyGroupBox.Text = "Properties";
            // 
            // viewport
            // 
            viewport.AutoSize = true;
            viewport.BackColor = SystemColors.ControlDark;
            viewport.Dock = DockStyle.Fill;
            viewport.Location = new Point(0, 0);
            viewport.Name = "viewport";
            viewport.Size = new Size(688, 592);
            viewport.TabIndex = 8;
            // 
            // sectionGroupBox
            // 
            sectionGroupBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            sectionGroupBox.Controls.Add(sectionTree);
            sectionGroupBox.Location = new Point(3, 3);
            sectionGroupBox.Name = "sectionGroupBox";
            sectionGroupBox.Size = new Size(171, 610);
            sectionGroupBox.TabIndex = 9;
            sectionGroupBox.TabStop = false;
            sectionGroupBox.Text = "KMP Sections";
            // 
            // splitContainer1
            // 
            splitContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(entryGroupBox);
            splitContainer1.Panel2.Controls.Add(propertyGroupBox);
            splitContainer1.Size = new Size(1113, 592);
            splitContainer1.SplitterDistance = 869;
            splitContainer1.TabIndex = 10;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer2.Location = new Point(0, 0);
            splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(sectionGroupBox);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(viewport);
            splitContainer2.Size = new Size(869, 592);
            splitContainer2.SplitterDistance = 177;
            splitContainer2.TabIndex = 0;
            // 
            // KMPEditorForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1113, 592);
            Controls.Add(splitContainer1);
            Controls.Add(menuStrip);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip;
            Name = "KMPEditorForm";
            Text = "KMP Editor";
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            entryGroupBox.ResumeLayout(false);
            propertyGroupBox.ResumeLayout(false);
            sectionGroupBox.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TreeView sectionTree;
        private ListBox entryListBox;
        private PropertyGrid entryPropertyGrid;
        private Button addButton;
        private Button removeButton;
        private GroupBox entryGroupBox;
        private GroupBox propertyGroupBox;
        private Viewport2D viewport;
        private GroupBox sectionGroupBox;
        private SplitContainer splitContainer1;
        private SplitContainer splitContainer2;
        private MenuStrip menuStrip;
        private ToolStripMenuItem editMenuItem;
        private ToolStripMenuItem importBackgroundModelMenuItem;
        private ToolStripMenuItem calculateYValuesMenuItem;
        private ToolStripMenuItem linkRespawnsMenuItem;
    }
}
