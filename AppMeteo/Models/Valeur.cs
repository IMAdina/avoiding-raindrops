using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppMeteo.Models
{
    public class Valeur
    {
        public int ValeurId { get; set; }
        //remarque générale: les valeurs ci-dessous correspondent aux records enregistrés dans chaque catégorie tels que présentés par Wikipédia
        //[Required]
        //[Range(-20, 44, ErrorMessage = "La valeur introduite ne correspond pas à une valeur possible pour ce champ")]
        public decimal Temperature { get; set; }

       //[Range(870, 1086.6, ErrorMessage = "La valeur introduite ne correspond pas à une valeur possible pour ce champ")]
        public decimal? Pression { get; set; }

        //[Range(2, 13500, ErrorMessage = "La valeur introduite ne correspond pas à une valeur possible pour ce champ")]
        public int? Precipitations { get; set; }

        //[Range(1, 10, ErrorMessage = "La valeur introduite ne correspond pas à une valeur possible pour ce champ")]
        public int? ATMO { get; set; }

        public virtual Mesure Mesure { get; set; }
        private static readonly Random random = new Random();

        private static double RandomNumberBetween(double minValue, double maxValue)
        {
            var next = random.NextDouble();

            return minValue + (next * (maxValue - minValue));
        }

        public static Valeur getValeurBd()
        {
            decimal temperature = (decimal)RandomNumberBetween(-20.00, 44.00);
            decimal pression = (decimal)RandomNumberBetween(870.00, 1190.00);
            int precipitations = random.Next(2, 13500);
            int atmo = random.Next(1, 11);
            Valeur valeur = new Valeur();
            valeur.Precipitations = precipitations;
            valeur.Pression = pression;
            valeur.Temperature = temperature;
            valeur.ATMO = atmo;
            return valeur;
        }
    }
}