using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppProduccion.Entities.ModulosOperaciones
{
    public class MoViewModel
    {
    }

    public partial class wh_LineasMO 
    {
        public string Order { get; set; }
        public string Lote { get; set; }
        public string QtyLote { get; set; }
        public string FechaExpiracion { get; set; }

    }
}