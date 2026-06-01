using DrawLib.Shapes;
using kartlib.Serial;
using static kartlib.Serial.KMP;

namespace KMP_Editor.Control.Nodes
{
    public class AREANode : Node
    {
        public _Section<_AREA> AREA { get; set; }
        private readonly Viewport2D _viewport;
        private readonly List<EntryPivotVertex<_AREA>> _vertices;

        public AREANode(KMP kmp, Viewport2D viewport)
        {
            AREA = kmp.AREA;
            _viewport = viewport;
            _vertices = new List<EntryPivotVertex<_AREA>>();
            RebuildVertices();
        }

        public override List<_ISectionEntry> GetData()
        {
            List<_ISectionEntry> result = new List<_ISectionEntry>();
            foreach (_AREA entry in AREA.Entries)
                result.Add(entry);

            return result;
        }

        public override string GetTitle(int index)
        {
            return "Area " + index;
        }

        public override void AddEntry()
        {
            _AREA entry = (_AREA)AREA.AddEntry();
            AddVertex(entry);
        }

        public override void RemoveEntry(int index)
        {
            AREA.RemoveEntry(index);
            _vertices.RemoveAt(index);
        }

        public override void AddShapes(int selectedIndex)
        {
            for (int i = 0; i < _vertices.Count; i++)
            {
                _vertices[i].Vertex.FillColor = i == selectedIndex ? KmpViewportSync.HighlightColor : Color.MediumPurple;
                _viewport.AddShape(_vertices[i].Vertex);
            }
        }

        private void RebuildVertices()
        {
            _vertices.Clear();
            foreach (_AREA entry in AREA.Entries)
                AddVertex(entry);
        }

        private void AddVertex(_AREA entry)
        {
            DraggableVertexPivotArrow vertex = new DraggableVertexPivotArrow(
                KmpViewportSync.ToVector2(entry.Position),
                (int)entry.Rotation.Y,
                _viewport);
            vertex.FillColor = Color.MediumPurple;
            _vertices.Add(new EntryPivotVertex<_AREA>(
                entry,
                vertex,
                value => value.Position,
                (value, position) => value.Position = position,
                value => value.Rotation,
                (value, rotation) => value.Rotation = rotation));
        }
    }
}
