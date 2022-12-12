using CRA.Context;
using CRA.Filters;
using CRA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRA.Controllers
{

    public class AuthentificationController : Controller
    {
        CRAContext db = new CRAContext();

        // GET: Authentification
        [AuthenticationFilter]
        public ActionResult Accueil()
        {
            return PartialView();
        }

        public ActionResult Authenticate(string username, string password)
        {
            Employee emp = db.Employees.SingleOrDefault(a => a.Name == username && a.Password == password);
            if (emp != null)
            {
                // Mis a jour de l'attribut IsActive dans la base de données car l'employé est maintenant connecté
                emp.IsActive = true;
                db.SaveChanges();

                if (emp.Role == "user")
                {
                    return RedirectToAction("CompteRendu","User");
                }
                else //role == "admin"
                {
                    return RedirectToAction("AffichageLignesSaisies", "Admin");
                }
            }
            else
                return RedirectToAction("Accueil");
        }

        public ActionResult Logout()
        {
            Employee CurrentUser = db.Employees.FirstOrDefault(m => m.IsActive);
            if (CurrentUser != null)
            {
                CurrentUser.IsActive = false;
                db.SaveChanges();
            }
            return RedirectToAction("Accueil");
        }
    }
}