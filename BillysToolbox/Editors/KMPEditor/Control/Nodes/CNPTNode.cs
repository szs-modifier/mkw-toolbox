using DrawLib.Shapes;
using kartlib.Serial;
using static kartlib.Serial.KMP;

namespace KMP_Editor.Control.Nodes
{
    public class CNPTNode : Node
    {
        public _Section<_CNPT> CNPT { get; set; }
        private readonly Viewport2D _viewport;
        private readonly List<EntryPivotVertex<_CNPT>> _vertices;

        public CNPTNode(KMP kmp, Viewport2D viewport)
        {
            CNPT = kmp.CNPT;
            _viewport = viewport;
            _vertices = new List<EntryPivotVertex<_CNPT>>();
            RebuildVertices();
        }

        public override List<_ISectionEntry> GetData()
        {
            List<_ISectionEntry> result = new List<_ISectionEntry>();
            foreach (_CNPT entry in CNPT.Entries)
                result.Add(entry);

            return result;
        }

        public override string GetTitle(int index)
        {
            return "Cannon " + index;
        }

        public override void AddEntry()
        {
            _CNPT entry = (_CNPT)CNPT.AddEntry();
            AddVertex(entry);
        }

        public override void RemoveEntry(int index)
        {
            CNPT.RemoveEntry(index);
            _vertices.RemoveAt(index);
        }

        public override void AddShapes(int selectedIndex)
        {
            for (int i = 0; i < _vertices.Count; i++)
            {
                _vertices[i].Vertex.FillColor = i == selectedIndex ? KmpViewportSync.HighlightColor : Color.OrangeRed;
                _viewport.AddShape(_vertices[i].Vertex);
            }
        }

        private void RebuildVertices()
        {
            _vertices.Clear();
            foreach (_CNPT entry in CNPT.Entries)
                AddVertex(entry);
        }

        private void AddVertex(_CNPT entry)
        {
            DraggableVertexPivotArrow vertex = new DraggableVertexPivotArrow(
                KmpViewportSync.ToVector2(entry.Position),
                (int)entry.Rotation.Y,
                _viewport);
            vertex.FillColor = Color.OrangeRed;
            _vertices.Add(new EntryPivotVertex<_CNPT>(
                entry,
                vertex,
                value => value.Position,
                (value, position) => value.Position = position,
                value => value.Rotation,
                (value, rotation) => value.Rotation = rotation));
        }
    }
}
