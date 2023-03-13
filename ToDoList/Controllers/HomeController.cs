using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class HomeController : Controller
    {
        private readonly ToDoListContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(UserManager<ApplicationUser> userManager, ToDoListContext db)
        {
            _userManager = userManager; 
            _db = db;   
        }


        [HttpGet("/")]
        public async Task<ActionResult> Index()
        {
            Category[] cates = _db.Categories.ToArray();
            Dictionary<string, object[]> model = new Dictionary<string, object[]>();
            model.Add("categories", cates);
            string userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ApplicationUser currentUser = await _userManager.FindByIdAsync(userId); 
            if (currentUser != null)
            {
                Item[] items = _db.Items
                    .Where(entry => entry.User.Id== currentUser.Id) 
                    .ToArray();
                model.Add("items", items);
            }
             return View(model);
        }

    }
}
