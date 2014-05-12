using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace AppMeteo.Models
{
    public class Pays
    {
        public int PaysId { get; set; }
        [Display(Name="Pays")]
        public string Nom { get; set; }
        public string ISO { get; set; }

        public virtual List<Station> Stations { get; set; }
    }
}
