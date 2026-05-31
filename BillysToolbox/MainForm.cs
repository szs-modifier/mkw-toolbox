using BillysToolbox.Editors;
using BillysToolbox.Tools.ImageScaler;
using kartlib.Serial;
using System.Text;

namespace BillysToolbox
{
    public partial class MainForm : Form
    {
        public Dictionary<string, string> FileTypes = new Dictionary<string, string>()
        {
            { "SZS Files (*.szs)", "*.szs" },
            { "ARC Files (*.arc, *.u8)", "*.arc;*.u8" },
            { "BMM Files (*.bmm)", "*.bmm" },
            { "KMP Files (*.kmp)", "*.kmp" },
            { "BLIGHT Files (*.blight)", "*.blight" },
            { "BDOF Files (*.bdof_", "*.bdof" },
            { "KCL Files (*.kcl)", "*.kcl" },
            { "TPL Files (*.tpl)", "*.tpl" },
            { "All Files (*.*)", "*.*" },
        };
        public List<KeyValuePair<string, byte[]>> Clipboard = new List<KeyValuePair<string, byte[]>>();

        public MainForm()
        {
            InitializeComponent();
        }

        private void OpenFileEditor()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            StringBuilder filter = new StringBuilder();

            foreach (KeyValuePair<string, string> type in FileTypes)
            {
                if (filter.ToString().CompareTo("") != 0)
                    filter.Append("|");

                filter.Append(type.Key);
                filter.Append("|");
                filter.Append(type.Value);
            }
            ofd.Filter = filter.ToString();

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Form? editor = EditorFactory.GetEditor(ofd.FileName);
                if (editor != null)
                {
                    editor.Show();
                    editor.MdiParent = this;
                }
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileEditor();
        }

        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            OpenFileEditor();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IEditor? activeEditor = (IEditor?)ActiveMdiChild;
            if (activeEditor == null) return;

            activeEditor.SaveAs();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IEditor? activeEditor = (IEditor?)ActiveMdiChild;
            if (activeEditor == null) return;

            activeEditor.Save();
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            IEditor? activeEditor = (IEditor?)ActiveMdiChild;
            if (activeEditor == null) return;

            activeEditor.Save();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            IEditor? activeEditor = (IEditor?)ActiveMdiChild;
            if (activeEditor == null) return;

            activeEditor.SaveAs();
        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AboutForm aboutForm = new AboutForm();
            aboutForm.ShowDialog();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                Form? editor = EditorFactory.GetEditor(args[1]);
                if (editor == null)
                {
                    MessageBox.Show("Unsupported file type!");
                    return;
                }

                editor.MdiParent = this;
                editor.WindowState = FormWindowState.Maximized;
                editor.Show();
            }
        }

        private void u8ArchiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            U8 u8 = new U8();
            U8EditorForm? editor = new U8EditorForm(u8);
            if (editor != null)
            {
                editor.MdiParent = this;
                editor.Show();
            }
        }

        private void kCLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KCL kcl = new KCL();
            KCLEditorForm? editor = new KCLEditorForm(kcl);
            if (editor != null)
            {
                editor.MdiParent = this;
                editor.Show();

                ImportOBJ importOBJ = new ImportOBJ();
                if (importOBJ.ShowDialog() == DialogResult.OK)
                {
                    if (importOBJ.OBJ != null)
                        editor.ParseOBJ(importOBJ.OBJ);
                }
            }
        }

        private void bLIGHTToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            BLIGHT blight = new BLIGHT();
            BLIGHTEditorForm? editor = new BLIGHTEditorForm(blight);
            if (editor != null)
            {
                editor.MdiParent = this;
                editor.Show();
            }

        }

        private void bDOFToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            BDOF bdof = new BDOF();
            BDOFEditorForm? editor = new BDOFEditorForm(bdof);
            if (editor != null)
            {
                editor.MdiParent = this;
                editor.Show();
            }

        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void bMMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BMM bmm = new BMM();
            BMMEditorForm? editor = new(bmm);
            if (editor != null)
            {
                editor.MdiParent = this;
                editor.Show();
            }
        }

        private void kMPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KMP kmp = new();
            KMPEditorForm? editor = new(kmp);
            if (editor != null)
            {
                editor.MdiParent = this;
                editor.Show();
            }
        }

        private void nImageScalerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImageScalerForm? imageScalerForm = new ImageScalerForm();
            if (imageScalerForm != null)
            {
                imageScalerForm.MdiParent = this;
                imageScalerForm.Show();
            }
        }

        private void tPLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TPL tpl = new TPL();
            TPLViewerForm? editor = new(tpl);
            if (editor != null)
            {
                editor.MdiParent = this;
                editor.Show();
            }
        }
    }
}