namespace kartlib.Serial
{
    public class KCL
    {
        public class _Header
        {
            public UInt32 PosDataOffset;    // 0-indexed
            public UInt32 NrmDataOffset;    // 0-indexed
            public UInt32 PrismDataOffset;  // 1-indexed
            public UInt32 BlockDataOffset;  // Octree Blocks
            public float PrismThickness;
            public Vector3f AreaMinPosition;
            public UInt32 AreaXWidthMask;
            public UInt32 AreaYWidthMask;
            public UInt32 AreaZWidthMask;
            public UInt32 BlockWidthShift;
            public UInt32 AreaXBlocksShift;
            public UInt32 AreaZBlocksShift;
            public float SphereRadius;      // Optional!

            public _Header()
            {
                PosDataOffset = 0u;
                NrmDataOffset = 0u;
                PrismDataOffset = 0u;
                BlockDataOffset = 0u;
                PrismThickness = 300f;
                AreaMinPosition = new Vector3f(0, 0, 0);
                AreaXWidthMask = 0u;
                AreaYWidthMask = 0u;
                AreaZWidthMask = 0u;
                BlockWidthShift = 0u;
                AreaXBlocksShift = 0u;
                AreaZBlocksShift = 0u;
                SphereRadius = 250f;
            }

            public _Header(EndianReader reader)
            {
                PosDataOffset = reader.ReadUInt32();
                NrmDataOffset = reader.ReadUInt32();
                PrismDataOffset = reader.ReadUInt32();
                BlockDataOffset = reader.ReadUInt32();
                PrismThickness = reader.ReadSingle();
                AreaMinPosition = reader.ReadSingles(3);
                AreaXWidthMask = reader.ReadUInt32();
                AreaYWidthMask = reader.ReadUInt32();
                AreaZWidthMask = reader.ReadUInt32();
                BlockWidthShift = reader.ReadUInt32();
                AreaXBlocksShift = reader.ReadUInt32();
                AreaZBlocksShift = reader.ReadUInt32();
                SphereRadius = reader.ReadSingle();
            }

            public void Write(EndianWriter writer)
            {
                writer.WriteUInt32(PosDataOffset);
                writer.WriteUInt32(NrmDataOffset);
                writer.WriteUInt32(PrismDataOffset);
                writer.WriteUInt32(BlockDataOffset);
                writer.WriteSingle(PrismThickness);
                writer.WriteSingles(AreaMinPosition);
                writer.WriteUInt32(AreaXWidthMask);
                writer.WriteUInt32(AreaYWidthMask);
                writer.WriteUInt32(AreaZWidthMask);
                writer.WriteUInt32(BlockWidthShift);
                writer.WriteUInt32(AreaXBlocksShift);
                writer.WriteUInt32(AreaZBlocksShift);
                writer.WriteSingle(SphereRadius);
            }
        }

        public class _Triangle
        {
            public float Height;
            public UInt16 PosIndex;         // Section 1
            public UInt16 FaceNormal;       // Section 2
            public UInt16[] EdgeNormals;    // Section 2
            public UInt16 Collision;

            public _Triangle()
            {
                Height = 0;
                PosIndex = 0;
                FaceNormal = 0;
                EdgeNormals = new UInt16[3] { 0, 0, 0 };
                Collision = 0;
            }

            public _Triangle(EndianReader reader)
            {
                Height = reader.ReadSingle();
                PosIndex = reader.ReadUInt16();
                FaceNormal = reader.ReadUInt16();
                EdgeNormals = reader.ReadUInt16s(3);
                Collision = reader.ReadUInt16();
            }

            public void Write(EndianWriter writer)
            {
                writer.WriteSingle(Height);
                writer.WriteUInt16(PosIndex);
                writer.WriteUInt16(FaceNormal);
                writer.WriteUInt16s(EdgeNormals);
                writer.WriteUInt16(Collision);
            }
        }

        public class _Octree
        {
            public class _OctreeNode
            {
                public uint Flag;

                public _OctreeNode[] SubNodes;

                public ushort[] Triangles;

                public uint DataOffset
                {
                    get
                    {
                        return this.Flag & 0x7FFFFFFFu;
                    }
                    set
                    {
                        this.Flag = (this.Flag & 0x80000000u) | value;
                    }
                }

                public bool IsLeaf
                {
                    get
                    {
                        return ((this.Flag >> 31) & 1) == 1;
                    }
                    set
                    {
                        this.Flag = (this.Flag & 0x7FFFFFFFu) | (value ? 2147483648u : 0u);
                    }
                }

                public _OctreeNode()
                {
                    this.Flag = 0u;
                    this.DataOffset = 0u;
                    this.IsLeaf = false;
                }

                public _OctreeNode(EndianReader Reader, int Start)
                {
                    this.Flag = Reader.ReadUInt32();
                    int position = Reader.Position;
                    Reader.Position = (int)this.DataOffset + Start;
                    if (this.IsLeaf)
                    {
                        Reader.Position += 2;
                        List<ushort> list = new List<ushort>();
                        while (true)
                        {
                            ushort num = Reader.ReadUInt16();
                            if (num == 0)
                            {
                                break;
                            }
                            list.Add((ushort)(num - 1));
                        }
                        this.Triangles = list.ToArray();
                    }
                    else
                    {
                        this.SubNodes = new _OctreeNode[8];
                        for (int i = 0; i < 8; i++)
                        {
                            this.SubNodes[i] = new _OctreeNode(Reader, (int)this.DataOffset + Start);
                        }
                    }
                    Reader.Position = position;
                }

                public static _OctreeNode Generate(Dictionary<ushort, Triangle> Triangles, Vector3f Position, float BoxSize, int MaxTris, int MinSize)
                {
                    _OctreeNode octreeNode = new _OctreeNode();
                    Vector3f vector3f = Position + new Vector3f(BoxSize / 2f, BoxSize / 2f, BoxSize / 2f);
                    float num = BoxSize + 400f;
                    Vector3f position = vector3f - new Vector3f(num / 2f, num / 2f, num / 2f);
                    Dictionary<ushort, Triangle> dictionary = new Dictionary<ushort, Triangle>();
                    foreach (KeyValuePair<ushort, Triangle> Triangle in Triangles)
                    {
                        if (_OctreeNode.tricube_overlap(Triangle.Value, position, num))
                        {
                            dictionary.Add(Triangle.Key, Triangle.Value);
                        }
                    }
                    if (BoxSize > (float)MinSize && dictionary.Count > MaxTris)
                    {
                        octreeNode.IsLeaf = false;
                        float num2 = BoxSize / 2f;
                        octreeNode.SubNodes = new _OctreeNode[8];
                        int num3 = 0;
                        for (int i = 0; i < 2; i++)
                        {
                            for (int j = 0; j < 2; j++)
                            {
                                for (int k = 0; k < 2; k++)
                                {
                                    Vector3f position2 = Position + new Vector3f(k, j, i) * num2;
                                    octreeNode.SubNodes[num3] = _OctreeNode.Generate(dictionary, position2, num2, MaxTris, MinSize);
                                    num3++;
                                }
                            }
                        }
                    }
                    else
                    {
                        octreeNode.IsLeaf = true;
                        octreeNode.Triangles = dictionary.Keys.ToArray();
                    }
                    return octreeNode;
                }

                private static bool axis_test(float a1, float a2, float b1, float b2, float c1, float c2, float half)
                {
                    float val = a1 * b1 + a2 * b2;
                    float val2 = a1 * c1 + a2 * c2;
                    float num = half * (Math.Abs(a1) + Math.Abs(a2));
                    if (!(Math.Min(val, val2) > num))
                    {
                        return Math.Max(val, val2) < 0f - num;
                    }
                    return true;
                }

                public static bool tricube_overlap(Triangle t, Vector3f Position, float BoxSize)
                {
                    float num = BoxSize / 2f;
                    Position += new Vector3f(num, num, num);
                    Vector3f vector3f = t.PointA - Position;
                    Vector3f vector3f2 = t.PointB - Position;
                    Vector3f vector3f3 = t.PointC - Position;
                    if (Math.Min(Math.Min(vector3f.X, vector3f2.X), vector3f3.X) > num || Math.Max(Math.Max(vector3f.X, vector3f2.X), vector3f3.X) < 0f - num)
                    {
                        return false;
                    }
                    if (Math.Min(Math.Min(vector3f.Y, vector3f2.Y), vector3f3.Y) > num || Math.Max(Math.Max(vector3f.Y, vector3f2.Y), vector3f3.Y) < 0f - num)
                    {
                        return false;
                    }
                    if (Math.Min(Math.Min(vector3f.Z, vector3f2.Z), vector3f3.Z) > num || Math.Max(Math.Max(vector3f.Z, vector3f2.Z), vector3f3.Z) < 0f - num)
                    {
                        return false;
                    }
                    float num2 = t.Normal.Dot(vector3f);
                    float num3 = num * (Math.Abs(t.Normal.X) + Math.Abs(t.Normal.Y) + Math.Abs(t.Normal.Z));
                    if (num2 > num3 || num2 < 0f - num3)
                    {
                        return false;
                    }
                    Vector3f vector3f4 = vector3f2 - vector3f;
                    if (_OctreeNode.axis_test(vector3f4.Z, 0f - vector3f4.Y, vector3f.Y, vector3f.Z, vector3f3.Y, vector3f3.Z, num))
                    {
                        return false;
                    }
                    if (_OctreeNode.axis_test(0f - vector3f4.Z, vector3f4.X, vector3f.X, vector3f.Z, vector3f3.X, vector3f3.Z, num))
                    {
                        return false;
                    }
                    if (_OctreeNode.axis_test(vector3f4.Y, 0f - vector3f4.X, vector3f2.X, vector3f2.Y, vector3f3.X, vector3f3.Y, num))
                    {
                        return false;
                    }
                    vector3f4 = vector3f3 - vector3f2;
                    if (_OctreeNode.axis_test(vector3f4.Z, 0f - vector3f4.Y, vector3f.Y, vector3f.Z, vector3f3.Y, vector3f3.Z, num))
                    {
                        return false;
                    }
                    if (_OctreeNode.axis_test(0f - vector3f4.Z, vector3f4.X, vector3f.X, vector3f.Z, vector3f3.X, vector3f3.Z, num))
                    {
                        return false;
                    }
                    if (_OctreeNode.axis_test(vector3f4.Y, 0f - vector3f4.X, vector3f.X, vector3f.Y, vector3f2.X, vector3f2.Y, num))
                    {
                        return false;
                    }
                    vector3f4 = vector3f - vector3f3;
                    if (_OctreeNode.axis_test(vector3f4.Z, 0f - vector3f4.Y, vector3f.Y, vector3f.Z, vector3f2.Y, vector3f2.Z, num))
                    {
                        return false;
                    }
                    if (_OctreeNode.axis_test(0f - vector3f4.Z, vector3f4.X, vector3f.X, vector3f.Z, vector3f2.X, vector3f2.Z, num))
                    {
                        return false;
                    }
                    if (_OctreeNode.axis_test(vector3f4.Y, 0f - vector3f4.X, vector3f2.X, vector3f2.Y, vector3f3.X, vector3f3.Y, num))
                    {
                        return false;
                    }
                    return true;
                }
            }

            public List<_OctreeNode> RootNodes;

            public _Octree()
            {
                this.RootNodes = new List<_OctreeNode>();
            }

            public _Octree(EndianReader Reader, int NrNodes)
            {
                int position = Reader.Position;
                this.RootNodes = new List<_OctreeNode>();
                for (int i = 0; i < NrNodes; i++)
                {
                    this.RootNodes.Add(new _OctreeNode(Reader, position));
                }
            }

            public void Write(EndianWriter Writer)
            {
                int position = Writer.Position;
                Queue<uint> queue = new Queue<uint>();
                Queue<_OctreeNode> queue2 = new Queue<_OctreeNode>();
                foreach (_OctreeNode rootNode in this.RootNodes)
                {
                    queue.Enqueue(0u);
                    queue2.Enqueue(rootNode);
                }
                uint num = (uint)(this.RootNodes.Count * 4);
                while (queue2.Count > 0)
                {
                    _OctreeNode octreeNode = queue2.Dequeue();
                    if (octreeNode.IsLeaf)
                    {
                        queue.Dequeue();
                        Writer.WriteUInt32(0u);
                        continue;
                    }
                    octreeNode.DataOffset = num - queue.Dequeue();
                    Writer.WriteUInt32(octreeNode.DataOffset);
                    _OctreeNode[] subNodes = octreeNode.SubNodes;
                    foreach (_OctreeNode item in subNodes)
                    {
                        queue.Enqueue(num);
                        queue2.Enqueue(item);
                    }
                    num += 32;
                }
                foreach (_OctreeNode rootNode2 in this.RootNodes)
                {
                    queue.Enqueue(0u);
                    queue2.Enqueue(rootNode2);
                }
                int position2 = Writer.Position;
                uint num2 = num;
                Writer.Position = position;
                num = (uint)(this.RootNodes.Count * 4);
                while (queue2.Count > 0)
                {
                    _OctreeNode octreeNode2 = queue2.Dequeue();
                    if (octreeNode2.IsLeaf)
                    {
                        Writer.WriteUInt32(0x80000000u | (num2 - queue.Dequeue() - 2));
                        int position3 = Writer.Position;
                        Writer.Position = position2;
                        ushort[] triangles = octreeNode2.Triangles;
                        foreach (ushort num3 in triangles)
                        {
                            Writer.WriteUInt16((ushort)(num3 + 1));
                        }
                        Writer.WriteUInt16(0);
                        num2 += (uint)(octreeNode2.Triangles.Length * 2 + 2);
                        position2 = Writer.Position;
                        Writer.Position = position3;
                    }
                    else
                    {
                        Writer.Position += 4;
                        queue.Dequeue();
                        _OctreeNode[] subNodes = octreeNode2.SubNodes;
                        foreach (_OctreeNode item2 in subNodes)
                        {
                            queue.Enqueue(num);
                            queue2.Enqueue(item2);
                        }
                        num += 32;
                    }
                }
            }

            private static int GetNearest2Power(float Value)
            {
                return (int)Math.Ceiling(Math.Log(Value, 2.0));
            }

            public static _Octree FromTriangles(Triangle[] Triangles, _Header Header, int MaxRootSize = 16384, int MinRootSize = 512, int MinCubeSize = 128, int MaxNrTris = 20)
            {
                Header.PrismThickness = 300f;
                Header.SphereRadius = 250f;
                Vector3f vector3f = new Vector3f(float.MaxValue, float.MaxValue, float.MaxValue);
                Vector3f vector3f2 = new Vector3f(float.MinValue, float.MinValue, float.MinValue);
                Dictionary<ushort, Triangle> dictionary = new Dictionary<ushort, Triangle>();
                ushort num = 0;
                foreach (Triangle triangle in Triangles)
                {
                    if (triangle.PointA.X < vector3f.X)
                    {
                        vector3f.X = triangle.PointA.X;
                    }
                    if (triangle.PointA.Y < vector3f.Y)
                    {
                        vector3f.Y = triangle.PointA.Y;
                    }
                    if (triangle.PointA.Z < vector3f.Z)
                    {
                        vector3f.Z = triangle.PointA.Z;
                    }
                    if (triangle.PointA.X > vector3f2.X)
                    {
                        vector3f2.X = triangle.PointA.X;
                    }
                    if (triangle.PointA.Y > vector3f2.Y)
                    {
                        vector3f2.Y = triangle.PointA.Y;
                    }
                    if (triangle.PointA.Z > vector3f2.Z)
                    {
                        vector3f2.Z = triangle.PointA.Z;
                    }
                    if (triangle.PointB.X < vector3f.X)
                    {
                        vector3f.X = triangle.PointB.X;
                    }
                    if (triangle.PointB.Y < vector3f.Y)
                    {
                        vector3f.Y = triangle.PointB.Y;
                    }
                    if (triangle.PointB.Z < vector3f.Z)
                    {
                        vector3f.Z = triangle.PointB.Z;
                    }
                    if (triangle.PointB.X > vector3f2.X)
                    {
                        vector3f2.X = triangle.PointB.X;
                    }
                    if (triangle.PointB.Y > vector3f2.Y)
                    {
                        vector3f2.Y = triangle.PointB.Y;
                    }
                    if (triangle.PointB.Z > vector3f2.Z)
                    {
                        vector3f2.Z = triangle.PointB.Z;
                    }
                    if (triangle.PointC.X < vector3f.X)
                    {
                        vector3f.X = triangle.PointC.X;
                    }
                    if (triangle.PointC.Y < vector3f.Y)
                    {
                        vector3f.Y = triangle.PointC.Y;
                    }
                    if (triangle.PointC.Z < vector3f.Z)
                    {
                        vector3f.Z = triangle.PointC.Z;
                    }
                    if (triangle.PointC.X > vector3f2.X)
                    {
                        vector3f2.X = triangle.PointC.X;
                    }
                    if (triangle.PointC.Y > vector3f2.Y)
                    {
                        vector3f2.Y = triangle.PointC.Y;
                    }
                    if (triangle.PointC.Z > vector3f2.Z)
                    {
                        vector3f2.Z = triangle.PointC.Z;
                    }
                    dictionary.Add(num, triangle);
                    num = (ushort)(num + 1);
                }
                vector3f -= new Vector3f(400f, 400f, 400f);
                vector3f2 += new Vector3f(400f, 400f, 400f);
                Header.AreaMinPosition = vector3f;
                Vector3f vector3f3 = vector3f2 - vector3f;
                int nearest2Power = GetNearest2Power(Math.Min(Math.Min(vector3f3.X, vector3f3.Y), vector3f3.Z));
                if (nearest2Power > GetNearest2Power(MaxRootSize))
                {
                    nearest2Power = GetNearest2Power(MaxRootSize);
                }
                Header.BlockWidthShift = (uint)nearest2Power;
                int num2 = 1 << nearest2Power;
                int num3 = (1 << GetNearest2Power(vector3f3.X)) / num2;
                int num4 = (1 << GetNearest2Power(vector3f3.Y)) / num2;
                int num5 = (1 << GetNearest2Power(vector3f3.Z)) / num2;
                if (num3 <= 0)
                {
                    num3 = 1;
                }
                if (num4 <= 0)
                {
                    num4 = 1;
                }
                if (num5 <= 0)
                {
                    num5 = 1;
                }
                Header.AreaXBlocksShift = (uint)(GetNearest2Power(vector3f3.X) - nearest2Power);
                Header.AreaZBlocksShift = (uint)(GetNearest2Power(vector3f3.X) - nearest2Power + GetNearest2Power(vector3f3.Y) - nearest2Power);
                Header.AreaXWidthMask = (uint)(-1 << GetNearest2Power(vector3f3.X));
                Header.AreaYWidthMask = (uint)(-1 << GetNearest2Power(vector3f3.Y));
                Header.AreaZWidthMask = (uint)(-1 << GetNearest2Power(vector3f3.Z));
                _Octree octree = new _Octree();
                _OctreeNode[] array = new _OctreeNode[num3 * num4 * num5];
                int num6 = 0;
                for (int j = 0; j < num5; j++)
                {
                    for (int k = 0; k < num4; k++)
                    {
                        for (int l = 0; l < num3; l++)
                        {
                            Vector3f position = vector3f + new Vector3f(l, k, j) * num2;
                            array[num6] = _OctreeNode.Generate(dictionary, position, num2, MaxNrTris, MinCubeSize);
                            num6++;
                        }
                    }
                }
                octree.RootNodes = array.ToList();
                return octree;
            }
        }

        public string Filename;
        public _Header Header;
        public List<Vector3f> Vertices;
        public List<Vector3f> Normals;
        public List<_Triangle> Triangles;
        public _Octree Octree;

        public KCL()
        {
            Vertices = new List<Vector3f>();
            Normals = new List<Vector3f>();
            Triangles = new List<_Triangle>();
            Filename = "Untitled.kcl";
        }

        public KCL(OBJ OBJ, KeyValuePair<ushort, bool>[] Collision)
        {
            this.Filename = "(Untitled)";
            this.Vertices = new List<Vector3f>();
            this.Normals = new List<Vector3f>();
            this.Triangles = new List<_Triangle>();
            this.Header = new _Header();
            try
            {
                List<Triangle> list = new List<Triangle>();
                for (int i = 0; i < OBJ.Groups.Count; i++)
                {
                    if (!Collision[i].Value)
                    {
                        continue;
                    }
                    foreach (OBJ._Face face in OBJ.Groups[i].Faces)
                    {
                        Triangle triangle = new Triangle(OBJ.Vertices[(int)face.Vertices[0]], OBJ.Vertices[(int)face.Vertices[1]], OBJ.Vertices[(int)face.Vertices[2]]);
                        Vector3f vector3f = (triangle.PointB - triangle.PointA).Cross(triangle.PointC - triangle.PointA);
                        if (!((double)(vector3f.X * vector3f.X + vector3f.Y * vector3f.Y + vector3f.Z * vector3f.Z) < 0.01))
                        {
                            _Triangle triangle2 = new _Triangle();
                            Vector3f vector3f2 = (triangle.PointC - triangle.PointA).Cross(triangle.Normal);
                            vector3f2.Normalize();
                            vector3f2 = -vector3f2;
                            Vector3f vector3f3 = (triangle.PointB - triangle.PointA).Cross(triangle.Normal);
                            vector3f3.Normalize();
                            Vector3f vector3f4 = (triangle.PointC - triangle.PointB).Cross(triangle.Normal);
                            vector3f4.Normalize();
                            triangle2.Height = (triangle.PointC - triangle.PointA).Dot(vector3f4);
                            int num = KCL.ContainsVector3(triangle.PointA, this.Vertices);
                            if (num == -1)
                            {
                                triangle2.PosIndex = (ushort)this.Vertices.Count;
                                this.Vertices.Add(triangle.PointA);
                            }
                            else
                            {
                                triangle2.PosIndex = (ushort)num;
                            }
                            num = KCL.ContainsVector3(triangle.Normal, this.Normals);
                            if (num == -1)
                            {
                                triangle2.FaceNormal = (ushort)this.Normals.Count;
                                this.Normals.Add(triangle.Normal);
                            }
                            else
                            {
                                triangle2.FaceNormal = (ushort)num;
                            }
                            num = KCL.ContainsVector3(vector3f2, this.Normals);
                            if (num == -1)
                            {
                                triangle2.EdgeNormals[0] = (ushort)this.Normals.Count;
                                this.Normals.Add(vector3f2);
                            }
                            else
                            {
                                triangle2.EdgeNormals[0] = (ushort)num;
                            }
                            num = KCL.ContainsVector3(vector3f3, this.Normals);
                            if (num == -1)
                            {
                                triangle2.EdgeNormals[1] = (ushort)this.Normals.Count;
                                this.Normals.Add(vector3f3);
                            }
                            else
                            {
                                triangle2.EdgeNormals[1] = (ushort)num;
                            }
                            num = KCL.ContainsVector3(vector3f4, this.Normals);
                            if (num == -1)
                            {
                                triangle2.EdgeNormals[2] = (ushort)this.Normals.Count;
                                this.Normals.Add(vector3f4);
                            }
                            else
                            {
                                triangle2.EdgeNormals[2] = (ushort)num;
                            }
                            triangle2.Collision = Collision[i].Key;
                            this.Triangles.Add(triangle2);
                            list.Add(triangle);
                        }
                    }
                    if (this.Normals.Count > 65535)
                    {
                        throw new ArgumentException("The max amount of supported normals have been reached.");
                    }
                    if (this.Vertices.Count > 65535)
                    {
                        throw new ArgumentException("The max amount of supported vertices have been reached.");
                    }
                }
                this.Octree = _Octree.FromTriangles(list.ToArray(), this.Header);
            }
            finally
            {
            }
        }

        public KCL(byte[] Data, string FileName)
        {
            this.Filename = FileName;
            EndianReader endianReader = new EndianReader(new MemoryStream(Data), Endianness.BigEndian);
            try
            {
                this.Vertices = new List<Vector3f>();
                this.Normals = new List<Vector3f>();
                this.Triangles = new List<_Triangle>();
                this.Header = new _Header(endianReader);
                int num = (int)(this.Header.NrmDataOffset - this.Header.PosDataOffset) / 12;
                int num2 = (int)(this.Header.PrismDataOffset + 16 - this.Header.NrmDataOffset) / 12;
                int num3 = (int)(this.Header.BlockDataOffset - (this.Header.PrismDataOffset + 16)) / 16;
                endianReader.Position = (int)this.Header.PosDataOffset;
                for (int i = 0; i < num; i++)
                {
                    this.Vertices.Add(endianReader.ReadSingles(3));
                }
                endianReader.Position = (int)this.Header.NrmDataOffset;
                for (int j = 0; j < num2; j++)
                {
                    this.Normals.Add(endianReader.ReadSingles(3));
                }
                endianReader.Position = (int)(this.Header.PrismDataOffset + 16);
                for (int k = 0; k < num3; k++)
                {
                    this.Triangles.Add(new _Triangle(endianReader));
                }
                endianReader.Position = (int)this.Header.BlockDataOffset;
                this.Octree = new _Octree(endianReader, this.GetNrNodesInOctree());
            }
            finally
            {
                endianReader.Close();
            }
        }

        public byte[] Write()
        {
            MemoryStream memoryStream = new MemoryStream();
            EndianWriter endianWriter = new EndianWriter(memoryStream, Endianness.BigEndian);
            try
            {
                Header.PosDataOffset = 60u;
                Header.NrmDataOffset = Header.PosDataOffset + (uint)(Vertices.Count * 12);
                Header.PrismDataOffset = (uint)((int)Header.NrmDataOffset + Normals.Count * 12 - 16);
                Header.BlockDataOffset = Header.PrismDataOffset + 16 + (uint)(Triangles.Count * 16);
                Header.Write(endianWriter);
                for (int i = 0; i < Vertices.Count; i++)
                {
                    endianWriter.WriteSingles(Vertices[i]);
                }

                for (int j = 0; j < Normals.Count; j++)
                {
                    endianWriter.WriteSingles(Normals[j]);
                }

                for (int k = 0; k < Triangles.Count; k++)
                {
                    Triangles[k].Write(endianWriter);
                }

                Octree.Write(endianWriter);
            }
            finally
            {
                endianWriter.Close();
                memoryStream.Close();
            }

            return memoryStream.ToArray();
        }

        public OBJ ToOBJ()
        {
            OBJ oBJ = new OBJ();
            uint num = 0u;
            foreach (_Triangle triangle in this.Triangles)
            {
                oBJ.Vertices.AddRange(this.GetTriangleVertices(triangle));
                string text = triangle.Collision.ToString("X4");
                bool flag = false;
                int index = -1;
                for (int i = 0; i < oBJ.Groups.Count; i++)
                {
                    if (oBJ.Groups[i].Name == text)
                    {
                        flag = true;
                        index = i;
                        break;
                    }
                }
                if (!flag)
                {
                    oBJ.Groups.Add(new OBJ._Group(text));
                    index = oBJ.Groups.Count - 1;
                }
                oBJ.Groups[index].Faces.Add(new OBJ._Face(new uint[3]
                {
                    num,
                    num + 1,
                    num + 2
                }));
                num += 3;
            }
            return oBJ;
        }

        public Vector3f[] GetTriangleVertices(_Triangle Plane)
        {
            Vector3f vector3f = this.Vertices[Plane.PosIndex];
            Vector3f vector3f2 = this.Normals[Plane.EdgeNormals[0]].Cross(this.Normals[Plane.FaceNormal]);
            Vector3f vector3f3 = this.Normals[Plane.EdgeNormals[1]].Cross(this.Normals[Plane.FaceNormal]);
            Vector3f vector3f4 = vector3f + vector3f3 * (Plane.Height / vector3f3.Dot(this.Normals[Plane.EdgeNormals[2]]));
            Vector3f vector3f5 = vector3f + vector3f2 * (Plane.Height / vector3f2.Dot(this.Normals[Plane.EdgeNormals[2]]));
            if (float.IsInfinity(vector3f4.X) || float.IsNaN(vector3f4.X) || float.IsInfinity(vector3f4.Y) || float.IsNaN(vector3f4.Y) || float.IsInfinity(vector3f4.Z) || float.IsNaN(vector3f4.Z))
            {
                vector3f4 = vector3f;
            }
            if (float.IsInfinity(vector3f5.X) || float.IsNaN(vector3f5.X) || float.IsInfinity(vector3f5.Y) || float.IsNaN(vector3f5.Y) || float.IsInfinity(vector3f5.Z) || float.IsNaN(vector3f5.Z))
            {
                vector3f5 = vector3f;
            }
            return new Vector3f[3] { vector3f, vector3f4, vector3f5 };
        }

        private int GetNrNodesInOctree()
        {
            return (((int)(~this.Header.AreaXWidthMask) >> (int)this.Header.BlockWidthShift) + 1) * (((int)(~this.Header.AreaYWidthMask) >> (int)this.Header.BlockWidthShift) + 1) * (((int)(~this.Header.AreaZWidthMask) >> (int)this.Header.BlockWidthShift) + 1);
        }

        private static int ContainsVector3(Vector3f a, List<Vector3f> b)
        {
            for (int i = 0; i < b.Count; i++)
            {
                if (b[i].X == a.X && b[i].Y == a.Y && b[i].Z == a.Z)
                {
                    return i;
                }
            }
            return -1;
        }
    }

    public class Triangle
    {
        public Vector3f PointA { get; set; }

        public Vector3f PointB { get; set; }

        public Vector3f PointC { get; set; }

        public Vector3f Normal => CalculateNormal();

        public Triangle(Vector3f A, Vector3f B, Vector3f C)
        {
            this.PointA = A;
            this.PointB = B;
            this.PointC = C;
        }

        public Vector3f CalculateNormal()
        {
            return (this.PointB - this.PointA).Cross(this.PointC - this.PointA).Normalize();
        }
    }
}
