using DrawLib.Shapes;
using kartlib.Serial;
using static kartlib.Serial.KMP;

namespace KMP_Editor.Control.Nodes
{
    public class CKPHNode : Node
    {
        public KMP KMP { get; set; }
        public _Section<_CKPH> CKPH { get; set; }
        public _Section<_CKPT> CKPT { get; set; }

        private readonly Viewport2D _viewport;

        public CKPHNode(KMP kmp, Viewport2D viewport)
        {
            KMP = kmp;
            CKPH = kmp.CKPH;
            CKPT = kmp.CKPT;
            _viewport = viewport;
        }

        public override List<_ISectionEntry> GetData()
        {
            List<_ISectionEntry> result = new List<_ISectionEntry>();
            for (int i = 0; i < CKPH.Length(); i++)
                result.Add(CKPH.GetEntry(i));
            return result;
        }

        public override string GetTitle(int index)
        {
            return "Group " + index;
        }

        public override void AddEntry()
        {
            _CKPH newEntry = (_CKPH)CKPH.AddEntry();
            if (CKPH.Length() > 1)
            {
                _CKPH previousEntry = (_CKPH)CKPH.GetEntry(CKPH.Length() - 2);
                newEntry.Start = (byte)(previousEntry.Start + previousEntry.Length);
            }

            RebuildCheckpointLinks(KMP);
        }

        public override void RemoveEntry(int index)
        {
            _CKPH removedNode = (_CKPH)CKPH.GetEntry(index);
            byte start = removedNode.Start;
            for (int i = 0; i < removedNode.Length; i++)
                CKPT.RemoveEntry(start);

            CKPH.RemoveEntry(index);
            ResetGroupStarts(index, start);
            RebuildCheckpointLinks(KMP);
        }

        public override void Populate(TreeNode node)
        {
            List<_ISectionEntry> ckphData = GetData();

            node.Nodes.Clear();
            node.Tag = this;
            for (int i = 0; i < ckphData.Count; i++)
            {
                CKPHGroupNode ckphGroupNode = new CKPHGroupNode(KMP, i, _viewport);

                TreeNode treeNode = new TreeNode("Group " + i);
                treeNode.Tag = ckphGroupNode;
                treeNode.ImageIndex = 3;
                treeNode.SelectedImageIndex = 3;

                node.Nodes.Add(treeNode);
            }
        }

        public override void AddShapes(int selectedIndex)
        {
            for (int i = 0; i < CKPH.Length(); i++)
            {
                CKPHGroupNode groupNode = new CKPHGroupNode(KMP, i, _viewport);
                groupNode.AddShapes(i == selectedIndex ? CKPHGroupNode.HighlightAllCheckpoints : -1);
            }
        }

        private void ResetGroupStarts(int startGroupIndex, byte startPosition)
        {
            byte position = startPosition;
            for (int i = startGroupIndex; i < CKPH.Length(); i++)
            {
                _CKPH current = (_CKPH)CKPH.GetEntry(i);
                current.Start = position;
                position += current.Length;
            }
        }

        internal static void RebuildCheckpointLinks(KMP kmp)
        {
            for (int i = 0; i < kmp.CKPH.Length(); i++)
            {
                _CKPH group = (_CKPH)kmp.CKPH.GetEntry(i);
                group.Previous = CreateGroupLink(i > 0 ? i - 1 : -1);
                group.Next = CreateGroupLink(i < kmp.CKPH.Length() - 1 ? i + 1 : -1);

                for (int j = 0; j < group.Length; j++)
                {
                    _CKPT checkpoint = (_CKPT)kmp.CKPT.GetEntry(group.Start + j);
                    checkpoint.Previous = j > 0 ? (byte)(j - 1) : (byte)0xFF;
                    checkpoint.Next = j < group.Length - 1 ? (byte)(j + 1) : (byte)0xFF;
                }
            }
        }

        private static byte[] CreateGroupLink(int groupIndex)
        {
            byte[] result = new byte[6] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
            if (groupIndex >= 0 && groupIndex <= byte.MaxValue)
                result[0] = (byte)groupIndex;
            return result;
        }
    }

    public class CKPHGroupNode : Node
    {
        internal const int HighlightAllCheckpoints = -2;

        public KMP KMP { get; private set; }
        public _Section<_CKPT> CKPT { get; private set; }
        public _CKPH CKPH { get; private set; }
        private readonly int _groupIndex;
        private readonly Viewport2D _viewport;
        private readonly List<CheckpointShape> _checkpoints;

        public CKPHGroupNode(KMP kmp, int index, Viewport2D viewport)
        {
            KMP = kmp;
            CKPT = kmp.CKPT;
            CKPH = (_CKPH)kmp.CKPH.GetEntry(index);
            _groupIndex = index;
            _viewport = viewport;
            _checkpoints = new List<CheckpointShape>();
            RebuildShapes();
        }

        public override List<_ISectionEntry> GetData()
        {
            List<_ISectionEntry> result = new List<_ISectionEntry>();
            for (int i = CKPH.Start; i < CKPH.Start + CKPH.Length; i++)
                result.Add(CKPT.GetEntry(i));
            return result;
        }

        public override string GetTitle(int index)
        {
            return "Checkpoint " + index;
        }

        public override void AddEntry()
        {
            if (CKPH.Length + 1 == byte.MaxValue)
                return;

            _CKPT entry = (_CKPT)CKPT.AddEntry(CKPH.Start + CKPH.Length);
            CKPH.Length++;
            IncrementFollowingGroupStarts(1);
            CKPHNode.RebuildCheckpointLinks(KMP);
            AddShape(entry);
        }

        public override void RemoveEntry(int index)
        {
            CKPT.RemoveEntry(CKPH.Start + index);
            CKPH.Length--;
            IncrementFollowingGroupStarts(-1);
            CKPHNode.RebuildCheckpointLinks(KMP);
            _checkpoints.RemoveAt(index);
        }

        public override void AddShapes(int selectedIndex)
        {
            List<_ISectionEntry> entries = GetData();
            for (int i = 0; i < _checkpoints.Count; i++)
            {
                _CKPT checkpoint = (_CKPT)entries[i];
                bool highlighted = selectedIndex == HighlightAllCheckpoints || selectedIndex == i;
                Color color = highlighted ? KmpViewportSync.HighlightColor : GetCheckpointColor(checkpoint);
                _checkpoints[i].Line.FillColor = color;
                _viewport.AddShape(_checkpoints[i].Line);
                _viewport.AddShape(CreateDirectionArrow(entries, i, color));
            }
        }

        private void IncrementFollowingGroupStarts(int amount)
        {
            for (int i = _groupIndex + 1; i < KMP.CKPH.Length(); i++)
            {
                _CKPH group = (_CKPH)KMP.CKPH.GetEntry(i);
                group.Start = (byte)(group.Start + amount);
            }
        }

        private void RebuildShapes()
        {
            _checkpoints.Clear();
            foreach (_CKPT entry in GetData())
                AddShape(entry);
        }

        private void AddShape(_CKPT entry)
        {
            DraggableLine line = new DraggableLine(entry.PositionL, entry.PositionR, _viewport);
            line.FillColor = GetCheckpointColor(entry);
            _checkpoints.Add(new CheckpointShape(entry, line));
        }

        private static Color GetCheckpointColor(_CKPT entry)
        {
            return entry.Type > 0 ? Color.Blue : Color.Green;
        }

        private static Arrow CreateDirectionArrow(List<_ISectionEntry> entries, int index, Color color)
        {
            _CKPT current = (_CKPT)entries[index];
            Vector2f start = KmpViewportSync.Center(current.PositionL, current.PositionR);

            if (index < entries.Count - 1)
            {
                _CKPT next = (_CKPT)entries[index + 1];
                return KmpViewportSync.DirectionArrow(start, KmpViewportSync.Center(next.PositionL, next.PositionR), color);
            }

            if (index > 0)
            {
                _CKPT previous = (_CKPT)entries[index - 1];
                return KmpViewportSync.DirectionArrow(KmpViewportSync.Center(previous.PositionL, previous.PositionR), start, color);
            }

            Vector2f line = current.PositionR - current.PositionL;
            Vector2f perpendicular = new Vector2f(-line.Y, line.X);
            return KmpViewportSync.DirectionArrow(start, start + perpendicular, color);
        }
    }
}
