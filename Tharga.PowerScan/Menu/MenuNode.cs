using System;

namespace Tharga.PowerScan.Menu
{
    public class MenuNode : NodeBase
    {
        public MenuNode(string name, Action<NodeBase, string> handler = null)
            : base(name, handler)
        {
        }

        internal void SetParent(NodeBase parent)
        {
            Parent = parent;
        }
    }
}