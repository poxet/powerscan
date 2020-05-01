using System;
using System.Collections.Generic;
using System.Linq;

namespace Tharga.PowerScan
{
    public abstract class NodeBase
    {
        private readonly Action<NodeBase, string> _handler;

        protected NodeBase(string name, Action<NodeBase, string> handler)
        {
            _handler = handler;
            Name = name;
        }

        public virtual string Name { get; }
        private readonly List<MenuNode> _nodes = new List<MenuNode>();
        protected internal NodeBase[] Nodes => _nodes.ToArray<NodeBase>();
        protected internal NodeBase Parent { get; internal set; }
        public virtual Menu Menu => Parent.Menu;
        public virtual string Path => GetPath(true);

        public void Handle(string data)
        {
            Handle(this, data);
        }

        protected void Handle(NodeBase sender, string data)
        {
            if (_handler != null)
                _handler.Invoke(sender, data);
            else if (Parent != null)
                Parent.Handle(sender, data);
            else
                throw new InvalidOperationException("No handler registered for menu.");
        }

        protected string GetPath(bool moreMarker)
        {
            var more = moreMarker && Nodes.Any() ? ">" : string.Empty;
            var value = Parent?.GetPath(false);
            var parentPath = string.IsNullOrEmpty(value) ? string.Empty : $"{value}/";
            if (GetType() == typeof(SubMenu))
                return string.Empty;
            return $"{parentPath}{Name}{more}";
        }

        public MenuNode AddNode(MenuNode menuNode)
        {
            menuNode.SetParent(this);
            _nodes.Add(menuNode);
            return this as MenuNode;
        }

        public MenuNode AddConfirm(string accept = "Yes", string reject = "No", Action<NodeBase, string> handler = null, bool acceptAsDefault = false)
        {
            var acceptNode = new MenuNode(accept, (s,d) =>
            {
                if (d == Menu.Constants.Select)
                    handler.Invoke(s, d);
                else
                    Handle(s, d);
            });
            acceptNode.SetParent(this);

            var rejectNode = new MenuNode(reject, (s, d) =>
            {
                if (d == Menu.Constants.Select)
                    s.Menu.Back();
                else
                    Handle(s, d);
            });
            rejectNode.SetParent(this);

            if (acceptAsDefault)
            {
                _nodes.Add(acceptNode);
                _nodes.Add(rejectNode);
            }
            else
            {
                _nodes.Add(rejectNode);
                _nodes.Add(acceptNode);
            }

            return this as MenuNode;
        }
    }
}