using CRA.Context;
using CRA.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace CRA.Controllers
{
    public class UserController : Controller
    {
        //Récupération du contexte
        CRAContext db = new CRAContext();
        // Semaine sélectionnée pour savoir quelles missions afficher
        
        // GET: User
        public ActionResult CompteRendu(string date=null)
        {
            /***RECUPERATION DES LIGNES DE SAISIES****/
            //Outils pour gestion des dates:
            CultureInfo myCI = new CultureInfo("fr-FR");
            Calendar myCal = myCI.Calendar;      
            // Gets the DTFI properties required by GetWeekOfYear.
            CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;
            DayOfWeek myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;
            // date sélectionnée (date vaut null lors du premier chargement, et vaut "" si aucune date n'est choisie et que l'utilisateur appuie
            // quand meme sur le bouton "choisir date"
            if (date != null && date != "")
            {
                DateTime ChosenDate = Convert.ToDateTime(date);
                GlobalValues.ChosenWeek = myCal.GetWeekOfYear(ChosenDate, myCWR, myFirstDOW);              
            }

            // on recupere les lignes de saisie correspondant à l'employé actif
            Employee currentEmployee = db.Employees.FirstOrDefault(m => m.IsActive);
            int currentEmployeeId = currentEmployee.EmployeeId;
            IEnumerable<LigneSaisie> lignes = db.LigneSaisies.Where(m => m.Employee.EmployeeId == currentEmployeeId);
            // on garde les lignes de saisie correspondant à la derniere semaine choisie et on les passe à la vue
            ViewBag.lignes = lignes.Where(m => myCal.GetWeekOfYear(m.MissionDay, myCWR, myFirstDOW) == GlobalValues.ChosenWeek).ToList();
            /****************************************/

            //On passe l'employé courant à la vue pour pouvoir choisir parmi ses missions dans le formulaire
            ViewBag.currentEmployee = currentEmployee;
            return View();
            
        }

        [HttpPost]
        public ActionResult AjoutLigne(FormCollection collection, LigneSaisie ls)
        {
            //Création de la nouvelle ligne de saisie et initialisation de ses attributs
            ls.State = "created";
            if (!collection["date"].IsEmpty()) { 
               ls.MissionDay = Convert.ToDateTime(collection["date"]);
            }
            else
            {
                ModelState.AddModelError("MissionDay", "Please choose a date");
            }
            ls.FractionDay = collection["fractionDay"];
            Employee currentEmployee = db.Employees.FirstOrDefault(m => m.IsActive);
            ls.Employee = currentEmployee;
            ls.Comment = collection["comment"];
            string missionStr = collection["Mission"];
            Mission mission = db.Missions.SingleOrDefault(m => m.Libelle == missionStr);
            ls.Mission = mission;
            ls.SendingDay = DateTime.Now;

            //On essaie d'ajouter le nouveau modele à la base de données
            try
            {
                db.LigneSaisies.Add(ls);
                db.SaveChanges();
                //feedback positif
                ViewBag.feedback = "Ligne de saisie bien ajoutee";
            }
            catch(Exception e)
            {
                //feedback négatif car exception levée en essayant d'ajouter le nouveau modele créé
                ViewBag.feedback = "Erreur lors de la saisie de la ligne";
            }

            //RECUPERATION DES LIGNES DE SAISIES
            int currentEmployeeId = currentEmployee.EmployeeId;
            //Outils pour gestion des dates:
            CultureInfo myCI = new CultureInfo("fr-FR");
            Calendar myCal = myCI.Calendar;
            // Gets the DTFI properties required by GetWeekOfYear.
            CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;
            DayOfWeek myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;
            // on recupere les lignes de saisie correspondant à l'employé actif
            IEnumerable<LigneSaisie> lignes = db.LigneSaisies.Where(m => m.Employee.EmployeeId == currentEmployeeId);
            // on garde les lignes de saisie correspondant à la derniere semaine choisie et on les passe à la vue
            ViewBag.lignes = lignes.Where(m => myCal.GetWeekOfYear(m.MissionDay, myCWR, myFirstDOW) == GlobalValues.ChosenWeek).ToList();

            //On passe l'employé courant à la vue pour pouvoir choisir parmi ses missions dans le formulaire
            ViewBag.currentEmployee = currentEmployee;
            return View("CompteRendu");
        }
        public ActionResult DeleteLigne(int id)
        {
            LigneSaisie ls = db.LigneSaisies.Find(id);
            if (ls != null)
            {
                db.LigneSaisies.Remove(ls);
                db.SaveChanges();
            }
            return RedirectToAction("CompteRendu", "User"); 
        }

        [HttpGet]
        public ActionResult UpdateLigne(int id)
        {
            // Recuperation de la ligne de saisie à modifier
            LigneSaisie ls = db.LigneSaisies.Find(id);
            //Si la ligne de saisie n'existe pas, ou a déja été envoyée, on redirige sur la page compte-rendu
            if (ls == null || ls.State =="sent")
            {
                return RedirectToAction("CompteRendu", "User");
            }
            //Passage de l'mployé courant/actif à la vue pour pouvoir choisir parmi ses missions dans le formulaire
            Employee currentEmployee = db.Employees.SingleOrDefault(m => m.IsActive);
            ViewBag.currentEmployee = currentEmployee;

            //Renvoi vers le formulaire de modification en passant la ligne de saisie à modifier
            return View(ls);
        }

        [HttpPost]
        public ActionResult UpdateLigne(FormCollection collection, int id)
        {
            // Recuperation de la ligne de saisie modifiée
            LigneSaisie ls = db.LigneSaisies.Find(id);

            //Modification de ses attributs
            if (!collection["date"].IsEmpty())
            {
                ls.MissionDay = Convert.ToDateTime(collection["date"]);
            }
            else
            {
                ModelState.AddModelError("MissionDay", "Please choose a date");
            }
            ls.FractionDay = collection["fractionDay"];
            Employee currentEmployee = db.Employees.SingleOrDefault(m => m.IsActive);
            ls.Employee = currentEmployee;
            ls.Comment = collection["comment"];
            string missionStr = collection["Mission"];
            Mission mission = db.Missions.SingleOrDefault(m => m.Libelle == missionStr);
            ls.Mission = mission;

            //Actualisation des changements
            try
            {
                db.LigneSaisies.AddOrUpdate(ls);
                db.SaveChanges();
            }
            catch (Exception e)
            {
            }

            return RedirectToAction("CompteRendu", "User");
        }
        public ActionResult EnvoyerLignes()
        {
            Employee currentEmployee = db.Employees.FirstOrDefault(m => m.IsActive);
            int currentEmployeeId = currentEmployee.EmployeeId;
            //Outils pour gestion des dates:
            CultureInfo myCI = new CultureInfo("fr-FR");
            Calendar myCal = myCI.Calendar;

            // Gets the DTFI properties required by GetWeekOfYear.
            CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;
            DayOfWeek myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;

            // on recupere les lignes à envoyer
            //lignes de l'emplyé courant...
            IEnumerable<LigneSaisie> ls = db.LigneSaisies.Where(m => m.Employee.EmployeeId == currentEmployeeId);
            //... de la semaine choisie...
            ls = ls.Where(m => myCal.GetWeekOfYear(m.MissionDay, myCWR, myFirstDOW) == GlobalValues.ChosenWeek).ToList();
            //... qui ne sont pas encore envoyée ou déjà validées
            ls = ls.Where(m => m.State == "created" || m.State == "saved" || m.State == "refused").ToList();

            if (ls != null)
            {
                //On passe l'etat des lignes a "sent"
                ls.ForEach(x => { x.State = "sent"; x.SendingDay = DateTime.Now; });
                db.SaveChanges();
            }
            return RedirectToAction("CompteRendu", "User");
        }
    }
}