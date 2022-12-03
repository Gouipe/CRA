using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CRA.Models
{
    public class LigneSaisie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Ligne_id { get; set; }

        [Required(ErrorMessage = "Veuillez choisir le jour de la mission")]
        [DisplayName("Jour de la mission")]
        //Jour pendant lequel la mission a été effectuée
        public DateTime MissionDay { get; set; }

        // Jour pendant lequel la saisie  a été envoyée
        public DateTime SendingDay { get; set; }
        [DisplayName("Commentaire")]
        public string Comment { get; set; }

        [Required]
        [DisplayName("Fraction de la journee")]
        // Fraction du jour pendant laquelle la mission a été effectuée
        // "matin" ou "apres midi"
        public string FractionDay { get; set; }

        [Required]
        //Etat = {"created", "saved", "sent", "approved", "refused"}
        public string State { get; set; }

        [Required]
        public Employee Employee { get; set; }

        [Required]
        public virtual Mission Mission { get; set; }
    }
}