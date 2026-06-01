namespace DrawLib.Shapes
{
    public class DraggableVertexPivotArrow : DraggableShape
    {
        private DraggableVertex _vertex;
        private PivotArrow _arrow;

        public float Angle
        {
            get { return _arrow.Angle; }
            private set { }
        }

        public DraggableVertexPivotArrow(Vector2f position, int angle, Viewport2D viewport)
            : base(new List<Vector2f> { position }, Color.Black, viewport)
        {
            _vertex = new DraggableVertex(Vertices[0], viewport);

            float arr_x = Vertices[0].X + (DraggableVertex._size / 2);
            float arr_y = Vertices[0].Y + (DraggableVertex._size / 2);
            _arrow  = new PivotArrow(new Vector2f(arr_x, arr_y), angle, viewport);
        }

        public override void Draw(Graphics g, List<Vector2f> pos)
        {
            float arr_x = pos[0].X + (DraggableVertex._size / 2);
            float arr_y = pos[0].Y + (DraggableVertex._size / 2);
            _vertex.FillColor = FillColor;
            _arrow.FillColor = FillColor == Color.Black ? Color.Gainsboro : FillColor;
            _arrow.Draw(g, new List<Vector2f> { new Vector2f(arr_x, arr_y)});
            _vertex.Draw(g, pos);
        }

        public override bool Colliding(float x, float y)
        {
            return false;
        }

        protected override void OnMouseMove(object? sender, MouseEventArgs e)
        {
            if (_vertex._dragging)
            {
                Vertices[0] = _vertex.Vertices[0];
                _arrow.Vertices[0] = _vertex.Vertices[0];
            }
        }
    }
}
