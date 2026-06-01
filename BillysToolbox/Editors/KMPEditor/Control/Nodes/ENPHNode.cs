using DrawLib.Shapes;
using kartlib.Serial;
using static kartlib.Serial.KMP;

namespace KMP_Editor.Control.Nodes
{
    public class ENPHNode : Node
    {
        public KMP KMP { get; private set; }
        public _Section<_ENPH> ENPH { get; private set; }
        public _Section<_ENPT> ENPT { get; private set; }
        public List<ENPTVertex> Vertices { get; private set; }

        private Viewport2D _viewport;

        public ENPHNode(KMP kmp, Viewport2D viewport)
        {
            KMP = kmp;
            ENPH = kmp.ENPH;
            ENPT = kmp.ENPT;
            _viewport = viewport;
            Vertices = new List<ENPTVertex>();

            foreach(_ENPT e in ENPT.Entries)
            {
                float x = e.Position.X;
                float y = e.Position.Z;
                Vector2f pos = new Vector2f(x, y);
                Vertices.Add(new ENPTVertex(e, new DraggableVertex(pos, viewport)));
            }
        }

        public override List<_ISectionEntry> GetData()
        {
            List<_ISectionEntry> result = new List<_ISectionEntry>();
            for (int i = 0; i < ENPH.Length(); i++)
            {
                result.Add(ENPH.GetEntry(i));
            }
            return result;
        }

        public override string GetTitle(int index)
        {
            return "Group " + index;
        }

        public override void AddEntry()
        {
            if (ENPH.Length() <= 0)
            {
                ENPH.AddEntry();
                return;
            }

            _ENPH lastEntry = (_ENPH)ENPH.GetEntry(ENPH.Length() - 1);
            if (lastEntry.Start == byte.MaxValue)
                return;

            _ENPH newEntry = (_ENPH)ENPH.AddEntry();
            newEntry.Start = (byte)(lastEntry.Start + lastEntry.Length);
        }

        public override void RemoveEntry(int index)
        {
            _ENPH removedNode = (_ENPH)ENPH.GetEntry(index);
            byte start = removedNode.Start;
            for (int i = start; i < removedNode.Length + start; i++)
            {
                ENPT.RemoveEntry(start);
            }
            ENPH.RemoveEntry(index);

            byte position = start;
            for (int i = index; i < ENPH.Length(); i++)
            {
                _ENPH current = (_ENPH)ENPH.GetEntry(i);
                current.Start = position;
                position += current.Length;
            }
        }

        public override void Populate(TreeNode node)
        {
            List<_ISectionEntry> enphData = GetData();

            node.Nodes.Clear();
            node.Tag = this;
            for (int i = 0; i < enphData.Count; i++)
            {
                ENPHGroupNode enphGroupNode = new ENPHGroupNode(KMP, i, _viewport);

                TreeNode treeNode = new TreeNode("Group " + i);
                treeNode.Tag = enphGroupNode;
                treeNode.ImageIndex = 1;
                treeNode.SelectedImageIndex = 1;

                node.Nodes.Add(treeNode);
            }
        }

        public override void AddShapes(int selectedIndex)
        {
            for (int i = 0; i < ENPH.Length(); i++)
            {
                ENPHGroupNode groupNode = new ENPHGroupNode(KMP, i, _viewport);
                groupNode.AddShapes(i == selectedIndex ? ENPHGroupNode.HighlightAllPoints : -1);
            }
        }
    }

    public class ENPHGroupNode : Node
    {
        internal const int HighlightAllPoints = -2;
        public _Section<_ENPT> ENPT { get; private set; }
        public _ENPH ENPH { get; private set; }
        public ENPHPath Path { get; private set; }
        public List<ENPTVertex> Vertices { get; private set; }

        private Viewport2D _viewport;

        public ENPHGroupNode(KMP kmp, int index, Viewport2D viewport)
        {
            ENPT = kmp.ENPT;
            ENPH = (_ENPH)kmp.ENPH.GetEntry(index);
            _viewport = viewport;
            Vertices = new List<ENPTVertex>();

            for(int i = 0; i < ENPT.Length(); i++)
            {
                _ENPT entry = ENPT.Entries[i];
                Vector2f pos = new Vector2f(entry.Position.X, entry.Position.Z);
                Vertices.Add(new ENPTVertex(ENPT.Entries[i], new DraggableVertex(pos, viewport)));
            }

            List<Vector2f> path_entries = new List<Vector2f>();
            foreach(_ENPT e in GetData())
            {
                Vector2f v = new Vector2f(e.Position.X, e.Position.Z);
                path_entries.Add(v);
            }
            Path = new ENPHPath(this, new DraggablePath(path_entries, viewport));
            Path.Path.FillColor = Color.Blue;

        }

        public override List<_ISectionEntry> GetData()
        {
            List<_ISectionEntry> result = new List<_ISectionEntry>();
            for (int i = ENPH.Start; i < ENPH.Start + ENPH.Length; i++)
            {
                result.Add(ENPT.GetEntry(i));
            }
            return result;
        }

        public override string GetTitle(int index)
        {
            return "Point " + index;
        }

        public override void AddEntry()
        {
            if (ENPH.Length + 1 == byte.MaxValue)
                return;

            _ENPT newEntry = (_ENPT)ENPT.AddEntry(ENPH.Start + ENPH.Length);
            Vector2f pos = new Vector2f(newEntry.Position.X, newEntry.Position.Z);
            Path.Path._endpoints.Add(new DraggableVertex(pos, _viewport));
            Path.Path.Vertices.Add(pos);
            ENPH.Length++;
        }

        public override void RemoveEntry(int index)
        {
            ENPT.RemoveEntry(ENPH.Start + index);
            ENPH.Length--;
        }

        public override void AddShapes(int selectedIndex)
        {
            Path.Path.FillColor = selectedIndex == HighlightAllPoints ? KmpViewportSync.HighlightColor : Color.Blue;
            _viewport.AddShape(Path.Path);

            if (selectedIndex >= 0 && selectedIndex < Path.Path.Vertices.Count)
                _viewport.AddShape(KmpViewportSync.HighlightAt(Path.Path.Vertices[selectedIndex]));
        }
    }

    public class ENPHPath
    {
        public ENPHGroupNode Group { get; private set; }
        public DraggablePath Path { get; private set; }

        public ENPHPath(ENPHGroupNode group, DraggablePath path)
        {
            Group = group;
            Path = path;

            path._viewport.MouseUp += OnMouseUp;
        }

        private void OnMouseUp(object? sender, MouseEventArgs e)
        {
            List<_ISectionEntry> entries = Group.GetData();
            if (e.Button == MouseButtons.Left && !Path._dragging)
            {
                for (int i = 0; i < entries.Count; i++)
                {
                    float x = Path.Vertices[i].X;
                    float y = ((_ENPT)entries[i]).Position.Y;
                    float z = Path.Vertices[i].Y;
                    ((_ENPT)entries[i]).Position = new Vector3f(x, y, z);
                }
            }
        }
    }

    public class ENPTVertex
    {
        public _ENPT Entry { get; private set; }
        public DraggableVertex Vertex { get; private set; }

        public ENPTVertex(_ENPT entry, DraggableVertex vertex)
        {
            Entry = entry;
            Vertex = vertex;

            vertex._viewport.MouseUp += OnMouseUp;
        }

        private void OnMouseUp(object? sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left && !Vertex._dragging)
            {
                float x = Vertex.Vertices[0].X;
                float y = Entry.Position.Y;
                float z = Vertex.Vertices[0].Y;
                Entry.Position = new Vector3f(x, y, z);
            }
        }
    }
}
