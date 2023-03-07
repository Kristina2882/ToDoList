using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class TagsController : Controller
    {
        private readonly ToDoListContext _db;

        public TagsController(ToDoListContext db) 
        {
            _db= db;    
        }
        public IActionResult Index()
        {
            return View(_db.Tags.ToList());
        }

        public IActionResult Details(int id)
        {
            Tag thisTag = _db.Tags
                          .Include(tag => tag.JoinEntities)
                          .ThenInclude(join => join.Item)
                          .FirstOrDefault(tag => tag.TagId== id);   
            return View(thisTag);    
        }
        
        public IActionResult Create()
        {
            return View();  
        }

        [HttpPost]

        public IActionResult Create(Tag tag)
        {
            _db.Tags.Add(tag);  

            _db.SaveChanges();  
            return RedirectToAction("Index");
        }

        public IActionResult AddItem(int id)
        {
            Tag thisTag = _db.Tags.FirstOrDefault(tags => tags.TagId == id);
            ViewBag.ItemId = new SelectList(_db.Items, "ItemId", "Description");
            return View(thisTag);

        }
        [HttpPost]
        public IActionResult AddItem(Tag tag, int itemId)
        {
#nullable enable
            ItemTag? joinEntity = _db.ItemTags.FirstOrDefault(join => (join.ItemId == itemId && join.TagId == tag.TagId));
#nullable disable         
            if (joinEntity == null && itemId != 0)
            {
                _db.ItemTags.Add(new ItemTag() { ItemId = itemId, TagId = tag.TagId });
                _db.SaveChanges();  
            }

            return RedirectToAction("Details", new {id = tag.TagId});   
        }

        public IActionResult Edit (int id)
        {
            Tag thisTag = _db.Tags.FirstOrDefault(tag => tag.TagId == id);
            return View(thisTag);
        }

        [HttpPost]
        public IActionResult Edit(Tag tag)
        {
            _db.Tags.Update(tag);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            Tag thisTag = _db.Tags.FirstOrDefault(tag => tag.TagId == id);
            return View(thisTag);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            Tag thisTag = _db.Tags.FirstOrDefault(tag => tag.TagId == id);
            _db.Tags.Update(thisTag);
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
