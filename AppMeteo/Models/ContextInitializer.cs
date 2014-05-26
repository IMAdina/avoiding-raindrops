using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace AppMeteo.Models
{
    public class ContextInitializer : CreateDatabaseIfNotExists<MyContext>
    {
        protected override void Seed(MyContext context)
        {
            Random rand = new Random();
            List<Pays> countries = new List<Pays>
            {
                new Pays { Nom= "Islande", ISO="is"},
                new Pays { Nom= "Nouvelle Zéelande", ISO="nz"},
                new Pays { Nom= "Roumanie", ISO="ro"},
            };

            List<Station> stations = new List<Station>();

            foreach (Pays pays in countries)
            {
                for (int i = 0; i < 10; i++)
                {

                    Station station = new Station { CodePostal = rand.Next(1000, 10000), Pays = pays };
                    stations.Add(station);
                }
            }

            List<Valeur> valeurs = new List<Valeur>();
            List<Mesure> mesures = new List<Mesure>();

            DateTime[] momentsPrelevement = new DateTime[300];
            momentsPrelevement[0] = DateTime.Now;
            for (int i = 0; i < 299; )
            {
                i++;
                momentsPrelevement[i] = momentsPrelevement[i - 1].AddMinutes(30);
            }

            foreach (Station station in stations)
            {
                for (int i = 0; i < 10; )
                {
                    i++;
                    Valeur valeur =Valeur.getValeurBd();
                    Mesure mesure = new Mesure { Station = station, Valeur = valeur, MomentPrelevement=momentsPrelevement[i] };
                    mesures.Add(mesure);

                }
            }

            mesures.ForEach(m => context.Mesures.Add(m));

            base.Seed(context);
        }


    }
}