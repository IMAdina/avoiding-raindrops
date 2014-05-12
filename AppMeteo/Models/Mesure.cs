using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace AppMeteo.Models
{
   public class Mesure
    {
       public int MesureId { get; set; }

       [Display(Name="Moment du prélèvement")]
       public DateTime MomentPrelevement { get; set; }

       public int StationId { get; set; }
       public virtual Station Station { get; set; }
       public virtual Valeur Valeur { get; set; }

    }
}
