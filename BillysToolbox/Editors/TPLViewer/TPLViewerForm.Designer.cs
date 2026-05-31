namespace BillysToolbox.Editors
{
    partial class TPLViewerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TPLViewerForm));
            splitContainer1 = new SplitContainer();
            listBox1 = new ListBox();
            imageStatusStrip = new StatusStrip();
            imageFormatLabel = new ToolStripStatusLabel();
            imageSizeLabel = new ToolStripStatusLabel();
            imagePreview = new PictureBox();
            menuStrip1 = new MenuStrip();
            editToolStripMenuItem = new ToolStripMenuItem();
            addImagesToolStripMenuItem = new ToolStripMenuItem();
            exportImageToolStripMenuItem = new ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            imageStatusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)imagePreview).BeginInit();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(listBox1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(imageStatusStrip);
            splitContainer1.Panel2.Controls.Add(imagePreview);
            splitContainer1.Size = new Size(473, 313);
            splitContainer1.SplitterDistance = 157;
            splitContainer1.TabIndex = 0;
            // 
            // listBox1
            // 
            listBox1.Dock = DockStyle.Fill;
            listBox1.FormattingEnabled = true;
            listBox1.IntegralHeight = false;
            listBox1.ItemHeight = 15;
            listBox1.Location = new Point(0, 0);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(157, 313);
            listBox1.TabIndex = 0;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // imageStatusStrip
            // 
            imageStatusStrip.Items.AddRange(new ToolStripItem[] { imageFormatLabel, imageSizeLabel });
            imageStatusStrip.Location = new Point(0, 291);
            imageStatusStrip.Name = "imageStatusStrip";
            imageStatusStrip.Size = new Size(312, 22);
            imageStatusStrip.TabIndex = 1;
            imageStatusStrip.Text = "statusStrip1";
            // 
            // imageFormatLabel
            // 
            imageFormatLabel.Name = "imageFormatLabel";
            imageFormatLabel.Size = new Size(93, 17);
            imageFormatLabel.Text = "Image Format:  |";
            // 
            // imageSizeLabel
            // 
            imageSizeLabel.Name = "imageSizeLabel";
            imageSizeLabel.Size = new Size(0, 17);
            // 
            // imagePreview
            // 
            imagePreview.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            imagePreview.BackgroundImageLayout = ImageLayout.Zoom;
            imagePreview.Location = new Point(0, 0);
            imagePreview.Name = "imagePreview";
            imagePreview.Size = new Size(312, 291);
            imagePreview.TabIndex = 0;
            imagePreview.TabStop = false;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { editToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(473, 24);
            menuStrip1.TabIndex = 7;
            menuStrip1.Text = "menuStrip1";
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { addImagesToolStripMenuItem, exportImageToolStripMenuItem });
            editToolStripMenuItem.MergeAction = MergeAction.Insert;
            editToolStripMenuItem.MergeIndex = 1;
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new Size(39, 20);
            editToolStripMenuItem.Text = "Edit";
            // 
            // addImagesToolStripMenuItem
            // 
            addImagesToolStripMenuItem.Image = Properties.Resources.page_white_add;
            addImagesToolStripMenuItem.Name = "addImagesToolStripMenuItem";
            addImagesToolStripMenuItem.Size = new Size(180, 22);
            addImagesToolStripMenuItem.Text = "Add Images...";
            addImagesToolStripMenuItem.Click += addImagesToolStripMenuItem_Click;
            // 
            // exportImageToolStripMenuItem
            // 
            exportImageToolStripMenuItem.Image = Properties.Resources.document_export;
            exportImageToolStripMenuItem.Name = "exportImageToolStripMenuItem";
            exportImageToolStripMenuItem.Size = new Size(180, 22);
            exportImageToolStripMenuItem.Text = "Export Image...";
            exportImageToolStripMenuItem.Click += exportImageToolStripMenuItem_Click;
            // 
            // TPLViewerForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(473, 313);
            Controls.Add(splitContainer1);
            Controls.Add(menuStrip1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            Name = "TPLViewerForm";
            Text = "TPL Viewer";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            imageStatusStrip.ResumeLayout(false);
            imageStatusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)imagePreview).EndInit();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private SplitContainer splitContainer1;
        private PictureBox imagePreview;
        private ListBox listBox1;
        private StatusStrip imageStatusStrip;
        private ToolStripStatusLabel imageFormatLabel;
        private ToolStripStatusLabel imageSizeLabel;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem addImagesToolStripMenuItem;
        private ToolStripMenuItem exportImageToolStripMenuItem;
    }
}