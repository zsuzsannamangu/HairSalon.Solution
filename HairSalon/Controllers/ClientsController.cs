using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using HairSalon.Models;

namespace HairSalon.Controllers
{
  public class ClientsController : Controller
  {

    [HttpGet("/clients")]
    public ActionResult Index()
    {
      return View();
    }

    [HttpGet("stylists/{stylistId}/clients/new")]
    public ActionResult New(int stylistId)
    {
      Stylist stylist = Stylist.Find(stylistId);

      return View(stylist);
    }

    [HttpPost("/clients")]
    public ActionResult Create()
    {
      return View("Index");
    }

    [HttpPost("/clients/delete")]
    public ActionResult DeleteAll()
    {
      Client.ClearAll();
      return View();
    }
    // 
    // HttpGet("/stylists/{stylistId}/items/{itemId}/edit")]
    // public ActionResult Edit(int categoryId, int itemId)
    // {
    //   Dictionary<string, object> model = new Dictionary<string, object>();
    //   Category category = Category.Find(categoryId);
    //   model.Add("category", category);
    //   Item item = Item.Find(itemId);
    //   model.Add("item", item);
    //   return View(model);
    // }

  }
}
