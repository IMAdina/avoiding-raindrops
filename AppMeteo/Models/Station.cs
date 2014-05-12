using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppMeteo.Models
{
    public class Station
    {
        public int StationId { get; set; }
        [Display(Name="Emplacement (code postal)")]
        public int CodePostal { get; set; }
        public int PaysId { get; set; }

        public virtual Pays Pays { get; set; }
        public virtual List<Mesure> Mesures { get; set; }

    }
}