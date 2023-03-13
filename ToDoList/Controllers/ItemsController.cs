using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ToDoList.Models;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ToDoList.Migrations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace ToDoList.Controllers
{
    [Authorize]
    public class ItemsController : Controller
    {
        private readonly ToDoListContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public ItemsController(UserManager<ApplicationUser> userManager, ToDoListContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        public async Task<ActionResult> Index()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
            List<Item> userItems = _db.Items
                .Where(entry => entry.User.Id == currentUser.Id)
                .Include(item => item.Category)
                 .OrderBy(item => item.DueDate)
                .ToList();
            return View(userItems);
        }

        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name");
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(Item item, int CategoryId)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name");
                return View(item);
            }
            else
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
                item.User = currentUser;    
                _db.Items.Add(item);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
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