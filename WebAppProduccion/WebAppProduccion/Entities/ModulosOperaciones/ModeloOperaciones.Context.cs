﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DB_A3F19C_producccionEntities3 : DbContext
    {
        public DB_A3F19C_producccionEntities3()
            : base("name=DB_A3F19C_producccionEntities3")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<wh_LineasMO> wh_LineasMO { get; set; }
        public virtual DbSet<wh_LotesMO> wh_LotesMO { get; set; }
        public virtual DbSet<wh_moveorder> wh_moveorder { get; set; }
        public virtual DbSet<wh_statusmo> wh_statusmo { get; set; }
        public virtual DbSet<wh_molines> wh_molines { get; set; }
        public virtual DbSet<wh_molinesdetalles> wh_molinesdetalles { get; set; }
        public virtual DbSet<wh_masterskus> wh_masterskus { get; set; }
    }
}
