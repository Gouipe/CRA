using CRA.Context;
using CRA.Filters;
using CRA.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRA.Controllers
{
    [AdminFilter]
    public class AdminController : Controller
    {
        CRAContext db = new CRAContext();

        // GET: Admin
        public ActionResult AffichageLignesSaisies()
        {

            //on récupère toutes les lignes de saisies envoyées par des employés
            IEnumerable<LigneSaisie> lignesEnvoyees = db.LigneSaisies.Where(ligne => ligne.State == "sent");
            ViewBag.lignesEnvoyees = lignesEnvoyees.ToList();

            return View();
        }

        public ActionResult Valider(int id)
        {
            LigneSaisie ls = db.LigneSaisies.Find(id);
            if (ls != null)
            {
                ls.State = "approved";
                db.SaveChanges();
            }
            return RedirectToAction("AffichageLignesSaisies", "Admin");
        }

        public ActionResult Refuser(int id)
        {
            LigneSaisie ls = db.LigneSaisies.Find(id);
            if (ls != null)
            {
                ls.State = "refused";
                db.SaveChanges();
            }
            return RedirectToAction("AffichageLignesSaisies", "Admin");
        }

        public ActionResult AfficherAllouerMission()
        {
            ViewBag.Employees = db.Employees.ToList();
            ViewBag.Missions = db.Missions.ToList();
            return View();
        }
        [HttpPost]
        public ActionResult AllouerMission(FormCollection collection)
        {
            // Récupération de l'emloyé et de la mission sélectionnés
            Employee employee = db.Employees.Find(Int32.Parse(collection["employeeId"]));
            Mission mission = db.Missions.Find(Int32.Parse(collection["missionId"]));

            // allouer la mission à l'employé
            try
            {
                employee.Missions.Add(mission);
                mission.Employees.Add(employee);
                db.SaveChanges();
                //feedback positif
                ViewBag.feedback = "Allocation de la mission bien effectuée";
            }
            catch (Exception e)
            {
                //feedback négatif car exception levée en essayant d'ajouter le nouveau modele créé
                ViewBag.feedback = "Erreur lors de l'allocation de mission";
            }


            //redirection
            return RedirectToAction("AfficherAllouerMission", "Admin");
        }
    }
}