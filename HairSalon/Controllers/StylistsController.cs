
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
    public ActionResult Create(string stylistName, string bio)
    {
      Stylist newStylist = new Stylist(stylistName, bio);
      newStylist.Save();
      List<Stylist> allStylists = Stylist.GetAll();
      return View("Index", allStylists);
    }

    [HttpPost("/stylists/delete")]
    public ActionResult DeleteAll()
    {
      Stylist.ClearAll();
      return View();
    }


    [HttpGet("/stylists/{id}")]
    public ActionResult Show(int id)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Stylist selectedStylist = Stylist.Find(id);
      List<Client> stylistsClients = selectedStylist.GetClients();
      List<Specialty> stylistSpecialties = selectedStylist.GetSpecialties();
      List<Specialty> allSpecialties = Specialty.GetAll();
      model.Add("stylist", selectedStylist);
      model.Add("clients", stylistsClients);
      model.Add("stylistSpecialties", stylistSpecialties);
      model.Add("allSpecialties", allSpecialties);
      return View(model);
    }

    [HttpPost("/stylists/{stylistId}/clients")]
    public ActionResult Create(int stylistId, string clientName, string clientPhone)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Stylist selectedStylist = Stylist.Find(stylistId);
      Client newClient = new Client(clientName, clientPhone, stylistId);
      newClient.Save();
      // List<Client> stylistsClients = selectedStylist.GetClients();
      // List<Client> clients = selectedStylist.GetClients();
      model.Add("newclient", newClient);
      model.Add("selectedStylist", selectedStylist);
      return View("Show", model);
    }

    [HttpPost("/stylists/{stylistId}/specialties/new")]
    public ActionResult AddSpecialty(int stylistId, int specialtyId)
    {
      Stylist stylist = Stylist.Find(stylistId);
      Specialty specialty = Specialty.Find(specialtyId);
      stylist.AddSpecialty(specialty);
      return RedirectToAction("Show",  new { id = stylistId });
    }

    [HttpGet("/stylists/{stylistId}/delete")]
    public ActionResult Delete(int stylistId)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();

      Stylist stylist = Stylist.Find(stylistId);
      model.Add("stylist", stylist);
      // Client client = Client.Find(clientId);
      // model.Add("client", client);

      return Redirect("/stylists");
    }

    [HttpGet("/stylists/{stylistId}/edit")]
    public ActionResult Edit(int stylistId)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();

      Stylist stylist = Stylist.Find(stylistId);
      model.Add("stylist", stylist);
      // Client client = Client.Find(clientId);
      // model.Add("client", client);

      return View(model);
    }

    [HttpPost("/stylists/{stylistId}/update")]
    public ActionResult Update(int stylistId, string newBio)
    {
      Stylist stylist = Stylist.Find(stylistId);
      stylist.Edit(newBio);
      Dictionary<string, object> model = new Dictionary<string, object>();
      model.Add("stylist", stylist);
      return View("Show", model);

    }



  }
}
