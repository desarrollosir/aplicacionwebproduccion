﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DB_A3F19C_producccionEntities2 : DbContext
    {
        public DB_A3F19C_producccionEntities2()
            : base("name=DB_A3F19C_producccionEntities2")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<credito> credito { get; set; }
        public virtual DbSet<monedafacturacion> monedafacturacion { get; set; }
        public virtual DbSet<categoriaproveedor> categoriaproveedor { get; set; }
        public virtual DbSet<statusproveedor> statusproveedor { get; set; }
        public virtual DbSet<direccionproveedor> direccionproveedor { get; set; }
        public virtual DbSet<informacionbancaria> informacionbancaria { get; set; }
        public virtual DbSet<proveedores> proveedores { get; set; }
        public virtual DbSet<contactosproveedores> contactosproveedores { get; set; }
    }
}
