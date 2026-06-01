using DrawLib.Shapes;
using kartlib.Serial;
using static kartlib.Serial.KMP;

namespace KMP_Editor.Control.Nodes
{
    public class ITPHNode : Node
    {
        public KMP KMP;
        public _Section<_ITPH> ITPH;
        public _Section<_ITPT> ITPT;
        public List<ITPTVertex> Vertices { get; private set; }

        private Viewport2D _viewport;

        public ITPHNode(KMP kmp, Viewport2D viewport)
        {
            KMP = kmp;
            ITPH = kmp.ITPH;
            ITPT = kmp.ITPT;
            _viewport = viewport;
            Vertices = new List<ITPTVertex>();

            foreach(_ITPT i in ITPT.Entries)
            {
                float x = i.Position.X;
                float y = i.Position.Z;
                Vector2f pos = new Vector2f(x, y);
                Vertices.Add(new ITPTVertex(i, new DraggableVertex(pos, viewport)));
            }
        }

        public override List<_ISectionEntry> GetData()
        {
            List<_ISectionEntry> result = new List<_ISectionEntry>();
            for (int i = 0; i < ITPH.Length(); i++)
            {
                result.Add(ITPH.GetEntry(i));
            }
            return result;
        }

        public override string GetTitle(int index)
        {
            return "Group " + index;
        }

        public override void AddEntry()
        {
            if (ITPH.Length() <= 0)
            {
                ITPH.AddEntry();
                return;
            }

            _ITPH lastEntry = (_ITPH)ITPH.GetEntry(ITPH.Length() - 1);
            if (lastEntry.Start == byte.MaxValue)
                return;

            _ITPH newEntry = (_ITPH)ITPH.AddEntry();
            newEntry.Start = (byte)(lastEntry.Start + lastEntry.Length);
        }

        public override void RemoveEntry(int index)
        {
            _ITPH removedNode = (_ITPH)ITPH.GetEntry(index);
            byte start = removedNode.Start;
            for (int i = start; i < removedNode.Length + start; i++)
            {
                ITPT.RemoveEntry(start);
            }
            ITPH.RemoveEntry(index);

            byte position = start;
            for (int i = index; i < ITPH.Length(); i++)
            {
                _ITPH current = (_ITPH)ITPH.GetEntry(i);
                current.Start = position;
                position += current.Length;
            }
        }

        public override void Populate(TreeNode node)
        {
            base.Populate(node);
            node.Nodes.Clear();
            for (int i = 0; i < GetData().Count; i++)
            {
                ITPHGroupNode itphGroupNode = new ITPHGroupNode(KMP, i, _viewport);
                TreeNode treeNode = new TreeNode("Group " + i);
                treeNode.Tag = itphGroupNode;
                treeNode.ImageIndex = 2;
                treeNode.SelectedImageIndex = 2;
                node.Nodes.Add(treeNode);
            }
        }

        public override void AddShapes(int selectedIndex)
        {
            for (int i = 0; i < ITPH.Length(); i++)
            {
                ITPHGroupNode groupNode = new ITPHGroupNode(KMP, i, _viewport);
                groupNode.AddShapes(i == selectedIndex ? ITPHGroupNode.HighlightAllPoints : -1);
            }
        }
    }

    public class ITPHGroupNode : Node
    {
        internal const int HighlightAllPoints = -2;
        public _Section<_ITPT> ITPT { get; private set; }
        public _ITPH ITPH { get; private set; }
        public ITPHPath Path { get; private set; }
        public List<ITPTVertex> Vertices { get; private set; }

        private Viewport2D _viewport;

        public ITPHGroupNode(KMP kmp, int index, Viewport2D viewport)
        {
            ITPT = kmp.ITPT;
            ITPH = (_ITPH)kmp.ITPH.GetEntry(index);
            _viewport = viewport;
            Vertices = new List<ITPTVertex>();

            for(int i = 0; i < ITPT.Length(); i++)
            {
                _ITPT entry = ITPT.Entries[i];
                Vector2f pos = new Vector2f(entry.Position.X, entry.Position.Z);
                Vertices.Add(new ITPTVertex(ITPT.Entries[i], new DraggableVertex(pos, viewport)));
            }

            List<Vector2f> path_entries = new List<Vector2f>();
            foreach(_ITPT e in GetData())
            {
                Vector2f v = new Vector2f(e.Position.X, e.Position.Z);
                path_entries.Add(v);
            }
            Path = new ITPHPath(this, new DraggablePath(path_entries, viewport));
            Path.Path.FillColor = Color.Blue;

        }

        public override List<_ISectionEntry> GetData()
        {
            List<_ISectionEntry> result = new List<_ISectionEntry>();
            for (int i = ITPH.Start; i < ITPH.Start + ITPH.Length; i++)
            {
                result.Add(ITPT.GetEntry(i));
            }
            return result;
        }

        public override string GetTitle(int index)
        {
            return "Point " + index;
        }

        public override void AddEntry()
        {
            if (ITPH.Length + 1 == byte.MaxValue)
                return;

            _ITPT newEntry = (_ITPT)ITPT.AddEntry(ITPH.Start + ITPH.Length);
            Vector2f pos = new Vector2f(newEntry.Position.X, newEntry.Position.Z);
            this.Path.Path._endpoints.Add(new DraggableVertex(pos, _viewport));
            this.Path.Path.Vertices.Add(pos);
            ITPH.Length++;
        }

        public override void RemoveEntry(int index)
        {
            ITPT.RemoveEntry(ITPH.Start + index);
            ITPH.Length--;
        }

        public override void AddShapes(int selectedIndex)
        {
            Path.Path.FillColor = selectedIndex == HighlightAllPoints ? KmpViewportSync.HighlightColor : Color.Blue;
            _viewport.AddShape(Path.Path);

            if (selectedIndex >= 0 && selectedIndex < Path.Path.Vertices.Count)
                _viewport.AddShape(KmpViewportSync.HighlightAt(Path.Path.Vertices[selectedIndex]));
        }
    }

    public class ITPHPath
    {
        public ITPHGroupNode Group { get; private set; }
        public DraggablePath Path { get; private set; }

        public ITPHPath(ITPHGroupNode group, DraggablePath path)
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
                    float y = ((_ITPT)entries[i]).Position.Y;
                    float z = Path.Vertices[i].Y;
                    ((_ITPT)entries[i]).Position = new Vector3f(x, y, z);
                }
            }
        }
    }

    public class ITPTVertex
    {
        public _ITPT Entry { get; private set; }
        public DraggableVertex Vertex { get; private set; }

        public ITPTVertex(_ITPT entry, DraggableVertex vertex)
        {
            Entry = entry;
            Vertex = vertex;

            vertex._viewport.MouseUp += OnMouseUp;
        }

        private void OnMouseUp(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && !Vertex._dragging)
            {
                float x = Vertex.Vertices[0].X;
                float y = Entry.Position.Y;
                float z = Vertex.Vertices[0].Y;
                Entry.Position = new Vector3f(x, y, z);
            }
        }
    }
}
