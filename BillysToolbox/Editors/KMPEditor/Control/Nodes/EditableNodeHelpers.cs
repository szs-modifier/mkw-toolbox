using DrawLib.Shapes;
using kartlib.Serial;
using static kartlib.Serial.KMP;

namespace KMP_Editor.Control.Nodes
{
    internal static class KmpViewportSync
    {
        public static Vector2f ToVector2(Vector3f position)
        {
            return new Vector2f(position.X, position.Z);
        }

        public static Vector3f WithXZ(Vector3f original, Vector2f position)
        {
            return new Vector3f(position.X, original.Y, position.Y);
        }

        public static readonly Color HighlightColor = Color.LightGreen;

        public static Vector2f Center(Vector2f a, Vector2f b)
        {
            return new Vector2f((a.X + b.X) / 2f, (a.Y + b.Y) / 2f);
        }

        public static float DistanceSquared(Vector2f a, Vector2f b)
        {
            float dx = a.X - b.X;
            float dy = a.Y - b.Y;
            return dx * dx + dy * dy;
        }

        public static Vertex HighlightAt(Vector2f position)
        {
            return new Vertex(position, HighlightColor);
        }

        public static Arrow DirectionArrow(Vector2f start, Vector2f end, Color color)
        {
            Vector2f delta = end - start;
            float length = delta.Length();
            if (length <= 0.0001f)
                return new Arrow(start, start, color);

            Vector2f unit = delta / length;
            return new Arrow(start, start + unit * Math.Min(length, 200f), color);
        }
    }

    internal class EntryVertex<T> where T : _ISectionEntry
    {
        private readonly Func<T, Vector3f> _getPosition;
        private readonly Action<T, Vector3f> _setPosition;

        public T Entry { get; private set; }
        public DraggableVertex Vertex { get; private set; }

        public EntryVertex(T entry, DraggableVertex vertex, Func<T, Vector3f> getPosition, Action<T, Vector3f> setPosition)
        {
            Entry = entry;
            Vertex = vertex;
            _getPosition = getPosition;
            _setPosition = setPosition;

            vertex._viewport.MouseUp += OnMouseUp;
        }

        private void OnMouseUp(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && !Vertex._dragging)
            {
                _setPosition(Entry, KmpViewportSync.WithXZ(_getPosition(Entry), Vertex.Vertices[0]));
            }
        }
    }

    internal class EntryPivotVertex<T> where T : _ISectionEntry
    {
        private readonly Func<T, Vector3f> _getPosition;
        private readonly Action<T, Vector3f> _setPosition;
        private readonly Func<T, Vector3f> _getRotation;
        private readonly Action<T, Vector3f> _setRotation;

        public T Entry { get; private set; }
        public DraggableVertexPivotArrow Vertex { get; private set; }

        public EntryPivotVertex(
            T entry,
            DraggableVertexPivotArrow vertex,
            Func<T, Vector3f> getPosition,
            Action<T, Vector3f> setPosition,
            Func<T, Vector3f> getRotation,
            Action<T, Vector3f> setRotation)
        {
            Entry = entry;
            Vertex = vertex;
            _getPosition = getPosition;
            _setPosition = setPosition;
            _getRotation = getRotation;
            _setRotation = setRotation;

            vertex._viewport.MouseUp += OnMouseUp;
        }

        private void OnMouseUp(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && !Vertex._dragging)
            {
                _setPosition(Entry, KmpViewportSync.WithXZ(_getPosition(Entry), Vertex.Vertices[0]));

                Vector3f rotation = _getRotation(Entry);
                _setRotation(Entry, new Vector3f(rotation.X, Vertex.Angle, rotation.Z));
            }
        }
    }

    internal class CheckpointShape
    {
        public _CKPT Entry { get; private set; }
        public DraggableLine Line { get; private set; }

        public CheckpointShape(_CKPT entry, DraggableLine line)
        {
            Entry = entry;
            Line = line;
            line._viewport.MouseUp += OnMouseUp;
        }

        private void OnMouseUp(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && !Line._dragging)
            {
                Entry.PositionL = Line.Vertices[0];
                Entry.PositionR = Line.Vertices[1];
            }
        }
    }

}
