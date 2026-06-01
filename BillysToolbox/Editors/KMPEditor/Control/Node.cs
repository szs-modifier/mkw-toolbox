using static kartlib.Serial.KMP;

namespace KMP_Editor.Control
{
    public abstract class Node
    {
        public abstract List<_ISectionEntry> GetData();

        public abstract string GetTitle(int index);

        public virtual bool CanAddEntries => true;

        public virtual bool CanRemoveEntries => true;
        
        public abstract void AddEntry();

        public abstract void RemoveEntry(int index);

        public virtual void Populate(TreeNode node)
        {
            node.Tag = this;
        }

        public virtual void AddShapes() { }

        public virtual void AddShapes(int selectedIndex)
        {
            AddShapes();
        }
    }
}
