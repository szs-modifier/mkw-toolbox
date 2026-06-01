using DrawLib;
using DrawLib.Shapes;
using kartlib.Serial;

namespace BillysToolbox.Editors
{
    internal class KclBackgroundModel
    {
        private static readonly Color[] Palette = new Color[]
        {
            Color.FromArgb(96, 244, 67, 54),
            Color.FromArgb(96, 33, 150, 243),
            Color.FromArgb(96, 76, 175, 80),
            Color.FromArgb(96, 255, 193, 7),
            Color.FromArgb(96, 156, 39, 176),
            Color.FromArgb(96, 255, 152, 0),
            Color.FromArgb(96, 0, 188, 212),
            Color.FromArgb(96, 205, 220, 57),
            Color.FromArgb(96, 233, 30, 99),
            Color.FromArgb(96, 121, 85, 72),
        };

        public string Name { get; private set; }
        public List<Shape> Shapes { get; private set; }
        public KCL Kcl { get; private set; }

        private KclBackgroundModel(string name, List<Shape> shapes, KCL kcl)
        {
            Name = name;
            Shapes = shapes;
            Kcl = kcl;
        }

        public static KclBackgroundModel FromKcl(KCL kcl)
        {
            List<Shape> shapes = new List<Shape>(kcl.Triangles.Count);

            foreach (KCL._Triangle triangle in kcl.Triangles)
            {
                Vector3f[] vertices = kcl.GetTriangleVertices(triangle);
                Color fill = GetCollisionColor(triangle.Collision);
                shapes.Add(new TrianglePolygon(
                    new List<Vector2f>
                    {
                        new Vector2f(vertices[0].X, vertices[0].Z),
                        new Vector2f(vertices[1].X, vertices[1].Z),
                        new Vector2f(vertices[2].X, vertices[2].Z),
                    },
                    fill,
                    Color.FromArgb(120, fill.R, fill.G, fill.B)));
            }

            return new KclBackgroundModel(Path.GetFileName(kcl.Filename), shapes, kcl);
        }

        public bool TryGetHeight(float x, float z, out float y)
        {
            y = 0;
            bool found = false;
            float bestY = float.MinValue;

            foreach (KCL._Triangle triangle in Kcl.Triangles)
            {
                Vector3f[] vertices = Kcl.GetTriangleVertices(triangle);
                if (TryGetTriangleHeight(vertices[0], vertices[1], vertices[2], x, z, out float candidateY) && candidateY > bestY)
                {
                    bestY = candidateY;
                    found = true;
                }
            }

            if (found)
                y = bestY;

            return found;
        }

        private static bool TryGetTriangleHeight(Vector3f a, Vector3f b, Vector3f c, float x, float z, out float y)
        {
            y = 0;
            float denominator = (b.Z - c.Z) * (a.X - c.X) + (c.X - b.X) * (a.Z - c.Z);
            if (Math.Abs(denominator) < 0.0001f)
                return false;

            float w1 = ((b.Z - c.Z) * (x - c.X) + (c.X - b.X) * (z - c.Z)) / denominator;
            float w2 = ((c.Z - a.Z) * (x - c.X) + (a.X - c.X) * (z - c.Z)) / denominator;
            float w3 = 1f - w1 - w2;

            const float tolerance = -0.0001f;
            if (w1 < tolerance || w2 < tolerance || w3 < tolerance)
                return false;

            y = (w1 * a.Y) + (w2 * b.Y) + (w3 * c.Y);
            return true;
        }

        private static Color GetCollisionColor(ushort collision)
        {
            return Palette[(collision & 0x1F) % Palette.Length];
        }
    }
}
