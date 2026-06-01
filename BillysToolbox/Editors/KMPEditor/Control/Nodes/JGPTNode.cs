using DrawLib.Shapes;
using kartlib.Serial;
using static kartlib.Serial.KMP;

namespace KMP_Editor.Control.Nodes
{
    public class JGPTNode : Node
    {
        public _Section<_JGPT> JGPT { get; set; }
        private readonly Viewport2D _viewport;
        private readonly List<EntryPivotVertex<_JGPT>> _vertices;

        public JGPTNode(KMP kmp, Viewport2D viewport)
        {
            JGPT = kmp.JGPT;
            _viewport = viewport;
            _vertices = new List<EntryPivotVertex<_JGPT>>();
            RebuildVertices();
        }

        public override List<_ISectionEntry> GetData()
        {
            List<_ISectionEntry> result = new List<_ISectionEntry>();
            foreach (_JGPT entry in JGPT.Entries)
                result.Add(entry);

            return result;
        }

        public override string GetTitle(int index)
        {
            return "Respawn " + index;
        }

        public override void AddEntry()
        {
            _JGPT entry = (_JGPT)JGPT.AddEntry();
            AddVertex(entry);
        }

        public override void RemoveEntry(int index)
        {
            JGPT.RemoveEntry(index);
            _vertices.RemoveAt(index);
        }

        public override void AddShapes(int selectedIndex)
        {
            for (int i = 0; i < _vertices.Count; i++)
            {
                _vertices[i].Vertex.FillColor = i == selectedIndex ? KmpViewportSync.HighlightColor : Color.LimeGreen;
                _viewport.AddShape(_vertices[i].Vertex);
            }
        }

        private void RebuildVertices()
        {
            _vertices.Clear();
            foreach (_JGPT entry in JGPT.Entries)
                AddVertex(entry);
        }

        private void AddVertex(_JGPT entry)
        {
            DraggableVertexPivotArrow vertex = new DraggableVertexPivotArrow(
                KmpViewportSync.ToVector2(entry.Position),
                (int)entry.Rotation.Y,
                _viewport);
            vertex.FillColor = Color.LimeGreen;
            _vertices.Add(new EntryPivotVertex<_JGPT>(
                entry,
                vertex,
                value => value.Position,
                (value, position) => value.Position = position,
                value => value.Rotation,
                (value, rotation) => value.Rotation = rotation));
        }
    }
}
