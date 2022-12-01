using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using CRA.Models;
using System.Reflection.Emit;
using Microsoft.Ajax.Utilities;

namespace CRA.Context
{
    public class CRAContext : DbContext
    {
        public DbSet<Mission> Missions { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<LigneSaisie> LigneSaisies{ get; set; }

    }
}