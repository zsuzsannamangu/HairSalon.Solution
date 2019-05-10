using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using HairSalon.Models;

namespace HairSalon.Controllers
{
  public class StylistsController : Controller
  {

    [HttpGet("/stylists")]
    public ActionResult Index()
    {
      List<Stylist> allStylists = Stylist.GetAll();
      return View(allStylists);
    }

    [HttpGet("/stylists/new")]
    public ActionResult New()
    {
      return View();
    }

    [HttpPost("/stylists")]
    public ActionResult Create(string stylistName)
    {
      Stylist newStylist = new Stylist(stylistName);
      List<Stylist> allStylists = Stylist.GetAll();
      return View("Index", allStylists);
    }

    [HttpGet("/stylists/{id}")]
    public ActionResult Show(int id)
    {

      Dictionary<string, object> model = new Dictionary<string, object>();
      Stylist selectedStylist = Stylist.Find(id);
      List<Client> stylistsClients = selectedStylist.GetClients();
      model.Add("stylist", selectedStylist);
      model.Add("clients", stylistsClients);
      return View(model);
    }

    [HttpPost("/stylists/{stylistId}/clients")]
    public ActionResult Create(int stylistId, string clientName, int clientPhone, int clientId)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Stylist foundStylist = Stylist.Find(stylistId);
      Client newClient = new Client(clientName, clientPhone, clientId);
      newClient.Save();
      foundStylist.AddClient(newClient);
      List<Client> stylistClients = foundStylist.GetClients();
      model.Add("clients", stylistClients);
      model.Add("stylist", foundStylist);
      return View("Show", model);
    }
  }
}
