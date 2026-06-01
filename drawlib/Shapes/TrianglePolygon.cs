namespace DrawLib.Shapes
{
    public class TrianglePolygon : Shape
    {
        public Color BorderColor { get; set; }

        public TrianglePolygon(List<Vector2f> vertices, Color fillColor, Color borderColor)
            : base(vertices, fillColor)
        {
            BorderColor = borderColor;
        }

        public override void Draw(Graphics g, List<Vector2f> pos)
        {
            if (pos.Count < 3)
                return;

            PointF[] points = new PointF[pos.Count];
            for (int i = 0; i < points.Length; i++)
                points[i] = new PointF(pos[i].X, pos[i].Y);

            using SolidBrush brush = new SolidBrush(FillColor);
            using Pen pen = new Pen(BorderColor, 1f);
            g.FillPolygon(brush, points);
            g.DrawPolygon(pen, points);
        }
    }
}
