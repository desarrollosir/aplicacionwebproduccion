﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebAppProduccion.Entities.ModulosArzys
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DB_A3F19C_producccionEntities4 : DbContext
    {
        public DB_A3F19C_producccionEntities4()
            : base("name=DB_A3F19C_producccionEntities4")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<arz_destinos> arz_destinos { get; set; }
        public virtual DbSet<arz_lineatransporte> arz_lineatransporte { get; set; }
        public virtual DbSet<arz_statusunidad> arz_statusunidad { get; set; }
        public virtual DbSet<arz_unidades> arz_unidades { get; set; }
        public virtual DbSet<arz_detunidadproceso> arz_detunidadproceso { get; set; }
        public virtual DbSet<arz_proceso> arz_proceso { get; set; }
        public virtual DbSet<arz_detunidadretrabajo> arz_detunidadretrabajo { get; set; }
        public virtual DbSet<arz_retrabajos> arz_retrabajos { get; set; }
        public virtual DbSet<arz_retrabajo_semaforo> arz_retrabajo_semaforo { get; set; }
    }
}
