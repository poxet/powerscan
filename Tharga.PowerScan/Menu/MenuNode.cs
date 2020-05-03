using System;
using System.Threading.Tasks;

namespace Tharga.PowerScan.Menu
{
    public class MenuNode : NodeBase
    {
        public MenuNode(string name, Func<NodeBase, string, Task<HandlerResult>> handler = null)
            : base(name, handler)
        {
        }

        internal void SetParent(NodeBase parent)
        {
            Parent = parent;
        }
    }
}