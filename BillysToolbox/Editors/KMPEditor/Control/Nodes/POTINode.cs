using DrawLib.Shapes;
using kartlib.Serial;
using static kartlib.Serial.KMP;

namespace KMP_Editor.Control.Nodes
{
    public class POTINode : Node
    {
        public _Section<_POTI> POTI { get; set; }
        private readonly Viewport2D _viewport;

        public POTINode(KMP kmp, Viewport2D viewport)
        {
            POTI = kmp.POTI;
            _viewport = viewport;
        }

        public override List<_ISectionEntry> GetData()
        {
            List<_ISectionEntry> result = new List<_ISectionEntry>();
            for (int i = 0; i < POTI.Entries.Count; i++)
                result.Add(POTI.Entries[i]);

            return result;
        }

        public override string GetTitle(int index)
        {
            return "Route " + index;
        }

        public override void AddEntry()
        {
            POTI.AddEntry();
        }

        public override void RemoveEntry(int index)
        {
            POTI.RemoveEntry(index);
        }

        public override void Populate(TreeNode node)
        {
            List<_ISectionEntry> data = GetData();

            node.Nodes.Clear();
            node.Tag = this;
            for (int i = 0; i < data.Count; i++)
            {
                POTIPointNode point = new POTIPointNode((_POTI)data[i], _viewport);
                TreeNode treeNode = new TreeNode("Group " + i);
                treeNode.Tag = point;
                treeNode.ImageIndex = 5;
                treeNode.SelectedImageIndex = 5;
                node.Nodes.Add(treeNode);
            }
        }

        public override void AddShapes(int selectedIndex)
        {
            for (int i = 0; i < POTI.Entries.Count; i++)
            {
                POTIPointNode point = new POTIPointNode(POTI.Entries[i], _viewport);
                point.AddShapes(i == selectedIndex ? POTIPointNode.HighlightAllPoints : -1);
            }
        }
    }

    public class POTIPointNode : Node
    {
        internal const int HighlightAllPoints = -2;
        private readonly _POTI POTI;
        private readonly Viewport2D _viewport;
        private DraggablePath Path;

        public POTIPointNode(_POTI poti, Viewport2D viewport)
        {
            POTI = poti;
            _viewport = viewport;
            Path = CreatePath();
            Path._viewport.MouseUp += OnPathMouseUp;
        }

        public override List<_ISectionEntry> GetData()
        {
            List<_ISectionEntry> result = new List<_ISectionEntry>();
            foreach (_POTI._Point point in POTI.Points)
                result.Add(point);

            return result;
        }

        public override string GetTitle(int index)
        {
            return "Point " + index;
        }

        public override void AddEntry()
        {
            _POTI._Point point = new _POTI._Point();
            POTI.PointCount++;
            POTI.Points.Add(point);
            Vector2f pos = KmpViewportSync.ToVector2(point.Position);
            Path._endpoints.Add(new DraggableVertex(pos, _viewport));
            Path.Vertices.Add(pos);
        }

        public override void RemoveEntry(int index)
        {
            POTI.PointCount--;
            POTI.Points.RemoveAt(index);
            Path = CreatePath();
            Path._viewport.MouseUp += OnPathMouseUp;
        }

        public override void AddShapes(int selectedIndex)
        {
            Path.FillColor = selectedIndex == HighlightAllPoints ? KmpViewportSync.HighlightColor : Color.DarkOrange;
            _viewport.AddShape(Path);

            if (selectedIndex >= 0 && selectedIndex < Path.Vertices.Count)
                _viewport.AddShape(KmpViewportSync.HighlightAt(Path.Vertices[selectedIndex]));
        }

        private DraggablePath CreatePath()
        {
            List<Vector2f> vertices = new List<Vector2f>();
            foreach (_POTI._Point point in POTI.Points)
                vertices.Add(KmpViewportSync.ToVector2(point.Position));

            DraggablePath path = new DraggablePath(vertices, _viewport);
            path.FillColor = Color.DarkOrange;
            return path;
        }

        private void OnPathMouseUp(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && !Path._dragging)
            {
                for (int i = 0; i < POTI.Points.Count && i < Path.Vertices.Count; i++)
                    POTI.Points[i].Position = KmpViewportSync.WithXZ(POTI.Points[i].Position, Path.Vertices[i]);
            }
        }
    }
}
