using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tharga.PowerScan.Menu
{
    public abstract class NodeBase
    {
        private readonly Func<NodeBase, string, Task<HandlerResult>> _handler;

        protected NodeBase(string name, Func<NodeBase, string, Task<HandlerResult>> handler)
        {
            _handler = handler;
            Name = name;
        }

        public virtual string Name { get; }
        private readonly List<MenuNode> _nodes = new List<MenuNode>();
        protected internal NodeBase[] Nodes => _nodes.ToArray<NodeBase>();
        protected internal NodeBase Parent { get; internal set; }
        public virtual MainMenu Menu => Parent.Menu;
        public virtual string Path => GetPath(true);

        public Task<HandlerResult> Handle(string data)
        {
            return Handle(this, data);
        }

        protected Task<HandlerResult> Handle(NodeBase sender, string data)
        {
            if (_handler != null)
                return _handler.Invoke(sender, data);

            if (Parent != null)
                return Parent.Handle(sender, data);

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

        public MenuNode AddConfirm(string accept = "Yes", string reject = "No", Func<NodeBase, string, Task<HandlerResult>> handler = null, bool acceptAsDefault = false)
        {
            var acceptNode = new MenuNode(accept, (s, d) =>
            {
                if (d == Constants.Select && handler != null)
                    return handler.Invoke(s, d);

                return Handle(s, d);
            });
            acceptNode.SetParent(this);

            var rejectNode = new MenuNode(reject, (s, d) =>
            {
                if (d == Constants.Select)
                    return s.Menu.Back();

                return Handle(s, d);
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