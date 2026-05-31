namespace BillysToolbox
{
    partial class AboutForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            logoPictureBox = new PictureBox();
            label1 = new Label();
            richTextBox1 = new RichTextBox();
            ((System.ComponentModel.ISupportInitialize)logoPictureBox).BeginInit();
            SuspendLayout();
            // 
            // logoPictureBox
            // 
            logoPictureBox.BackgroundImage = (Image)resources.GetObject("logoPictureBox.BackgroundImage");
            logoPictureBox.BackgroundImageLayout = ImageLayout.Stretch;
            logoPictureBox.Location = new Point(282, 9);
            logoPictureBox.Name = "logoPictureBox";
            logoPictureBox.Size = new Size(54, 54);
            logoPictureBox.TabIndex = 0;
            logoPictureBox.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft Sans Serif", 23.9999962F, FontStyle.Bold);
            label1.Location = new Point(53, 18);
            label1.Name = "label1";
            label1.Size = new Size(229, 37);
            label1.TabIndex = 1;
            label1.Text = "MKW Toolbox";
            // 
            // richTextBox1
            // 
            richTextBox1.Location = new Point(12, 72);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.ReadOnly = true;
            richTextBox1.Size = new Size(363, 65);
            richTextBox1.TabIndex = 2;
            richTextBox1.Text = "MKW Toolbox is a program created by BillyNoodles. Inspired by Wexos's Toolbox, it uses MDI windows to display its editors.";
            // 
            // AboutForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(387, 149);
            Controls.Add(richTextBox1);
            Controls.Add(label1);
            Controls.Add(logoPictureBox);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "AboutForm";
            Text = "About";
            ((System.ComponentModel.ISupportInitialize)logoPictureBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox logoPictureBox;
        private Label label1;
        private RichTextBox richTextBox1;
    }
}