using System;
using System.Linq;

namespace Tharga.PowerScan
{
    public class SubMenu : NodeBase
    {
        private NodeBase _entered;
        private int _index;
        private Menu _menu;

        public SubMenu(string name, Action<NodeBase, string> handler = null)
            : base(name, handler)
        {
            _entered = this;
        }

        public override Menu Menu => _menu;
        public override string Path => Selected.Path;

        private NodeBase Entered => _entered;
        public NodeBase Selected => Entered.Nodes.Any() ? Entered.Nodes[_index] : this;

        internal void SetRoot(Menu menu)
        {
            _menu = menu;
        }

        public void Up()
        {
            if (Entered.Nodes.Length <= 1)
            {
                Selected.Handle(Menu.Constants.Up);
                return;
            }

            _index--;
            if (_index < 0)
                _index = Entered.Nodes.Length - 1;
        }

        public void Down()
        {
            if (Entered.Nodes.Length <= 1)
            {
                Selected.Handle(Menu.Constants.Down);
                return;
            }

            _index++;
            if (_index >= Entered.Nodes.Length)
                _index = 0;
        }

        public void Select()
        {
            if (Entered.Nodes.Length == 0)
            {
                Selected.Handle(Menu.Constants.Select);
            }
            else if (Entered.Nodes[_index].Nodes.Any())
            {
                _entered = Entered.Nodes[_index];
                _index = 0;
            }
            else
            {
                Selected.Handle(Menu.Constants.Select);
                //TODO: Indicate invalid selection
            }
        }

        public void Back()
        {
            if (Entered.Parent != null)
            {
                for (var index = 0; index < Entered.Parent.Nodes.Length; index++)
                {
                    if (!ReferenceEquals(Entered.Parent.Nodes[index], Entered)) continue;
                    _index = index;
                    break;
                }

                _entered = Entered.Parent;
            }
            else
            {
                Selected.Handle(Menu.Constants.Back);
                //TODO: Indicate invalid selection
            }
        }

        public void Select(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                _entered = this;
                _index = 0;
                return;
            }

            throw new NotImplementedException("Select node by path is not yet implemented.");
        }
    }
}