using System;
using System.Collections.Generic;

namespace Tharga.PowerScan.Menu
{
    public class MainMenu
    {
        private SubMenu _selectedMenu;
        private readonly Dictionary<string, SubMenu> _subMenus = new Dictionary<string, SubMenu>();

        public MainMenu(Action<NodeBase, string> defaultHandler = null)
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

        public void Back()
        {
            _selectedMenu.Back();
        }

        public void Select()
        {
            _selectedMenu.Select();
        }

        public void Up()
        {
            _selectedMenu.Up();
        }

        public void Down()
        {
            _selectedMenu.Down();
        }

        public void Handle(string data)
        {
            _selectedMenu.Selected.Handle(data);
        }

        //public void Select(string name, NodeBase node)
        //{
        //    throw new NotImplementedException();
        //}

        public void Select(string name, string path = null)
        {
            if (name == null) name = String.Empty;

            if (!_subMenus.TryGetValue(name, out var subMenu))
                throw new InvalidOperationException($"Cannot find a menu with name '{name}'.");

            _selectedMenu = subMenu;
            _selectedMenu.Select(path);
        }

        public MainMenu AddSubMenu(SubMenu subMenu)
        {
            if (String.IsNullOrEmpty(subMenu.Name)) throw new ArgumentNullException(nameof(subMenu.Name), "No name set for submenu.");

            subMenu.SetRoot(this);
            _subMenus.Add(subMenu.Name, subMenu);
            return this;
        }
    }
}