using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ToDoList.Models;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ToDoList.Controllers
{
    public class ItemsController : Controller
    {
        private readonly ToDoListContext _db;

        public ItemsController(ToDoListContext db)
        {
            _db = db;
        }

        public ActionResult Index()
        {
            List<Item> model = _db.Items
                .Include(item => item.Category)
                .ToList();
            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Item item)
        {
            if (item.CategoryId == 0)
            {
                return RedirectToAction("Create");
            }
            _db.Items.Add(item);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            Item thisItem = _db.Items
                .Include (item => item.Category)    
                .FirstOrDefault(item => item.ItemId == id);
            return View(thisItem);
        }

        public IActionResult Edit(int id)
        {
            var thisItem = _db.Items.FirstOrDefault(item => item.ItemId == id);
            ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name");
            return View(thisItem);

        }

        [HttpPost]
        public IActionResult Edit(Item item)
        {
            _db.Items.Update(item);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            Item thisItem = _db.Items.FirstOrDefault(item => item.ItemId == id);
            return View(thisItem);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            Item thisItem = _db.Items.FirstOrDefault(item => item.ItemId == id);
            _db.Items.Remove(thisItem);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}