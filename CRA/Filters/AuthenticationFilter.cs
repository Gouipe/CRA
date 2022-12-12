using CRA.Context;
using CRA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;

namespace CRA.Filters
{
    public class AuthenticationFilter : ActionFilterAttribute, IAuthenticationFilter
    {
        CRAContext db = new CRAContext();

        public void OnAuthentication(AuthenticationContext filterContext)
        {
            //Employé de la session courante
            Employee ActiveEmployee = null;

            try
            {
                // On récupère l'employé actif
                ActiveEmployee = db.Employees.SingleOrDefault(a => a.IsActive);
            } catch // si plusieurs employés actifs
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }

            // Si il ya bien un unique employé avec une session ouverte
            if (ActiveEmployee != null)
            {
                // l'employé est un user normal
                if (ActiveEmployee.Role == "user")
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(
                        new { action = "CompteRendu", controller = "User" }));

                }
                //l'employe est un admin
                else
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(
                        new { action = "AffichageLignesSaisies", controller = "Admin" }));
                }
            }

            
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {


        }
    }
}