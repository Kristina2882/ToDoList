using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ToDoListContext _db;
        public CategoriesController(ToDoListContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            List<Category> model = _db.Categories.ToList();
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            _db.Categories.Add(category);
            _db.SaveChanges();  
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            Category thisCategory = _db.Categories
                .Include(category => category.Items)
                .FirstOrDefault(category => category.CategoryId == id);
            return View(thisCategory);  
        }

        public IActionResult Edit(int id) 
        {
            Category thisCategory = _db.Categories.FirstOrDefault(category => category.CategoryId == id);
            return View(thisCategory);
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            _db.Categories.Update(category);    
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            Category thisCategory = _db.Categories.FirstOrDefault(category => category.CategoryId == id);
            return View(thisCategory);
        }
        [HttpPost,ActionName("Delete")] 
        public IActionResult DeleteConfirmed(int id) 
        {
            Category thisCategory = _db.Categories.FirstOrDefault(category => category.CategoryId == id);
            _db.Categories.Remove(thisCategory);
            _db.SaveChanges();
            return RedirectToAction("Index");  
        }


    }

    }

