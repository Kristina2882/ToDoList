using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ToDoList.Models;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ToDoList.Migrations;

namespace ToDoList.Controllers
{
    public class ItemsController : Controller
    {
        private readonly ToDoListContext _db;

        public ItemsController(ToDoListContext db)
        {
            _db = db;
        }

        public ActionResult Index(Item item)
        {
            List<Item> model = _db.Items
                .Include(item => item.Category)
                .OrderBy(item => item.DueDate)
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
                .Include(item => item.JoinEntities)
                .ThenInclude(join => join.Tag)
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

        public IActionResult AddTag(int id)
        {
            Item thisItem = _db.Items.FirstOrDefault(items => items.ItemId == id);
            ViewBag.TagId = new SelectList(_db.Tags, "TagId", "Title");
            return View(thisItem);
        }

        [HttpPost]
        public IActionResult AddTag(Item item, int tagId)
        {
#nullable enable
            ItemTag? joinEntity = _db.ItemTags.FirstOrDefault(join => (join.TagId == tagId && join.ItemId == item.ItemId));
#nullable disable
            if (joinEntity == null && tagId != 0)
            {
                _db.ItemTags.Add(new ItemTag() { TagId = tagId, ItemId = item.ItemId });
                _db.SaveChanges();

            }
            return RedirectToAction("Details", new { id = item.ItemId });
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
        [HttpPost]
        public IActionResult DeleteJoin(int joinId)
        {
            ItemTag joinEntity = _db.ItemTags.FirstOrDefault(join => join.ItemTagId == joinId);
            _db.ItemTags.Remove(joinEntity);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }



    }
}