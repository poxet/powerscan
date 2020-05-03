using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tharga.PowerScan.Menu
{
    public class MainMenu
    {
        private SubMenu _selectedMenu;
        private readonly Dictionary<string, SubMenu> _subMenus = new Dictionary<string, SubMenu>();

        public MainMenu(Func<NodeBase, string, Task<HandlerResult>> defaultHandler = null)
        {
            _selectedMenu = new SubMenu(string.Empty, defaultHandler);
            _selectedMenu.SetRoot(this);
            _subMenus.Add(_selectedMenu.Name, _selectedMenu);
        }

        public string Name => _selectedMenu.Name;
        public string Path => _selectedMenu.Path;

        public void AddNode(MenuNode menuNode)
        {
            _selectedMenu.AddNode(menuNode);
        }

        public Task<HandlerResult> Back()
        {
            return _selectedMenu.Back();
        }

        public Task<HandlerResult> Select()
        {
            return _selectedMenu.Select();
        }

        public Task<HandlerResult> Up()
        {
            return _selectedMenu.Up();
        }

        public Task<HandlerResult> Down()
        {
            return _selectedMenu.Down();
        }

        public Task<HandlerResult> Handle(string data)
        {
            return _selectedMenu.Selected.Handle(data);
        }

        public void Select(string name, string path = null)
        {
            if (name == null) name = string.Empty;

            if (!_subMenus.TryGetValue(name, out var subMenu))
                throw new InvalidOperationException($"Cannot find a menu with name '{name}'.");

            _selectedMenu = subMenu;
            _selectedMenu.Select(path);
        }

        public MainMenu AddSubMenu(SubMenu subMenu)
        {
            if (string.IsNullOrEmpty(subMenu.Name)) throw new ArgumentNullException(nameof(subMenu.Name), "No name set for submenu.");

            subMenu.SetRoot(this);
            _subMenus.Add(subMenu.Name, subMenu);
            return this;
        }
    }
}