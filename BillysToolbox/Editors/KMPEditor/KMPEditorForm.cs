using KMP_Editor.Control.Nodes;
using KMP_Editor.Control;
using kartlib.Serial;
using System.Text;

namespace BillysToolbox.Editors
{
    public partial class KMPEditorForm : Form, IEditor
    {
        private KMP? FileInstance;
        private Node? SelectedNode;
        private U8? ParentInstance;
        private KclBackgroundModel? BackgroundModel;
        private bool UnsavedChanges;

        public KMPEditorForm(KMP kmp)
        {
            UnsavedChanges = false;
            FileInstance = kmp;

            InitializeComponent();
            InitializeUI();

            InitNodes();
            LoadDefaultBackgroundModel();
            PopulateUI();
        }

        public KMPEditorForm(KMP kmp, U8? parentInstance)
        {
            UnsavedChanges = false;
            FileInstance = kmp;
            ParentInstance = parentInstance;

            InitializeComponent();
            InitializeUI();

            InitNodes();
            LoadDefaultBackgroundModel();
            PopulateUI();
        }

        // Helper functions

        public void SaveAs()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = Path.GetFileNameWithoutExtension(FileInstance.Filename);
            sfd.Filter = "KMP Files (*.kmp)|*.kmp";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                byte[] buffer = FileInstance.Write();
                File.WriteAllBytes(sfd.FileName, buffer);
            }
        }

        public void Save()
        {
            if (ParentInstance != null)
            {
                int index = ParentInstance.FindIndexFromName(FileInstance.Filename);
                if (index > 0)
                {
                    ParentInstance.Nodes[index].Data = FileInstance.Write();
                }
            }
            else if (!File.Exists(FileInstance.Filename))
            {
                SaveAs();
                return;
            }
            else
            {
                byte[] buffer = FileInstance.Write();
                File.WriteAllBytes(FileInstance.Filename, buffer);
            }
        }

        private void InitNodes()
        {
            if (FileInstance == null)
                return;

            sectionTree.Nodes[0].Tag = new KTPTNode(FileInstance, viewport);
            sectionTree.Nodes[1].Tag = new ENPHNode(FileInstance, viewport);
            sectionTree.Nodes[2].Tag = new ITPHNode(FileInstance, viewport);
            sectionTree.Nodes[3].Tag = new CKPHNode(FileInstance, viewport);
            sectionTree.Nodes[4].Tag = new GOBJNode(FileInstance, viewport);
            sectionTree.Nodes[5].Tag = new POTINode(FileInstance, viewport);
            sectionTree.Nodes[6].Tag = new AREANode(FileInstance, viewport);
            sectionTree.Nodes[7].Tag = new CAMENode(FileInstance, viewport);
            sectionTree.Nodes[8].Tag = new JGPTNode(FileInstance, viewport);
            sectionTree.Nodes[9].Tag = new CNPTNode(FileInstance, viewport);
            sectionTree.Nodes[10].Tag = new MSPTNode(FileInstance, viewport);
            sectionTree.Nodes[11].Tag = new STGINode(FileInstance);
        }

        private void InitializeUI()
        {
            // Load tree icons
            ImageList icons = new ImageList();
            icons.Images.Add(Properties.Resources.star);
            icons.Images.Add(Properties.Resources.balloons);
            icons.Images.Add(Properties.Resources.redshell);
            icons.Images.Add(Properties.Resources.flag);
            icons.Images.Add(Properties.Resources.crate);
            icons.Images.Add(Properties.Resources.goomba);
            icons.Images.Add(Properties.Resources.blueshell);
            icons.Images.Add(Properties.Resources.camera);
            icons.Images.Add(Properties.Resources.lakitu);
            icons.Images.Add(Properties.Resources.cannon);
            icons.Images.Add(Properties.Resources.coin);
            icons.Images.Add(Properties.Resources.greenshell);
            icons.ImageSize = new Size(24, 24);
            sectionTree.ImageList = icons;

            for (int i = 0; i < sectionTree.Nodes.Count; i++)
            {
                sectionTree.Nodes[i].ImageIndex = i;
                sectionTree.Nodes[i].SelectedImageIndex = i;
            }
        }

        private void PopulateUI()
        {
            sectionTree.Enabled = true;
            propertyGroupBox.Enabled = true;
            entryGroupBox.Enabled = true;
            viewport.ClearShapes();
            ApplyBackgroundModel();

            foreach (TreeNode node in sectionTree.Nodes)
                if (node.Tag != null) ((Node)node.Tag).Populate(node);
        }

        private void UpdateUI()
        {
            entryListBox.Items.Clear();
            entryPropertyGrid.SelectedObject = null;
            for (int i = 0; i < SelectedNode?.GetData().Count; i++)
            {
                entryListBox.Items.Add(SelectedNode.GetTitle(i));
            }

            if (UnsavedChanges)
            {
                foreach (char c in Text) if (c == '*') return;
                Text += "*";
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                foreach (char c in Text) { if (c != '*') sb.Append(c); }
                Text = sb.ToString();
            }
        }


        private void ApplyBackgroundModel()
        {
            if (BackgroundModel == null)
                viewport.ClearBackgroundShapes();
            else
                viewport.SetBackgroundShapes(BackgroundModel.Shapes);
        }

        private void LoadDefaultBackgroundModel()
        {
            if (ParentInstance == null)
                return;

            U8._Node? kclNode = ParentInstance.Nodes.FirstOrDefault(node =>
                node.Type == U8._Node.NodeType.File &&
                node.Data != null &&
                node.Name.EndsWith(".kcl", StringComparison.OrdinalIgnoreCase));

            if (kclNode?.Data == null)
                return;

            LoadBackgroundModel(kclNode.Data, kclNode.Name);
        }

        private void LoadBackgroundModel(byte[] buffer, string fileName)
        {
            KCL kcl = new KCL(buffer, fileName);
            BackgroundModel = KclBackgroundModel.FromKcl(kcl);
            ApplyBackgroundModel();
            viewport.Invalidate();
        }


        private void CalculateYValues()
        {
            if (FileInstance == null || BackgroundModel == null)
                return;

            int updated = 0;

            Vector3f UpdatePosition(Vector3f position)
            {
                if (BackgroundModel.TryGetHeight(position.X, position.Z, out float y))
                {
                    updated++;
                    return new Vector3f(position.X, y, position.Z);
                }

                return position;
            }

            foreach (KMP._KTPT entry in FileInstance.KTPT.Entries)
                entry.StartPosition = UpdatePosition(entry.StartPosition);

            foreach (KMP._ENPT entry in FileInstance.ENPT.Entries)
                entry.Position = UpdatePosition(entry.Position);

            foreach (KMP._ITPT entry in FileInstance.ITPT.Entries)
                entry.Position = UpdatePosition(entry.Position);

            foreach (KMP._GOBJ entry in FileInstance.GOBJ.Entries)
                entry.Position = UpdatePosition(entry.Position);

            foreach (KMP._POTI route in FileInstance.POTI.Entries)
                foreach (KMP._POTI._Point point in route.Points)
                    point.Position = UpdatePosition(point.Position);

            foreach (KMP._JGPT entry in FileInstance.JGPT.Entries)
                entry.Position = UpdatePosition(entry.Position);

            foreach (KMP._CNPT entry in FileInstance.CNPT.Entries)
                entry.Position = UpdatePosition(entry.Position);

            foreach (KMP._MSPT entry in FileInstance.MSPT.Entries)
                entry.Position = UpdatePosition(entry.Position);

            UnsavedChanges = true;
            InitNodes();
            PopulateUI();
            SelectedNode = sectionTree.SelectedNode?.Tag as Node;
            UpdateUI();
            UpdateShapes();

            MessageBox.Show(
                $"Updated {updated} KMP Y value(s).",
                "Calculate Y values",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void LinkRespawns()
        {
            if (FileInstance == null)
                return;

            if (FileInstance.JGPT.Entries.Count <= 0)
            {
                MessageBox.Show(
                    "No respawn points are available to link.",
                    "Link respawns",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            foreach (KMP._CKPT checkpoint in FileInstance.CKPT.Entries)
            {
                Vector2f center = KmpViewportSync.Center(checkpoint.PositionL, checkpoint.PositionR);
                int nearestIndex = 0;
                float nearestDistance = float.MaxValue;

                for (int i = 0; i < FileInstance.JGPT.Entries.Count; i++)
                {
                    Vector2f respawn = KmpViewportSync.ToVector2(FileInstance.JGPT.Entries[i].Position);
                    float distance = KmpViewportSync.DistanceSquared(center, respawn);
                    if (distance < nearestDistance)
                    {
                        nearestDistance = distance;
                        nearestIndex = i;
                    }
                }

                checkpoint.RespawnID = (byte)Math.Min(byte.MaxValue, nearestIndex);
            }

            UnsavedChanges = true;
            entryPropertyGrid.Refresh();
            UpdateUI();
            UpdateShapes();
        }

        private void UpdateShapes()
        {
            if (SelectedNode == null)
                return;

            viewport.ClearShapes();
            ApplyBackgroundModel();
            SelectedNode.AddShapes(entryListBox.SelectedIndex);
            viewport.Invalidate();
        }

        // Event handlers

        private void openMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "KMP Files (*.kmp)|*.kmp|All Files (*.*)|*.*";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                byte[] buffer = File.ReadAllBytes(ofd.FileName);
                FileInstance = new KMP(buffer, ofd.FileName);
                ParentInstance = null;
                BackgroundModel = null;
                Text = "KMP Editor - " + Path.GetFileName(FileInstance.Filename);
                InitNodes();
                PopulateUI();
                sectionTree.SelectedNode = sectionTree.Nodes[0];
            }
        }


        private void importBackgroundModelMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "KCL Files (*.kcl)|*.kcl|All Files (*.*)|*.*";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    LoadBackgroundModel(File.ReadAllBytes(ofd.FileName), ofd.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "Unable to import background model: " + ex.Message,
                        "Import background model",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }


        private void calculateYValuesMenuItem_Click(object sender, EventArgs e)
        {
            if (BackgroundModel == null)
            {
                MessageBox.Show(
                    "Import a KCL background model before calculating Y values.",
                    "Calculate Y values",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            CalculateYValues();
        }

        private void linkRespawnsMenuItem_Click(object sender, EventArgs e)
        {
            LinkRespawns();
        }

        private void saveMenuItem_Click(object sender, EventArgs e)
        {
            if (FileInstance != null && File.Exists(FileInstance.Filename))
            {
                File.WriteAllBytes(FileInstance.Filename, FileInstance.Write());
                UnsavedChanges = false;
                UpdateUI();
            }
            else if (FileInstance != null)
            {
                saveAsMenuItem_Click(sender, e);
            }
        }

        private void saveAsMenuItem_Click(object sender, EventArgs e)
        {
            if (FileInstance == null)
                return;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "KMP Files (*.kmp)|*.kmp|All Files (*.*)|*.*";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllBytes(sfd.FileName, FileInstance.Write());
                UnsavedChanges = false;
                UpdateUI();
            }
        }

        private void newMenuItem_Click(object sender, EventArgs e)
        {
            FileInstance = new KMP();
            ParentInstance = null;
            BackgroundModel = null;
            Text = "KMP Editor - " + Path.GetFileName(FileInstance.Filename);
            InitNodes();
            PopulateUI();
        }

        private void sectionTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Node node = (Node)sectionTree.SelectedNode.Tag;
            if (node == null)
                return;

            entryListBox.Items.Clear();
            entryPropertyGrid.SelectedObject = null;
            SelectedNode = node;
            UpdateShapes();
            for (int i = 0; i < node.GetData().Count; i++)
            {
                entryListBox.Items.Add(node.GetTitle(i));
            }
            if (entryListBox.Items.Count > 0) entryListBox.SelectedIndex = entryListBox.Items.Count - 1;
            addButton.Enabled = node.CanAddEntries;
            removeButton.Enabled = node.CanRemoveEntries;
        }

        private void entryListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectedNode == null || entryListBox.SelectedIndex < 0)
                return;

            List<KMP._ISectionEntry> data = SelectedNode.GetData();
            entryPropertyGrid.SelectedObject = data[entryListBox.SelectedIndex];
            UpdateShapes();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            if (SelectedNode == null)
                return;

            SelectedNode.AddEntry();
            UnsavedChanges = true;
            UpdateUI();
            PopulateUI();
            UpdateShapes();
            if (entryListBox.Items.Count > 0)
                entryListBox.SelectedIndex = entryListBox.Items.Count - 1;
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            if (SelectedNode == null)
                return;

            if (entryListBox.SelectedIndex < 0)
                return;

            int tmpIndex = entryListBox.SelectedIndex;
            SelectedNode.RemoveEntry(entryListBox.SelectedIndex);
            UnsavedChanges = true;
            UpdateUI();
            PopulateUI();
            UpdateShapes();
            if (entryListBox.Items.Count > 0)
                entryListBox.SelectedIndex = Math.Max(0, tmpIndex - 1);
        }
    }
}
