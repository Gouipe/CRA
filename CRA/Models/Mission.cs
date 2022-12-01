using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CRA.Models
{
    public class Mission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Mission_id { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Libelle { get; set; }
        public virtual List<Employee> Employees{ get; set; }
        public virtual List<LigneSaisie> LigneSaisies{ get; set; }
        public override string ToString()
        {
            return Libelle;
        }
    }
}