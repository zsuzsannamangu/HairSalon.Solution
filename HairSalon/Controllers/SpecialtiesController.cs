using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HairSalon.Models;

namespace HairSalon.Controllers
{
  public class SpecialtiesController : Controller
  {
    [HttpGet("/specialties")]
    public ActionResult Index()
    {
      List<Specialty> allSpecialties = Specialty.GetAll();
      return View(allSpecialties);
    }

    [HttpGet("/specialties/new")]
    public ActionResult New()
    {
      return View();
    }

    [HttpPost("/specialties")]
    public ActionResult Create(string name, string price)
    {
      Specialty newSpecialty = new Specialty(name, price);
      newSpecialty.Save();
      List<Specialty> allSpecialties = Specialty.GetAll();
      return View("Index", allSpecialties);
    }

    [HttpPost("/specialties/delete")]
    public ActionResult DeleteAll()
    {
      Specialty.ClearAll();
      return View();
    }

    [HttpGet("/specialties/{id}")]
    public ActionResult Show(int id)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Specialty selectedSpecialty = Specialty.Find(id);
      List<Stylist> specialtyStylists = selectedSpecialty.GetStylists();
      List<Stylist> allStylists = Stylist.GetAll();
      model.Add("selectedSpecialty", selectedSpecialty);
      model.Add("specialtyStylists", specialtyStylists);
      model.Add("allStylists", allStylists);
      return View(model);
    }

    [HttpPost("/specialties/{specialtiesId}/stylists/new")]
    public ActionResult AddStylist(int specialtiesId, int stylistId)
    {
      Specialty specialty = Specialty.Find(specialtiesId);
      Stylist stylist = Stylist.Find(stylistId);
      specialty.AddStylist(stylist);
      return RedirectToAction("Show",  new { id = specialtiesId });
    }

    // [HttpPost("/specialties/{specialtiesId}/delete-specialty")]
    // public ActionResult DeleteSpecialty(int specialtyId)
    // {
    //   Specialty selectedSpecialty = Specialty.Find(specialtyId);
    //   selectedSpecialty.DeleteSpecialty(specialtyId);
    //   Dictionary<string, object> model = new Dictionary<string, object>();
    //   List<Specialty> specialtyStylists = selectedSpecialty.GetStylists();
    //   model.Add("selectedSpecialty", selectedSpecialty);
    //   return RedirectToAction("Index", "Specialties");
    // }
  }

}
