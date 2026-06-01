using DrawLib.Shapes;
using kartlib.Serial;
using static kartlib.Serial.KMP;

namespace KMP_Editor.Control.Nodes
{
    public class KTPTNode : Node
    {
        public _Section<_KTPT>  KTPT { get; private set; }
        public List<KTPTVertex> Vertices { get; private set; }

        private Viewport2D      Viewport;

        public KTPTNode(KMP kmp, Viewport2D viewport)
        {
            KTPT = kmp.KTPT;
            Vertices = new List<KTPTVertex>();
            Viewport = viewport;
            foreach(_KTPT k in KTPT.Entries)
            {
                Vector2f pos = new Vector2f(k.StartPosition.X, k.StartPosition.Z);
                Vertices.Add(new KTPTVertex(k, new DraggableVertexPivotArrow(pos, (int)k.StartRotation.Y, viewport)));
            }
        }

        public override List<_ISectionEntry> GetData()
        {
            List<_ISectionEntry> result = new List<_ISectionEntry>();
            for (int i = 0; i < KTPT.Length(); i++)
            {
                result.Add(KTPT.GetEntry(i));
            }
            return result;
        }

        public override string GetTitle(int index)
        {
            return "Start Point " + index;
        }

        public override void AddEntry()
        {
            _KTPT k = (_KTPT)KTPT.AddEntry();
            Vector2f pos = new Vector2f(k.StartPosition.X, k.StartPosition.Z);
            Vertices.Add(new KTPTVertex(k, new DraggableVertexPivotArrow(pos, (int)k.StartRotation.X, Viewport)));
        }

        public override void RemoveEntry(int index)
        {
            KTPT.RemoveEntry(index);
            Vertices.RemoveAt(index);
        }

        public override void AddShapes(int selectedIndex)
        {
            for (int i = 0; i < Vertices.Count; i++)
            {
                Vertices[i].Vertex.FillColor = i == selectedIndex ? KmpViewportSync.HighlightColor : Color.Black;
                Viewport.AddShape(Vertices[i].Vertex);
            }
        }
    }

    public class KTPTVertex
    {
        public  _KTPT KTPTEntry { get; private set; }
        public  DraggableVertexPivotArrow Vertex { get; private set; }

        public KTPTVertex(_KTPT ktpt, DraggableVertexPivotArrow vertex)
        {
            KTPTEntry = ktpt;
            Vertex = vertex;

            Vertex._viewport.MouseUp += OnMouseUp;
        }

        private void OnMouseUp(object? sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left && !Vertex._dragging)
            {
                float x = Vertex.Vertices[0].X;
                float y = KTPTEntry.StartPosition.Y;
                float z = Vertex.Vertices[0].Y;
                KTPTEntry.StartPosition = new Vector3f(x, y, z);
                
                float rx = KTPTEntry.StartRotation.X;
                float ry = Vertex.Angle;
                float rz = KTPTEntry.StartRotation.Z;
                KTPTEntry.StartRotation = new Vector3f(rx, ry, rz);
            }
        }
    }
}
