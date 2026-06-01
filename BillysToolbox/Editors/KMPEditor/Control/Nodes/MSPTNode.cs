using DrawLib.Shapes;
using kartlib.Serial;
using static kartlib.Serial.KMP;

namespace KMP_Editor.Control.Nodes
{
    public class MSPTNode : Node
    {
        public _Section<_MSPT> MSPT { get; set; }
        private readonly Viewport2D _viewport;
        private readonly List<EntryPivotVertex<_MSPT>> _vertices;

        public MSPTNode(KMP kmp, Viewport2D viewport)
        {
            MSPT = kmp.MSPT;
            _viewport = viewport;
            _vertices = new List<EntryPivotVertex<_MSPT>>();
            RebuildVertices();
        }

        public override List<_ISectionEntry> GetData()
        {
            List<_ISectionEntry> result = new List<_ISectionEntry>();
            foreach (_MSPT entry in MSPT.Entries)
                result.Add(entry);

            return result;
        }

        public override string GetTitle(int index)
        {
            return "Point " + index;
        }

        public override void AddEntry()
        {
            _MSPT entry = (_MSPT)MSPT.AddEntry();
            AddVertex(entry);
        }

        public override void RemoveEntry(int index)
        {
            MSPT.RemoveEntry(index);
            _vertices.RemoveAt(index);
        }

        public override void AddShapes(int selectedIndex)
        {
            for (int i = 0; i < _vertices.Count; i++)
            {
                _vertices[i].Vertex.FillColor = i == selectedIndex ? KmpViewportSync.HighlightColor : Color.MediumSeaGreen;
                _viewport.AddShape(_vertices[i].Vertex);
            }
        }

        private void RebuildVertices()
        {
            _vertices.Clear();
            foreach (_MSPT entry in MSPT.Entries)
                AddVertex(entry);
        }

        private void AddVertex(_MSPT entry)
        {
            DraggableVertexPivotArrow vertex = new DraggableVertexPivotArrow(
                KmpViewportSync.ToVector2(entry.Position),
                (int)entry.Rotation.Y,
                _viewport);
            vertex.FillColor = Color.MediumSeaGreen;
            _vertices.Add(new EntryPivotVertex<_MSPT>(
                entry,
                vertex,
                value => value.Position,
                (value, position) => value.Position = position,
                value => value.Rotation,
                (value, rotation) => value.Rotation = rotation));
        }
    }
}
