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
    public ActionResult Create(string clientName, string clientPhone, int stylistId)
    {
      Client newClient = new Client(clientName, clientPhone, stylistId);
      newClient.Save();
      List<Client> allClients = Client.GetAll();
      return View("Index", allClients);
    }

    [HttpGet("/stylists/{stylistId}/clients/{clientId}")]
    public ActionResult Show(int stylistId, int clientId)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Stylist selectedStylist = Stylist.Find(stylistId);
      Client selectedClient = Client.Find(clientId);
      model.Add("selectedStylist", selectedStylist);
      model.Add("selectedClient", selectedClient);
      return View(model);
    }

    [HttpPost("/clients/delete")]
    public ActionResult DeleteAll()
    {
      Client.ClearAll();
      return View("/stylists");
    }

    [HttpPost("/stylists/{stylistId}/clients/{clientId}/delete")]
    public ActionResult Delete(int stylistId, int clientId)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Stylist selectedStylist = Stylist.Find(stylistId);
      Client selectedClient = Client.Find(clientId);
      selectedClient.Delete();
      model.Add("selectedStylist", selectedStylist);
      model.Add("selectedClient", selectedClient);
      return Redirect("/stylists");
    }

    [HttpGet("/stylists/{stylistId}/clients/{clientId}/edit")]
    public ActionResult Edit(int stylistId, int clientId)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Stylist selectedStylist = Stylist.Find(stylistId);
      Client selectedClient = Client.Find(clientId);
      model.Add("selectedStylist", selectedStylist);
      model.Add("selectedClient", selectedClient);
      return View(model);
    }

    [HttpPost("/stylists/{stylistId}/clients/{clientId}")]
    public ActionResult Update(int stylistId, int clientId, string newName, string newPhone)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Stylist selectedStylist = Stylist.Find(stylistId);
      Client selectedClient = Client.Find(clientId);
      selectedClient.Edit(newName, newPhone);
      model.Add("selectedStylist", selectedStylist);
      model.Add("selectedClient", selectedClient);
      return View("Show", model);
    }

  }
}
