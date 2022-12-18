using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc.Filters;
using System.Web.Mvc;
using CRA.Context;
using CRA.Models;
using System.Web.Routing;

namespace CRA.Filters
{
    public class AdminFilter : ActionFilterAttribute, IAuthenticationFilter
    {
        CRAContext db = new CRAContext();

        public void OnAuthentication(AuthenticationContext filterContext)
        {
            // on vérifie si un employé est actif
            Employee ActiveEmployee = db.Employees.FirstOrDefault(m => m.IsActive);

            //si aucun employé actif, renvoi vers la page d'authentification
            if (ActiveEmployee == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(
                    new { action = "Accueil", controller = "Authentification" }));
            }

            // vérification que l'employé actif a le role admin
            else if (ActiveEmployee.Role != "admin")
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(
                    new { action = "CompteRendu", controller = "User" }));
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
        }
    }
}