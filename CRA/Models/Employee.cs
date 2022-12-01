using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CRA.Models
{
    public class Employee
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        // Vaut true pour l'employé qui a actuellement une session ouverte sur le site
        public bool IsActive { get; set; }

        [Required]
        // "user" ou "admin"
        public string Role { get; set; }

        public virtual List<Mission> Missions{ get; set; }

        public virtual List<LigneSaisie> LigneSaisies { get; set; }

    }
}