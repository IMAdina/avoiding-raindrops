using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AppMeteo.Models
{
    public class MyContext : DbContext
    {
        public DbSet<Valeur> Valeurs { get; set; }
        public DbSet<Mesure> Mesures { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<Pays> Countries { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Mesure>()
                .HasOptional(m => m.Valeur)
                .WithRequired(v => v.Mesure)
                .WillCascadeOnDelete();
        }
    }
}