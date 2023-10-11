using Microsoft.AspNetCore.Mvc;
using Aspnet_Project.Models;

namespace Aspnet_Project.Components.ViewComponents
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var menuItems = new List<MenuItem>
            {
                new MenuItem {Controller = "Home", Action = "Index", Label = "Home"},
                new MenuItem {Controller = "Departments", Action = "Index", Label = "Departments"},
                new MenuItem {Controller = "Products", Action = "Index", Label = "Products"},
                new MenuItem {Controller = "Home", Action = "Brief", Label = "Brief"},
                new MenuItem {Controller = "Home", Action = "Privacy", Label = "Privacy"}
            
            };

            return View(menuItems);
        }
    }
}