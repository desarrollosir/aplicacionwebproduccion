//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebAppProduccion.Entities.ModulosOperaciones
{
    using System;
    using System.Collections.Generic;
    
    public partial class wh_molinesdetalles
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public string Line { get; set; }
        public string TransactionType { get; set; }
        public string Item { get; set; }
        public string Rev { get; set; }
        public string SourceSubinv { get; set; }
        public string SourceLocator { get; set; }
        public string DestinationSubinv { get; set; }
        public string DestinationLocator { get; set; }
        public string UOM { get; set; }
        public Nullable<int> TransactionQty { get; set; }
        public Nullable<int> RequestedQty { get; set; }
        public Nullable<int> AllocatedQty { get; set; }
        public Nullable<System.DateTime> DateRequired { get; set; }
        public string Reason { get; set; }
        public string Reference { get; set; }
        public string LineStatus { get; set; }
        public Nullable<System.DateTime> StatusDate { get; set; }
        public string CreatedBy { get; set; }
        public int wh_molines_Id { get; set; }
    
        public virtual wh_molines wh_molines { get; set; }
    }
}
