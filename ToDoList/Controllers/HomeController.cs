using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class HomeController : Controller
    {
        private readonly ToDoListContext _db;

        public HomeController(ToDoListContext db)
        {
            _db = db;   
        }


        [HttpGet("/")]
        public IActionResult Index()
        {
            Category[] cates = _db.Categories.ToArray();
            Item[] items= _db.Items.ToArray();  
            Dictionary<string, object[]> model = new Dictionary<string, object[]>();    
            model.Add("categories", cates);
            model.Add("items", items);
            return View(model);
        }

    }
}
