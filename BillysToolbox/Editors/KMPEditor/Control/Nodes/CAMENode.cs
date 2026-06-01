using DrawLib.Shapes;
using kartlib.Serial;
using static kartlib.Serial.KMP;

namespace KMP_Editor.Control.Nodes
{
    public class CAMENode : Node
    {
        public _Section<_CAME> CAME { get; set; }
        private readonly Viewport2D _viewport;
        private readonly List<EntryPivotVertex<_CAME>> _vertices;

        public CAMENode(KMP kmp, Viewport2D viewport)
        {
            CAME = kmp.CAME;
            _viewport = viewport;
            _vertices = new List<EntryPivotVertex<_CAME>>();
            RebuildVertices();
        }

        public override List<_ISectionEntry> GetData()
        {
            List<_ISectionEntry> result = new List<_ISectionEntry>();
            foreach (_CAME entry in CAME.Entries)
                result.Add(entry);

            return result;
        }

        public override string GetTitle(int index)
        {
            return "Camera " + index;
        }

        public override void AddEntry()
        {
            _CAME entry = (_CAME)CAME.AddEntry();
            AddVertex(entry);
        }

        public override void RemoveEntry(int index)
        {
            CAME.RemoveEntry(index);
            _vertices.RemoveAt(index);
        }

        public override void AddShapes(int selectedIndex)
        {
            for (int i = 0; i < _vertices.Count; i++)
            {
                _vertices[i].Vertex.FillColor = i == selectedIndex ? KmpViewportSync.HighlightColor : Color.CornflowerBlue;
                _viewport.AddShape(_vertices[i].Vertex);
            }
        }

        private void RebuildVertices()
        {
            _vertices.Clear();
            foreach (_CAME entry in CAME.Entries)
                AddVertex(entry);
        }

        private void AddVertex(_CAME entry)
        {
            DraggableVertexPivotArrow vertex = new DraggableVertexPivotArrow(
                KmpViewportSync.ToVector2(entry.Position),
                (int)entry.Rotation.Y,
                _viewport);
            vertex.FillColor = Color.CornflowerBlue;
            _vertices.Add(new EntryPivotVertex<_CAME>(
                entry,
                vertex,
                value => value.Position,
                (value, position) => value.Position = position,
                value => value.Rotation,
                (value, rotation) => value.Rotation = rotation));
        }
    }
}
