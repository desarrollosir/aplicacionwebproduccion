//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebAppProduccion.Entities.ModulosEscaneos
{
    using System;
    using System.Collections.Generic;
    
    public partial class hd_detordenesskus
    {
        public int id { get; set; }
        public Nullable<int> cantidad { get; set; }
        public int hd_ordenes_id { get; set; }
        public int hd_skushomedelivery_Id { get; set; }
    
        public virtual hd_ordenes hd_ordenes { get; set; }
        public virtual hd_skushomedelivery hd_skushomedelivery { get; set; }
    }
}