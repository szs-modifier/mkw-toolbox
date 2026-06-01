using DrawLib;

namespace System.Windows.Forms
{
    public partial class Viewport2D : Panel
    {
        private List<Shape> Shapes;
        private List<Shape> BackgroundShapes;

        private const float ZoomRate = 0.0005f;
        private float       Zoom = 0.01f;
        private bool        Panning  = false;
        private Vector2f    Offset;
        private Vector2f    MouseDelta;

        private bool Debounce = false;

        public Viewport2D() : base()
        {
            this.DoubleBuffered = true;
            this.Shapes         = new List<Shape>();
            this.BackgroundShapes = new List<Shape>();
            this.Offset         = new Vector2f();
            this.MouseDelta     = new Vector2f();

            this.Paint      += this.OnPaint;
            this.MouseDown  += this.OnMouseDown;
            this.MouseUp    += this.OnMouseUp;
            this.MouseMove  += this.OnMouseMove;
            this.MouseWheel += this.OnMouseWheel;
        }

        // Public methods

        public void AddShape(Shape shape)
        {
            this.Shapes.Add(shape);
            if (!Debounce)
            {
                this.Debounce = true;
                Shape current = Shapes[Shapes.Count - 1];
                if(current.Vertices.Count > 0)
                    CenterAt(current.Vertices[0].X, current.Vertices[0].Y);
            }
        }

        public void ClearShapes()
        {
            this.Shapes.Clear();
        }

        public void SetBackgroundShapes(IEnumerable<Shape> shapes)
        {
            this.BackgroundShapes = new List<Shape>(shapes);
        }

        public void ClearBackgroundShapes()
        {
            this.BackgroundShapes.Clear();
        }

        public Vector2f GetOffset() { return this.Offset; }

        public float GetZoom() { return this.Zoom; }

        // Private methods
        
        private void CenterAt(float x, float y)
        {
            this.Offset.X = (this.Width / 2) - (x * Zoom);
            this.Offset.Y = (this.Height / 2) - (y * Zoom);
        }

        private void DrawShapes(Graphics graphics)
        {
            DrawShapeList(graphics, this.BackgroundShapes);
            DrawShapeList(graphics, this.Shapes);
        }

        private void DrawShapeList(Graphics graphics, List<Shape> shapes)
        {
            foreach(Shape shape in shapes)
            {
                List<Vector2f> transPos = new List<Vector2f>(shape.Vertices.Count);
                for(int i = 0; i < shape.Vertices.Count; i++)
                    transPos.Add((shape.Vertices[i] * Zoom) + Offset);
                shape.Draw(graphics, transPos);
            }
        }

        private void DragOneAtATime()
        {
            bool debounce = false;
            foreach(Shape shape in this.Shapes)
            {
                if (shape.GetType().IsSubclassOf(typeof(DraggableShape)))
                {
                    if (((DraggableShape)shape)._dragging && !debounce)
                    {
                        debounce = true;
                    }
                    else if(((DraggableShape)shape)._dragging && debounce)
                    { 
                        ((DraggableShape)shape)._dragging = false;
                    }
                }
            }
        }

        // Event handlers

        protected void OnPaint(object? sender, PaintEventArgs e)
        {
            e.Graphics.PixelOffsetMode = Drawing.Drawing2D.PixelOffsetMode.HighSpeed;
            e.Graphics.SmoothingMode = Drawing.Drawing2D.SmoothingMode.HighSpeed;
            e.Graphics.CompositingQuality = Drawing.Drawing2D.CompositingQuality.HighSpeed;

            DrawShapes(e.Graphics);
        }

        protected void OnMouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle || (e.Button == MouseButtons.Left && Control.ModifierKeys == Keys.Control))
            {
                this.MouseDelta.X = e.Location.X - Offset.X;
                this.MouseDelta.Y = e.Location.Y - Offset.Y;

                if (!this.Panning)
                    this.Panning = true;
            }
        }

        protected void OnMouseUp(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle || (e.Button == MouseButtons.Left && Control.ModifierKeys == Keys.Control))
            {
                this.Panning = false;
            }
        }

        protected void OnMouseMove(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle || (e.Button == MouseButtons.Left && Control.ModifierKeys == Keys.Control))
            {
                if (this.Panning)
                {
                    this.Offset.X = e.Location.X - this.MouseDelta.X;
                    this.Offset.Y = e.Location.Y - this.MouseDelta.Y;

                    this.Invalidate();
                }
            }

            if (e.Button == MouseButtons.Left)
            {
                DragOneAtATime();
            }
        }

        protected void OnMouseWheel(object? sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
                this.Zoom += ZoomRate;
            else if (e.Delta < 0 && this.Zoom > 0.0001f)
                this.Zoom -= ZoomRate;

            this.Invalidate();
        }
    }
}
