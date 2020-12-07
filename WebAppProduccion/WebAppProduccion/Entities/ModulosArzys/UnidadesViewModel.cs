using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppProduccion.Entities.ModulosArzys
{
    public class UnidadesViewModel
    {
    }

    public partial class arz_unidades 
    {
        public string lineatransporte { get; set; }

        public string destino { get; set; }

        public string tiemporestante { get; set; }

        public string statusunidad { get; set; }

        public string retrabajo { get; set; }

        public string proceso { get; set; }

        public DateTime citaarriboconsulta { get; set; }

        public DateTime citadestinoconsulta { get; set; }

        public string citaarribostring { get; set; }

        public string citadestinostring { get; set; }

        public string horasalida { get; set; }

    }
}