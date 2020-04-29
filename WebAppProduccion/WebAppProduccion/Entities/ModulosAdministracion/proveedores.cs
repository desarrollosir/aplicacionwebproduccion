//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebAppProduccion.Entities.ModulosAdministracion
{
    using System;
    using System.Collections.Generic;
    
    public partial class proveedores
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public proveedores()
        {
            this.contactosproveedores = new HashSet<contactosproveedores>();
            this.direccionproveedor = new HashSet<direccionproveedor>();
            this.informacionbancaria = new HashSet<informacionbancaria>();
        }
    
        public int id { get; set; }
        public Nullable<System.DateTime> FechaAlta { get; set; }
        public string RazonSocial { get; set; }
        public string RFC { get; set; }
        public string NombreComercial { get; set; }
        public string ActividadEmpresarial { get; set; }
        public string RepresentanteLegal { get; set; }
        public int MonedaFacturacion_Id { get; set; }
        public int Credito_Id { get; set; }
        public int CategoriaProveedor_Id { get; set; }
        public Nullable<int> StatusProveedor_Id { get; set; }
    
        public virtual categoriaproveedor categoriaproveedor { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<contactosproveedores> contactosproveedores { get; set; }
        public virtual credito credito { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<direccionproveedor> direccionproveedor { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<informacionbancaria> informacionbancaria { get; set; }
        public virtual monedafacturacion monedafacturacion { get; set; }
        public virtual statusproveedor statusproveedor { get; set; }
    }
}