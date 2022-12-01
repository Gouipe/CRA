namespace CRA.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using CRA.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<CRA.Context.CRAContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(CRA.Context.CRAContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
            Mission m1 = new Mission() { Code = "AAA", Libelle = "Test unitaire" };
            Mission m2 = new Mission() { Code = "AAB", Libelle = "Debuggage" };
            Mission m3 = new Mission() { Code = "AAC", Libelle = "Ajout fonctionnalite" };
            Mission m4 = new Mission() { Code = "AAD", Libelle = "Presentation client" };
            context.Missions.AddOrUpdate(
                    m1,
                    m2,
                    m3,
                    m4
                );
            Employee e1 = new Employee() { IsActive = false, Missions = new List<Mission> { m1, m2 }, Name = "roger", Role = "user", Password = "roger" };
            context.Employees.AddOrUpdate(
                    e1,
                    new Employee() { IsActive =false, Missions=new List<Mission> { m3, m2 },Name="marie",Role="user",Password= "marie" },
                    new Employee() { IsActive =false, Missions=new List<Mission> { m3, m2,m4 },Name="damien",Role="user",Password= "damien" },
                    new Employee() { IsActive = false, Name = "cesar", Role = "admin", Password = "cesar" }

                );
           //LigneSaisie ls1 = new LigneSaisie() { }
        }
    }
}
