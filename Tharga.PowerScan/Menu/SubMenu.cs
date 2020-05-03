using System;
using System.Linq;
using System.Threading.Tasks;

namespace Tharga.PowerScan.Menu
{
    public class SubMenu : NodeBase
    {
        private NodeBase _entered;
        private int _index;
        private MainMenu _menu;

        public SubMenu(string name, Func<NodeBase, string, Task<HandlerResult>> handler = null)
            : base(name, handler)
        {
            _entered = this;
        }

        public override MainMenu Menu => _menu;
        public override string Path => Selected.Path;

        private NodeBase Entered => _entered;
        public NodeBase Selected => Entered.Nodes.Any() ? Entered.Nodes[_index] : this;

        internal void SetRoot(MainMenu menu)
        {
            _menu = menu;
        }

        public async Task<HandlerResult> Up()
        {
            if (Entered.Nodes.Length <= 1)
            {
                return await Selected.Handle(Constants.Up);
            }

            _index--;
            if (_index < 0)
                _index = Entered.Nodes.Length - 1;

            return null;
        }

        public async Task<HandlerResult> Down()
        {
            if (Entered.Nodes.Length <= 1)
            {
                return await Selected.Handle(Constants.Down);
            }

            _index++;
            if (_index >= Entered.Nodes.Length)
                _index = 0;

            return null;
        }

        public async Task<HandlerResult> Select()
        {
            if (Entered.Nodes.Length == 0)
            {
                return await Selected.Handle(Constants.Select);
            }
            else if (Entered.Nodes[_index].Nodes.Any())
            {
                _entered = Entered.Nodes[_index];
                _index = 0;
                return null;
            }
            else
            {
                return await Selected.Handle(Constants.Select);
                //TODO: Indicate invalid selection
            }
        }

        public async Task<HandlerResult> Back()
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
                return await Selected.Handle(Constants.Back);
                //TODO: Indicate invalid selection
            }

            return null;
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