using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

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
      List<Item> sorted = _db.Items.ToList().OrderBy(item => item.DueDate).ToList();
      return View(sorted);
    }

    public ActionResult Details(int id)
    {
      var thisItem = _db.Items
        .Include(item => item.JoinEntities)
        .ThenInclude(join => join.Category)
        .FirstOrDefault(item => item.ItemId == id);
      return View(thisItem);
    }

    // [HttpPost, ActionName("Details")]
    // public ActionResult Details(int id, Item item, bool Complete)
    // {
    //   Console.WriteLine(Complete + "Blah blah blah");
    //   if (Complete != false)
    //   {
    //   var thisItem = _db.Items.FirstOrDefault(item => item.ItemId == id);
    //   thisItem.Complete = true;
    //   }
    //   _db.Entry(item).State = EntityState.Modified;
    //   _db.SaveChanges();
    //   return RedirectToAction("Index");
    // }

    public ActionResult Complete(int id)
    {
      var thisItem = _db.Items.FirstOrDefault(item => item.ItemId == id);
      return View(thisItem);
    }

    [HttpPost, ActionName("Complete")]
    public ActionResult CompleteConfirm(int id, Item item, bool Complete)
    {
      Console.WriteLine(Complete + "Blah blah blah");
      if (Complete != true)
      {
        var thisItem = _db.Items.FirstOrDefault(item => item.ItemId == id);
        thisItem.Complete = true;
        _db.Entry(thisItem).State = EntityState.Modified;
        _db.SaveChanges();
      }
      return RedirectToAction("Index");
    }

    public ActionResult AddCategory(int id)
    {
      var thisItem = _db.Items.FirstOrDefault(item => item.ItemId == id);
      ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name");
      return View(thisItem);
    }

    [HttpPost]
    public ActionResult AddCategory(Item item, int CategoryId)
    {
      if (CategoryId != 0)
      {
        _db.CategoryItem.Add(new CategoryItem() {CategoryId = CategoryId, ItemId = item.ItemId});
      }
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

//     // public ActionResult Find(string description)
//     // {
//     //   var thisItem = _db.Items.FirstOrDefault(item => item.ItemDescription == description);
//     //   return View(thisItem);
//     // }

    //start edit item
    public ActionResult Edit(int id)
    {
      var thisItem = _db.Items.FirstOrDefault(item => item.ItemId == id);
      ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name");
      return View(thisItem);
    }

    [HttpPost]
    public ActionResult Edit(Item item, int CategoryId)
    {
      if (CategoryId != 0)
      {
        _db.CategoryItem.Add(new CategoryItem() {CategoryId = CategoryId, ItemId = item.ItemId});
      }
      _db.Entry(item).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
    //end edit item

    //start delete item
    public ActionResult Delete(int id)
    {
      var thisItem = _db.Items.FirstOrDefault(item => item.ItemId == id);
      return View(thisItem);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed (int id)
    {
      var thisItem = _db.Items.FirstOrDefault(item => item.ItemId == id);
      _db.Items.Remove(thisItem);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
    //end delete item

    [HttpPost]
    public ActionResult DeleteCategory(int joinId)
    {
      var joinEntry = _db.CategoryItem.FirstOrDefault(entry => entry.CategoryItemId == joinId);
      _db.CategoryItem.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Create()
    {
        ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name");
        return View();
    }

    [HttpPost]
    public ActionResult Create(Item item, int CategoryId)
    {
        _db.Items.Add(item);
        _db.SaveChanges();
        if (CategoryId != 0)
        {
          _db.CategoryItem.Add(new CategoryItem() { CategoryId = CategoryId, ItemId = item.ItemId});
        }
        _db.SaveChanges();
        return RedirectToAction("Index");
    }

  }
}

// // var thisItem = _db.Items.FirstOrDefault(item => item.ItemId == id);
// //       ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name");
// //       return View(thisItem);